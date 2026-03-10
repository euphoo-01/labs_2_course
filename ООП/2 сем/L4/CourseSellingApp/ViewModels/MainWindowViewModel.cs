using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using CourseSellingApp.Models;
using CourseSellingApp.Services;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Collections.Generic;
using System.Linq;

namespace CourseSellingApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ICourseService _courseService;
        private Course? _selectedCourse;
        private bool _isAdmin;
        private string _searchQuery = string.Empty;
        private string? _selectedCategory;
        private decimal? _minPrice;
        private decimal? _maxPrice;

        public ObservableCollection<Course> Courses { get; }
        public ObservableCollection<Course> FilteredCourses { get; }
        public ObservableCollection<string> Categories { get; }
        public Interaction<EditCourseViewModel, Course?> ShowDialog { get; }

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
            set { this.RaiseAndSetIfChanged(ref _minPrice, value); FilterCourses(); }
        }

        public decimal? MaxPrice
        {
            get => _maxPrice;
            set { this.RaiseAndSetIfChanged(ref _maxPrice, value); FilterCourses(); }
        }

        public ReactiveCommand<Unit, Unit> AddCourseCommand { get; }
        public ReactiveCommand<Unit, Unit> EditCourseCommand { get; }
        public ReactiveCommand<Unit, Unit> DeleteCourseCommand { get; }
        public ReactiveCommand<string,
 Unit> ChangeLanguageCommand
        { get; }
        public ReactiveCommand<UserRole, Unit> LoginCommand { get; }

        public MainWindowViewModel()
        {
            _courseService = new CourseService();
            Courses = new ObservableCollection<Course>();
            FilteredCourses = new ObservableCollection<Course>();
            Categories = new ObservableCollection<string>();
            ShowDialog = new Interaction<EditCourseViewModel, Course?>();

            UserService.OnRoleChanged += (sender, role) => IsAdmin = role == UserRole.Administrator;
            IsAdmin = UserService.CurrentUserRole == UserRole.Administrator;

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
            ChangeLanguageCommand = ReactiveCommand.Create<string>(ChangeLanguage);
            LoginCommand = ReactiveCommand.Create<UserRole>(UserService.LoginAs);

            RxApp.MainThreadScheduler.Schedule(async () => await LoadCoursesAsync());
        }

        private async Task LoadCoursesAsync()
        {
            var courses = await _courseService.GetCoursesAsync();
            Courses.Clear();
            Categories.Clear();
            Categories.Add(string.Empty);

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

        private void FilterCourses()
        {
            var filtered = Courses.Where(c =>
                (string.IsNullOrWhiteSpace(SearchQuery) || c.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) || c.FullName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)) &&
                (string.IsNullOrWhiteSpace(SelectedCategory) || c.Category == SelectedCategory) &&
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
                await _courseService.AddCourseAsync(result);
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
                await _courseService.UpdateCourseAsync(result);
                await LoadCoursesAsync();
            }
        }

        private async Task DeleteCourseAsync()
        {
            if (SelectedCourse == null) return;

            await _courseService.DeleteCourseAsync(SelectedCourse.Id);
            Courses.Remove(SelectedCourse);
            SelectedCourse = null;
            FilterCourses();
        }

        private void ChangeLanguage(string languageCode)
        {
            var app = Application.Current;
            if (app == null) return;

            var translations = new ResourceInclude(new Uri("avares://CourseSellingApp/App.axaml"))
            {
                Source = new Uri($"avares://CourseSellingApp/Resources/Strings/Strings.{languageCode}.axaml")
            };

            app.Resources.MergedDictionaries.Clear();
            app.Resources.MergedDictionaries.Add(translations);
        }
    }
}
