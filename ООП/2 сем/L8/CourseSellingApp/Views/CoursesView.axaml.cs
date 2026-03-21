using Avalonia.Controls;
using Avalonia.ReactiveUI;
using CourseSellingApp.Models;
using CourseSellingApp.ViewModels;
using ReactiveUI;
using System.Threading.Tasks;

namespace CourseSellingApp.Views
{
    public partial class CoursesView : ReactiveUserControl<CoursesViewModel>
    {
        public CoursesView()
        {
            InitializeComponent();

            this.WhenActivated(d => d(ViewModel!.ShowDialog.RegisterHandler(DoShowDialogAsync)));
        }

        private async Task DoShowDialogAsync(InteractionContext<EditCourseViewModel, Course?> context)
        {
            var dialog = new EditCourseView
            {
                DataContext = context.Input
            };

            var parentWindow = (Window)TopLevel.GetTopLevel(this)!;
            var result = await dialog.ShowDialog<Course?>(parentWindow);
            context.SetOutput(result);
        }
    }
}
