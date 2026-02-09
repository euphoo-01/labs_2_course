using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab10
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string Color { get; set; }
        public decimal Price { get; set; }

        public override string ToString()
        {
            return $"{Brand} {Model} ({Year}), {Color}, {Price}";
        }
    }

    public class Owner
    {
        public string Name { get; set; }
        public int CarId { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string[] months =
            {
                "June", "July", "May", "December", "January", "February", "March", "April", "August", "September",
                "October", "November"
            };

            Console.WriteLine("Исходный: " + string.Join(", ", months));

            var len4 = months.Where(m => m.Length == 4);
            Console.WriteLine("Длина 4: " + string.Join(", ", len4));

            string[] target = { "December", "January", "February", "June", "July", "August" };
            var seasons = months.Where(m => target.Contains(m));
            Console.WriteLine("Зима/Лето: " + string.Join(", ", seasons));

            var alpha = months.OrderBy(m => m);
            Console.WriteLine("Алфавит: " + string.Join(", ", alpha));

            var uCount = months.Count(m => m.Contains("u") && m.Length >= 4);
            Console.WriteLine("С буквой 'u' и длиной >=4: " + uCount);

            List<Car> cars = new List<Car>
            {
                new Car { Id = 1, Brand = "Toyota", Model = "Camry", Year = 2010, Color = "Black", Price = 15000 },
                new Car { Id = 2, Brand = "Toyota", Model = "Corolla", Year = 2018, Color = "White", Price = 18000 },
                new Car { Id = 3, Brand = "BMW", Model = "X5", Year = 2005, Color = "Black", Price = 12000 },
                new Car { Id = 4, Brand = "BMW", Model = "X5", Year = 2020, Color = "Blue", Price = 60000 },
                new Car { Id = 5, Brand = "Audi", Model = "A4", Year = 2015, Color = "Silver", Price = 22000 },
                new Car { Id = 6, Brand = "Lada", Model = "Vesta", Year = 2019, Color = "Red", Price = 10000 },
                new Car { Id = 7, Brand = "Toyota", Model = "Rav4", Year = 2021, Color = "White", Price = 35000 },
                new Car { Id = 8, Brand = "Mercedes", Model = "E-Class", Year = 1999, Color = "Black", Price = 5000 },
                new Car { Id = 9, Brand = "Ford", Model = "Focus", Year = 2012, Color = "Blue", Price = 9000 },
                new Car { Id = 10, Brand = "Toyota", Model = "Camry", Year = 2022, Color = "Black", Price = 40000 },
                new Car { Id = 11, Brand = "Tesla", Model = "Model 3", Year = 2023, Color = "Red", Price = 55000 }
            };

            Console.WriteLine("\nМашины Toyota:");
            var toyotas = cars.Where(c => c.Brand == "Toyota");
            foreach (var c in toyotas) Console.WriteLine(c);

            Console.WriteLine("\nBMW X5 старше 10 лет:");
            var oldBmw = from c in cars
                where c.Brand == "BMW" && c.Model == "X5" && (DateTime.Now.Year - c.Year) > 10
                select c;
            foreach (var c in oldBmw) Console.WriteLine(c);

            var blackCount = cars.Count(c => c.Color == "Black" && c.Price >= 10000 && c.Price <= 50000);
            Console.WriteLine("\nЧерные авто 10k-50k: " + blackCount);

            var oldest = cars.OrderBy(c => c.Year).First();
            Console.WriteLine("\nСамый старый: " + oldest);

            Console.WriteLine("\n5 самых новых:");
            var newest = cars.OrderByDescending(c => c.Year).Take(5);
            foreach (var c in newest) Console.WriteLine(c);

            Console.WriteLine("\nМассив по цене:");
            var byPrice = cars.OrderBy(c => c.Price).ToArray();
            foreach (var c in byPrice) Console.WriteLine($"{c.Brand} - {c.Price}");

            Console.WriteLine("\nКоличество машин по брендам:");
            var countByBrands = cars.Where(c => c.Price > 10000)
                    .GroupBy(c => c.Brand)
                    .OrderByDescending(g => g.Count())
                    .Select(g => new { Brand = g.Key, Count = g.Count() });
            foreach (var item in countByBrands) Console.WriteLine($"{item.Brand}: {item.Count}");

            Console.WriteLine("\nВладельцы:");
            List<Owner> owners = new List<Owner>
            {
                new Owner { Name = "Ivan", CarId = 1 },
                new Owner { Name = "Petr", CarId = 3 },
                new Owner { Name = "Maria", CarId = 11 }
            };
            var joined = owners.Join(cars, o => o.CarId, c => c.Id, (o, c) => $"{o.Name} владеет {c.Brand} {c.Model}");
            foreach (var j in joined) Console.WriteLine(j);

            Console.ReadKey();
        }
    }
}