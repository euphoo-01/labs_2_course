using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

namespace L3
{
    
    public class Production : ICloneable
    {
        public Guid Id { get; set; }
        public string Org { get; set; }

        
        public Production()
        {
            this.Id = Guid.NewGuid();
            this.Org = "unknown";
        }

        public Production(string org)
        {
            this.Id = Guid.NewGuid();
            this.Org = org;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Organization: {Org}";
        }
        
        public object Clone()
        {
            return new Production(this.Org) { Id = this.Id };
        }
    }
    
    
    public interface ICollectable<T>
    {
        void Add(T item);
        void Remove(T item);
        void Show();
    }

    
    public class SimpleCollection<T> : ICollectable<T>
    {
        protected List<T> items = new List<T>();

        public void Add(T item)
        {
            items.Add(item);
            Console.WriteLine($"Элемент {item} добавлен.");
        }

        public void Remove(T item)
        {
            if (items.Remove(item))
            {
                Console.WriteLine($"Элемент {item} удален.");
            }
            else
            {
                Console.WriteLine($"Элемент {item} не найден.");
            }
        }

        public void Show()
        {
            Console.WriteLine("\nЭлементы коллекции\n");
            foreach (var item in items)
            {
                Console.WriteLine(item?.ToString() ?? "null");
            }
        }
    }

    
    public class CollectionType<T> : SimpleCollection<T> where T: ICloneable, new()
    {
        
        public T? Find(Predicate<T> predicate)
        {
            return items.Find(predicate);
        }
        
        
        public void SaveToFile(string filePath)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(filePath, jsonString);
                Console.WriteLine($"Коллекция сохранена в файл {filePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении в файл: {ex.Message}");
            }
        }

        public void LoadFromFile(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    throw new FileNotFoundException("Файл не найден.", filePath);
                }
                string jsonString = File.ReadAllText(filePath);
                var loadedItems = JsonSerializer.Deserialize<List<T>>(jsonString);
                if (loadedItems != null)
                {
                    items = loadedItems;
                    Console.WriteLine($"Коллекция загружена из файла {filePath}");
                }
                else
                {
                    Console.WriteLine("Не удалось десериализовать данные из файла.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке из файла: {ex.Message}");
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            
            Console.WriteLine("\nТип int\n");
            var intCollection = new SimpleCollection<int>();
            intCollection.Add(10);
            intCollection.Add(20);
            intCollection.Add(30);
            intCollection.Show();
            intCollection.Remove(20);
            intCollection.Show();
            
            Console.WriteLine("\nТип string\n");
            var stringCollection = new SimpleCollection<string>();
            stringCollection.Add("Hello");
            stringCollection.Add("World");
            stringCollection.Show();
            stringCollection.Remove("Hello");
            stringCollection.Show();

            
            Console.WriteLine("\nТип Production\n");
            var prodCollection = new CollectionType<Production>();
            var p1 = new Production("GeekBrains");
            var p2 = new Production("SkillFactory");
            prodCollection.Add(p1);
            prodCollection.Add(p2);
            prodCollection.Show();
            
            
            Console.WriteLine("\nOrg = SkillFactory:\n");
            var foundProd = prodCollection.Find(p => p.Org == "SkillFactory");
            if (foundProd != null)
            {
                Console.WriteLine($"Найден элемент: {foundProd}");
            }

            
            Console.WriteLine("\nCохранение и загрузка\n");
            var fileCollection = new CollectionType<Production>();
            fileCollection.Add(new Production("Microsoft"));
            fileCollection.Add(new Production("Apple"));
            fileCollection.Add(new Production("Google"));
            
            string filePath = "collection.json";
            fileCollection.SaveToFile(filePath);
            
            fileCollection.Show(); 
            
            
            var newFileCollection = new CollectionType<Production>();
            newFileCollection.LoadFromFile(filePath);
            Console.WriteLine("\nКоллекция из файла:");
            newFileCollection.Show();
        }
    }
}
