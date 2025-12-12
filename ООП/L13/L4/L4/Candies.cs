using System;

namespace L4
{
    [Serializable]
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
}
