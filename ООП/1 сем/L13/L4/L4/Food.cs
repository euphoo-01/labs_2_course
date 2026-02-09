using System;

namespace L4
{
    [Serializable]
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
}
