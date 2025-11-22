using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

// --- Задание 1: Делегаты и События ---

// 1. Определяем делегаты для событий
public delegate void DeleteHandler();
public delegate void MutateHandler();

/// <summary>
/// Класс Программист, который инициирует события.
/// </summary>
public class Programmer
{
    // 2. Определяем события на основе делегатов
    public event DeleteHandler? Delete;
    public event MutateHandler? Mutate;

    /// <summary>
    /// Метод для вызова события "Удалить".
    /// </summary>
    public void TriggerDelete()
    {
        Console.WriteLine($"[СОБЫТИЕ] Программист инициировал удаление...");
        Delete?.Invoke();
    }

    /// <summary>
    /// Метод для вызова события "Мутировать".
    /// </summary>
    public void TriggerMutate()
    {
        Console.WriteLine($"[СОБЫТИЕ] Программист инициировал мутацию...");
        Mutate?.Invoke();
    }
}

/// <summary>
/// Класс для объекта данных (списка), который будет реагировать на события.
/// </summary>
public class DataList
{
    public int Id { get; }
    public List<string> Data { get; private set; }
    private static readonly Random _random = new();

    public DataList(int id, IEnumerable<string> initialData)
    {
        Id = id;
        Data = new List<string>(initialData);
    }

    /// <summary>
    /// Удаляет первый элемент списка.
    /// </summary>
    public void RemoveFirst()
    {
        if (Data.Any())
        {
            Console.WriteLine($"  -> DataList({Id}): Удален первый элемент '{Data.First()}'.");
            Data.RemoveAt(0);
        }
        else
        {
            Console.WriteLine($"  -> DataList({Id}): Список уже пуст, удаление невозможно.");
        }
    }

    /// <summary>
    /// Удаляет последний элемент списка.
    /// </summary>
    public void RemoveLast()
    {
        if (Data.Any())
        {
            Console.WriteLine($"  -> DataList({Id}): Удален последний элемент '{Data.Last()}'.");
            Data.RemoveAt(Data.Count - 1);
        }
        else
        {
            Console.WriteLine($"  -> DataList({Id}): Список уже пуст, удаление невозможно.");
        }
    }

    /// <summary>
    /// Перемешивает элементы списка.
    /// </summary>
    public void Shuffle()
    {
        if (Data.Count > 1)
        {
            Console.WriteLine($"  -> DataList({Id}): Список перемешан.");
            Data = Data.OrderBy(x => _random.Next()).ToList();
        }
        else
        {
            Console.WriteLine($"  -> DataList({Id}): Недостаточно элементов для перемешивания.");
        }
    }

    public override string ToString()
    {
        return $"Объект(Id:{Id}): [{string.Join(", ", Data)}]";
    }
}


// --- Задание 2: Обработка строк ---

public static class StringManipulator
{
    /// <summary>
    /// Удаляет гласные буквы из строки.
    /// </summary>
    public static string RemoveVowels(string s) => Regex.Replace(s, "[aeiouyAEIOUYаеёиоуыэюяАЕЁИОУЫЭЮЯ]", "");

    /// <summary>
    /// Добавляет звездочки по краям строки.
    /// </summary>
    public static string AddStars(string s) => $"***{s}***";

    /// <summary>
    /// Переворачивает каждое слово в строке.
    /// </summary>
    public static string ReverseWords(string s)
    {
        var words = s.Split(' ').Select(word => new string(word.Reverse().ToArray()));
        return string.Join(" ", words);
    }
    
    /// <summary>
    /// Удаляет лишние пробелы.
    /// </summary>
    public static string RemoveExtraSpaces(string s) => Regex.Replace(s.Trim(), @"\s+", " ");
    
    /// <summary>
    /// Считает количество символов в строке.
    /// </summary>
    public static string CountChars(string s) => $"{s} (Длина: {s.Length})";
}


public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("--- Задание 1: Делегаты, события и лямбда-выражения (Вариант 9) ---");
        
        var programmer = new Programmer();

        // Создаем несколько объектов-списков
        var list1 = new DataList(1, new[] { "alpha", "beta", "gamma" });
        var list2 = new DataList(2, new[] { "one", "two", "three", "four" });
        var list3 = new DataList(3, new[] { "A", "B", "C", "D", "E" });
        var list4 = new DataList(4, new[] { "apple", "orange", "banana" }); // Неподписанный объект

        Console.WriteLine("\nНачальное состояние объектов:");
        Console.WriteLine(list1);
        Console.WriteLine(list2);
        Console.WriteLine(list3);
        Console.WriteLine(list4);

        // Подписываем объекты на события с помощью лямбда-выражений
        // list1: реагирует на 'Удалить', удаляя первый элемент
        programmer.Delete += () => list1.RemoveFirst();
        
        // list2: реагирует на 'Мутировать', перемешивая себя
        programmer.Mutate += () => list2.Shuffle();
        
        // list3: реагирует на оба события
        programmer.Delete += () => list3.RemoveLast();
        programmer.Mutate += () => list3.Shuffle();
        
        Console.WriteLine("\n--- Программист инициирует событие 'Удалить' ---");
        programmer.TriggerDelete();

        Console.WriteLine("\nСостояние объектов после удаления:");
        Console.WriteLine(list1);
        Console.WriteLine(list2);
        Console.WriteLine(list3);
        Console.WriteLine(list4);

        Console.WriteLine("\n--- Программист инициирует событие 'Мутировать' ---");
        programmer.TriggerMutate();

        Console.WriteLine("\nСостояние объектов после мутации:");
        Console.WriteLine(list1);
        Console.WriteLine(list2);
        Console.WriteLine(list3);
        Console.WriteLine(list4);
        
        Console.WriteLine("\n--- Повторное событие 'Удалить' ---");
        programmer.TriggerDelete();
        
        Console.WriteLine("\nСостояние после второго удаления:");
        Console.WriteLine(list1);
        Console.WriteLine(list2);
        Console.WriteLine(list3);
        Console.WriteLine(list4);

        Console.WriteLine("\n\n--- Задание 2: Обработка строк ---");

        string testString = "  Это пример   строки для демонстрации работы   делегатов.  ";
        
        // 1. Цепочка функций обработки (Func)
        Func<string, string> processingPipeline = StringManipulator.RemoveExtraSpaces;
        processingPipeline += StringManipulator.ReverseWords;
        processingPipeline += StringManipulator.RemoveVowels;
        processingPipeline += StringManipulator.AddStars;
        processingPipeline += StringManipulator.CountChars;
        
        // 2. Условие для обработки (Predicate)
        Predicate<string> shouldProcess = (s) => !string.IsNullOrWhiteSpace(s);
        
        // 3. Действие для вывода результата (Action)
        Action<string> printResult = (s) => Console.WriteLine($"\nИтоговый результат: {s}");

        Console.WriteLine($"Исходная строка: \"{testString}\"");

        if (shouldProcess(testString))
        {
            Console.WriteLine("Строка прошла проверку, начинаем обработку...");
            string result = testString;
            foreach (Func<string, string> func in processingPipeline.GetInvocationList())
            {
                string tempResult = func(result);
                Console.WriteLine($"После '{func.Method.Name}': {tempResult}");
                result = tempResult;
            }
            printResult(result);
        }
        else
        {
            Console.WriteLine("Строка не прошла проверку предиката.");
        }
    }
}