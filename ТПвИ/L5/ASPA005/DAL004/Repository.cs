using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Collections.Generic;

namespace DAL004
{
    public class Repository : IRepository
    {
        public static string JSONFileName { get; set; } = "Celebrities.json";

        public string BasePath { get; }
        private Celebrity[] _celebrities;
        private int _changes = 0;

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
            return _celebrities.Where(c => string.Equals(c.Surname, Surname, StringComparison.OrdinalIgnoreCase)).ToArray();
        }

        public string? getPhotoPathById(int id)
        {
            var celebrity = getCelebrityById(id);
            if (celebrity == null) return null;

            return celebrity.PhotoPath;
        }

        public int? addCelebrity(Celebrity celebrity)
        {
            int newId = _celebrities.Length > 0 ? _celebrities.Max(c => c.id) + 1 : 1;
            var newCelebrity = celebrity with { id = newId };

            var list = _celebrities.ToList();
            list.Add(newCelebrity);
            _celebrities = list.ToArray();

            _changes++;
            return newId;
        }

        public bool delCelebrityById(int id)
        {
            int initialCount = _celebrities.Length;
            _celebrities = _celebrities.Where(c => c.id != id).ToArray();

            if (_celebrities.Length < initialCount)
            {
                _changes++;
                return true;
            }

            return false;
        }

        public bool updCelebrityById(int id, Celebrity celebrity)
        {
            int index = Array.FindIndex(_celebrities, c => c.id == id);
            if (index == -1) return false;

            _celebrities[index] = celebrity with { id = id };
            _changes++;
            return true;
        }

        public int SaveChanges()
        {
            try
            {
                string jsonFilePath = Path.Combine(BasePath, JSONFileName);
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(_celebrities, options);
                File.WriteAllText(jsonFilePath, jsonString);

                int result = _changes > 0 ? _changes : 1;
                _changes = 0;
                return result;
            }
            catch
            {
                return 0;
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
