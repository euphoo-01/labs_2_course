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
                d(ViewModel!.SaveCommand.Subscribe(Close));
                d(ViewModel!.CancelCommand.Subscribe(Close));
            });
        }
    }
}
