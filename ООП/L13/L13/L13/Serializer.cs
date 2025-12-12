using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Xml.Serialization;

namespace L13
{
    public interface ISerializer
    {
        void Serialize<T>(T obj, string filePath);
        T Deserialize<T>(string filePath);
    }

    public class CustomSerializer
    {
        public enum Format
        {
            Binary,
            JSON,
            XML,
            SOAP
        }

        public static void Serialize<T>(T obj, string filePath, Format format)
        {
            switch (format)
            {
                case Format.Binary:
                    try
                    {
                        // #pragma warning disable SYSLIB0011
                        BinaryFormatter binFormatter = new BinaryFormatter();
                        using (FileStream fs = new FileStream(filePath, FileMode.Create))
                        {
                            binFormatter.Serialize(fs, obj);
                        }
                        // #pragma warning restore SYSLIB0011
                        Console.WriteLine($"[CustomSerializer] Объект успешно сериализован в {format} формат: {filePath}");
                    }
                    catch (PlatformNotSupportedException)
                    {
                        Console.WriteLine($"[CustomSerializer] Внимание: Бинарная сериализация не поддерживается в данной версии .NET (удалена с .NET 9+). Пропускаем.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[CustomSerializer] Ошибка при бинарной сериализации: {ex.Message}");
                    }
                    break;

                case Format.SOAP:
                    Console.WriteLine("[CustomSerializer] Внимание: Для SOAP формата требуется установка пакета System.Runtime.Serialization.Formatters.Soap, который не входит в стандартную библиотеку .NET Core/.NET 5+. Пропускаем.");
                    break;

                case Format.JSON:
                    string jsonString = JsonSerializer.Serialize(obj, new JsonSerializerOptions { WriteIndented = true, IncludeFields = true });
                    File.WriteAllText(filePath, jsonString);
                    Console.WriteLine($"[CustomSerializer] Объект успешно сериализован в {format} формат: {filePath}");
                    break;

                case Format.XML:
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        xmlSerializer.Serialize(fs, obj);
                    }
                    Console.WriteLine($"[CustomSerializer] Объект успешно сериализован в {format} формат: {filePath}");
                    break;
            }
        }

        public static T Deserialize<T>(string filePath, Format format)
        {
            T result = default(T);
            switch (format)
            {
                case Format.Binary:
                    try
                    {
                        // #pragma warning disable SYSLIB0011
                        BinaryFormatter binFormatter = new BinaryFormatter();
                        using (FileStream fs = new FileStream(filePath, FileMode.Open))
                        {
                            result = (T)binFormatter.Deserialize(fs);
                        }
                        // #pragma warning restore SYSLIB0011
                        Console.WriteLine($"[CustomSerializer] Объект успешно десериализован из {format} формата: {filePath}");
                    }
                    catch (PlatformNotSupportedException)
                    {
                        Console.WriteLine($"[CustomSerializer] Бинарная десериализация не поддерживается в данной версии .NET");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[CustomSerializer] Ошибка при бинарной десериализации: {ex.Message}");
                    }
                    break;

                case Format.SOAP:
                    Console.WriteLine("[CustomSerializer] SOAP формат не поддерживается.");
                    break;

                case Format.JSON:
                    string jsonString = File.ReadAllText(filePath);
                    result = JsonSerializer.Deserialize<T>(jsonString, new JsonSerializerOptions { IncludeFields = true });
                    Console.WriteLine($"[CustomSerializer] Объект успешно десериализован из {format} формата: {filePath}");
                    break;

                case Format.XML:
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    using (FileStream fs = new FileStream(filePath, FileMode.Open))
                    {
                        result = (T)xmlSerializer.Deserialize(fs);
                    }
                    Console.WriteLine($"[CustomSerializer] Объект успешно десериализован из {format} формата: {filePath}");
                    break;
            }
            return result;
        }
    }
}