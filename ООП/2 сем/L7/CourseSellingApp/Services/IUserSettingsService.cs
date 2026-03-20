using CourseSellingApp.Models;

namespace CourseSellingApp.Services
{
    public interface IUserSettingsService
    {
        UserSettings LoadSettings();
        void SaveSettings(UserSettings settings);
    }
}
