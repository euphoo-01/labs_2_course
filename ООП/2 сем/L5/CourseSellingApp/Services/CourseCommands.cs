using System.Threading.Tasks;
using CourseSellingApp.Models;

namespace CourseSellingApp.Services
{
    public class AddCourseCommand : ICommand
    {
        private readonly ICourseService _service;
        private readonly Course _course;

        public AddCourseCommand(ICourseService service, Course course)
        {
            _service = service;
            _course = course;
        }

        public async Task ExecuteAsync() => await _service.AddCourseAsync(_course);

        public async Task UndoAsync() => await _service.DeleteCourseAsync(_course.Id);
    }

    public class DeleteCourseCommand : ICommand
    {
        private readonly ICourseService _service;
        private readonly Course _course;

        public DeleteCourseCommand(ICourseService service, Course course)
        {
            _service = service;
            _course = course;
        }

        public async Task ExecuteAsync() => await _service.DeleteCourseAsync(_course.Id);

        public async Task UndoAsync() => await _service.AddCourseAsync(_course);
    }

    public class EditCourseCommand : ICommand
    {
        private readonly ICourseService _service;
        private readonly Course _oldCourse;
        private readonly Course _newCourse;

        public EditCourseCommand(ICourseService service, Course oldCourse, Course newCourse)
        {
            _service = service;
            _oldCourse = oldCourse;
            _newCourse = newCourse;
        }

        public async Task ExecuteAsync() => await _service.UpdateCourseAsync(_newCourse);

        public async Task UndoAsync() => await _service.UpdateCourseAsync(_oldCourse);
    }
}
