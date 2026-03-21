using ReactiveUI;

namespace CourseSellingApp.ViewModels
{
    public abstract class AdminViewModelBase : ViewModelBase, IActivatableViewModel
    {
        public ViewModelActivator Activator { get; } = new();
    }
}
