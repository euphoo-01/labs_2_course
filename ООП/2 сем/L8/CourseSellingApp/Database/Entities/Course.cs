#nullable enable
using System;

namespace CourseSellingApp.Database.Entities
{
    /// <summary>
    /// Сущность, представляющая таблицу Courses в базе данных.
    /// </summary>
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? FullName { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public bool IsAvailable { get; set; }
        public int PurchasesCount { get; set; }
        public double Rating { get; set; }
        public byte[]? CoverImage { get; set; }
        public string? CoverImagePath { get; set; }
        public string? ImagePaths { get; set; } // JSON string
        public string? RelatedCoursesIds { get; set; } // JSON string
        public int CategoryId { get; set; }
        public int? AuthorId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
