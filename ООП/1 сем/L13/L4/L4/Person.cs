using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace L4
{
    [Serializable]
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public decimal Balance { get; set; }
        public List<Product> Inventory { get; set; } = new List<Product>();

        [NonSerialized]
        [XmlIgnore]
        [JsonIgnore]
        public string SecretPassword = "MySecretPassword123";

        public Person(string name, int age, decimal balance)
        {
            Name = name;
            Age = age;
            Balance = balance;
        }

        public Person() { } // Required for XmlSerializer

        public void ShowInventory()
        {
            Console.WriteLine($"\nИнвентарь {Name}:");
            Console.WriteLine($"Секрет (должен быть пустым/null после десериализации): '{SecretPassword}'");
            
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
}