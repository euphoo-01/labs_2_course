using System;
using System.IO;

namespace L12
{
    public static class LSADirInfo
    {
        public static void ShowDirInfo(string dirPath)
        {
            LSALog.Write("LSADirInfo.ShowDirInfo", $"запрос инфо о директории {dirPath}");
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
                if (dirInfo.Exists)
                {
                    Console.WriteLine($"--- Информация о директории: {dirInfo.Name} ---");
                    Console.WriteLine($"Количество файлов: {dirInfo.GetFiles().Length}");
                    Console.WriteLine($"Время создания: {dirInfo.CreationTime}");
                    Console.WriteLine($"Количество поддиректориев: {dirInfo.GetDirectories().Length}");
                    
                    Console.WriteLine("Родительские директории:");
                    DirectoryInfo? parent = dirInfo.Parent;
                    while (parent != null)
                    {
                        Console.WriteLine($"  {parent.Name}");
                        parent = parent.Parent;
                    }
                }
                else
                {
                    Console.WriteLine($"директория {dirPath} не найдена.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка: {ex.Message}");
            }
        }
    }
}
