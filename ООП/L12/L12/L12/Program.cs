using System;
using System.IO;

namespace L12
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                
                
                LSALog.Write("Start", "Программа запущена");
                
                
                
                LSADiskInfo.ShowAllDrivesInfo();


                
                string testFile = "test_lsa.txt";
                File.WriteAllText(testFile, "hello world");
                LSAFileInfo.ShowFileInfo(testFile);
                File.Delete(testFile);
                
                
                
                LSADirInfo.ShowDirInfo(Directory.GetCurrentDirectory());
                
                
                
                // a
                string? currentDrive = Path.GetPathRoot(Directory.GetCurrentDirectory());
                if (currentDrive == null) currentDrive = "/";
                LSAFileManager.TaskA(currentDrive);
                
                // b
                Console.WriteLine("--- Task B ---");
                string dummyDir = "dummy_source";
                Directory.CreateDirectory(dummyDir);
                File.WriteAllText(Path.Combine(dummyDir, "f1.txt"), "text1");
                File.WriteAllText(Path.Combine(dummyDir, "f2.log"), "log1");
                File.WriteAllText(Path.Combine(dummyDir, "f3.txt"), "text2");
                
                LSAFileManager.TaskB(dummyDir, ".txt");
                
                // cleanup dummy
                Directory.Delete(dummyDir, true);
                
                // c
                Console.WriteLine("--- Task C ---");
                LSAFileManager.TaskC();
                
                
                LSALog.Read();
                LSALog.Search("Task");
                Console.WriteLine($"Количество записей: {LSALog.Count()}");
                
                LSALog.KeepCurrentHourLogs();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Глобальная ошибка: {ex.Message}");
            }
        }
    }
}