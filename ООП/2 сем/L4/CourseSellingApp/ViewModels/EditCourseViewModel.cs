using Avalonia.Input;
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

        public ReactiveCommand<Unit, Course> SaveCommand { get; }
        public ReactiveCommand<Unit, Course?> CancelCommand { get; }
        public ReactiveCommand<Unit, Unit> SelectImageCommand { get; }
        public Interaction<string, Unit> ShowMessage { get; }
        public Interaction<Unit, string?> SelectImage { get; }

        public EditCourseViewModel()
        {
            Course = new Course();
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

        public void OnDragOver(object? sender, DragEventArgs e)
        {
            e.DragEffects = DragDropEffects.None;

#pragma warning disable CS0618
            if (e.Data.GetFiles()?.Any() == true)
#pragma warning restore CS0618
            {
                e.DragEffects = DragDropEffects.Copy;
            }
        }

        public async void OnDrop(object? sender, DragEventArgs e)
        {
#pragma warning disable CS0618
            var files = e.Data.GetFiles();
#pragma warning restore CS0618
            if (files?.FirstOrDefault()?.TryGetLocalPath() is { } filePath)
            {
                await ProcessImageFileAsync(filePath);
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
