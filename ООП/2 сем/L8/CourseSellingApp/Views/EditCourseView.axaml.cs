using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using CourseSellingApp.ViewModels;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;

namespace CourseSellingApp.Views
{
    public partial class EditCourseView : ReactiveWindow<EditCourseViewModel>
    {
        public EditCourseView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(ViewModel!.SaveCommand.Subscribe(Close));
                d(ViewModel!.CancelCommand.Subscribe(Close));

                d(ViewModel!.SelectImage.RegisterHandler(DoShowOpenFileDialogAsync));

                d(ViewModel!.ShowMessage.RegisterHandler(interaction =>
                {
                    Console.WriteLine($"Message to user: {interaction.Input}");
                    interaction.SetOutput(Unit.Default);
                }));
            });
        }

        private async Task DoShowOpenFileDialogAsync(InteractionContext<Unit, string?> context)
        {
            var topLevel = GetTopLevel(this);
            if (topLevel?.StorageProvider == null)
            {
                context.SetOutput(null);
                return;
            }

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select Cover Image",
                AllowMultiple = false,
                FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
            });

            var selectedFile = files.FirstOrDefault();
            context.SetOutput(selectedFile?.TryGetLocalPath());
        }
    }
}
