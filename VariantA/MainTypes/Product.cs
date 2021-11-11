using System;
using System.Collections.Generic;


namespace VariantA
{
    class Product
    {
        public string Title { get; set; }
        public string Description { get; set; }
        private double _price;
        public double Price
        {
            get => _price;
            set
            {
                _price = value;
                PriceChanged?.Invoke(Title, _price); // Вызов события
            }

        }
        public static event Action<string, double> PriceChanged; // Обьявление события
        public Product(string title, string description, double price)
        {
            Title = title;
            Description = description;
            Price = price;
        }
        public Product() { }
        public override string ToString() => $"{Title}, {Description}, цена: {Price}$";
    }
}