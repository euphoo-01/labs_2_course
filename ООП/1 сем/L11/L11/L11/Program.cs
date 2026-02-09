using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Reflection
{

    public class TestAttr : System.Attribute;
    
    public interface IVehicle
    {
        void Drive();
    }

    public class Car : IVehicle
    {
        public string Brand;
        public string Model { get; set; }
        public int Year { get; set; }
        private decimal _price;

        public Car() { }
        [TestAttr]
        public Car(string brand, string model, int year, decimal price)
        {
            Brand = brand;
            Model = model;
            Year = year;
            _price = price;
        }

        [TestAttr]
        public void Drive()
        {
            Console.WriteLine($"{Brand} {Model} поехала!");
        }

        public void UpdateInfo(string newColor, int kilometers)
        {
            Console.WriteLine($"Авто обновлено: Цвет {newColor}, Пробег {kilometers}");
        }

        private void SecretMethod() { }
    }

    public static class Reflector
    {
        private static string FilePath = "reflection_output.txt";

        public static void ClearFile()
        {
            File.WriteAllText(FilePath, string.Empty);
        }

        private static void WriteToFile(string title, IEnumerable<string> data)
        {
            using (StreamWriter sw = new StreamWriter(FilePath, true, Encoding.UTF8))
            {
                sw.WriteLine($"--- {title} ---");
                if (data == null || !data.Any()) sw.WriteLine("Нет данных");
                else foreach (var item in data) sw.WriteLine(item);
                sw.WriteLine();
            }
        }

        public static void GetAssemblyName(string typeName)
        {
            Type t = Type.GetType(typeName);
            if (t == null) return;
            WriteToFile($"Сборка {typeName}", new List<string> { t.Assembly.FullName });
        }

        public static void HasPublicConstructors(string typeName)
        {
            Type t = Type.GetType(typeName);
            if (t == null) return;
            bool hasCtors = t.GetConstructors(BindingFlags.Public | BindingFlags.Instance).Any();
            WriteToFile($"Конструкторы у {typeName}", new List<string> { hasCtors ? "Есть" : "Нет" });
        }

        public static IEnumerable<string> GetPublicMethods(string typeName)
        {
            Type t = Type.GetType(typeName);
            if (t == null) return new List<string>();
            var methods = t.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Select(m => m.Name);
            WriteToFile($"Методы {typeName}", methods);
            return methods;
        }

        public static IEnumerable<string> GetFieldsAndProperties(string typeName)
        {
            Type t = Type.GetType(typeName);
            if (t == null) return new List<string>();
            var fields = t.GetFields().Select(f => "Поле: " + f.Name);
            var props = t.GetProperties().Select(p => "Свойство: " + p.Name);
            var result = fields.Concat(props);
            WriteToFile($"Поля и свойства {typeName}", result);
            return result;
        }

        public static IEnumerable<string> GetInterfaces(string typeName)
        {
            Type t = Type.GetType(typeName);
            if (t == null) return new List<string>();
            var interfaces = t.GetInterfaces().Select(i => i.Name);
            WriteToFile($"Интерфейсы {typeName}", interfaces);
            return interfaces;
        }

        public static void GetMethodsByParamType(string typeName, string paramType)
        {
            Type t = Type.GetType(typeName);
            if (t == null) return;
            var result = new List<string>();
            foreach (var m in t.GetMethods())
            {
                if (m.GetParameters().Any(p => p.ParameterType.Name == paramType))
                    result.Add(m.Name);
            }
            WriteToFile($"Методы {typeName} с параметром {paramType}", result);
        }

        public static void InvokeMethod(object obj, string methodName, int mode)
        {
            Type t = obj.GetType();
            MethodInfo method = t.GetMethod(methodName);
            if (method == null) return;

            ParameterInfo[] paramsInfo = method.GetParameters();
            object[] args = new object[paramsInfo.Length];

            if (mode == 1)
            {
                if (File.Exists("params.txt"))
                {
                    string[] lines = File.ReadAllLines("params.txt");
                    for (int i = 0; i < paramsInfo.Length && i < lines.Length; i++)
                    {
                        args[i] = Convert.ChangeType(lines[i], paramsInfo[i].ParameterType);
                    }
                    Console.WriteLine($"Вызов {methodName} (из файла):");
                    method.Invoke(obj, args);
                }
            }
            else if (mode == 2)
            {
                Random rnd = new Random();
                for (int i = 0; i < paramsInfo.Length; i++)
                {
                    Type pt = paramsInfo[i].ParameterType;
                    if (pt == typeof(int)) args[i] = rnd.Next(0, 1000);
                    else if (pt == typeof(string)) args[i] = "RandomStr_" + rnd.Next(1, 100);
                    else if (pt == typeof(bool)) args[i] = rnd.Next(0, 2) == 1;
                    else if (pt == typeof(decimal)) args[i] = (decimal)rnd.NextDouble() * 1000;
                }
                Console.WriteLine($"Вызов {methodName} (генерация):");
                method.Invoke(obj, args);
            }
        }
        public static T Create<T>() where T : new()
        {
            return Activator.CreateInstance<T>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Reflector.ClearFile();
            File.WriteAllLines("params.txt", new[] { "Зеленый металлик", "75000" });

            string carType = "Reflection.Car";
            string strType = "System.String";

            Console.WriteLine("Car:");
            Reflector.GetAssemblyName(carType);
            Reflector.HasPublicConstructors(carType);
            Reflector.GetPublicMethods(carType);
            Reflector.GetFieldsAndProperties(carType);
            Reflector.GetInterfaces(carType);
            Reflector.GetMethodsByParamType(carType, "String");

            Console.WriteLine("String:");
            Reflector.GetPublicMethods(strType);

            Console.WriteLine("\nCreate:");
            Car myCar = Reflector.Create<Car>();
            myCar.Brand = "Toyota";
            myCar.Model = "Supra";
            Console.WriteLine($"Создана машина: {myCar.Brand} {myCar.Model}");

            Console.WriteLine("\nInvoke:");
            Reflector.InvokeMethod(myCar, "UpdateInfo", 1);
            Reflector.InvokeMethod(myCar, "UpdateInfo", 2);

            Console.WriteLine("\nДанные сохранены в reflection_output.txt");
            Console.ReadKey();
        }
    }
}