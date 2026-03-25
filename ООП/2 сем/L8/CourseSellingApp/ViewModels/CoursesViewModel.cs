using CourseSellingApp.Models;
using CourseSellingApp.Services;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseSellingApp.ViewModels
{
    public class CoursesViewModel : AdminViewModelBase
    {
        private readonly CourseService _courseService;

        public ObservableCollection<Course> Courses { get; }

        private Course? _selectedCourse;
        public Course? SelectedCourse
        {
            get => _selectedCourse;
            set => this.RaiseAndSetIfChanged(ref _selectedCourse, value);
        }

        public System.Windows.Input.ICommand AddCourseCommand { get; }
        public System.Windows.Input.ICommand EditCourseCommand { get; }
        public System.Windows.Input.ICommand DeleteCourseCommand { get; }

        public Interaction<EditCourseViewModel, Course?> ShowDialog { get; }

        public CoursesViewModel()
        {
            _courseService = new CourseService();
            Courses = new ObservableCollection<Course>();
            ShowDialog = new Interaction<EditCourseViewModel, Course?>();

            var canExecuteEditOrDelete = this.WhenAnyValue(x => x.SelectedCourse)
                .Select(c => c != null);

            AddCourseCommand = ReactiveCommand.CreateFromTask(AddCourseAsync);
            EditCourseCommand = ReactiveCommand.CreateFromTask(EditCourseAsync, canExecuteEditOrDelete);
            DeleteCourseCommand = ReactiveCommand.CreateFromTask(DeleteCourseAsync, canExecuteEditOrDelete);

            RxApp.MainThreadScheduler.Schedule(async () => await LoadCoursesAsync());
        }

        private async Task LoadCoursesAsync()
        {
            try
            {
                var coursesFromDb = await _courseService.GetCoursesAsync();

                Courses.Clear();
                foreach (var course in coursesFromDb)
                {
                    Courses.Add(course);
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading courses: {ex.Message}");
            }
        }

        private async Task AddCourseAsync()
        {
            var viewModel = new EditCourseViewModel(new Course());

            var result = await ShowDialog.Handle(viewModel).FirstAsync();
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

            var result = await ShowDialog.Handle(viewModel).FirstAsync();
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

            await LoadCoursesAsync();
        }
    }
}
