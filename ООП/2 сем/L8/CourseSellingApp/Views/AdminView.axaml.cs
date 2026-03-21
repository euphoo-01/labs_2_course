using Avalonia.ReactiveUI;
using CourseSellingApp.ViewModels;
using ReactiveUI;
using System.Reactive;

namespace CourseSellingApp.Views
{
    public partial class AdminView : ReactiveUserControl<AdminViewModel>
    {
        public AdminView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                // Interactions handled by parent ViewModels should not be handled here.
            });
        }
    }
}
