using Avalonia.Platform.Storage;
using CourseSellingApp.Models;
using ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace CourseSellingApp.ViewModels
{
    public class EditCourseViewModel : ViewModelBase
    {
        public Course Course { get; }
        public System.Collections.ObjectModel.ObservableCollection<string> AvailableAuthors { get; }

        public ReactiveCommand<Unit, Course> SaveCommand { get; }
        public ReactiveCommand<Unit, Course?> CancelCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectImageCommand { get; }
        public Interaction<string, Unit> ShowMessage { get; }
        public Interaction<Unit, string?> SelectImage { get; }

        public EditCourseViewModel()
        {
            Course = new Course();
            AvailableAuthors = new System.Collections.ObjectModel.ObservableCollection<string>();
            _ = LoadAuthorsAsync();
            ShowMessage = new Interaction<string, Unit>();
            SelectImage = new Interaction<Unit, string?>();

            SelectImageCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var filePath = await SelectImage.Handle(Unit.Default);
                if (!string.IsNullOrEmpty(filePath))
                {
                    await ProcessImageFileAsync(filePath);
                }
            });

            var canSave = this.WhenAnyValue(
                x => x.Course.Name,
                x => x.Course.Author,
                (name, author) => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(author));
            SaveCommand = ReactiveCommand.Create(() => Course, canSave);
            CancelCommand = ReactiveCommand.Create(() => (Course?)null);
        }

        public EditCourseViewModel(Course course)
        {
            Course = course;
            AvailableAuthors = new System.Collections.ObjectModel.ObservableCollection<string>();
            _ = LoadAuthorsAsync();
            ShowMessage = new Interaction<string, Unit>();
            SelectImage = new Interaction<Unit, string?>();

            SelectImageCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var filePath = await SelectImage.Handle(Unit.Default);
                if (!string.IsNullOrEmpty(filePath))
                {
                    await ProcessImageFileAsync(filePath);
                }
            });

            var canSave = this.WhenAnyValue(
                x => x.Course.Name,
                x => x.Course.Author,
                (name, author) => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(author));
            SaveCommand = ReactiveCommand.Create(() => Course, canSave);
            CancelCommand = ReactiveCommand.Create(() => (Course?)null);
        }

        private async Task LoadAuthorsAsync()
        {
            var authorService = new CourseSellingApp.Services.AuthorService();
            var authors = await authorService.GetAuthorsAsync();
            AvailableAuthors.Clear();
            foreach (var author in authors)
            {
                AvailableAuthors.Add(author.FullName);
            }
        }

        private async Task ProcessImageFileAsync(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
            {
                await ShowMessage.Handle("Please select a .png, .jpg, or .jpeg file.");
                return;
            }

            try
            {
                var assetsDir = Path.Combine(AppContext.BaseDirectory, "Assets", "Covers");
                if (!Directory.Exists(assetsDir))
                {
                    Directory.CreateDirectory(assetsDir);
                }

                var newFileName = $"{Guid.NewGuid()}{extension}";
                var newPath = Path.Combine(assetsDir, newFileName);
                File.Copy(filePath, newPath, true);

                Course.CoverImagePath = newPath;
            }
            catch (Exception ex)
            {
                await ShowMessage.Handle($"Error copying file: {ex.Message}");
            }
        }
    }
}
