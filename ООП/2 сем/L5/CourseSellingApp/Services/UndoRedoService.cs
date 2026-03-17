using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CourseSellingApp.Services
{
    public class UndoRedoService : IUndoRedoService
    {
        private readonly Stack<ICommand> _undoStack = new Stack<ICommand>();
        private readonly Stack<ICommand> _redoStack = new Stack<ICommand>();

        public event EventHandler? StateChanged;

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        public async Task ExecuteAsync(ICommand command)
        {
            await command.ExecuteAsync();
            _undoStack.Push(command);
            _redoStack.Clear();
            NotifyStateChanged();
        }

        public async Task UndoAsync()
        {
            if (_undoStack.Count > 0)
            {
                var command = _undoStack.Pop();
                await command.UndoAsync();
                _redoStack.Push(command);
                NotifyStateChanged();
            }
        }

        public async Task RedoAsync()
        {
            if (_redoStack.Count > 0)
            {
                var command = _redoStack.Pop();
                await command.ExecuteAsync();
                _undoStack.Push(command);
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged()
        {
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
