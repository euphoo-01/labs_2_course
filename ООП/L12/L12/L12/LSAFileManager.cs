using System;
using System.IO;
using System.IO.Compression;

namespace L12
{
    public static class LSAFileManager
    {
        public static void TaskA(string driveName)
        {
            LSALog.Write("LSAFileManager.TaskA", $"начало задания A с диском {driveName}");
            try
            {
                DriveInfo drive = new DriveInfo(driveName);
                if (!drive.IsReady)
                {
                    Console.WriteLine("диск не готов.");
                    return;
                }

                // получаем список файлов и папок корня диска
                var entries = drive.RootDirectory.GetFileSystemInfos();
                
                string inspectDir = "LSAInspect";
                Directory.CreateDirectory(inspectDir);
                
                string dirInfoFile = Path.Combine(inspectDir, "lsadirinfo.txt");

                using (StreamWriter sw = new StreamWriter(dirInfoFile))
                {
                    sw.WriteLine($"Содержимое корня диска {driveName}:");
                    foreach (var entry in entries)
                    {
                        sw.WriteLine($"{entry.Name} ({entry.GetType().Name})");
                    }
                }
                
                Console.WriteLine($"информация сохранена в {dirInfoFile}");

                // копирование и переименование
                string copyPath = Path.Combine(inspectDir, "lsadirinfo_copy.txt");
                File.Copy(dirInfoFile, copyPath, true);
                
                File.Delete(dirInfoFile);
                Console.WriteLine("оригинальный файл удален, осталась копия: lsadirinfo_copy.txt");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка TaskA: {ex.Message}");
            }
        }

        public static void TaskB(string sourceDir, string extension)
        {
            LSALog.Write("LSAFileManager.TaskB", $"начало задания B, источник {sourceDir}, расширение {extension}");
            try
            {
                if (!Directory.Exists(sourceDir))
                {
                    Console.WriteLine("исходная директория не найдена");
                    return;
                }

                string filesDir = "LSAFiles";
                Directory.CreateDirectory(filesDir);
                
                if (!extension.StartsWith(".")) extension = "." + extension;
                
                var files = Directory.GetFiles(sourceDir, "*" + extension);
                foreach (var file in files)
                {
                    string fileName = Path.GetFileName(file);
                    string destFile = Path.Combine(filesDir, fileName);
                    File.Copy(file, destFile, true);
                }
                Console.WriteLine($"скопировано файлов: {files.Length}");
                
                
                string inspectDir = "LSAInspect";
                if (!Directory.Exists(inspectDir)) Directory.CreateDirectory(inspectDir);
                
                string destDir = Path.Combine(inspectDir, "LSAFiles");
                
                if (Directory.Exists(destDir)) Directory.Delete(destDir, true);
                
                Directory.Move(filesDir, destDir);
                Console.WriteLine($"директория перемещена в {destDir}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка TaskB: {ex.Message}");
            }
        }

        public static void TaskC()
        {
            LSALog.Write("LSAFileManager.TaskC", "начало задания C (архивация)");
            try
            {
                string sourceDir = Path.Combine("LSAInspect", "LSAFiles");
                string zipPath = Path.Combine("LSAInspect", "LSAFiles.zip");
                string extractDir = "LSAUnzip";

                if (!Directory.Exists(sourceDir))
                {
                    Console.WriteLine("директория для архивации не найдена");
                    return;
                }
                
                if (File.Exists(zipPath)) File.Delete(zipPath);
                
                ZipFile.CreateFromDirectory(sourceDir, zipPath);
                Console.WriteLine($"архив создан: {zipPath}");
                
                if (Directory.Exists(extractDir)) Directory.Delete(extractDir, true);
                Directory.CreateDirectory(extractDir);
                
                ZipFile.ExtractToDirectory(zipPath, extractDir);
                Console.WriteLine($"архив распакован в {extractDir}");
                
                // проверка
                Console.WriteLine("файлы в распакованной папке:");
                foreach(var f in Directory.GetFiles(extractDir))
                {
                    Console.WriteLine(Path.GetFileName(f));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка TaskC: {ex.Message}");
            }
        }
    }
}
