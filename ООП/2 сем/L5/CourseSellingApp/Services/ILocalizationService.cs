using System;

namespace CourseSellingApp.Services
{
    public interface ILocalizationService
    {
        event EventHandler? LanguageChanged;
        void SetLanguage(string languageCode);
    }
}
