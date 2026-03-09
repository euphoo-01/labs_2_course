using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.ReactiveUI;
using CourseSellingApp.ViewModels;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using MessageBox.Avalonia;
using MessageBox.Avalonia.Enums;

using Avalonia.Me
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
                        // Using lambdas to connect the events to the ViewModel's methods
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

                    // Register a handler for the ShowMessage interaction
                    ViewModel.ShowMessage.RegisterHandler(interaction =>
                    {
                        var messageBoxStandardWindow = MessageBoxManager
                            .GetMessageBoxStandardWindow("Invalid File", interaction.Input, ButtonEnum.Ok, Icon.Warning);
                        messageBoxStandardWindow.ShowDialog(this);
                        interaction.SetOutput(Unit.Default);
                    }).DisposeWith(d);
                }
            });
        }
    }
}
