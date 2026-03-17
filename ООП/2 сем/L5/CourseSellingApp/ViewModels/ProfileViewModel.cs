using System;
using System.Reactive;
using ReactiveUI;
using CourseSellingApp.Models;
using CourseSellingApp.Services;
using Splat;

namespace CourseSellingApp.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        private readonly IUserSettingsService _userSettingsService;
        private readonly ILocalizationService _localizationService;
        private readonly IThemeService _themeService;

        private string _userName = "User";
        public string UserName
        {
            get => _userName;
            set => this.RaiseAndSetIfChanged(ref _userName, value);
        }

        private string _email = "user@example.com";
        public string Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        private string _language;
        private string _theme;

        public ReactiveCommand<string, Unit> ChangeLanguageCommand { get; }
        public ReactiveCommand<string, Unit> ChangeThemeCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveChangesCommand { get; }

        public ProfileViewModel(
            IUserSettingsService? userSettingsService = null,
            ILocalizationService? localizationService = null,
            IThemeService? themeService = null)
        {
            _userSettingsService = userSettingsService ?? Locator.Current.GetService<IUserSettingsService>()!;
            _localizationService = localizationService ?? Locator.Current.GetService<ILocalizationService>()!;
            _themeService = themeService ?? Locator.Current.GetService<IThemeService>()!;

            var settings = _userSettingsService.LoadSettings();
            _userName = settings.UserName;
            _email = settings.Email;
            _language = settings.Language;
            _theme = settings.Theme;

            ChangeLanguageCommand = ReactiveCommand.Create<string>(lang =>
            {
                _language = lang;
                _localizationService.SetLanguage(lang);
            });

            ChangeThemeCommand = ReactiveCommand.Create<string>(theme =>
            {
                _theme = theme;
                _themeService.SetTheme(theme);
            });

            SaveChangesCommand = ReactiveCommand.Create(() =>
            {
                var newSettings = new UserSettings
                {
                    UserName = this.UserName,
                    Email = this.Email,
                    Language = _language,
                    Theme = _theme
                };
                _userSettingsService.SaveSettings(newSettings);
            });
        }
    }
}
