using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CourseSellingApp.Models;
using Newtonsoft.Json;

namespace CourseSellingApp.Services
{
    public class CourseService : ICourseService
    {
        private readonly string _filePath = "courses.json";
        private List<Course>? _courses;

        public async Task<IEnumerable<Course>> GetCoursesAsync()
        {
            if (_courses == null)
            {
                await LoadCoursesFromFileAsync();
            }
            return _courses!;
        }

        public async Task AddCourseAsync(Course course)
        {
            if (_courses == null)
            {
                await LoadCoursesFromFileAsync();
            }

            var courses = _courses!;
            course.Id = courses.Any() ? courses.Max(c => c.Id) + 1 : 1;
            courses.Add(course);
            await SaveChangesToFileAsync();
        }

        public async Task UpdateCourseAsync(Course course)
        {
            if (_courses == null)
            {
                await LoadCoursesFromFileAsync();
            }

            var courses = _courses!;
            var existingCourse = courses.FirstOrDefault(c => c.Id == course.Id);
            if (existingCourse != null)
            {
                existingCourse.Name = course.Name;
                existingCourse.FullName = course.FullName;
                existingCourse.Description = course.Description;
                existingCourse.ImagePaths = course.ImagePaths;
                existingCourse.Category = course.Category;
                existingCourse.Rating = course.Rating;
                existingCourse.Price = course.Price;
                existingCourse.Quantity = course.Quantity;
                existingCourse.Discount = course.Discount;
                existingCourse.IsAvailable = course.IsAvailable;
                existingCourse.RelatedCoursesIds = course.RelatedCoursesIds;
                existingCourse.PurchasesCount = course.PurchasesCount;
                existingCourse.Author = course.Author;
            }
            await SaveChangesToFileAsync();
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            if (_courses == null)
            {
                await LoadCoursesFromFileAsync();
            }

            var courses = _courses!;
            var courseToRemove = courses.FirstOrDefault(c => c.Id == courseId);
            if (courseToRemove != null)
            {
                courses.Remove(courseToRemove);
                await SaveChangesToFileAsync();
            }
        }

        private async Task LoadCoursesFromFileAsync()
        {
            if (!File.Exists(_filePath))
            {
                _courses = GetDefaultCourses();
                await SaveChangesToFileAsync();
                return;
            }

            var json = await File.ReadAllTextAsync(_filePath);
            _courses = JsonConvert.DeserializeObject<List<Course>>(json) ?? new List<Course>();
        }

        private async Task SaveChangesToFileAsync()
        {
            if (_courses is null)
            {
                return;
            }
            var json = JsonConvert.SerializeObject(_courses, Formatting.Indented);
            await File.WriteAllTextAsync(_filePath, json);
        }

        private List<Course> GetDefaultCourses()
        {
            return new List<Course>
            {
                new Course
                {
                    Id = 1,
                    Name = "C# Advanced",
                    FullName = "Advanced C# Programming Course",
                    Description = "Deep dive into advanced C# concepts like asynchronous programming, LINQ, and more.",
                    ImagePaths = new List<string> { "/Assets/placeholder.png" },
                    Category = "Programming",
                    Rating = 4.8,
                    Price = 199.99m,
                    Quantity = 50,
                    Discount = 10,
                    Author = "John Doe"
                },
                new Course
                {
                    Id = 2,
                    Name = "Intro to Math",
                    FullName = "Introduction to University Mathematics",
                    Description = "Covering the fundamentals of calculus, algebra, and discrete mathematics.",
                    ImagePaths = new List<string> { "/Assets/placeholder.png" },
                    Category = "Mathematics",
                    Rating = 4.5,
                    Price = 99.99m,
                    Quantity = 100,
                    IsAvailable = true,
                    Author = "Jane Smith"
                },
                new Course
                {
                    Id = 3,
                    Name = "Avalonia UI",
                    FullName = "Building Cross-Platform Apps with Avalonia UI",
                    Description = "Learn how to build beautiful, native UIs for Windows, macOS, Linux, iOS, Android, and WebAssembly from a single C# codebase.",
                    ImagePaths = new List<string> { "/Assets/placeholder.png" },
                    Category = "Programming",
                    Rating = 4.9,
                    Price = 249.99m,
                    Quantity = 30,
                    Discount = 15,
                    Author = "Avalonia Team"
                }
            };
        }
    }
}
