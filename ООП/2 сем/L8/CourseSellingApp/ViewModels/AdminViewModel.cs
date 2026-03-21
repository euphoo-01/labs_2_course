using ReactiveUI;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;

namespace CourseSellingApp.ViewModels
{
    public class AdminViewModel : ViewModelBase
    {
        private ViewModelBase _currentView;

        public ViewModelBase CurrentView
        {
            get => _currentView;
            set => this.RaiseAndSetIfChanged(ref _currentView, value);
        }

        // Child ViewModels
        private readonly AdminHomeViewModel _adminHomeViewModel;
        private readonly UsersViewModel _usersViewModel;
        private readonly CoursesViewModel _coursesViewModel;

        // Navigation Commands
        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateUsersCommand { get; }
        public ICommand NavigateCoursesCommand { get; }
        public ICommand BackCommand { get; }
        public Interaction<Unit, Unit> BackInteraction { get; }

        public AdminViewModel()
        {
            // Instantiate the child view models
            _adminHomeViewModel = new AdminHomeViewModel();
            _usersViewModel = new UsersViewModel();
            _coursesViewModel = new CoursesViewModel();

            // Set the default view
            _currentView = _adminHomeViewModel;

            // Create the navigation commands
            NavigateHomeCommand = ReactiveCommand.Create(NavigateToHome);
            NavigateUsersCommand = ReactiveCommand.Create(NavigateToUsers);
            NavigateCoursesCommand = ReactiveCommand.Create(NavigateToCourses);

            BackInteraction = new Interaction<Unit, Unit>();
            BackCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                await BackInteraction.Handle(Unit.Default).FirstAsync();
            });
        }

        private void NavigateToHome()
        {
            CurrentView = _adminHomeViewModel;
        }

        private void NavigateToUsers()
        {
            CurrentView = _usersViewModel;
        }

        private void NavigateToCourses()
        {
            CurrentView = _coursesViewModel;
        }
    }
}
