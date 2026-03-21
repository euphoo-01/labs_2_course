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
        public Interaction<ProfileViewModel, Unit> ShowProfileDialog { get; }
        public AdminViewModel AdminViewModel { get; }

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

        public ReactiveCommand<UserRole, Unit> LoginCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenProfileCommand { get; }

        public MainWindowViewModel(
            ICourseService? courseService = null,
            IUserService? userService = null,
            ILocalizationService? localizationService = null)
        {
            _courseService = courseService ?? Locator.Current.GetService<ICourseService>()!;
            _userService = userService ?? Locator.Current.GetService<IUserService>()!;
            _localizationService = localizationService ?? Locator.Current.GetService<ILocalizationService>()!;

            Courses = new ObservableCollection<Course>();
            FilteredCourses = new ObservableCollection<Course>();
            Categories = new ObservableCollection<string>();
            ShowProfileDialog = new Interaction<ProfileViewModel, Unit>();

            AdminViewModel = new AdminViewModel();
            AdminViewModel.BackInteraction.RegisterHandler(async interaction =>
            {
                _userService.LoginAs(UserRole.Client);
                await LoadCoursesAsync();
                interaction.SetOutput(Unit.Default);
            });

            _userService.OnRoleChanged += (sender, role) => IsAdmin = role == UserRole.Administrator;
            IsAdmin = _userService.CurrentUserRole == UserRole.Administrator;

            LoginCommand = ReactiveCommand.CreateFromTask<UserRole>(async role =>
            {
                _userService.LoginAs(role);
                if (role == UserRole.Client)
                {
                    await LoadCoursesAsync();
                }
            });
            OpenProfileCommand = ReactiveCommand.CreateFromTask(OpenProfileAsync);

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



        private async Task OpenProfileAsync()
        {
            var profileViewModel = new ProfileViewModel();
            await ShowProfileDialog.Handle(profileViewModel);
        }
    }
}
