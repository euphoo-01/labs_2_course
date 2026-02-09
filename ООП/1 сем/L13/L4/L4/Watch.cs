using System;
using System.Collections.Generic;

namespace L4
{
    [Serializable]
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
}
