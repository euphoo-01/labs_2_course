#nullable enable

namespace CourseSellingApp.Database.Entities
{
    public class Author
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public byte[]? Photo { get; set; }
    }
}
