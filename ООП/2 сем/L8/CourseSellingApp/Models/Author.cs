using ReactiveUI;

namespace CourseSellingApp.Models
{
    public class Author : ReactiveObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set => this.RaiseAndSetIfChanged(ref _id, value);
        }

        private string _fullName = string.Empty;
        public string FullName
        {
            get => _fullName;
            set
            {
                this.RaiseAndSetIfChanged(ref _fullName, value);
                this.RaisePropertyChanged(nameof(Name));
            }
        }

        private string? _biography;
        public string? Biography
        {
            get => _biography;
            set => this.RaiseAndSetIfChanged(ref _biography, value);
        }

        private byte[]? _photo;
        public byte[]? Photo
        {
            get => _photo;
            set => this.RaiseAndSetIfChanged(ref _photo, value);
        }

        public string Name => FullName;

        public override string ToString()
        {
            return FullName;
        }
    }
}
