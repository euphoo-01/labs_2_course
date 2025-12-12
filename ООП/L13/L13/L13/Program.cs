using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;
using L4;

namespace L13
{
    class Program
    {
        static void Main(string[] args)
        {
            var cake = new Cake
            {
                Name = "Прага",
                Price = 50,
                Manufacturer = "Хлебозавод",
                Weight = 1000,
                Calories = 4000,
                ExpirationDate = DateTime.Now.AddDays(5),
                Cooker = "Мария",
                SugarPercent = 45,
                Filling = "сливочный крем",
                Diameter = 20
            };

            var person = new Person("Алексей", 25, 500);
            person.Inventory.Add(cake);
            person.SecretPassword = "ShouldNotBeSeen";

            Console.WriteLine("Исходный объект:");
            person.ShowInventory();
            Console.WriteLine();
            
            // binary
            string binFile = "person.bin";
            CustomSerializer.Serialize(person, binFile, CustomSerializer.Format.Binary);
            var personBin = CustomSerializer.Deserialize<Person>(binFile, CustomSerializer.Format.Binary);
            Console.WriteLine("Результат Binary (Secret д.б. null/empty):");
            if (personBin != null)
                personBin.ShowInventory();
            else
                Console.WriteLine("Binary deserialization returned null (skipped).");

            // SOAP
            CustomSerializer.Serialize(person, "person.soap", CustomSerializer.Format.SOAP);
            
            // JSON
            string jsonFile = "person.json";
            CustomSerializer.Serialize(person, jsonFile, CustomSerializer.Format.JSON);
            var personJson = CustomSerializer.Deserialize<Person>(jsonFile, CustomSerializer.Format.JSON);
            Console.WriteLine("Результат JSON:");
            personJson.ShowInventory();

            // XML
            string xmlFile = "person.xml";
            CustomSerializer.Serialize(person, xmlFile, CustomSerializer.Format.XML);
            var personXml = CustomSerializer.Deserialize<Person>(xmlFile, CustomSerializer.Format.XML);
            Console.WriteLine("Результат XML:");
            personXml.ShowInventory();
            
            
            
            List<Product> products = new List<Product>
            {
                cake,
                new Flowers { Name = "Тюльпаны", Price = 15, Manufacturer = "Holland", Type = "Тюльпан", Color = "Желтый", Quantity = 5, Expiration = DateTime.Now.AddDays(7) },
                new Watch { Name = "Rolex", Price = 5000, Manufacturer = "Rolex SA", Mechanism = "механический", Material = "Золото", IsWaterproof = true }
            };

            string listFile = "products.xml";
            CustomSerializer.Serialize(products, listFile, CustomSerializer.Format.XML);
            var loadedProducts = CustomSerializer.Deserialize<List<Product>>(listFile, CustomSerializer.Format.XML);
            
            Console.WriteLine($"Загружено объектов: {loadedProducts.Count}");
            foreach (var p in loadedProducts)
            {
                Console.WriteLine(p);
            }

            
            
            Thread serverThread = new Thread(StartServer);
            serverThread.Start();
            Thread.Sleep(500);

            StartClient(person);
            
            XDocument xDoc = XDocument.Load("person.xml");
            
            var nameElement = xDoc.XPathSelectElement("//Name");
            Console.WriteLine($"XPath //Name: {nameElement?.Value}");
            
            var expensiveItems = xDoc.XPathSelectElements("//Product[Price > 0]");
            Console.WriteLine("XPath //Product[Price > 0]:");
            foreach (var item in expensiveItems)
            {
                Console.WriteLine($"- {item.Element("Name")?.Value}");
            }

            
            
            XDocument newDoc = new XDocument(
                new XElement("Shop",
                    new XElement("Item", new XAttribute("id", 1), new XElement("Name", "Хлеб"), new XElement("Price", 2)),
                    new XElement("Item", new XAttribute("id", 2), new XElement("Name", "Молоко"), new XElement("Price", 3)),
                    new XElement("Item", new XAttribute("id", 3), new XElement("Name", "Сыр"), new XElement("Price", 10))
                )
            );
            
            newDoc.Save("shop.xml");
            Console.WriteLine("Создан shop.xml");

            var expensiveItemsLinq = from item in newDoc.Descendants("Item")
                                     where (int)item.Element("Price") > 5
                                     select item.Element("Name")?.Value;

            Console.WriteLine("Товары дороже 5 (LINQ):");
            foreach (var name in expensiveItemsLinq)
            {
                Console.WriteLine($"- {name}");
            }
        }

        static void StartServer()
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8005);
            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            try
            {
                listenSocket.Bind(ipPoint);
                listenSocket.Listen(10);

                Socket handler = listenSocket.Accept();
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                byte[] data = new byte[256];

                do
                {
                    bytes = handler.Receive(data);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (handler.Available > 0);

                string receivedJson = builder.ToString();
                Console.WriteLine($"[Server] Получены данные: {receivedJson.Substring(0, Math.Min(receivedJson.Length, 50))}...");
                
                
                
                Person receivedPerson = JsonSerializer.Deserialize<Person>(receivedJson, new JsonSerializerOptions { IncludeFields = true });
                Console.WriteLine($"[Server] Объект десериализован: {receivedPerson.Name}, Баланс: {receivedPerson.Balance}");

                
                
                
                string message = "Успешно получено";
                data = Encoding.UTF8.GetBytes(message);
                handler.Send(data);
                
                handler.Shutdown(SocketShutdown.Both);
                handler.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Server Error] {ex.Message}");
            }
        }

        static void StartClient(Person p)
        {
            try
            {
                IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8005);
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ipPoint);
                
                string json = JsonSerializer.Serialize(p, new JsonSerializerOptions { IncludeFields = true });
                byte[] data = Encoding.UTF8.GetBytes(json);
                
                socket.Send(data);
                
                data = new byte[256];
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                
                do
                {
                    bytes = socket.Receive(data);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                }
                while (socket.Available > 0);
                
                Console.WriteLine($"[Client] Ответ сервера: {builder.ToString()}");

                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Client Error] {ex.Message}");
            }
        }
    }
}
