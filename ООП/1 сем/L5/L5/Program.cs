namespace L4
{
    public enum GiftComponentType
    {
        Flowers,
        Cake,
        Candies,
        Watch,
        Other
    }

    public struct Dimensions
    {
        public double Length { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }

        public Dimensions(double length, double width, double height)
        {
            Length = length;
            Width = width;
            Height = height;
        }

        public double Volume
        {
            get { return Length * Width * Height; }
        }

        public override string ToString()
        {
            return $"{Length}×{Width}×{Height} см";
        }
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
            {
                return Name == other.Name && Manufacturer == other.Manufacturer;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Manufacturer);
        }
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

        public override string GetDescription()
        {
            return $"Пищевой продукт: {Name}, Калорийность: {Calories} ккал";
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
                Console.WriteLine($"Товар уже есть в инвентаре. Зачем копить барахло?");
            }
            else if (p.Balance >= this.Price && this.Expiration > DateTime.Now)
            {
                Console.WriteLine($"{p.Name} покупает {this.Name} за {this.Price} BYN");
                p.Balance -= this.Price;
                p.Inventory.Add(this);
            }
            else if (Expiration <= DateTime.Now)
            {
                Console.WriteLine($"Цветы {this.Name} увяли. Реализация невозможна");
            }
            else
            {
                Console.WriteLine($"У {p.Name} недостаточный баланс для покупки");
            }
        }

        public override void Sell(Person p)
        {
            if (Expiration > DateTime.Now && p.Inventory.Contains(this))
            {
                Console.WriteLine($"Продаем цветы: {Name} по цене {Price} BYN");
                p.Balance += this.Price;
                p.Inventory.Remove(this);
            }
            else if (Expiration <= DateTime.Now)
            {
                Console.WriteLine($"Срок годности цветов: {Name} истёк. Продажа невозможна");
            }
            else
            {
                Console.WriteLine("Этого предмета нет в инвентаре. Нужно купить чтобы продать");
            }
        }

        public override string ToString()
        {
            return base.ToString() +
                   $", Тип: {Type}, Количество: {Quantity}, Цвет: {Color}, Увядание: {Expiration:dd.MM.yyyy}, " +
                   $"Вес: {Weight}г, Габариты: {Size}";
        }

        public override string GetDescription()
        {
            return $"Цветы: {Name}, Сорт: {Type}, Цвет: {Color}";
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
            get { return _filling; }
            set
            {
                if (_availableFilling.Contains(value.ToLower()))
                    _filling = value;
                else
                    throw new ArgumentException($"Начинка '{value}' не доступна");
            }
        }

        public int Diameter { get; set; }

        public override void Bake()
        {
            Console.WriteLine($"Выпекаем торт '{Name}' с начинкой {Filling}");
        }

        public override void Eat()
        {
            if (ExpirationDate > DateTime.Now)
            {
                Console.WriteLine($"Едим торт '{Name}' - очень вкусно!");
            }
            else
            {
                Console.WriteLine($"Торт '{Name}' просрочен! Нельзя есть!");
            }
        }

        public override void Buy(Person p)
        {
            if (p.Inventory.Contains(this))
            {
                Console.WriteLine($"Товар уже есть в инвентаре. Зачем копить барахло?");
            }
            else if (p.Balance >= this.Price && this.ExpirationDate > DateTime.Now)
            {
                Console.WriteLine($"{p.Name} покупает {this.Name} за {this.Price} BYN");
                p.Balance -= this.Price;
                p.Inventory.Add(this);
            }
            else if (ExpirationDate <= DateTime.Now)
            {
                Console.WriteLine($"Торт {this.Name} испортился. Реализация невозможна");
            }
            else
            {
                Console.WriteLine($"У {p.Name} недостаточный баланс для покупки");
            }
        }

        public override void Sell(Person p)
        {
            if (ExpirationDate > DateTime.Now && p.Inventory.Contains(this))
            {
                Console.WriteLine($"Продаем торт: {Name} по цене {Price} BYN");
                p.Inventory.Remove(this);
                p.Balance += this.Price;
            }
            else if (ExpirationDate <= DateTime.Now)
            {
                Console.WriteLine($"Срок годности торта: {Name} истёк. Продажа невозможна");
            }
            else
            {
                Console.WriteLine("Этого предмета нет в инвентаре. Нужно купить чтобы продать");
            }
        }

        public override string ToString()
        {
            return base.ToString() + $", Начинка: {Filling}, Диаметр: {Diameter}см, Вес: {Weight}г, Габариты: {Size}";
        }

        public override string GetDescription()
        {
            return $"Торт: {Name}, Начинка: {Filling}, Диаметр: {Diameter}см";
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
            get { return this._mechanism; }
            set
            {
                if (availableMechanisms.Contains(value.ToLower()))
                {
                    this._mechanism = value;
                }
                else
                {
                    throw new Exception("Такой тип механизма не предоставляется в оборот.");
                }
            }
        }

        public string Material { get; set; }
        public bool IsWaterproof { get; set; }

        public override void Buy(Person p)
        {
            if (p.Inventory.Contains(this))
            {
                Console.WriteLine($"Товар уже есть в инвентаре. Зачем копить барахло?");
            }
            else if (p.Balance >= this.Price)
            {
                Console.WriteLine($"{p.Name} покупает {this.Name} за {this.Price} BYN");
                p.Balance -= this.Price;
                p.Inventory.Add(this);
            }
            else
            {
                Console.WriteLine($"У {p.Name} недостаточный баланс для покупки");
            }
        }

        public override void Sell(Person p)
        {
            if (p.Inventory.Contains(this))
            {
                Console.WriteLine($"Продаем часы: {Name} по цене {Price} BYN");
                p.Inventory.Remove(this);
                p.Balance += this.Price;
            }
            else
            {
                Console.WriteLine("Этого предмета нет в инвентаре. Нужно купить чтобы продать");
            }
        }

        public override string ToString()
        {
            string waterproof = IsWaterproof ? "водонепроницаемые" : "не водонепроницаемые";
            return base.ToString() + $", Механизм: {Mechanism}, Материал: {Material}, {waterproof}, " +
                   $"Вес: {Weight}г, Габариты: {Size}";
        }

        public override string GetDescription()
        {
            return $"Часы: {Name}, {Mechanism} механизм, {Material}";
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
            Console.WriteLine($"Готовим конфеты '{Name}' с вкусом {Flavor}");
        }

        public override void Eat()
        {
            if (ExpirationDate > DateTime.Now)
            {
                Console.WriteLine($"Едим конфеты '{Name}' - сладко!");
            }
            else
            {
                Console.WriteLine($"Конфеты '{Name}' просрочены! Нельзя есть!");
            }
        }

        public override void Buy(Person p)
        {
            if (p.Inventory.Contains(this))
            {
                Console.WriteLine($"Товар уже есть в инвентаре. Зачем копить барахло?");
            }
            else if (p.Balance >= this.Price && this.ExpirationDate > DateTime.Now)
            {
                Console.WriteLine($"{p.Name} покупает {this.Name} за {this.Price} BYN");
                p.Balance -= this.Price;
                p.Inventory.Add(this);
            }
            else if (ExpirationDate <= DateTime.Now)
            {
                Console.WriteLine($"Конфеты {this.Name} испортились. Реализация невозможна");
            }
            else
            {
                Console.WriteLine($"У {p.Name} недостаточный баланс для покупки");
            }
        }

        public override void Sell(Person p)
        {
            if (ExpirationDate > DateTime.Now && p.Inventory.Contains(this))
            {
                Console.WriteLine($"Продаем конфеты: {Name} по цене {Price} BYN");
                p.Inventory.Remove(this);
                p.Balance += this.Price;
            }
            else if (ExpirationDate <= DateTime.Now)
            {
                Console.WriteLine($"Срок годности конфет: {Name} истёк. Продажа невозможна");
            }
            else
            {
                Console.WriteLine("Этого предмета нет в инвентаре. Нужно купить чтобы продать");
            }
        }

        public override string ToString()
        {
            return base.ToString() +
                   $", Вкус: {Flavor}, Количество штук: {PiecesCount}, Вес: {Weight}г, Габариты: {Size}";
        }

        public override string GetDescription()
        {
            return $"Конфеты: {Name}, Вкус: {Flavor}, {PiecesCount} шт. в упаковке";
        }
    }

    public class GiftContainer
    {
        private List<Product> components;

        public GiftContainer()
        {
            components = new List<Product>();
        }

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
            if (index >= 0 && index < components.Count)
                return components[index];
            return null;
        }

        public void SetComponent(int index, Product component)
        {
            if (index >= 0 && index < components.Count)
                components[index] = component;
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

        public List<Product> GetAllComponents()
        {
            return new List<Product>(components);
        }

        public int Count => components.Count;
    }

    public class GiftController
    {
        private GiftContainer _gift;

        public GiftController()
        {
            _gift = new GiftContainer();
        }

        public GiftController(GiftContainer container)
        {
            _gift = container;
        }

        public void LoadFromTextFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception($"Файл {filePath} не найден");
            }

            var lines = File.ReadAllLines(filePath);

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                try
                {
                    var component = ParseLineToProduct(line);
                    if (component != null)
                    {
                        _gift.AddComponent(component);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка на строке: {line}");
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
            }

            Console.WriteLine($"Загружено {_gift.Count} компонентов из файла {filePath}");
        }

        private Product ParseLineToProduct(string line)
        {
            var parts = line.Split(';');
            if (parts.Length < 3)
                return null;

            string type = parts[0].Trim();

            string name = parts[1].Trim();
            decimal price = decimal.Parse(parts[2].Trim());
            
            

            switch (Enum.Parse<GiftComponentType>(type))
            {
                case GiftComponentType.Flowers:
                    return CreateFlowers(parts);
                case GiftComponentType.Cake:
                    return CreateCake(parts);
                case GiftComponentType.Watch:
                    return CreateWatch(parts);
                case GiftComponentType.Candies:
                    return CreateCandies(parts);
                default:
                    Console.WriteLine($"Неизвестный тип: {type}");
                    return null;
            }
        }

        private Flowers CreateFlowers(string[] parts)
        {
            if (parts.Length < 10) return null;

            var flowers = new Flowers
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
            return flowers;
        }

        private Cake CreateCake(string[] parts)
        {
            if (parts.Length < 13) return null;

            var cake = new Cake
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
            return cake;
        }

        private Watch CreateWatch(string[] parts)
        {
            if (parts.Length < 9) return null;

            var watch = new Watch
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
            return watch;
        }

        private Candies CreateCandies(string[] parts)
        {
            if (parts.Length < 13) return null;

            var candies = new Candies
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
            return candies;
        }

        private Dimensions ParseDimensions(string dimensionsStr)
        {
            var dimParts = dimensionsStr.Split(',');
            if (dimParts.Length == 3)
            {
                return new Dimensions(
                    double.Parse(dimParts[0]),
                    double.Parse(dimParts[1]),
                    double.Parse(dimParts[2])
                );
            }

            return new Dimensions(0, 0, 0);
        }

        public decimal CalculateTotalPrice()
        {
            return _gift.GetAllComponents().Sum(component => component.Price);
        }

        public Product FindLightestComponent()
        {
            var components = _gift.GetAllComponents();
            if (components.Count == 0) return null;

            return components.OrderBy(c => c.Weight).First();
        }

        public List<Product> SortByDimensions()
        {
            return _gift.GetAllComponents()
                .OrderBy(c => c.Size.Volume)
                .ToList();
        }

        public void DisplaySortedByDimensions()
        {
            var sorted = SortByDimensions();

            for (int i = 0; i < sorted.Count; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {sorted[i].Name} - Габариты: {sorted[i].Size} (Объем: {sorted[i].Size.Volume:F2} см³)");
            }
        }

        public void AddToGift(Product component)
        {
            _gift.AddComponent(component);
        }

        public void DisplayGiftInfo()
        {
            _gift.DisplayAll();
            Console.WriteLine($"\nОбщая стоимость подарка: {CalculateTotalPrice()} BYN");

            var lightest = FindLightestComponent();
            if (lightest != null)
            {
                Console.WriteLine($"Самый легкий компонент: {lightest.Name} ({lightest.Weight}г)");
            }
        }
    }

    public class Printer
    {
        public void IAmPrinting(IPurchasable someobj)
        {
            if (someobj == null)
            {
                throw new Exception("Объект для печати не задан!");
            }

            if (someobj is Flowers flowers)
            {
                Console.WriteLine("Информация о цветах:");
            }
            else if (someobj is Cake cake)
            {
                Console.WriteLine("Информация о торте:");
            }
            else if (someobj is Watch watch)
            {
                Console.WriteLine("Информация о часах:");
            }
            else if (someobj is Candies candies)
            {
                Console.WriteLine("Информация о конфетах:");
            }
            else
            {
                Console.WriteLine("Информацию о товаре:");
            }

            Console.WriteLine(someobj.ToString());
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Person person = new Person("Иван ИванОв", 18, 1000);
            Person person2 = new Person("Иван ИвАнов", 18, 0);


            GiftController giftController = new GiftController();
            giftController.LoadFromTextFile("test.txt");
            giftController.DisplayGiftInfo();
            giftController.DisplaySortedByDimensions();

            var lightest = giftController.FindLightestComponent();
            if (lightest != null)
            {
                Console.WriteLine($"\nСамый легкий компонент в подарке: {lightest.Name} ({lightest.Weight}г)");
            }

            Console.WriteLine("\n=== Демонстрация отдельных операций ===");

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

            var container = new GiftContainer();
            container.AddComponent(roses);
            container.AddComponent(chocolates);

            Console.WriteLine($"Количество компонентов в контейнере: {container.Count}");
            Console.WriteLine($"Первый компонент: {container.GetComponent(0)?.Name}");

            IPurchasable[] purchasables = { roses, birthdayCake, smartWatch, chocolates };
            Printer printer = new Printer();

            foreach (var item in purchasables)
            {
                printer.IAmPrinting(item);
            }

            roses.Buy(person);
            roses.Buy(person2);
            birthdayCake.Buy(person);
            smartWatch.Buy(person);
            chocolates.Buy(person);

            person.ShowInventory();

            roses.Sell(person);
            chocolates.Sell(person);

            person.ShowInventory();

            try
            {
                Cake invalidCake = new Cake
                {
                    Name = "Экспериментальный",
                    Filling = "невозможная начинка"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            try
            {
                Watch invalidWatch = new Watch
                {
                    Name = "Неправильные часы",
                    Mechanism = "паровой"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}