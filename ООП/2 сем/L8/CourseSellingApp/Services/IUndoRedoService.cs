using System;
using System.Threading.Tasks;

namespace CourseSellingApp.Services
{
    public interface IUndoRedoService
    {
        event EventHandler? StateChanged;

        bool CanUndo { get; }
        bool CanRedo { get; }

        Task ExecuteAsync(ICommand command);
        Task UndoAsync();
        Task RedoAsync();
    }
}
