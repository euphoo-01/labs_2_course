using Avalonia.Input;
using CourseSellingApp.Models;
using ReactiveUI;
using System;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

namespace CourseSellingApp.ViewModels
{
    public class EditCourseViewModel : ViewModelBase
    {
        public Course Course { get; }

        public ReactiveCommand<Unit, Course> SaveCommand { get; }
        public ReactiveCommand<Unit, Course?> CancelCommand { get; }
        public Interaction<string, Unit> ShowMessage { get; }

        // This constructor is used by the designer
        public EditCourseViewModel()
        {
            Course = new Course();
            ShowMessage = new Interaction<string, Unit>();

            var canSave = this.WhenAnyValue(
                x => x.Course.Name,
                x => x.Course.Author,
                (name, author) => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(author));
            SaveCommand = ReactiveCommand.Create(() => Course, canSave);
            CancelCommand = ReactiveCommand.Create(() => (Course?)null);
        }

        // This constructor is used at runtime
        public EditCourseViewModel(Course course)
        {
            Course = course;
            ShowMessage = new Interaction<string, Unit>();

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

            if (e.Data.GetFileNames()?.Any() == true)
            {
                e.DragEffects = DragDropEffects.Copy;
            }
        }

        public async void OnDrop(object? sender, DragEventArgs e)
        {
            if (e.Data.GetFileNames()?.FirstOrDefault() is { } filePath)
            {
                var extension = Path.GetExtension(filePath).ToLowerInvariant();
                if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
                {
                    await ShowMessage.Handle("Please drop a .png, .jpg, or .jpeg file.");
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
}
