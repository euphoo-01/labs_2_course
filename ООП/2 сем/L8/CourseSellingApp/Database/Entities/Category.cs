#nullable enable

namespace CourseSellingApp.Database.Entities
{
    /// <summary>
    /// Сущность, представляющая таблицу Categories в базе данных.
    /// </summary>
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
