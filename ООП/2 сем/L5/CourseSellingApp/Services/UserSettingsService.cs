using System;
using System.IO;
using Newtonsoft.Json;
using CourseSellingApp.Models;

namespace CourseSellingApp.Services
{
    public class UserSettingsService : IUserSettingsService
    {
        private readonly string SettingsFilePath = "user.json";

        public UserSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    var json = File.ReadAllText(SettingsFilePath);
                    return JsonConvert.DeserializeObject<UserSettings>(json) ?? new UserSettings();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading settings: {ex.Message}");
            }
            return new UserSettings();
        }

        public void SaveSettings(UserSettings settings)
        {
            try
            {
                var json = JsonConvert.SerializeObject(settings, Formatting.Indented);
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving settings: {ex.Message}");
            }
        }
    }
}
