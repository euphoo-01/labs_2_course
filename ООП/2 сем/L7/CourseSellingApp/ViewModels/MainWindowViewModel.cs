using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml.Styling;
using CourseSellingApp.Models;
using CourseSellingApp.Services;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Collections.Generic;
using System.Linq;
using Splat;

namespace CourseSellingApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ICourseService _courseService;
        private readonly IUndoRedoService _undoRedoService;
        private readonly IUserService _userService;
        private readonly ILocalizationService _localizationService;
        private Course? _selectedCourse;
        private bool _isAdmin;
        private string _searchQuery = string.Empty;
        private string? _selectedCategory;
        private decimal? _minPrice = 0m;
        private decimal? _maxPrice = 3000m;

        public ObservableCollection<Course> Courses { get; }
        public ObservableCollection<Course> FilteredCourses { get; }
        public ObservableCollection<string> Categories { get; }
        public Interaction<EditCourseViewModel, Course?> ShowDialog { get; }
        public Interaction<ProfileViewModel, Unit> ShowProfileDialog { get; }

        public bool IsAdmin
        {
            get => _isAdmin;
            set => this.RaiseAndSetIfChanged(ref _isAdmin, value);
        }

        public Course? SelectedCourse
        {
            get => _selectedCourse;
            set => this.RaiseAndSetIfChanged(ref _selectedCourse, value);
        }

        public bool CanUndo => _undoRedoService.CanUndo;
        public bool CanRedo => _undoRedoService.CanRedo;

        public string SearchQuery
        {
            get => _searchQuery;
            set { this.RaiseAndSetIfChanged(ref _searchQuery, value); FilterCourses(); }
        }

        public string? SelectedCategory
        {
            get => _selectedCategory;
            set { this.RaiseAndSetIfChanged(ref _selectedCategory, value); FilterCourses(); }
        }

        public decimal? MinPrice
        {
            get => _minPrice;
            set
            {
                this.RaiseAndSetIfChanged(ref _minPrice, value);
                this.RaisePropertyChanged(nameof(MinPriceDouble));
                FilterCourses();
            }
        }

        public decimal? MaxPrice
        {
            get => _maxPrice;
            set
            {
                this.RaiseAndSetIfChanged(ref _maxPrice, value);
                this.RaisePropertyChanged(nameof(MaxPriceDouble));
                FilterCourses();
            }
        }

        public double? MinPriceDouble
        {
            get => (double?)MinPrice;
            set => MinPrice = (decimal?)value;
        }

        public double? MaxPriceDouble
        {
            get => (double?)MaxPrice;
            set => MaxPrice = (decimal?)value;
        }

        public ReactiveCommand<Unit, Unit> AddCourseCommand { get; }
        public ReactiveCommand<Unit, Unit> EditCourseCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteCourseCommand { get; }
        public ReactiveCommand<UserRole, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenProfileCommand { get; }
        public ReactiveCommand<Unit, Unit> UndoCommand { get; }
        public ReactiveCommand<Unit, Unit> RedoCommand { get; }

        public MainWindowViewModel(
            ICourseService? courseService = null,
            IUndoRedoService? undoRedoService = null,
            IUserService? userService = null,
            ILocalizationService? localizationService = null)
        {
            _courseService = courseService ?? Locator.Current.GetService<ICourseService>()!;
            _undoRedoService = undoRedoService ?? Locator.Current.GetService<IUndoRedoService>()!;
            _userService = userService ?? Locator.Current.GetService<IUserService>()!;
            _localizationService = localizationService ?? Locator.Current.GetService<ILocalizationService>()!;

            _undoRedoService.StateChanged += (s, e) =>
            {
                this.RaisePropertyChanged(nameof(CanUndo));
                this.RaisePropertyChanged(nameof(CanRedo));
            };
            Courses = new ObservableCollection<Course>();
            FilteredCourses = new ObservableCollection<Course>();
            Categories = new ObservableCollection<string>();
            ShowDialog = new Interaction<EditCourseViewModel, Course?>();
            ShowProfileDialog = new Interaction<ProfileViewModel, Unit>();

            _userService.OnRoleChanged += (sender, role) => IsAdmin = role == UserRole.Administrator;
            IsAdmin = _userService.CurrentUserRole == UserRole.Administrator;

            var canEditOrDelete = this.WhenAnyValue(
                x => x.SelectedCourse,
                x => x.IsAdmin,
                (course, isAdmin) => course != null && isAdmin)
                .ObserveOn(RxApp.MainThreadScheduler);

            var canAdd = this.WhenAnyValue(x => x.IsAdmin)
                .ObserveOn(RxApp.MainThreadScheduler);

            AddCourseCommand = ReactiveCommand.CreateFromTask(AddCourseAsync, canAdd);
            EditCourseCommand = ReactiveCommand.CreateFromTask(EditCourseAsync, canEditOrDelete);
            DeleteCourseCommand = ReactiveCommand.CreateFromTask(DeleteCourseAsync, canEditOrDelete);
            LoginCommand = ReactiveCommand.Create<UserRole>(_userService.LoginAs);
            OpenProfileCommand = ReactiveCommand.CreateFromTask(OpenProfileAsync);

            var canUndoObservable = this.WhenAnyValue(x => x.CanUndo).ObserveOn(RxApp.MainThreadScheduler);
            var canRedoObservable = this.WhenAnyValue(x => x.CanRedo).ObserveOn(RxApp.MainThreadScheduler);

            UndoCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await _undoRedoService.UndoAsync();
                await LoadCoursesAsync();
            }, canUndoObservable);

            RedoCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await _undoRedoService.RedoAsync();
                await LoadCoursesAsync();
            }, canRedoObservable);

            _localizationService.LanguageChanged += (s, e) =>
            {
                if (Categories.Count > 0)
                {
                    string oldAll = Categories[0];
                    string newAll = GetAllCategoryText();
                    Categories[0] = newAll;
                    if (SelectedCategory == oldAll)
                    {
                        SelectedCategory = newAll;
                    }
                }
            };

            RxApp.MainThreadScheduler.Schedule(async () => await LoadCoursesAsync());
        }

        private async Task LoadCoursesAsync()
        {
            var courses = await _courseService.GetCoursesAsync();
            Courses.Clear();
            Categories.Clear();
            Categories.Add(GetAllCategoryText());

            foreach (var course in courses)
            {
                Courses.Add(course);
                if (!string.IsNullOrWhiteSpace(course.Category) && !Categories.Contains(course.Category))
                {
                    Categories.Add(course.Category);
                }
            }
            FilterCourses();
        }

        private string GetAllCategoryText()
        {
            if (Application.Current != null && Application.Current.TryFindResource("Filter.Category.All", out var resource) && resource is string text)
            {
                return text;
            }
            return "All";
        }

        private void FilterCourses()
        {
            string allCategory = GetAllCategoryText();
            var filtered = Courses.Where(c =>
                (string.IsNullOrWhiteSpace(SearchQuery) || c.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) || c.FullName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(SelectedCategory) || SelectedCategory == allCategory || c.Category == SelectedCategory) &&
                (!MinPrice.HasValue || c.Price >= MinPrice.Value) &&
                (!MaxPrice.HasValue || c.Price <= MaxPrice.Value)
            ).ToList();

            FilteredCourses.Clear();
            foreach (var c in filtered)
            {
                FilteredCourses.Add(c);
            }
        }

        private async Task AddCourseAsync()
        {
            var newCourse = new Course();
            var viewModel = new EditCourseViewModel(newCourse);

            var result = await ShowDialog.Handle(viewModel);
            if (result != null)
            {
                var command = new AddCourseCommand(_courseService, result);
                await _undoRedoService.ExecuteAsync(command);
                await LoadCoursesAsync();
            }
        }

        private async Task EditCourseAsync()
        {
            if (SelectedCourse == null) return;
            var courseToEdit = new Course
            {
                Id = SelectedCourse.Id,
                Name = SelectedCourse.Name,
                FullName = SelectedCourse.FullName,
                Description = SelectedCourse.Description,
                ImagePaths = new List<string>(SelectedCourse.ImagePaths ?? new List<string>()),
                CoverImagePath = SelectedCourse.CoverImagePath,
                Category = SelectedCourse.Category,
                Rating = SelectedCourse.Rating,
                Price = SelectedCourse.Price,
                Quantity = SelectedCourse.Quantity,
                Discount = SelectedCourse.Discount,
                IsAvailable = SelectedCourse.IsAvailable,
                RelatedCoursesIds = new List<int>(SelectedCourse.RelatedCoursesIds ?? new List<int>()),
                PurchasesCount = SelectedCourse.PurchasesCount,
                Author = SelectedCourse.Author
            };

            var viewModel = new EditCourseViewModel(courseToEdit);

            var result = await ShowDialog.Handle(viewModel);
            if (result != null)
            {
                var command = new EditCourseCommand(_courseService, SelectedCourse, result);
                await _undoRedoService.ExecuteAsync(command);
                await LoadCoursesAsync();
            }
        }

        private async Task DeleteCourseAsync()
        {
            if (SelectedCourse == null) return;

            var command = new DeleteCourseCommand(_courseService, SelectedCourse);
            await _undoRedoService.ExecuteAsync(command);

            SelectedCourse = null;
            await LoadCoursesAsync();
        }

        private async Task OpenProfileAsync()
        {
            var profileViewModel = new ProfileViewModel();
            await ShowProfileDialog.Handle(profileViewModel);
        }
    }
}
