using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using CourseSellingApp.Models;
using CourseSellingApp.Services;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Collections.Generic;

namespace CourseSellingApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ICourseService _courseService;
        private Course? _selectedCourse;
        private bool _isAdmin;

        public ObservableCollection<Course> Courses { get; }
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
            ShowDialog = new Interaction<EditCourseViewModel, Course?>();

            // In a larger app, you'd use a DI container to manage service instances.
            // For this example, we use a static class for simplicity.
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
            foreach (var course in courses)
            {
                Courses.Add(course);
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
            // Create a copy for editing, so the original isn't modified if the user cancels
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
        }

        private void ChangeLanguage(string languageCode)
        {
            // Placeholder for localization logic
        }
    }
}
