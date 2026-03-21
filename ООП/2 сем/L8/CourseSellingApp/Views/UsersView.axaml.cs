using Avalonia.Controls;
using Avalonia.ReactiveUI;
using CourseSellingApp.Models;
using CourseSellingApp.ViewModels;
using ReactiveUI;
using System.Threading.Tasks;

namespace CourseSellingApp.Views
{
    public partial class UsersView : ReactiveUserControl<UsersViewModel>
    {
        public UsersView()
        {
            InitializeComponent();

            this.WhenActivated(d => d(ViewModel!.ShowAuthorDialog.RegisterHandler(DoShowDialogAsync)));
        }

        private async Task DoShowDialogAsync(InteractionContext<EditAuthorViewModel, Author?> context)
        {
            var dialog = new EditAuthorView
            {
                DataContext = context.Input
            };

            var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
            var result = await dialog.ShowDialog<Author?>(parentWindow);
            context.SetOutput(result);
        }
    }
}
