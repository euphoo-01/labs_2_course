using System;
using System.IO;

namespace L12
{
    public static class LSAFileInfo
    {
        public static void ShowFileInfo(string filePath)
        {
            LSALog.Write("LSAFileInfo.ShowFileInfo", $"запрос инфы о файле {filePath}");
            try
            {
                FileInfo fileInfo = new FileInfo(filePath);
                if (fileInfo.Exists)
                {
                    Console.WriteLine($"--- Информация о файле: {fileInfo.Name} ---");
                    Console.WriteLine($"Полный путь: {fileInfo.FullName}");
                    Console.WriteLine($"Размер: {fileInfo.Length} байт");
                    Console.WriteLine($"Расширение: {fileInfo.Extension}");
                    Console.WriteLine($"Имя: {fileInfo.Name}");
                    Console.WriteLine($"Дата создания: {fileInfo.CreationTime}");
                    Console.WriteLine($"Дата изменения: {fileInfo.LastWriteTime}");
                }
                else
                {
                    Console.WriteLine($"файл {filePath} не найден.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка: {ex.Message}");
            }
        }
    }
}
