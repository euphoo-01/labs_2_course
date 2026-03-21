using CourseSellingApp.Models;
using ReactiveUI;
using System.Reactive;

namespace CourseSellingApp.ViewModels
{
    public class EditAuthorViewModel : ViewModelBase
    {
        public Author Author { get; }

        public ReactiveCommand<Unit, Author> SaveCommand { get; }
        public ReactiveCommand<Unit, Author?> CancelCommand { get; }

        public EditAuthorViewModel()
        {
            Author = new Author();

            var canSave = this.WhenAnyValue(
                x => x.Author.FullName,
                name => !string.IsNullOrWhiteSpace(name));

            SaveCommand = ReactiveCommand.Create(() => Author, canSave);
            CancelCommand = ReactiveCommand.Create(() => (Author?)null);
        }

        public EditAuthorViewModel(Author author)
        {
            Author = author;

            var canSave = this.WhenAnyValue(
                x => x.Author.FullName,
                name => !string.IsNullOrWhiteSpace(name));

            SaveCommand = ReactiveCommand.Create(() => Author, canSave);
            CancelCommand = ReactiveCommand.Create(() => (Author?)null);
        }
    }
}
