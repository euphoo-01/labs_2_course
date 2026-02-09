using System;

namespace L4
{
    [Serializable]
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
}
