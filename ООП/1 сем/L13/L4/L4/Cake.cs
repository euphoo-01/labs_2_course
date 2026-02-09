using System;
using System.Collections.Generic;

namespace L4
{
    [Serializable]
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
}
