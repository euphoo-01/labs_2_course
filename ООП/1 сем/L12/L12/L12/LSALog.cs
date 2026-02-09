using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace L12
{
    public static class LSALog
    {
        // путь к лог-файлу
        private static readonly string LogPath = "lsalogfile.txt";

        public static void Write(string action, string details)
        {
            try
            {
                // запись в конец файла
                using (StreamWriter sw = new StreamWriter(LogPath, true, Encoding.UTF8))
                {
                    sw.WriteLine($"{DateTime.Now}: {action} - {details}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка записи лога: {ex.Message}");
            }
        }

        public static void Read()
        {
            try
            {
                if (!File.Exists(LogPath))
                {
                    Console.WriteLine("лог файл не найден.");
                    return;
                }

                using (StreamReader sr = new StreamReader(LogPath))
                {
                    Console.WriteLine(sr.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка чтения лога: {ex.Message}");
            }
        }

        public static void Search(string keyword)
        {
            try
            {
                if (!File.Exists(LogPath)) return;

                Console.WriteLine($"--- Поиск по '{keyword}' ---");
                using (StreamReader sr = new StreamReader(LogPath))
                {
                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Contains(keyword))
                        {
                            Console.WriteLine(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка поиска в логе: {ex.Message}");
            }
        }
        
        public static void KeepCurrentHourLogs()
        {
             try
             {
                 if (!File.Exists(LogPath)) return;
                 
                 var lines = File.ReadAllLines(LogPath);
                 var currentHourLines = new List<string>();
                 var now = DateTime.Now;
                 
                 string datePrefix = now.ToString("dd.MM.yyyy HH");

                 foreach (var line in lines)
                 {
                     // простая проверка начала строки
                     if (line.StartsWith(datePrefix))
                     {
                         currentHourLines.Add(line);
                     }
                 }
                 
                 File.WriteAllLines(LogPath, currentHourLines);
                 Console.WriteLine($"лог очищен, оставлено записей за текущий час: {currentHourLines.Count}");
             }
             catch (Exception ex)
             {
                 Console.WriteLine($"ошибка очистки лога: {ex.Message}");
             }
        }

        public static int Count()
        {
             try
             {
                 if (!File.Exists(LogPath)) return 0;
                 return File.ReadAllLines(LogPath).Length;
             }
             catch
             {
                 return 0;
             }
        }
        
        public static string GetLogPath() => Path.GetFullPath(LogPath);
    }
}
