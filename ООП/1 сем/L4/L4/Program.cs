namespace L4
{
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

    public abstract class Product : IPurchasable
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
        public int Weight { get; set; }
        public int Calories { get; set; }
        public DateTime ExpirationDate { get; set; }

        public abstract void Eat();

        public override string ToString()
        {
            return base.ToString() +
                   $", Вес: {Weight}г, Калории: {Calories}, Срок годности: {ExpirationDate:dd.MM.yyyy}";
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
                   $", Тип: {Type}, Количество: {Quantity}, Цвет: {Color}, Увядание: {Expiration:dd.MM.yyyy}";
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
            return base.ToString() + $", Начинка: {Filling}, Диаметр: {Diameter}см";
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
            return base.ToString() + $", Механизм: {Mechanism}, Материал: {Material}, {waterproof}";
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
            return base.ToString() + $", Вкус: {Flavor}, Количество штук: {PiecesCount}";
        }

        public override string GetDescription()
        {
            return $"Конфеты: {Name}, Вкус: {Flavor}, {PiecesCount} шт. в упаковке";
        }
    }

    public class Printer
    {
        public void IAmPrinting(IPurchasable someobj)
        {
            if (someobj == null)
            {
                throw new Exception("Объект для печати не задан!");
                return;
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

            Flowers roses = new Flowers
            {
                Name = "Розы красные",
                Price = 21,
                Manufacturer = "Белгосцветы",
                Type = "Розы",
                Quantity = 7,
                Color = "Красный",
                Expiration = DateTime.Now.AddDays(5)
            };

            Cake birthdayCake = new Cake
            {
                Name = "Наполеон",
                Price = 75,
                Manufacturer = "Кондитерская 'Тъерри'",
                Weight = 2000,
                Calories = 3500,
                ExpirationDate = DateTime.Now.AddDays(3),
                Cooker = "Иван Петров",
                SugarPercent = 40,
                Filling = "заварной крем",
                Diameter = 25
            };

            Watch smartWatch = new Watch
            {
                Name = "Watch SE 3",
                Price = 350,
                Manufacturer = "Apple",
                Mechanism = "электронный",
                Material = "нержавеющая сталь",
                IsWaterproof = true
            };

            Candies chocolates = new Candies
            {
                Name = "M&M's",
                Price = 3,
                Manufacturer = "Mars Inc.",
                Weight = 100,
                Calories = 400,
                ExpirationDate = DateTime.Now.AddMonths(6),
                Cooker = "John Snow",
                SugarPercent = 60,
                Flavor = "ассорти",
                PiecesCount = 15
            };

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

            Product product = smartWatch;
            Food food = chocolates;

            product.Buy(person);
            if (person.Inventory.Contains(chocolates))
            {
                food.Eat();
            }

            foreach (var item in purchasables)
            {
                if (item is Food foodItem)
                {
                    Console.WriteLine($"{foodItem.Name} это еда. Можно съесть");
                    if (person.Inventory.Contains(foodItem))
                    {
                        foodItem.Eat();
                    }
                    else
                    {
                        Console.WriteLine($"{foodItem.Name} нет в инвентаре. Чтобы съесть нужно купить");
                    }
                }

                if (item is Sweet sweetItem)
                {
                    Console.WriteLine($"{sweetItem.Name} это сладкое");
                }
                else
                {
                    Console.WriteLine($"{item.Name} это НЕ сладкое");
                }

                Watch watchItem = item as Watch;
                if (watchItem != null)
                {
                    Console.WriteLine($"{watchItem.Name}, механизм: {watchItem.Mechanism}");
                }
            }

            Console.WriteLine();

            foreach (var item in purchasables)
            {
                string interfaceDescription = item.GetDescription();

                if (item is Product productItem)
                {
                    string classDescription = productItem.GetDescription();
                    Console.WriteLine($"Интерфейс: {interfaceDescription}");
                    Console.WriteLine($"Класс: {classDescription}");
                    Console.WriteLine();
                }
            }

            foreach (var item in purchasables)
            {
                Console.WriteLine(item);
            }

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