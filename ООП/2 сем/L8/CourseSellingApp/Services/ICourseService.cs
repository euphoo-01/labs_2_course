using System.Collections.Generic;
using System.Threading.Tasks;
using CourseSellingApp.Models;

namespace CourseSellingApp.Services
{
    public interface ICourseService
    {
        Task<IEnumerable<Course>> GetCoursesAsync();
        Task AddCourseAsync(Course course);
        Task UpdateCourseAsync(Course course);
        Task DeleteCourseAsync(int courseId);
    }
}
