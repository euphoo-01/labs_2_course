using System.Reactive;
using Avalonia.ReactiveUI;
using CourseSellingApp.Models;
using CourseSellingApp.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace CourseSellingApp.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
            this.WhenActivated(disposables =>
            {
                if (ViewModel != null)
                {
                    ViewModel.ShowDialog.RegisterHandler(async interaction =>
                        await DoShowDialogAsync(interaction))
                        .DisposeWith(disposables);

                    ViewModel.ShowProfileDialog.RegisterHandler(async interaction =>
                        await DoShowProfileDialogAsync(interaction))
                        .DisposeWith(disposables);
                }
            });
        }

        private async Task DoShowDialogAsync(IInteractionContext<EditCourseViewModel, Course?> interaction)
        {
            var dialog = new EditCourseView
            {
                DataContext = interaction.Input
            };

            var result = await dialog.ShowDialog<Course?>(this);
            interaction.SetOutput(result);
        }

        private async Task DoShowProfileDialogAsync(IInteractionContext<ProfileViewModel, Unit> interaction)
        {
            var dialog = new ProfileView
            {
                DataContext = interaction.Input
            };

            await dialog.ShowDialog(this);
            interaction.SetOutput(Unit.Default);
        }
    }
}
