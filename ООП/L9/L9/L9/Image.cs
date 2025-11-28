using System;

namespace L9;

public class Image : IEquatable<Image>
{
    public string Name { get; set; }
    public string Resolution { get; set; }
    public int FileSizeKB { get; set; }
    public string Format { get; set; }

    public Image(string name, string resolution, int fileSizeKb, string format)
    {
        Name = name;
        Resolution = resolution;
        FileSizeKB = fileSizeKb;
        Format = format;
    }

    public void ShowInfo()
    {
        Console.WriteLine(ToString());
    }

    public override string ToString()
    {
        return $"Image '{Name}.{Format.ToLower()}' ({Resolution}, {FileSizeKB} KB)";
    }

    // Реализация IEquatable<T> для корректной работы в коллекциях,
    // требующих уникальности (например, ISet<T>).
    // Два изображения считаются одинаковыми, если у них совпадают имя и формат.
    public bool Equals(Image? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Name == other.Name && Format == other.Format;
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Image)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Format);
    }
}
