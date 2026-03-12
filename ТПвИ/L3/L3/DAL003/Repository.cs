using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DAL003
{
    public class Repository : IRepository
    {
        public static string JSONFileName { get; set; } = "Celebrities.json";

        public string BasePath { get; }
        private Celebrity[] _celebrities;

        private Repository(string basePath)
        {
            BasePath = basePath;
            string jsonFilePath = Path.Combine(basePath, JSONFileName);

            if (File.Exists(jsonFilePath))
            {
                string jsonString = File.ReadAllText(jsonFilePath);
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                _celebrities = JsonSerializer.Deserialize<Celebrity[]>(jsonString, options) ?? Array.Empty<Celebrity>();
            }
            else
            {
                _celebrities = Array.Empty<Celebrity>();
            }
        }

        public static IRepository Create(string basePath)
        {
            return new Repository(basePath);
        }

        public Celebrity[] getAllCelebrities()
        {
            return _celebrities;
        }

        public Celebrity? getCelebrityById(int id)
        {
            return _celebrities.FirstOrDefault(c => c.id == id);
        }

        public Celebrity[] getCelebritiesBySurname(string Surname)
        {
            return _celebrities.Where(c => c.surname.Equals(Surname, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public string? getPhotoPathById(int id)
        {
            var celebrity = getCelebrityById(id);
            if (celebrity == null) return null;

            return celebrity.PhotoPath;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
