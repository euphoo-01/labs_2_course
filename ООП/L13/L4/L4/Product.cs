using System;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace L4
{
    [Serializable]
    [XmlInclude(typeof(Food))]
    [XmlInclude(typeof(Sweet))]
    [XmlInclude(typeof(Flowers))]
    [XmlInclude(typeof(Cake))]
    [XmlInclude(typeof(Watch))]
    [XmlInclude(typeof(Candies))]
    [JsonDerivedType(typeof(Flowers), typeDiscriminator: "Flowers")]
    [JsonDerivedType(typeof(Cake), typeDiscriminator: "Cake")]
    [JsonDerivedType(typeof(Watch), typeDiscriminator: "Watch")]
    [JsonDerivedType(typeof(Candies), typeDiscriminator: "Candies")]
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
}
