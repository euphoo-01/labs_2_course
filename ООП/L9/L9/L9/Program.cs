using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace L9;

public static class Program
{
    public static void Main()
    {
        var imageSet = new ImageSet();
        Console.WriteLine("Создана пустая коллекция ImageSet.");
        
        var catImage = new Image("cat", "800x600", 120, "jpg");
        var dogImage = new Image("dog", "1920x1080", 950, "png");
        var catImageDuplicate = new Image("cat", "1024x768", 256, "jpg"); // дубликат по ключу (имя + формат)

        Console.WriteLine($"Добавляем '{catImage}'. Результат: {imageSet.Add(catImage)}");
        Console.WriteLine($"Добавляем '{dogImage}'. Результат: {imageSet.Add(dogImage)}");
        Console.WriteLine(
            $"Добавляем дубликат '{catImageDuplicate}'. Результат: {imageSet.Add(catImageDuplicate)}");

        Console.WriteLine("\nСодержимое коллекции:");
        PrintCollection(imageSet);
        
        Console.WriteLine($"Удаляем '{dogImage}'. Результат: {imageSet.Remove(dogImage)}");
        Console.WriteLine($"Попытка удалить еще раз. Результат: {imageSet.Remove(dogImage)}");

        Console.WriteLine("\nСодержимое коллекции:");
        PrintCollection(imageSet);
        
        var searchImage = new Image("cat", "800x600", 120, "jpg");
        Console.WriteLine($"Поиск '{searchImage}'. Результат: {imageSet.Contains(searchImage)}");
        Console.WriteLine($"Поиск '{dogImage}'. Результат: {imageSet.Contains(dogImage)}");
        
        var anotherSet = new List<Image>
        {
            new("landscape", "4k", 5000, "tiff"),
            new("cat", "1200x900", 400, "jpg") // Этот элемент не добавится, т.к. уже есть
        };
        Console.WriteLine("Вторая коллекция:");
        PrintCollection(anotherSet);

        imageSet.UnionWith(anotherSet);
        Console.WriteLine("\nРезультат объединения:");
        PrintCollection(imageSet);


        var list = new LinkedList<int>(new[] { 10, 20, 30, 40, 50 });
        Console.WriteLine("Исходная коллекция LinkedList<int>:");
        PrintCollection(list);
        
        var nodeToRemove = list.Find(20);
        if (nodeToRemove?.Next != null)
        {
            list.Remove(nodeToRemove.Next); // удаляем 30
            list.Remove(nodeToRemove); // удаляем 20
        }

        PrintCollection(list);

        
        list.AddFirst(5);
        list.AddLast(60);
        var node40 = list.Find(40);
        if (node40 != null)
        {
            list.AddBefore(node40, 35);
        }

        PrintCollection(list);
        
        var hashSet = new HashSet<int>(list);
        PrintCollection(hashSet);
        
        int valueToFind = 35;
        if (hashSet.Contains(valueToFind))
        {
            Console.WriteLine($"Значение {valueToFind} найдено.");
        }
        else
        {
            Console.WriteLine($"Значение {valueToFind} не найдено.");
        }


        var observableImages = new ObservableCollection<Image>();
        observableImages.CollectionChanged += OnCollectionChanged;

        Console.WriteLine("Подписались на событие CollectionChanged.");
        
        var newImage = new Image("портрет Шимана", "1080x1080", 300, "jpeg");
        observableImages.Add(newImage);
        
        observableImages.Remove(newImage);
    }

    private static void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Событие. Чето с коллекцией произошло");
        Console.WriteLine($"Действие: {e.Action}");

        if (e.NewItems != null)
        {
            Console.WriteLine("Добавлены элементы:");
            foreach (var item in e.NewItems)
            {
                Console.WriteLine($"{item}");
            }
        }

        if (e.OldItems != null)
        {
            Console.WriteLine("Удалены элементы:");
            foreach (var item in e.OldItems)
            {
                Console.WriteLine($"- {item}");
            }
        }

        Console.ResetColor();
    }

    private static void PrintCollection<T>(IEnumerable<T> collection)
    {
        Console.Write("[ ");
        Console.Write(string.Join(", ", collection));
        Console.WriteLine(" ]");
    }
}