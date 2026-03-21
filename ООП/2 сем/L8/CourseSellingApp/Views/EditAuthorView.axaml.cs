using Avalonia.ReactiveUI;
using CourseSellingApp.ViewModels;
using ReactiveUI;
using System;

namespace CourseSellingApp.Views
{
    public partial class EditAuthorView : ReactiveWindow<EditAuthorViewModel>
    {
        public EditAuthorView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                // When the Save or Cancel commands are executed, close the window
                // and return the command's result (the Author object or null).
                d(ViewModel!.SaveCommand.Subscribe(Close));
                d(ViewModel!.CancelCommand.Subscribe(Close));
            });
        }
    }
}
