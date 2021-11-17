using System;
using System.Collections.Generic;



namespace VariantA
{
    class Program
    {
        public static void OnPriceChanged(string title, double price)
        {
            Console.WriteLine($"Цена товара \"{title}\" была измененена на {price} $"); // Обработчик события
        }
        static void Main(string[] args)
        {
            int key = 0;
            DatabaseManagement.CreateDatabaseFile("orders.txt");
            DatabaseManagement.CreateDatabaseFile("queryresults.txt");
            List<ProductsInOrder> SendProducts = new(); // Создание списка товаров в заказе 
            Dictionary<int, Order> Orders = new(); // Создание списка всех заказов
            Order SendOrder = new(); // Создание заказа


            Product mouse = new("Мышь проводная", "изготовленна в Германии", 8); // Создание товара
            Product keyboard = new("Клавиатура проводная", "изготовленна в Украине", 5); // Создание товара
            Product gamepad = new("Джойстик проводной", "изготовленна в Китае", 2); // Создание товара
            SendProducts.Add(new ProductsInOrder(mouse, 4)); // Добавление товара в список товаров
            SendProducts.Add(new ProductsInOrder(keyboard, 1)); // Добавление товара в список товаров
            SendProducts.Add(new ProductsInOrder(gamepad, 2)); // Добавление товара в список товаров


            SendOrder = new(ref SendProducts, new DateTime(2021, 10, 24)); // Создание заказа
            CRUDOperations.AppendOrder("orders.txt", SendOrder); // Добавление заказа в базу данных
            Orders.Add(key++, SendOrder); // Добавление заказа в список заказов


            Product table = new("Стол для компьютера", "из дуба", 25); // Создание товара
            Product chair = new("Стул для компьютера", "из тополя", 15); // Создание товара
            SendProducts.Add(new ProductsInOrder(table, 1)); // Добавление товара в список товаров
            SendProducts.Add(new ProductsInOrder(chair, 1)); // Добавление товара в список товаров
            SendProducts.Add(new ProductsInOrder(keyboard, 1)); // Добавление товара в список товаров


            SendOrder = new(ref SendProducts, new DateTime(2021, 10, 24)); // Создание заказа
            CRUDOperations.AppendOrder("orders.txt", SendOrder); // Добавление заказа в базу данных
            Orders.Add(key++, SendOrder); // Добавление заказа в список заказов


            Product monitor = new("Монитор", "частота 144 герц", 35); // Создание товара
            SendProducts.Add(new ProductsInOrder(monitor, 1)); // Добавление товара в список товаров
            SendProducts.Add(new ProductsInOrder(gamepad, 3)); // Добавление товара в список товаров


            SendOrder = new(ref SendProducts, new DateTime(2021, 10, 26)); // Создание заказа
            CRUDOperations.AppendOrder("orders.txt", SendOrder); // Добавление заказа в базу данных
            Orders.Add(key++, SendOrder); // Добавление заказа в список заказов


            SendOrder = Order.CreateNewOrder(Orders, new DateTime(2021, 10, 24), ref SendProducts); // Создание заказа, состоящего из товаров, заказанных в заданную дату
            Orders.Add(key++, SendOrder); // Добавление заказа в список заказов


            Order.RemoveOrder(Orders, table, 1); // Удаление заказа, содержащего 4 ттовара типа mouse


            foreach (var order in Orders)
            {
                Console.WriteLine(Order.ShowOrder(order.Value));
            }
            Order.ShowNumber(Orders, 100, 3); // Вывод номеров заказов, сумма которых не превышает 100, и содержащих
            Order.ShowNumber(Orders, monitor); // Вывод номеров заказов, содержащих данный товар
            Order.ShowNumber(Orders, monitor, new DateTime(2021, 10, 24)); // Вывод номеров заказов, не содержащих данный товар, и поступивших в заданную дату
            Console.ReadKey();
            Console.Clear();
            Dictionary<int, Order> FileOrders = CRUDOperations.ReadOrders("orders.txt"); // Чтение из фалйа 
            JSONManagement.JSONSerialize(Orders); // Сериализация словаря
            Dictionary<int, Order> JSONOrders = JSONManagement.JSONDeSerialize(); // Десериализация словаря
            foreach (var item in JSONOrders)
            {
                Console.WriteLine(Order.ShowOrder(item.Value)); // Вывод десериализованного словаря
            }
            Product.PriceChanged += OnPriceChanged; // Добавление обработчика
        }
    }
}