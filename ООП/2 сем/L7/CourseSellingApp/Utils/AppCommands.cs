using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CourseSellingApp.Views;

namespace CourseSellingApp.Utils
{
    public class CustomCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public string Name { get; }
        public string Text { get; }

        public CustomCommand(string name, string text, Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            Name = name;
            Text = text;
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute(parameter);

        public void Execute(object? parameter) => _execute(parameter);

        public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }

    public static class AppCommands
    {
        public static CustomCommand ShowHelpCommand { get; } = new CustomCommand(
            "ShowHelp",
            "Show Help",
            execute: _ => ShowHelpMessage()
        );

        private static async void ShowHelpMessage()
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
                desktop.MainWindow != null)
            {
                var helpWindow = new HelpWindow();
                await helpWindow.ShowDialog(desktop.MainWindow);
            }
        }
    }
}
