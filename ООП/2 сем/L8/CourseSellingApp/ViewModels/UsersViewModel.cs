using CourseSellingApp.Models;
using CourseSellingApp.Services;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CourseSellingApp.ViewModels
{
    public class UsersViewModel : AdminViewModelBase
    {
        private readonly AuthorService _authorService;

        private ObservableCollection<Author> _authors;
        public ObservableCollection<Author> Authors
        {
            get => _authors;
            set => this.RaiseAndSetIfChanged(ref _authors, value);
        }

        private Author? _selectedAuthor;
        public Author? SelectedAuthor
        {
            get => _selectedAuthor;
            set => this.RaiseAndSetIfChanged(ref _selectedAuthor, value);
        }

        public System.Windows.Input.ICommand AddAuthorCommand { get; }
        public System.Windows.Input.ICommand EditAuthorCommand { get; }
        public System.Windows.Input.ICommand DeleteAuthorCommand { get; }
        public ReactiveCommand<Unit, Unit> LoadAuthorsCommand { get; }

        public Interaction<EditAuthorViewModel, Author?> ShowAuthorDialog { get; }

        public UsersViewModel()
        {
            _authorService = new AuthorService();
            _authors = new ObservableCollection<Author>();
            ShowAuthorDialog = new Interaction<EditAuthorViewModel, Author?>();

            var canExecuteEditOrDelete = this.WhenAnyValue(x => x.SelectedAuthor)
                .Select(a => a != null);

            AddAuthorCommand = ReactiveCommand.CreateFromTask(AddAuthorAsync);
            EditAuthorCommand = ReactiveCommand.CreateFromTask(EditAuthorAsync, canExecuteEditOrDelete);
            DeleteAuthorCommand = ReactiveCommand.CreateFromTask(DeleteAuthorAsync, canExecuteEditOrDelete);

            LoadAuthorsCommand = ReactiveCommand.CreateFromTask(LoadAuthorsAsync);

            RxApp.MainThreadScheduler.Schedule(async () => await LoadAuthorsAsync());
        }

        private async Task LoadAuthorsAsync()
        {
            var authorsFromDb = await _authorService.GetAuthorsAsync();
            Authors.Clear();
            foreach (var author in authorsFromDb)
            {
                Authors.Add(author);
            }
        }

        private async Task AddAuthorAsync()
        {
            var viewModel = new EditAuthorViewModel(new Author());

            var result = await ShowAuthorDialog.Handle(viewModel).FirstAsync();
            if (result != null)
            {
                await _authorService.AddAuthorAsync(result);
                await LoadAuthorsAsync();
            }
        }

        private async Task EditAuthorAsync()
        {
            if (SelectedAuthor == null) return;

            var authorToEdit = new Author
            {
                Id = SelectedAuthor.Id,
                FullName = SelectedAuthor.FullName,
                Biography = SelectedAuthor.Biography,
                Photo = SelectedAuthor.Photo
            };

            var viewModel = new EditAuthorViewModel(authorToEdit);

            var result = await ShowAuthorDialog.Handle(viewModel).FirstAsync();
            if (result != null)
            {
                await _authorService.UpdateAuthorAsync(result);
                await LoadAuthorsAsync();
            }
        }

        private async Task DeleteAuthorAsync()
        {
            if (SelectedAuthor == null) return;

            await _authorService.DeleteAuthorAsync(SelectedAuthor.Id);
            Authors.Remove(SelectedAuthor);
        }
    }
}
