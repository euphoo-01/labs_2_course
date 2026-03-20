using System.Collections.Generic;
using Newtonsoft.Json;
using ReactiveUI;

namespace CourseSellingApp.Models
{
    [JsonObject(MemberSerialization.OptOut)]
    public class Course : ReactiveObject
    {
        private int _id;
        public int Id { get => _id; set => this.RaiseAndSetIfChanged(ref _id, value); }

        private string _name = string.Empty;
        public string Name { get => _name; set => this.RaiseAndSetIfChanged(ref _name, value); }

        private string _fullName = string.Empty;
        public string FullName { get => _fullName; set => this.RaiseAndSetIfChanged(ref _fullName, value); }

        private string _description = string.Empty;
        public string Description { get => _description; set => this.RaiseAndSetIfChanged(ref _description, value); }

        private List<string> _imagePaths = new List<string>();
        public List<string> ImagePaths { get => _imagePaths; set => this.RaiseAndSetIfChanged(ref _imagePaths, value); }

        private string? _coverImagePath;
        public string? CoverImagePath { get => _coverImagePath; set => this.RaiseAndSetIfChanged(ref _coverImagePath, value); }

        private string _category = string.Empty;
        public string Category { get => _category; set => this.RaiseAndSetIfChanged(ref _category, value); }

        private double _rating;
        public double Rating { get => _rating; set => this.RaiseAndSetIfChanged(ref _rating, value); }

        private decimal _price;
        public decimal Price { get => _price; set => this.RaiseAndSetIfChanged(ref _price, value); }

        private int _quantity;
        public int Quantity { get => _quantity; set => this.RaiseAndSetIfChanged(ref _quantity, value); }

        private double _discount;
        public double Discount { get => _discount; set => this.RaiseAndSetIfChanged(ref _discount, value); }

        private bool _isAvailable = true;
        public bool IsAvailable { get => _isAvailable; set => this.RaiseAndSetIfChanged(ref _isAvailable, value); }

        private List<int> _relatedCoursesIds = new List<int>();
        public List<int> RelatedCoursesIds { get => _relatedCoursesIds; set => this.RaiseAndSetIfChanged(ref _relatedCoursesIds, value); }

        private int _purchasesCount;
        public int PurchasesCount { get => _purchasesCount; set => this.RaiseAndSetIfChanged(ref _purchasesCount, value); }

        private string _author = string.Empty;
        public string Author { get => _author; set => this.RaiseAndSetIfChanged(ref _author, value); }
    }
}
