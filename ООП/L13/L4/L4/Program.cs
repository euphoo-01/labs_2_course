using System;

namespace L4
{
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