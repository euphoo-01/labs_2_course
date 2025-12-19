using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

internal static class Program
{
    private const string OutputFile = "output.txt";
    private static readonly object LogLock = new object();

    static async Task Main()
    {
        File.WriteAllText(OutputFile, "");

        Task1_EratosthenesSieve();
        Console.WriteLine();

        Task2_CancellationToken();
        Console.WriteLine();

        Task3_MultipleTasksWithResult();
        Console.WriteLine();

        Task4_ContinuationTasks();
        Console.WriteLine();

        Task5_ParallelLoops();
        Console.WriteLine();

        Task6_ParallelInvoke();
        Console.WriteLine();

        Task7_BlockingCollection();
        Console.WriteLine();

        await Task8_AsyncAwait();
        Console.WriteLine();

        Log($"Готово. Результаты: {Path.GetFullPath(OutputFile)}");
        Console.WriteLine("Enter для выхода...");
        Console.ReadLine();
    }

    // 1) TPL Task: поиск простых чисел (Эратосфен) + статус + производительность
    private static void Task1_EratosthenesSieve()
    {
        int n = ReadInt("n для решета Эратосфена: ", min: 1000, fallback: 100000);

        // Последовательно
        Log("Последовательный поиск простых чисел:");
        var sw = Stopwatch.StartNew();
        var seqPrimes = EratosthenesSequential(n);
        sw.Stop();
        Log($"Последовательно: {seqPrimes.Count} простых чисел за {sw.ElapsedMilliseconds}ms");
        Log($"Первые 20: {string.Join(", ", seqPrimes.Take(20))}");

        // Используя Task
        Log("");
        Log("Использование TPL (Task):");

        var task = Task.Run(() =>
        {
            Log($"Task ID: {Task.CurrentId}");
            return EratosthenesSequential(n);
        });

        Log($"Task статус до завершения: IsCompleted={task.IsCompleted}, Status={task.Status}");
        task.Wait();
        Log($"Task статус после завершения: IsCompleted={task.IsCompleted}, Status={task.Status}");

        Log($"Найдено: {task.Result.Count} простых чисел");

        // Несколько прогонов для оценки
        Log("");
        Log("Прогоны для оценки производительности:");
        int runs = 3;
        long[] times = new long[runs];

        for (int i = 0; i < runs; i++)
        {
            sw.Restart();
            var t = Task.Run(() => EratosthenesSequential(n));
            t.Wait();
            sw.Stop();
            times[i] = sw.ElapsedMilliseconds;
            Log($"Прогон {i + 1}: {times[i]}ms");
        }

        double avg = times.Average();
        Log($"Среднее: {avg:F1}ms");
    }

    // 2) Task с CancellationToken
    private static void Task2_CancellationToken()
    {
        int n = 50000;
        var cts = new CancellationTokenSource();

        Log("Task с CancellationToken:");

        var task = Task.Run(() =>
        {
            for (int i = 0; i < n; i++)
            {
                cts.Token.ThrowIfCancellationRequested();

                if (i % 5000 == 0)
                    Log($"Обработано {i} элементов");

                Thread.Sleep(1);
            }

            return "Завершено";
        });

        Thread.Sleep(100);
        Log("Отмена задачи...");
        cts.Cancel();

        try
        {
            task.Wait();
            Log($"Результат: {task.Result}");
        }
        catch (AggregateException ex)
        {
            Log($"Task отменена: {ex.InnerException?.GetType().Name}");
        }
    }

    // 3) Три задачи с результатом + четвертая, использующая их
    private static void Task3_MultipleTasksWithResult()
    {
        Log("Три задачи с результатом + четвертая:");

        var t1 = Task.Run(() => { Thread.Sleep(100); return 10; });
        var t2 = Task.Run(() => { Thread.Sleep(150); return 20; });
        var t3 = Task.Run(() => { Thread.Sleep(80); return 30; });

        Task.WaitAll(t1, t2, t3);

        var t4 = Task.Run(() =>
        {
            int sum = t1.Result + t2.Result + t3.Result;
            int product = t1.Result * t2.Result * t3.Result;
            return $"sum={sum}, product={product}";
        });

        Log($"t1={t1.Result}, t2={t2.Result}, t3={t3.Result}");
        Log($"t4 результат: {t4.Result}");
    }

    // 4) Continuation tasks (ContinueWith + Awaiter)
    private static void Task4_ContinuationTasks()
    {
        Log("Continuation tasks:");

        // Вариант 1: ContinueWith
        Log("1) ContinueWith:");
        var t1 = Task.Run(() =>
        {
            Thread.Sleep(200);
            return 42;
        });

        var t1Cont = t1.ContinueWith(prev =>
        {
            return $"Результат предыдущей: {prev.Result}";
        });

        Log($"t1Cont результат: {t1Cont.Result}");

        // Вариант 2: Awaiter + GetResult()
        Log("2) GetAwaiter() + GetResult():");
        var t2 = Task.Run(() =>
        {
            Thread.Sleep(150);
            return 100;
        });

        var awaiter = t2.GetAwaiter();
        int result = awaiter.GetResult();
        Log($"GetResult: {result}");
    }

    // 5) Parallel.For и Parallel.ForEach
    private static void Task5_ParallelLoops()
    {
        int size = 10000000;

        Log("Parallel.For (умножение массива на число):");

        var arr1 = new int[size];
        for (int i = 0; i < arr1.Length; i++) arr1[i] = i + 1;

        var sw = Stopwatch.StartNew();
        for (int i = 0; i < arr1.Length; i++) arr1[i] *= 2;
        sw.Stop();
        Log($"Последовательный цикл: {sw.ElapsedMilliseconds}ms");

        var arr2 = new int[size];
        for (int i = 0; i < arr2.Length; i++) arr2[i] = i + 1;

        sw.Restart();
        Parallel.For(0, arr2.Length, i => arr2[i] *= 2);
        sw.Stop();
        Log($"Parallel.For: {sw.ElapsedMilliseconds}ms");

        Log("");
        Log("Parallel.ForEach (обработка последовательности):");

        var list = Enumerable.Range(1, 1000000).ToList();
        var results = new int[list.Count];

        sw.Restart();
        for (int i = 0; i < list.Count; i++)
            results[i] = list[i] * 3;
        sw.Stop();
        Log($"Обычный цикл: {sw.ElapsedMilliseconds}ms");

        var results2 = new int[list.Count];
        sw.Restart();
        Parallel.ForEach(list, (item, state, idx) =>
        {
            results2[(int)idx] = item * 3;
        });
        sw.Stop();
        Log($"Parallel.ForEach: {sw.ElapsedMilliseconds}ms");
    }

    // 6) Parallel.Invoke
    private static void Task6_ParallelInvoke()
    {
        Log("Parallel.Invoke (выполнение блока операторов):");

        var sw = Stopwatch.StartNew();

        Parallel.Invoke(
            () => { Log("Action 1"); Thread.Sleep(300); },
            () => { Log("Action 2"); Thread.Sleep(250); },
            () => { Log("Action 3"); Thread.Sleep(200); },
            () => { Log("Action 4"); Thread.Sleep(150); }
        );

        sw.Stop();
        Log($"Всего времени: {sw.ElapsedMilliseconds}ms");
    }

    // 7) BlockingCollection: поставщики/покупатели
    private static void Task7_BlockingCollection()
    {
        Log("BlockingCollection (5 поставщиков, 10 покупателей):");

        var stock = new BlockingCollection<string>(boundedCapacity: 100);
        var products = new[] { "Холодильник", "Микроволновка", "Посудомойка", "Стиральная машина", "Кондиционер" };

        var suppliers = Enumerable.Range(0, 5).Select(i => Task.Run(() =>
        {
            int delay = 100 + i * 50;
            for (int j = 0; j < 4; j++)
            {
                Thread.Sleep(delay);
                string product = $"{products[i]}_#{j + 1}";
                stock.Add(product);
                Log($"Поставщик {i}: добавил {product}. На складе: {stock.Count}");
            }
        })).ToArray();

        var buyers = Enumerable.Range(0, 10).Select(i => Task.Run(() =>
        {
            while (stock.TryTake(out string product, 5000))
            {
                Log($"Покупатель {i}: купил {product}. На складе: {stock.Count}");
                Thread.Sleep(200);
            }
            Log($"Покупатель {i}: ушел (товар закончился)");
        })).ToArray();

        Task.WaitAll(suppliers);
        stock.CompleteAdding();
        Task.WaitAll(buyers);

        Log($"Процесс завершен. На складе осталось: {stock.Count}");
    }

    // 8) Async/Await
    private static async Task Task8_AsyncAwait()
    {
        Log("Async/Await:");

        var result = await LongRunningAsync();
        Log($"Результат асинхронного метода: {result}");

        var t1 = LongRunningAsync();
        var t2 = LongRunningAsync();
        var t3 = LongRunningAsync();

        var results = await Task.WhenAll(t1, t2, t3);
        Log($"Три асинхронных задачи завершены: {string.Join(", ", results)}");
    }

    private static async Task<string> LongRunningAsync()
    {
        await Task.Delay(300);
        return $"Готово в {DateTime.Now:HH:mm:ss}";
    }

    // Helpers
    private static System.Collections.Generic.List<int> EratosthenesSequential(int limit)
    {
        var sieve = new bool[limit + 1];
        for (int i = 2; i <= limit; i++) sieve[i] = true;

        for (int i = 2; i * i <= limit; i++)
            if (sieve[i])
                for (int j = i * i; j <= limit; j += i)
                    sieve[j] = false;

        var primes = new System.Collections.Generic.List<int>();
        for (int i = 2; i <= limit; i++)
            if (sieve[i]) primes.Add(i);

        return primes;
    }

    private static void Log(string message, bool toConsole = true, bool toFile = true)
    {
        lock (LogLock)
        {
            if (toConsole) Console.WriteLine(message);
            if (toFile) File.AppendAllText(OutputFile, message + Environment.NewLine);
        }
    }

    private static int ReadInt(string prompt, int min, int fallback)
    {
        Console.Write(prompt);
        if (int.TryParse(Console.ReadLine(), out int n) && n >= min) return n;
        Console.WriteLine($"Некорректно, беру {fallback}");
        return fallback;
    }
}
