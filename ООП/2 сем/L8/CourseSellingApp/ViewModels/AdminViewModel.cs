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

        private readonly AdminHomeViewModel _adminHomeViewModel;
        private readonly UsersViewModel _usersViewModel;
        private readonly CoursesViewModel _coursesViewModel;

        public ICommand NavigateHomeCommand { get; }
        public ICommand NavigateUsersCommand { get; }
        public ICommand NavigateCoursesCommand { get; }
        public ICommand BackCommand { get; }
        public Interaction<Unit, Unit> BackInteraction { get; }

        public AdminViewModel()
        {
            _adminHomeViewModel = new AdminHomeViewModel();
            _usersViewModel = new UsersViewModel();
            _coursesViewModel = new CoursesViewModel();

            _currentView = _adminHomeViewModel;

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
