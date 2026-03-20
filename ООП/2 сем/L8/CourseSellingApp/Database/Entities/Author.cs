#nullable enable

namespace CourseSellingApp.Database.Entities
{
    /// <summary>
    /// Сущность, представляющая таблицу Authors в базе данных.
    /// </summary>
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public byte[]? Photo { get; set; }
    }
}
