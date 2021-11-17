using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace VariantA
{

    class Order
    {
        public Comparison<ProductsInOrder> ComparePrice = (first, second) => // Делегат для сортировки по цене
            first.Item.Price.CompareTo(second.Item.Price);
        private static int i = 1;
        public List<ProductsInOrder> Products = new();
        public int Number { get; init; }
        public DateTime Date { get; init; }
        // Конструкторы
        public Order(ref List<ProductsInOrder> products, DateTime date)
        {
            Number = i++;
            products.Sort(ComparePrice); // Сортировка
            Products.AddRange(products.ToArray());
            Date = date;
            products.Clear();
        }
        public Order() { }

        // Вывод информации о заказе
        public static string ShowOrder(Order order)
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append($"Номер заказа: {order.Number}\n");
            stringBuilder.Append($"Товары в заказе:\n");
            foreach (var item in order.Products)
            {
                stringBuilder.Append(item + "\n");
            }
            stringBuilder.Append($"Дата заказа: {order.Date:d}\n");
            stringBuilder.Append('-', 50).Append('\n'); // Визуальное разделение заказов
            return stringBuilder.ToString();
        }

        // Вывод номеров заказов, содержащих данный товар
        public static void ShowNumber(Dictionary<int, Order> orders, Product product)
        {
            StringBuilder str = new("Номера заказов, содержащих \"" + product.Title + "\": ");
            IEnumerable<int> numberQuery = orders.SelectMany(order => order.Value.Products, // Создание запроса
                                         (order, prod) => new { Orders = order,ProductsInOrder = prod })
                                         .Where(order => order.ProductsInOrder.Item.Title == product.Title) // Совпадение по название товара
                                         .Select(order => order.Orders.Value.Number) // Получение номера заказа
                                         .Distinct(); // Удаление повторяющихся номеров заказа
            foreach (int number in numberQuery) // Выполнение запроса
                str.Append(number + " ");
            Console.WriteLine(str.ToString());
            CRUDOperations.QueryResult("queryresults.txt", str); // Добавление запроса в файл
        }

        // Вывод номеров заказов, не содержащих данный товар, и поступивших в заданную дату
        public static void ShowNumber(Dictionary<int, Order> orders, Product product, DateTime date)
        {
            StringBuilder str = new("Номера заказов, не содержащих \"" + product.Title +
                "\", и заказанных " + date.ToString("d") + ": ");
            IEnumerable<int> numberQuery = (from order in orders // Создание запроса
                                           from prod in order.Value.Products // Перебор продуктов в заказе
                                           where order.Value.Date == date // Проверка даты
                                           where prod.Item.Title != product.Title  // Проверка на отличие названий  
                                           select order.Value.Number).Distinct(); // Удаление одинаковых номеров
            foreach (int number in numberQuery) // Выполнение запроса
                    str.Append(number + " ");   
            Console.WriteLine(str.ToString());
            CRUDOperations.QueryResult("queryresults.txt", str); // Добавление запроса в файл
        }

        // Вывод номеров заказов, стоимость которых не превышает заданную, и содержащих данное количество товаров
        public static void ShowNumber(Dictionary<int, Order> orders, double price, int count)
        {
            StringBuilder str = new("Номера заказов, стоимость которых не превышает " + price +
                "$ и имеет " + count + " различных товаров: ");
            foreach (var order in orders)
            {
                var check = true;
                double totalprice = 0;
                int totalcount = 0;
                foreach (var products in order.Value.Products) // Определение общей стоимости
                {
                    totalprice += products.Item.Price;
                    totalcount++;
                }
                if (totalprice > price || totalcount != count)
                    check = false;
                if (check == true)
                    str.Append(order.Value.Number + " ");
            }
            Console.WriteLine(str.ToString());
            CRUDOperations.QueryResult("queryresults.txt", str); // Добавление запроса в файл
        }

        // Удаление заказов, содержащих данный продукт в заданном количестве
        public static void RemoveOrder(Dictionary<int, Order> orders, Product product, int count)
        {
            for (int i = 0; i < orders.Count; i++) // перебор всех заказов
            {
                var check = false;
                if (orders.ContainsKey(i) == true)
                    for (int j = 0; j < orders[i].Products.Count; j++) // Перебор всех товаров в заказе
                    {
                        if (orders[i].Products[j].Item.Title == product.Title && orders[i].Products[j].Amount == count) // Проверка условия
                            check = true;
                    }
                if (check == true)
                {
                    orders.Remove(i); // Удаление заказа
                }
            }
            CRUDOperations.OverrideFile("orders.txt", orders); // Перезапись файла
        }

        //  Формирование нового заказа, состоящего из товаров, заказанных в текущий день.
        public static Order CreateNewOrder(Dictionary<int, Order> orders, DateTime date, ref List<ProductsInOrder> sendproducts)
        {
            foreach (var order in orders)
            {
                if (order.Value.Date == date) // Проверка  на совпадение даты
                {
                    sendproducts.AddRange(order.Value.Products); // Добавление всех продуктов в заказе в новый заказ
                }
            }
            Order SendOrder = new(ref sendproducts, new DateTime(2021, 11, 30)); // Создание заказа
            CRUDOperations.AppendOrder("orders.txt", SendOrder); // Добавление заказа в файл
            return SendOrder;
        }
    }
}