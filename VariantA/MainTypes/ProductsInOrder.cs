using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VariantA
{
    class ProductsInOrder
    {
        public Product Item { get; init; }
        public int Amount { get; init; }
        public ProductsInOrder(Product item, int amount)
        {
            Item = item;
            Amount = amount;
        }
        public ProductsInOrder() { }
        public string GetProductsInOrder()
            => $" | {Item.Title} | {Item.Description} | {Item.Price} | {Amount}";
        public override string ToString() => $"{Item}, количество: {Amount} шт.";
    }
}