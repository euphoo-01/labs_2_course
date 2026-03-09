using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using System;
using System.Linq;

namespace CourseSellingApp.Services
{
    public static class LocalizationManager
    {
        public static void SetLanguage(string languageCode)
        {
            if (Application.Current?.Resources.MergedDictionaries is null)
            {
                return;
            }

            var dictionaries = Application.Current.Resources.MergedDictionaries;

            var oldDictionary = dictionaries
                .OfType<ResourceInclude>()
                .FirstOrDefault(d => d.Source?.OriginalString.Contains("Resources/Strings/") ?? false);

            if (oldDictionary != null)
            {
                dictionaries.Remove(oldDictionary);
            }

            var newDictionary = new ResourceInclude(new Uri($"avares://CourseSellingApp/Resources/Strings/Strings.{languageCode}.axaml"));

            dictionaries.Add(newDictionary);
        }
    }
}
