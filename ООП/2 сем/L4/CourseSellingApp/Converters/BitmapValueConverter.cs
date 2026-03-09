using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;
using System.IO;

namespace CourseSellingApp.Converters
{
    public class BitmapValueConverter : IValueConverter
    {
        public static BitmapValueConverter Instance { get; } = new BitmapValueConverter();

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                // Ensure the path is absolute. Relative paths might be from the execution directory.
                var absolutePath = Path.IsPathRooted(path) ? path : Path.Combine(AppContext.BaseDirectory, path);

                if (File.Exists(absolutePath))
                {
                    try
                    {
                        // Use a FileStream to avoid locking the image file.
                        using (var stream = new FileStream(absolutePath, FileMode.Open, FileAccess.Read))
                        {
                            return new Bitmap(stream);
                        }
                    }
                    catch (Exception)
                    {
                        // If the file is not a valid image or is corrupted, return null.
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
