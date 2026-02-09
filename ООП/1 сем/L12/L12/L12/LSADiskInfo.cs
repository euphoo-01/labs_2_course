using System;
using System.IO;

namespace L12
{
    public static class LSADiskInfo
    {
        public static void ShowFreeSpace(string driveName)
        {
            LSALog.Write("LSADiskInfo.ShowFreeSpace", $"запрос свободного места для {driveName}");
            try
            {
                DriveInfo drive = new DriveInfo(driveName);
                if (drive.IsReady)
                {
                    Console.WriteLine($"Свободное место на диске {drive.Name}: {drive.TotalFreeSpace} байт");
                }
                else
                {
                    Console.WriteLine($"Диск {driveName} не готов.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка получения инфо о диске: {ex.Message}");
            }
        }

        public static void ShowFileSystemInfo(string driveName)
        {
            LSALog.Write("LSADiskInfo.ShowFileSystemInfo", $"запрос файловой системы для {driveName}");
            try
            {
                DriveInfo drive = new DriveInfo(driveName);
                if (drive.IsReady)
                {
                    Console.WriteLine($"Файловая система диска {drive.Name}: {drive.DriveFormat}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка: {ex.Message}");
            }
        }

        public static void ShowAllDrivesInfo()
        {
            LSALog.Write("LSADiskInfo.ShowAllDrivesInfo", "вывод информации о всех дисках");
            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                foreach (DriveInfo drive in drives)
                {
                    if (drive.IsReady)
                    {
                        Console.WriteLine($"Диск: {drive.Name}");
                        Console.WriteLine($"  Объем: {drive.TotalSize} байт");
                        Console.WriteLine($"  Доступно: {drive.AvailableFreeSpace} байт");
                        Console.WriteLine($"  Метка тома: {drive.VolumeLabel}");
                    }
                    else
                    {
                        Console.WriteLine($"Диск {drive.Name} не готов.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ошибка вывода дисков: {ex.Message}");
            }
        }
    }
}
