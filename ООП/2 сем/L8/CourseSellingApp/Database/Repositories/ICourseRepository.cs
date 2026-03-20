using CourseSellingApp.Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseSellingApp.Database.Repositories
{
    public interface ICourseRepository
    {
        Task<Course?> GetAsync(int id);
        Task<IEnumerable<Course>> GetAllWithDetailsAsync();
        Task AddAsync(Course course);
        Task UpdateAsync(Course course);
        Task DeleteAsync(int id);
    }
}
