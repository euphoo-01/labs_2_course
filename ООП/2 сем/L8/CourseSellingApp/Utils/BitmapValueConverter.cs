using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using System.IO;

namespace CourseSellingApp.Utils
{
    public class BitmapValueConverter : IValueConverter
    {
        public static BitmapValueConverter Instance { get; } = new BitmapValueConverter();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                var absolutePath = Path.IsPathRooted(path) ? path : Path.Combine(AppContext.BaseDirectory, path);

                if (File.Exists(absolutePath))
                {
                    try
                    {
                        using (var stream = new FileStream(absolutePath, FileMode.Open, FileAccess.Read))
                        {
                            return new Bitmap(stream);
                        }
                    }
                    catch (Exception)
                    {
                        return null;
                    }
                }
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
