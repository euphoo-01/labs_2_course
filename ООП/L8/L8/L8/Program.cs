using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

public delegate void DeleteDelegate();

public delegate void MutateDelegate();

public class Programmer
{
    public event DeleteDelegate? Delete;
    public event MutateDelegate? Mutate;

    public void TriggerDelete()
    {
        Console.WriteLine("Программист инициировал удаление.");
        Delete?.Invoke();
    }

    public void TriggerMutate()
    {
        Console.WriteLine("Программист инициировал мутацию.");
        Mutate?.Invoke();
    }
}

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

    public void RemoveFirst()
    {
        if (Data.Any())
        {
            Console.WriteLine($"DataList({Id}). Удален первый элемент '{Data.First()}'.");
            Data.RemoveAt(0);
        }
        else
        {
            Console.WriteLine($"DataList({Id}). Список уже пуст, удаление невозможно.");
        }
    }

    public void RemoveLast()
    {
        if (Data.Any())
        {
            Console.WriteLine($"DataList({Id}). Удален последний элемент '{Data.Last()}'.");
            Data.RemoveAt(Data.Count - 1);
        }
        else
        {
            Console.WriteLine($"DataList({Id}). Список уже пуст, удаление невозможно.");
        }
    }

    public void Shuffle()
    {
        if (Data.Count > 1)
        {
            Console.WriteLine($"DataList({Id}). Список перемешан.");
            Data = Data.OrderBy(x => _random.Next()).ToList();
        }
        else
        {
            Console.WriteLine($"DataList({Id}). Недостаточно элементов для перемешивания.");
        }
    }

    public override string ToString()
    {
        return $"Объект(Id:{Id}): [{string.Join(", ", Data)}]";
    }
}


public static class StringManipulator
{
    public static string RemoveVowels(string s) => Regex.Replace(s, "[aeiouyAEIOUYаеёиоуыэюяАЕЁИОУЫЭЮЯ]", "");

    public static string AddStars(string s) => $"***{s}***";

    public static string ReverseWords(string s)
    {
        var words = s.Split(' ').Select(word => new string(word.Reverse().ToArray()));
        return string.Join(" ", words);
    }

    public static string RemoveExtraSpaces(string s) => Regex.Replace(s.Trim(), @"\s+", " ");

    public static string CountChars(string s) => $"Длина строки: {s.Length}";
}


public class Program
{
    public static void Main(string[] args)
    {
        var programmer = new Programmer();

        var list1 = new DataList(1, new[] { "alpha", "beta", "gamma" });
        var list2 = new DataList(2, new[] { "one", "two", "three", "four" });
        var list3 = new DataList(3, new[] { "A", "B", "C", "D", "E" });
        var list4 = new DataList(4, new[] { "apple", "orange", "banana" }); // Неподписанный объект

        Console.WriteLine("\nНачальное состояние объектов:");
        Console.WriteLine(list1);
        Console.WriteLine(list2);
        Console.WriteLine(list3);
        Console.WriteLine(list4);

        programmer.Delete += () => list1.RemoveFirst();

        programmer.Mutate += () => list2.Shuffle();

        programmer.Delete += () => list3.RemoveLast();
        programmer.Mutate += () => list3.Shuffle();

        programmer.TriggerDelete();

        Console.WriteLine("\nСостояние объектов после удаления:");
        Console.WriteLine(list1);
        Console.WriteLine(list2);
        Console.WriteLine(list3);
        Console.WriteLine(list4);

        programmer.TriggerMutate();

        Console.WriteLine("\nСостояние объектов после мутации:");
        Console.WriteLine(list1);
        Console.WriteLine(list2);
        Console.WriteLine(list3);
        Console.WriteLine(list4);

        programmer.TriggerDelete();

        Console.WriteLine("\nСостояние после второго удаления:");
        Console.WriteLine(list1);
        Console.WriteLine(list2);
        Console.WriteLine(list3);
        Console.WriteLine(list4);


        string testString = "  Абра    кадабра   абра ка дабраааа   .  ";

        Func<string, string> stringProcessing = StringManipulator.RemoveExtraSpaces;
        stringProcessing += StringManipulator.ReverseWords;
        stringProcessing += StringManipulator.RemoveVowels;
        stringProcessing += StringManipulator.AddStars;
        stringProcessing += StringManipulator.CountChars;

        Predicate<string> shouldProcess = (s) => !string.IsNullOrWhiteSpace(s);

        Action<string> printResult = (s) => Console.WriteLine($"\nИтоговый результат: {s}");

        Console.WriteLine($"Исходная строка: \"{testString}\"");

        if (shouldProcess(testString))
        {
            Console.WriteLine("Строка прошла проверку");
            string result = testString;
            foreach (Func<string, string> func in stringProcessing.GetInvocationList())
            {
                string temp = func(result);
                Console.WriteLine($"После '{func.Method.Name}': {temp}");
                result = temp;
            }

            printResult(result);
        }
        else
        {
            Console.WriteLine("Строка не прошла проверку предиката.");
        }
    }
}