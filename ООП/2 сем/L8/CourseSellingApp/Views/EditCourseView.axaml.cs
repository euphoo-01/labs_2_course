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
                // When the Save or Cancel commands are executed, close the window
                // and return the command's result (the Course object or null).
                d(ViewModel!.SaveCommand.Subscribe(Close));
                d(ViewModel!.CancelCommand.Subscribe(Close));

                // Register the handler for the SelectImage interaction.
                d(ViewModel!.SelectImage.RegisterHandler(DoShowOpenFileDialogAsync));

                // Register a handler for showing messages.
                // In a real application, you'd use a proper dialog service.
                d(ViewModel!.ShowMessage.RegisterHandler(interaction =>
                {
                    // For this example, we'll just write to the console.
                    // A proper implementation would show a message box to the user.
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
                // This can happen if the window is not yet shown.
                // A robust solution might wait or disable the button until the provider is ready.
                context.SetOutput(null);
                return;
            }

            // Start the file picker
            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Select Cover Image",
                AllowMultiple = false,
                FileTypeFilter = new[] { FilePickerFileTypes.ImageAll }
            });

            // If the user selected a file, return its path. Otherwise, return null.
            var selectedFile = files.FirstOrDefault();
            context.SetOutput(selectedFile?.TryGetLocalPath());
        }
    }
}
