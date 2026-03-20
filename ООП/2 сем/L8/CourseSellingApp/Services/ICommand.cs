using System.Threading.Tasks;

namespace CourseSellingApp.Services
{
    public interface ICommand
    {
        Task ExecuteAsync();
        Task UndoAsync();
    }
}
