using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace L4
{
    public struct Dimensions
    {
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Dimensions(double length, double width, double height)
        {
            if (length <= 0 || width <= 0 || height <= 0)
                throw new InvalidDimensionsException(length, width, height);

            Length = length;
            Width = width;
            Height = height;
        }

        public double Volume => Length * Width * Height;

        public override string ToString()
        {
            return $"{Length}×{Width}×{Height} см";
        }
    }

    public enum GiftComponentType
    {
        Flowers,
        Cake,
        Candies,
        Watch,
        Other
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Balance { get; set; }
        public List<Product> Inventory { get; set; } = new List<Product>();

        public Person(string name, int age, decimal balance)
        {
            Name = name;
            Age = age;
            Balance = balance;
        }

        public void ShowInventory()
        {
            Console.WriteLine($"\nИнвентарь {Name}:");
            if (Inventory.Count == 0)
            {
                Console.WriteLine("Инвентарь пуст");
                return;
            }

            foreach (var item in Inventory)
            {
                Console.WriteLine($"- {item.Name} ({item.Price} BYN)");
            }

            Console.WriteLine($"Баланс: {Balance} BYN");
        }
    }

    public interface IPurchasable
    {
        string Name { get; set; }
        decimal Price { get; set; }
        void Buy(Person p);
        void Sell(Person p);
        string GetDescription();
    }

    public abstract partial class Product : IPurchasable
    {
        public static ILogger Logger { get; set; } = new ConsoleLogger();

        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Manufacturer { get; set; }

        public abstract void Buy(Person p);
        public abstract void Sell(Person p);

        public virtual string GetDescription()
        {
            return $"Продукт: {Name}, Производитель: {Manufacturer}";
        }

        public override string ToString()
        {
            return $"Тип: {GetType().Name}, Название: {Name}, Цена: {Price} BYN, Производитель: {Manufacturer}";
        }

        public override bool Equals(object obj)
        {
            if (obj is Product other)
                return Name == other.Name && Manufacturer == other.Manufacturer;

            return false;
        }

        public override int GetHashCode() => HashCode.Combine(Name, Manufacturer);
    }

    public abstract class Food : Product
    {
        public int FoodWeight { get; set; }
        public int Calories { get; set; }
        public DateTime ExpirationDate { get; set; }

        public abstract void Eat();

        public override string ToString()
        {
            return base.ToString() +
                   $", Вес: {FoodWeight}г, Калории: {Calories}, Срок годности: {ExpirationDate:dd.MM.yyyy}";
        }
    }

    public abstract class Sweet : Food
    {
        public string Cooker { get; set; }
        public int SugarPercent { get; set; }

        public abstract void Bake();

        public override string ToString()
        {
            return base.ToString() + $", Повар: {Cooker}, Сахар: {SugarPercent}%";
        }
    }

    public class Flowers : Product
    {
        public string Type { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public DateTime Expiration { get; set; }

        public Flowers()
        {
            ComponentType = GiftComponentType.Flowers;
        }

        public override void Buy(Person p)
        {
            if (p.Inventory.Contains(this))
            {
                Logger.Warning($"Повторная покупка: {Name}");
                throw new GiftException($"Товар '{Name}' уже есть в инвентаре!");
            }

            if (Expiration <= DateTime.Now)
                throw new GiftException($"Цветы '{Name}' увяли. Продажа невозможна.");

            if (p.Balance < Price)
                throw new GiftException($"Недостаточно денег на покупку '{Name}'!");

            Logger.Info($"{p.Name} покупает {Name}");
            p.Inventory.Add(this);
            p.Balance -= Price;
        }

        public override void Sell(Person p)
        {
            if (!p.Inventory.Contains(this))
                throw new ComponentNotFoundException(Name);

            if (Expiration <= DateTime.Now)
                throw new GiftException($"Цветы '{Name}' испортились. Продажа невозможна.");

            Logger.Info($"{p.Name} продаёт {Name}");
            p.Inventory.Remove(this);
            p.Balance += Price;
        }

        public override string GetDescription()
        {
            return $"Цветы: {Name}, Сорт: {Type}, Цвет: {Color}";
        }

        public override string ToString()
        {
            return base.ToString() +
                   $", Тип: {Type}, Цвет: {Color}, Кол-во: {Quantity}, Увядание: {Expiration:dd.MM.yyyy}, Вес: {Weight}г, Габариты: {Size}";
        }
    }

    public sealed class Cake : Sweet
    {
        private string _filling;

        private static List<string> _availableFilling = new List<string>
        {
            "заварной крем", "меренга", "сливочный крем", "сливочно-сырный крем", "маскарпоне", "клубничная"
        };

        public Cake()
        {
            ComponentType = GiftComponentType.Cake;
        }

        public string Filling
        {
            get => _filling;
            set
            {
                if (!_availableFilling.Contains(value.ToLower()))
                    throw new InvalidFillingException(value);

                _filling = value;
            }
        }

        public int Diameter { get; set; }

        public override void Bake()
        {
            Logger.Info($"Выпекается торт '{Name}'");
        }

        public override void Eat()
        {
            if (ExpirationDate <= DateTime.Now)
                throw new GiftException($"Торт '{Name}' испорчен!");

            Logger.Info($"Едим торт {Name}");
        }

        public override void Buy(Person p)
        {
            if (p.Inventory.Contains(this))
                throw new GiftException("Товар уже есть в инвентаре!");

            if (ExpirationDate <= DateTime.Now)
                throw new GiftException($"Торт '{Name}' испорчен!");

            if (p.Balance < Price)
                throw new GiftException($"Недостаточно средств для покупки '{Name}'!");

            p.Inventory.Add(this);
            p.Balance -= Price;
        }

        public override void Sell(Person p)
        {
            if (!p.Inventory.Contains(this))
                throw new ComponentNotFoundException(Name);

            if (ExpirationDate <= DateTime.Now)
                throw new GiftException($"Продажа торта '{Name}' невозможна. Просрочен.");

            p.Inventory.Remove(this);
            p.Balance += Price;
        }

        public override string GetDescription()
        {
            return $"Торт: {Name}, Начинка: {Filling}, Диаметр: {Diameter}см";
        }

        public override string ToString()
        {
            return base.ToString() +
                   $", Начинка: {Filling}, Диаметр: {Diameter}см, Вес: {Weight}г, Габариты: {Size}";
        }
    }

    public class Watch : Product
    {
        private string _mechanism;

        private static List<string> availableMechanisms = new List<string>
        {
            "механический", "кварцевый", "электронный"
        };

        public Watch()
        {
            ComponentType = GiftComponentType.Watch;
        }

        public string Mechanism
        {
            get => _mechanism;
            set
            {
                if (!availableMechanisms.Contains(value.ToLower()))
                    throw new GiftException($"Неизвестный механизм: {value}");

                _mechanism = value;
            }
        }

        public string Material { get; set; }
        public bool IsWaterproof { get; set; }

        public override void Buy(Person p)
        {
            if (p.Inventory.Contains(this))
                throw new GiftException("Часы уже есть в инвентаре!");

            if (p.Balance < Price)
                throw new GiftException("Недостаточно денег!");

            p.Inventory.Add(this);
            p.Balance -= Price;
        }

        public override void Sell(Person p)
        {
            if (!p.Inventory.Contains(this))
                throw new ComponentNotFoundException(Name);

            p.Inventory.Remove(this);
            p.Balance += Price;
        }
    }

    public class Candies : Sweet
    {
        public string Flavor { get; set; }
        public int PiecesCount { get; set; }

        public Candies()
        {
            ComponentType = GiftComponentType.Candies;
        }

        public override void Bake()
        {
            Logger.Info($"Готовим конфеты '{Name}'");
        }

        public override void Eat()
        {
            if (ExpirationDate <= DateTime.Now)
                throw new GiftException($"Конфеты '{Name}' просрочены!");

            Logger.Info($"Едим конфеты '{Name}'");
        }

        public override void Buy(Person p)
        {
            if (p.Inventory.Contains(this))
                throw new GiftException("Конфеты уже есть в инвентаре!");

            if (ExpirationDate <= DateTime.Now)
                throw new GiftException($"Конфеты '{Name}' испортились!");

            if (p.Balance < Price)
                throw new GiftException("Недостаточно денег!");

            p.Inventory.Add(this);
            p.Balance -= Price;
        }

        public override void Sell(Person p)
        {
            if (!p.Inventory.Contains(this))
                throw new ComponentNotFoundException(Name);

            if (ExpirationDate <= DateTime.Now)
                throw new GiftException($"Конфеты '{Name}' испорчены!");

            p.Inventory.Remove(this);
            p.Balance += Price;
        }
    }

    public class GiftContainer
    {
        private readonly List<Product> components = new();

        public void AddComponent(Product component)
        {
            components.Add(component);
        }

        public bool RemoveComponent(Product component)
        {
            return components.Remove(component);
        }

        public Product GetComponent(int index)
        {
            if (index < 0 || index >= components.Count)
                throw new ComponentNotFoundException($"index={index}");

            return components[index];
        }

        public void DisplayAll()
        {
            Console.WriteLine("\n=== Все компоненты подарка ===");

            if (components.Count == 0)
            {
                Console.WriteLine("Подарок пуст");
                return;
            }

            for (int i = 0; i < components.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {components[i]}");
            }
        }

        public List<Product> GetAllComponents() => new(components);

        public int Count => components.Count;
    }

    public class GiftController
    {
        private readonly GiftContainer _gift;

        public GiftController()
        {
            _gift = new GiftContainer();
        }

        public void LoadFromTextFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл {filePath} не найден");

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                try
                {
                    var component = ParseLineToProduct(line);
                    if (component != null)
                        _gift.AddComponent(component);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка строки: {line}");
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.WriteLine($"Загружено {_gift.Count} компонентов");
        }

        private Product ParseLineToProduct(string line)
        {
            var parts = line.Split(';');
            if (parts.Length < 3) return null;

            GiftComponentType type = Enum.Parse<GiftComponentType>(parts[0]);

            return type switch
            {
                GiftComponentType.Flowers => CreateFlowers(parts),
                GiftComponentType.Cake => CreateCake(parts),
                GiftComponentType.Watch => CreateWatch(parts),
                GiftComponentType.Candies => CreateCandies(parts),
                _ => null
            };
        }

        private Dimensions ParseDimensions(string str)
        {
            var p = str.Split(',');
            return new Dimensions(double.Parse(p[0]), double.Parse(p[1]), double.Parse(p[2]));
        }

        private Flowers CreateFlowers(string[] parts)
        {
            return new Flowers
            {
                Name = parts[1],
                Price = decimal.Parse(parts[2]),
                Manufacturer = parts[3],
                Type = parts[4],
                Quantity = int.Parse(parts[5]),
                Color = parts[6],
                Expiration = DateTime.Parse(parts[7]),
                Weight = double.Parse(parts[8]),
                Size = ParseDimensions(parts[9])
            };
        }

        private Cake CreateCake(string[] parts)
        {
            return new Cake
            {
                Name = parts[1],
                Price = decimal.Parse(parts[2]),
                Manufacturer = parts[3],
                FoodWeight = int.Parse(parts[4]),
                Calories = int.Parse(parts[5]),
                ExpirationDate = DateTime.Parse(parts[6]),
                Cooker = parts[7],
                SugarPercent = int.Parse(parts[8]),
                Filling = parts[9],
                Diameter = int.Parse(parts[10]),
                Weight = double.Parse(parts[11]),
                Size = ParseDimensions(parts[12])
            };
        }

        private Watch CreateWatch(string[] parts)
        {
            return new Watch
            {
                Name = parts[1],
                Price = decimal.Parse(parts[2]),
                Manufacturer = parts[3],
                Mechanism = parts[4],
                Material = parts[5],
                IsWaterproof = bool.Parse(parts[6]),
                Weight = double.Parse(parts[7]),
                Size = ParseDimensions(parts[8])
            };
        }

        private Candies CreateCandies(string[] parts)
        {
            return new Candies
            {
                Name = parts[1],
                Price = decimal.Parse(parts[2]),
                Manufacturer = parts[3],
                FoodWeight = int.Parse(parts[4]),
                Calories = int.Parse(parts[5]),
                ExpirationDate = DateTime.Parse(parts[6]),
                Cooker = parts[7],
                SugarPercent = int.Parse(parts[8]),
                Flavor = parts[9],
                PiecesCount = int.Parse(parts[10]),
                Weight = double.Parse(parts[11]),
                Size = ParseDimensions(parts[12])
            };
        }

        public decimal CalculateTotalPrice()
        {
            return _gift.GetAllComponents().Sum(c => c.Price);
        }

        public void DisplayAll()
        {
            _gift.DisplayAll();
        }

        public Product FindLightestComponent()
        {
            var components = _gift.GetAllComponents();
            return components.Count == 0 ? null : components.OrderBy(c => c.Weight).First();
        }

        public List<Product> SortByDimensions()
        {
            return _gift.GetAllComponents().OrderBy(c => c.Size.Volume).ToList();
        }

        public void DisplaySortedByDimensions()
        {
            var sorted = SortByDimensions();
            for (int i = 0; i < sorted.Count; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {sorted[i].Name} — габариты: {sorted[i].Size}, объем: {sorted[i].Size.Volume:F2} см³");
            }
        }
    }

    public class Printer
    {
        public void IAmPrinting(IPurchasable obj)
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            Console.WriteLine($"\nИнформация о {obj.GetType().Name}:");
            Console.WriteLine(obj.ToString());
        }
    }

    class Program
    {
        public static void tryInvalidCake()
        {
            try
            {
                Cake invalidCake = new Cake
                {
                    Name = "Экспериментальный",
                    Filling = "бетон"
                };
            }
            catch (InvalidFillingException ex)
            {
                Product.Logger.Error("Ошибка начинки: " + ex.Message);
                Console.WriteLine("Ошибка начинки: " + ex.Message);
                
                throw;
            }
        }
        static void Main(string[] args)
        {
            Product.Logger = new FileLogger("log.txt");
            ConsoleLogger consoleLogger = new ConsoleLogger();
            
            Debug.Assert(args != null, "Аргументы программы не должны быть null");

            try
            {
                Console.WriteLine("=== Загрузка компонентов из файла ===");

                GiftController controller = new GiftController();
                
                controller.LoadFromTextFile("test.txt");


                controller.DisplaySortedByDimensions();

                var lightest = controller.FindLightestComponent();
                if (lightest != null)
                    Console.WriteLine($"\nСамый лёгкий компонент: {lightest.Name}, {lightest.Weight}г");

                
                Person person = new Person("Иван ИванОв", 18, 1000);
                Person person2 = new Person("Иван ИвАнов", 18, 0);

                Flowers roses = new Flowers
                {
                    Name = "Розы красные",
                    Price = 21,
                    Manufacturer = "Белгосцветы",
                    Type = "Розы",
                    Quantity = 7,
                    Color = "Красный",
                    Expiration = DateTime.Now.AddDays(5),
                    Weight = 500,
                    Size = new Dimensions(40, 20, 10)
                };

                Cake birthdayCake = new Cake
                {
                    Name = "Наполеон",
                    Price = 75,
                    Manufacturer = "Кондитерская 'Тъерри'",
                    FoodWeight = 2000,
                    Calories = 3500,
                    ExpirationDate = DateTime.Now.AddDays(3),
                    Cooker = "Иван Петров",
                    SugarPercent = 40,
                    Filling = "заварной крем",
                    Diameter = 25,
                    Weight = 2100,
                    Size = new Dimensions(25, 25, 10)
                };

                Watch smartWatch = new Watch
                {
                    Name = "Watch SE 3",
                    Price = 350,
                    Manufacturer = "Apple",
                    Mechanism = "электронный",
                    Material = "нержавеющая сталь",
                    IsWaterproof = true,
                    Weight = 150,
                    Size = new Dimensions(4, 4, 1)
                };

                Candies chocolates = new Candies
                {
                    Name = "M&M's",
                    Price = 3,
                    Manufacturer = "Mars Inc.",
                    FoodWeight = 100,
                    Calories = 400,
                    ExpirationDate = DateTime.Now.AddMonths(6),
                    Cooker = "John Snow",
                    SugarPercent = 60,
                    Flavor = "ассорти",
                    PiecesCount = 15,
                    Weight = 100,
                    Size = new Dimensions(15, 8, 2)
                };
                
                IPurchasable[] purch = { roses, birthdayCake, smartWatch, chocolates };
                Printer printer = new Printer();

                foreach (var item in purch)
                    printer.IAmPrinting(item);

                
                roses.Buy(person);
                birthdayCake.Buy(person);
                chocolates.Buy(person);

                person.ShowInventory();

                roses.Sell(person);
                chocolates.Sell(person);

                person.ShowInventory();

                
                
                Program.tryInvalidCake();
            }
            // catch (InvalidFillingException ex)
            // {
            //     Product.Logger.Error("Ошибка начинки: " + ex.Message);
            //     Console.WriteLine("Ошибка начинки: " + ex.Message);
            //     
            //     throw;
            // }
            catch (ComponentNotFoundException ex)
            {
                Product.Logger.Error(ex.Message);
                consoleLogger.Error("Нет компонента: " + ex.Message);
            }
            catch (FileNotFoundException ex)
            {
                Product.Logger.Error("Файл не найден: " + ex.Message);
                Console.WriteLine("Ошибка файла: " + ex.Message);
            }
            catch (GiftException ex)
            {
                Product.Logger.Error("GiftException: " + ex.Message);
                Console.WriteLine("GiftException: " + ex.Message);
            }
            catch (Exception ex)
            {
                Product.Logger.Error("Неожиданное исключение: " + ex.Message);
                Console.WriteLine("Неожиданное исключение: " + ex.Message);
            }
            finally
            {
                Product.Logger.Info("Программа завершила работу (finally).");
                Console.WriteLine("\nРабота программы завершена (finally).");
            }
        }
    }
}