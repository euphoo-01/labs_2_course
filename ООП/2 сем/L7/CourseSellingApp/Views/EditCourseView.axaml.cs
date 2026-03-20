using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using Avalonia.ReactiveUI;
using CourseSellingApp.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;
namespace CourseSellingApp.Views
{
    public partial class EditCourseView : ReactiveWindow<EditCourseViewModel>
    {
        public EditCourseView()
        {
            InitializeComponent();
            this.WhenActivated(d =>
            {
                if (ViewModel != null)
                {
                    ViewModel.SaveCommand.Subscribe(Close).DisposeWith(d);
                    ViewModel.CancelCommand.Subscribe(_ => Close()).DisposeWith(d);

                    var dropZone = this.FindControl<Border>("DropZone");
                    if (dropZone != null)
                    {
                        dropZone.AddHandler(DragDrop.DragOverEvent, (s, e) =>
                        {
                            ViewModel.OnDragOver(s, e);
                            e.Handled = true;
                        });

                        dropZone.AddHandler(DragDrop.DropEvent, (s, e) =>
                        {
                            ViewModel.OnDrop(s, e);
                            e.Handled = true;
                        });
                    }

                    ViewModel.SelectImage.RegisterHandler(async interaction =>
                    {
                        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
                        {
                            Title = "Select Cover Image",
                            AllowMultiple = false,
                            FileTypeFilter = new[]
                            {
                                new FilePickerFileType("Images")
                                {
                                    Patterns = new[] { "*.png", "*.jpg", "*.jpeg" }
                                }
                            }
                        });

                        if (files.Count > 0 && files[0].TryGetLocalPath() is { } path)
                        {
                            interaction.SetOutput(path);
                        }
                        else
                        {
                            interaction.SetOutput(null);
                        }
                    }).DisposeWith(d);

                    ViewModel.ShowMessage.RegisterHandler(interaction =>
                    {
                        interaction.SetOutput(Unit.Default);
                    }).DisposeWith(d);
                }
            });
        }
    }
}
