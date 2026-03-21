using System.Reactive;
using Avalonia.ReactiveUI;
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
                    ViewModel.ShowProfileDialog.RegisterHandler(DoShowProfileDialogAsync)
                        .DisposeWith(disposables);
                }
            });
        }

        private async Task DoShowProfileDialogAsync(InteractionContext<ProfileViewModel, Unit> interaction)
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
