using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;

internal static class Program
{
    private const string OutputFile = "output.txt";
    private static readonly object LogLock = new object();

    static void Main()
    {
        File.WriteAllText(OutputFile, "");

        PrintProcesses();
        Console.WriteLine();

        ShowDomainAndAssemblies();
        Console.WriteLine();

        PrimesInThread();
        Console.WriteLine();

        EvenOddThreads();
        Console.WriteLine();

        TimerDemo();
        Console.WriteLine();

        Log("Готово. Результаты записаны в " + Path.GetFullPath(OutputFile));
        Console.WriteLine("Enter для выхода...");
        Console.ReadLine();
    }

    // 1) Процессы: вывести на консоль/в файл информацию о запущенных процессах
    private static void PrintProcesses()
    {
        Log("Процессы (в консоль ограничено, в файл пишется полный список):");

        Process[] processes;
        try
        {
            processes = Process.GetProcesses()
                .OrderBy(p => p.ProcessName, StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }
        catch (Exception ex)
        {
            Log("Не удалось получить процессы: " + ex.Message);
            return;
        }

        string header = string.Format("{0,-8} {1,-28} {2,-10} {3,-19} {4,-10} {5,-8} {6,-10}",
            "PID", "Name", "Prio", "StartTime", "State", "CPU(s)", "RAM(MB)");

        Log(header);

        // В консоль выводим, например, первые 30, чтобы не засорять.
        int consoleLimit = 30;

        for (int i = 0; i < processes.Length; i++)
        {
            var p = processes[i];

            int pid = p.Id;
            string name = TrimTo(p.ProcessName, 28);

            string prio = Safe(() => p.PriorityClass.ToString(), "n/a");
            string start = Safe(() => p.StartTime.ToString("yyyy-MM-dd HH:mm:ss"), "n/a");

            // Состояние в смысле “отвечает/не отвечает” и “вышел/не вышел” (как минимум доступно в Process API).
            string responding = Safe(() => p.Responding ? "Responding" : "NoResp", "n/a");
            string exited = Safe(() => p.HasExited ? "Exited" : "Running", "n/a");
            string state = $"{exited}/{responding}";

            string cpu = Safe(() => p.TotalProcessorTime.TotalSeconds.ToString("F2"), "n/a");
            string ram = Safe(() => (p.WorkingSet64 / 1024 / 1024).ToString(), "n/a");

            string line = string.Format("{0,-8} {1,-28} {2,-10} {3,-19} {4,-10} {5,-8} {6,-10}",
                pid, name, prio, start, TrimTo(state, 10), cpu, ram);

            // В файл — всё, в консоль — ограничение.
            Log(line, toConsole: i < consoleLimit, toFile: true);
        }

        var cur = Process.GetCurrentProcess();
        Log("");
        Log("Текущий процесс:");
        Log($"PID: {cur.Id}");
        Log($"Name: {cur.ProcessName}");
        Log($"Threads: {Safe(() => cur.Threads.Count.ToString(), "n/a")}");
        Log($"RAM(MB): {cur.WorkingSet64 / 1024 / 1024}");
        Log($"CPU total(s): {Safe(() => cur.TotalProcessorTime.TotalSeconds.ToString("F2"), "n/a")}");
    }

    // 2) Текущий домен + попытка CreateDomain + загрузка/выгрузка через AssemblyLoadContext
    private static void ShowDomainAndAssemblies()
    {
        var cur = AppDomain.CurrentDomain;

        Log("Текущий домен (AppDomain.CurrentDomain):");
        Log($"Name: {cur.FriendlyName}");
        Log($"BaseDirectory: {cur.BaseDirectory}");

        var assemblies = cur.GetAssemblies();
        Log($"Assemblies loaded: {assemblies.Length}");
        foreach (var a in assemblies.Take(12))
            Log($"- {a.GetName().Name}, v{a.GetName().Version}");

        Log("");
        Log("Попытка создать новый AppDomain:");

        try
        {
#pragma warning disable SYSLIB0024
            var d = AppDomain.CreateDomain("NewDomain");
#pragma warning restore SYSLIB0024
            Log("Создан домен: " + d.FriendlyName);
            
            AppDomain.Unload(d);
            Log("Домен выгружен.");
        }
        catch (Exception ex)
        {
            Log("CreateDomain/Unload недоступны: " + ex.GetType().Name + " — " + ex.Message);
        }

        Log("");
        Log("Загрузка/выгрузка сборки через AssemblyLoadContext (collectible):");

        var alc = new AssemblyLoadContext("TempContext", isCollectible: true);

        try
        {
            // Загружаем текущую сборку по пути (просто для демонстрации “загрузил в отдельный контекст”).
            string path = Assembly.GetExecutingAssembly().Location;
            var asm = alc.LoadFromAssemblyPath(path);

            Log("Loaded into ALC: " + asm.GetName().Name);
        }
        catch (Exception ex)
        {
            Log("Ошибка при работе с AssemblyLoadContext: " + ex.Message);
        }
        finally
        {
            alc.Unload();
            // Выгрузка “кооперативная” — чуть помогаем GC завершить выгрузку.
            for (int i = 0; i < 3; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
            Log("Unload requested (AssemblyLoadContext).");
        }
    }

    // 3) Простой поток: простые числа + управление (пауза/продолжить) + инфо о потоке
    private static void PrimesInThread()
    {
        int n = ReadInt("Введите n для простых чисел: ", min: 2, fallback: 100);

        using var pause = new ManualResetEventSlim(true); // true = выполнение разрешено

        var t = new Thread(() => PrimeWorker(n, pause))
        {
            Name = "Primes",
            Priority = ThreadPriority.Normal,
            IsBackground = false
        };

        Log($"Поток до старта: name={t.Name}, id={t.ManagedThreadId}, priority={t.Priority}, state={t.ThreadState}");
        t.Start();

        Thread.Sleep(400);
        Log($"Поток во время работы: name={t.Name}, id={t.ManagedThreadId}, priority={t.Priority}, state={t.ThreadState}");

        Log("Пауза на 2 секунды...");
        pause.Reset();
        Thread.Sleep(2000);

        Log("Продолжение...");
        pause.Set();

        t.Join();
        Log($"Поток после завершения: name={t.Name}, id={t.ManagedThreadId}, priority={t.Priority}, state={t.ThreadState}");
    }

    private static void PrimeWorker(int n, ManualResetEventSlim pause)
    {
        var primes = new List<int>();

        for (int i = 2; i <= n; i++)
        {
            pause.Wait();
            Thread.Sleep(15); // задержка для наглядности

            if (IsPrime(i))
            {
                primes.Add(i);
                Log($"prime: {i} (threadId={Thread.CurrentThread.ManagedThreadId})", toConsole: n <= 300, toFile: true);
            }
        }

        Log($"primes count: {primes.Count}");
        Log("primes: " + string.Join(", ", primes));
    }

    private static bool IsPrime(int x)
    {
        if (x < 2) return false;
        if (x == 2) return true;
        if (x % 2 == 0) return false;

        for (int d = 3; d * d <= x; d += 2)
            if (x % d == 0) return false;

        return true;
    }

    // 4) Два потока: четные/нечетные, разная скорость, смена приоритета, синхронизация
    private static void EvenOddThreads()
    {
        int n = ReadInt("Введите n для чет/нечет: ", min: 2, fallback: 20);

        Log("Параллельный вывод (разная скорость), один поток с повышенным приоритетом:");
        var evenA = new Thread(() => PrintEvens(n, delayMs: 120, prefix: "evenA")) { Priority = ThreadPriority.Highest };
        var oddA  = new Thread(() => PrintOdds(n, delayMs: 60, prefix: "oddA")) { Priority = ThreadPriority.Normal };
        evenA.Start(); oddA.Start();
        evenA.Join(); oddA.Join();

        Log("");
        Log("Сначала четные, потом нечетные (синхронизация через Join):");
        var evenB = new Thread(() => PrintEvens(n, delayMs: 80, prefix: "evenB"));
        var oddB = new Thread(() => PrintOdds(n, delayMs: 80, prefix: "oddB"));
        evenB.Start();
        evenB.Join();
        oddB.Start();
        oddB.Join();

        Log("");
        Log("Поочередно: четное, нечетное (Monitor.Wait/Pulse):");

        var sync = new object();
        bool evenTurn = true;

        var evenC = new Thread(() =>
        {
            for (int x = 2; x <= n; x += 2)
            {
                lock (sync)
                {
                    while (!evenTurn) Monitor.Wait(sync);
                    Log($"evenC: {x}");
                    evenTurn = false;
                    Monitor.PulseAll(sync);
                }
                Thread.Sleep(90);
            }
        });

        var oddC = new Thread(() =>
        {
            for (int x = 1; x <= n; x += 2)
            {
                lock (sync)
                {
                    while (evenTurn) Monitor.Wait(sync);
                    Log($"oddC:  {x}");
                    evenTurn = true;
                    Monitor.PulseAll(sync);
                }
                Thread.Sleep(40);
            }
        });

        evenC.Start(); oddC.Start();
        evenC.Join(); oddC.Join();
    }

    private static void PrintEvens(int n, int delayMs, string prefix)
    {
        for (int x = 2; x <= n; x += 2)
        {
            Log($"{prefix}: {x} (prio={Thread.CurrentThread.Priority})");
            Thread.Sleep(delayMs);
        }
    }

    private static void PrintOdds(int n, int delayMs, string prefix)
    {
        for (int x = 1; x <= n; x += 2)
        {
            Log($"{prefix}: {x} (prio={Thread.CurrentThread.Priority})");
            Thread.Sleep(delayMs);
        }
    }

    // 5) Timer: повторяющаяся задача
    private static void TimerDemo()
    {
        Log("Timer: 5 секунд, период 1 секунда:");

        int ticks = 0;
        using var timer = new Timer(_ =>
        {
            int t = Interlocked.Increment(ref ticks);
            Log($"tick={t}, time={DateTime.Now:HH:mm:ss}, threadId={Thread.CurrentThread.ManagedThreadId}");
        },
        state: null,
        dueTime: TimeSpan.Zero,
        period: TimeSpan.FromSeconds(1));

        Thread.Sleep(5200);
        Log("ticks total: " + ticks);
    }

    // helpers
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
        string? s = Console.ReadLine();
        if (int.TryParse(s, out int n) && n >= min) return n;
        Console.WriteLine($"Некорректно, беру {fallback}");
        return fallback;
    }

    private static string TrimTo(string s, int max) => s.Length <= max ? s : s.Substring(0, max - 1) + "…";

    private static string Safe(Func<string> get, string fallback)
    {
        try { return get(); }
        catch { return fallback; }
    }
}
