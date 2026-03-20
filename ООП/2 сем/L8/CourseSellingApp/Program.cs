using Avalonia;
using Avalonia.ReactiveUI;
using System;
using CourseSellingApp.Database.Services;

namespace CourseSellingApp;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        if (!TryInitializeDatabase())
        {
            // The error is logged in the TryInitializeDatabase method.
            // The application will exit if initialization fails.
            return;
        }

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();

    private static bool TryInitializeDatabase()
    {
        try
        {
            // This must run synchronously before the UI starts, so blocking is intentional and safe here.
            new DatabaseService().InitializeDatabaseAsync().GetAwaiter().GetResult();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("FATAL: Database initialization failed. The application will not start.");
            Console.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }
}
