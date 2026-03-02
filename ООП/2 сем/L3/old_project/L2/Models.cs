using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace ShopApp.Models
{
    public class MyCustomValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is string str)
            {
                return str.Length > 2;
            }
            return false;
        }
    }

    [Serializable]
    public class Manufacturer
    {
        [Required(ErrorMessage = "Название организации обязательно")]
        public string Organization { get; set; } = "";

        [Required(ErrorMessage = "Страна обязательна")]
        public string Country { get; set; } = "";
    }

    [Serializable]
    public class Seller
    {
        [Required(ErrorMessage = "ФИО продавца обязательно")]
        [RegularExpression(@"^[а-яА-ЯёЁ\s-]+$", ErrorMessage = "ФИО должно содержать только русские буквы")]
        public string FullName { get; set; } = "Не указано";

        [Range(0, 60, ErrorMessage = "Стаж должен быть от 0 до 60 лет")]
        public int Experience { get; set; }
    }

    [Serializable]
    [XmlRoot("Product")]
    public class Product
    {
        [Required(ErrorMessage = "Название товара обязательно")]
        [MyCustomValidation(ErrorMessage = "Название должно быть длиннее 2 символов")]
        public string Name { get; set; }

        [RegularExpression(@"^[A-Z]{3}-\d{6}$", ErrorMessage = "Инв. номер должен быть формата AAA-000000")]
        public string InventoryNumber { get; set; }

        public string Category { get; set; }

        [Range(1, 1000, ErrorMessage = "Вес должен быть от 1 до 1000 кг")]
        public int Weight { get; set; }

        public string Size { get; set; }

        [Range(0.01, 1000000, ErrorMessage = "Цена должна быть положительной")]
        public decimal Price { get; set; }

        [Range(0, 10000, ErrorMessage = "Количество должно быть от 0 до 10000")]
        public int Quantity { get; set; }

        public DateTime ArrivalDate { get; set; }

        public Manufacturer ManufacturerInfo { get; set; } = new Manufacturer();
        public Seller SellerInfo { get; set; } = new Seller();
    }
}
