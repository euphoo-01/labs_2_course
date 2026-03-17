using Avalonia;
using Avalonia.Markup.Xaml.Styling;
using System;
using System.Linq;

namespace CourseSellingApp.Services
{
    public class ThemeService : IThemeService
    {
        public void SetTheme(string themeName)
        {
            if (Application.Current?.Resources.MergedDictionaries is null)
            {
                return;
            }

            var dictionaries = Application.Current.Resources.MergedDictionaries;

            var oldDictionary = dictionaries
                .OfType<ResourceInclude>()
                .FirstOrDefault(d => d.Source?.OriginalString.Contains("Resources/Themes/") ?? false);

            if (oldDictionary != null)
            {
                dictionaries.Remove(oldDictionary);
            }

            var newDictionary = new ResourceInclude(new Uri("avares://CourseSellingApp/App.axaml"))
            {
                Source = new Uri($"avares://CourseSellingApp/Resources/Themes/{themeName}.axaml")
            };

            dictionaries.Add(newDictionary);
        }
    }
}
