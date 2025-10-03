using System;
using System.Linq;
using System.Collections.Generic;

namespace Garage
{
    class Car
    {
        private readonly string id;
        private const string MANUFACTURER = "Garage Inc.";
        private string model;
        private string year;
        private string color;
        private string regNumber;
        private static int objectCount = 0;
        private int errorCode = 0;
        
        private static string[] Models =
        [
            "Tesla Cybertruck", "Mercedes-Benz S-class", "Hyundai Solaris I",
            "ВАЗ 2107", "Audi 80", "Nissan Skyline R34",
            "Неизвестная модель"
        ];

        private static (string model, string[] years)[] ModelsYears;
        
        static Car()
        {
            ModelsYears = [
                (Models[0], GetAvailableYears(2023, 2025)),
                (Models[1], GetAvailableYears(2020, 2025)),
                (Models[2], GetAvailableYears(2010, 2017)),
                (Models[3], GetAvailableYears(1987, 2012)),
                (Models[4], GetAvailableYears(1972, 1996)),
                (Models[5], GetAvailableYears(1998, 2002)),
                (Models[6], ["Нет даты"])
            ];
            Console.WriteLine("Статический конструктор Car вызван");
        }

        private static string[] GetAvailableYears(int from, int to)
        {
            string[] result = new string[to - from + 1];
            int idx = 0;
            for (int i = from; i <= to; i++, idx++)
            {
                result[idx] = i.ToString();
            }
            return result;
        }
        
        public Car()
        {
            id = GenerateId();
            model = Models[6];
            year = "Неизвестная дата производства";
            color = "Неизвестный";
            regNumber = "Не зарегистрирован";
            objectCount++;
        }

        public Car(string model, string color, string year)
        {
            id = GenerateId();
            
            if (Models.Contains(model))
                this.model = model;
            else
            {
                this.model = Models[6];
                errorCode = 1;
            }

            if (ModelsYears.First(el => el.model == this.model).years.Contains(year))
                this.year = year;
            else
            {
                this.year = "Неизвестная дата производства";
                errorCode = 2;
            }
            
            this.color = color;
            regNumber = "Не зарегистрирован";
            objectCount++;
        }

        public Car(string model = "Неизвестная модель", string year = "2023", 
                  string color = "Белый", string regNumber = "A000AA") 
        {
            id = GenerateId();
            this.model = model;
            this.year = year;
            this.color = color;
            this.regNumber = regNumber;
            objectCount++;
        }
        
        private Car(string id, string model)
        {
            this.id = id;
            this.model = model;
            year = "2023";
            color = "Серый";
            regNumber = "Тестовый";
            objectCount++;
        }
        public static Car CreateTestCar(string id, string model)
        {
            return new Car(id, model);
        }
        
        public string Id
        {
            get { return id; }
        }

        public string Model
        {
            get { return model; }
            set 
            { 
                if (Models.Contains(value))
                    model = value;
                else
                {
                    model = Models[6];
                    errorCode = 1;
                }
            }
        }

        public string Year
        {
            get { return year; }
            set 
            { 
                if (ModelsYears.First(el => el.model == model).years.Contains(value))
                    year = value;
                else
                {
                    year = "Неизвестная дата производства";
                    errorCode = 2;
                }
            }
        }

        public string Color
        {
            get { return color; }
            set { color = value; }
        }

        public string RegNumber
        {
            get { return regNumber; }
            set { regNumber = value; }
        }

        public int ErrorCode
        {
            get { return errorCode; }
            private set { errorCode = value; }
        }
        
        public string FullInfo
        {
            get { return $"{model} {year} {color}"; }
        }
        
        public bool TryUpdateInfo(ref string newColor, out string oldColor, string newRegNumber)
        {
            oldColor = color;
            color = newColor;
            regNumber = newRegNumber;
            return true;
        }

        public int CalculateAge()
        {
            if (int.TryParse(year, out int carYear) && carYear > 1900)
            {
                return DateTime.Now.Year - carYear;
            }
            return -1;
        }
        
        public static void PrintClassInfo()
        {
            Console.WriteLine($"Класс: Car");
            Console.WriteLine($"Количество созданных объектов: {objectCount}");
            Console.WriteLine($"Доступные модели: {string.Join(", ", Models)}");
        }

        public bool NeedsMaintenance(out string recommendation)
        {
            int age = CalculateAge();
            if (age > 5)
            {
                recommendation = "Рекомендуется пройти полное техническое обслуживание";
                return true;
            }
            else if (age > 2)
            {
                recommendation = "Рекомендуется проверить основные системы";
                return true;
            }
            else
            {
                recommendation = "Техническое обслуживание не требуется";
                return false;
            }
        }
        
        public override bool Equals(object obj)
        {
            if (obj is Car other)
            {
                return id == other.id && 
                       model == other.model && 
                       year == other.year;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (id?.GetHashCode() ?? 0);
                hash = hash * 23 + (model?.GetHashCode() ?? 0);
                hash = hash * 23 + (year?.GetHashCode() ?? 0);
                return hash;
            }
        }

        public override string ToString()
        {
            return $"Car [ID: {id}, Model: {model}, Year: {year}, Color: {color}, RegNumber: {regNumber}, ErrorCode: {errorCode}]";
        }

        private string GenerateId()
        {
            return Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            
            Car car1 = new Car();
            Car car2 = new Car("Tesla Cybertruck", "Красный", "2024");
            Car car3 = new Car("Nissan Skyline R34", "2000", "Синий", "B123CD");
            Car testCar = Car.CreateTestCar("TEST123", "Audi 80");
            
            Console.WriteLine("Созданные автомобили:");
            Console.WriteLine(car1.ToString());
            Console.WriteLine(car2.ToString());
            Console.WriteLine(car3.ToString());
            Console.WriteLine(testCar.ToString());
            
            car2.Color = "Зеленый";
            Console.WriteLine($"car2 после изменения цвета: {car2.Color}");
            Console.WriteLine($"car2 полная информация: {car2.FullInfo}");
            
            string newColor = "Черный";
            string oldColor;
            bool updated = car3.TryUpdateInfo(ref newColor, out oldColor, "C456EF");
            Console.WriteLine($"\ncar3 обновлен: {updated}, Старый цвет: {oldColor}, Новый цвет: {car3.Color}");
            
            Console.WriteLine($"\nВозраст car2: {car2.CalculateAge()} лет");
            Console.WriteLine($"Возраст car3: {car3.CalculateAge()} лет");
            
            Car.PrintClassInfo();
            
            Console.WriteLine($"\ncar1 equals car2: {car1.Equals(car2)}");
            
            Car[] cars = [car1, car2, car3, testCar];
            
            string targetModel = "Nissan Skyline R34";
            var filteredByModel = cars.Where(c => c.Model == targetModel).ToArray();
            Console.WriteLine($"\nАвтомобили марки '{targetModel}':");
            foreach (var car in filteredByModel)
            {
                Console.WriteLine($"  - {car}");
            }
            
            int yearsThreshold = 10;
            var oldCars = cars.Where(c => c.CalculateAge() > yearsThreshold).ToArray();
            Console.WriteLine($"\nАвтомобили старше {yearsThreshold} лет:");
            foreach (var car in oldCars)
            {
                Console.WriteLine($"  - {car.Model} ({car.Year}) - {car.CalculateAge()} лет");
            }
            
            var anonymousCar = new
            {
                Model = "Anonymous Model",
                Year = "2024",
                Color = "Transparent"
            };
            Console.WriteLine($"\nАнонимный автомобиль: {anonymousCar}");
            
        }
    }
}