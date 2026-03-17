using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CourseSellingApp.Services;
using CourseSellingApp.ViewModels;
using CourseSellingApp.Views;
using Splat;

namespace CourseSellingApp;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
        RegisterDependencies();
    }

    private void RegisterDependencies()
    {
        Locator.CurrentMutable.RegisterConstant<IUserSettingsService>(new UserSettingsService());
        Locator.CurrentMutable.RegisterConstant<IThemeService>(new ThemeService());
        Locator.CurrentMutable.RegisterConstant<ILocalizationService>(new LocalizationService());
        Locator.CurrentMutable.RegisterConstant<IUserService>(new UserService());
        Locator.CurrentMutable.RegisterConstant<IUndoRedoService>(new UndoRedoService());
        Locator.CurrentMutable.RegisterConstant<ICourseService>(new CourseService());
    }

    public override void OnFrameworkInitializationCompleted()
    {
        var userSettingsService = Locator.Current.GetService<IUserSettingsService>();
        var localizationService = Locator.Current.GetService<ILocalizationService>();
        var themeService = Locator.Current.GetService<IThemeService>();

        var settings = userSettingsService?.LoadSettings() ?? new Models.UserSettings();
        localizationService?.SetLanguage(settings.Language);
        themeService?.SetTheme(settings.Theme);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}
