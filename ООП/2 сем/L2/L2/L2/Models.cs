using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShopApp.Models
{
    [Serializable]
    public class Manufacturer {
        public string Organization { get; set; } = "Не указано";
        public string Country { get; set; } = "Не указано";
    }

    [Serializable]
    public class Seller {
        public string FullName { get; set; } = "Не указано";
        public int Experience { get; set; }
    }

    [Serializable]
    [XmlRoot("Product")]
    public class Product {
        public string Name { get; set; }
        public string InventoryNumber { get; set; }
        public string Category { get; set; }
        public int Weight { get; set; }
        public string Size { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime ArrivalDate { get; set; }
        public Manufacturer ManufacturerInfo { get; set; } = new Manufacturer();
        public Seller SellerInfo { get; set; } = new Seller();
    }
}