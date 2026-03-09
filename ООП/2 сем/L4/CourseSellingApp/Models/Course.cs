using System.Collections.Generic;

namespace CourseSellingApp.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> ImagePaths { get; set; } = new List<string>();
        public string? CoverImagePath { get; set; }
        public string Category { get; set; } = string.Empty;
        public double Rating { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public bool IsAvailable { get; set; } = true;
        public List<int> RelatedCoursesIds { get; set; } = new List<int>();
        public int PurchasesCount { get; set; }
        public string Author { get; set; } = string.Empty;
    }
}
