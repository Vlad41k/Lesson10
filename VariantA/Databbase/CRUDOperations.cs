using System;
using System.Text;
using System.IO;
using System.Collections.Generic;


namespace VariantA
{
    class CRUDOperations
    {
        public static string AddOrder(Order order) //  Метод для записи заказа в файл в определенном формате
        {
            StringBuilder stringBuilder = new StringBuilder("");
            stringBuilder.Append($"{order.Number}");
            foreach (var product in order.Products)
            {
                stringBuilder.Append(product.GetProductsInOrder());

            }
            stringBuilder.Append($" | {order.Date:d}\n");
            return stringBuilder.ToString();
        }
        public static void AppendOrder(string filename, Order order) // Добавление заказа в файл
        {
            StringBuilder path = new("");
            path.Append(@"database"); // Определение пути
            try
            {
                using FileStream fstream = new($"{path}\\{filename}", FileMode.Append); // Открытие файла для записи 
                {
                    byte[] array = System.Text.Encoding.Default.GetBytes(AddOrder(order)); // Перевод в массив байтов
                    fstream.Write(array, 0, array.Length); // Запись в файл
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void OverrideFile(string filename, Dictionary<int, Order> orders) // Перезапись файла
        {
            StringBuilder path = new("");
            path.Append(@"database\" + filename); // Определение пути
            try
            {
                using FileStream fstream = new(path.ToString(), FileMode.Truncate);
                {
                    foreach (var order in orders)
                    {
                        byte[] array = System.Text.Encoding.Default.GetBytes(AddOrder(order.Value)); // Перевод в массив байтов
                        fstream.Write(array, 0, array.Length); // Запись в файл
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void Clearfile(string filename) // Сделать файл пустым.
        {
            StringBuilder path = new("");
            path.Append(@"database\" + filename); // Определение пути
            try
            {
                File.WriteAllText(path.ToString(), String.Empty); // Очистка файла
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void QueryResult(string filename, StringBuilder str) // Вывод результатов запросов в файл
        {
            StringBuilder path = new("");
            path.Append(@"database"); // Определение пути
            try
            {
                using FileStream fstream = new($"{path}\\{filename}", FileMode.Append); // Открытие файла для записи 
                {
                    byte[] array = System.Text.Encoding.Default.GetBytes(str.ToString() + "\n"); // Перевод в массив байтов
                    fstream.Write(array, 0, array.Length); // Запись в файл
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static Dictionary<int, Order> ReadOrders(string filename)
        {
            List<ProductsInOrder> SendProducts = new(); // Создание списка товаров в заказе 
            Dictionary<int, Order> FileOrders = new(); // Создание списка всех заказов
            Order SendOrder; // Создание заказа


            StringBuilder path = new("");
            path.Append(@"database\" + filename); // Определение пути
            string[] Text = File.ReadAllLines(path.ToString(), Encoding.Default); // Считать все строки в файле в массив строк
            foreach (var Line in Text) // Перебор каждой строки массива
            {
                var Product = Line.Split(" | "); // Разбитие по символу 
                int number = int.Parse(Product[0]); // Чтение номера заказа
                for (int i = 1; i < Product.Length; i++) //Перебор до конца массива 
                {
                    if (i + 3 < Product.Length) // Создание нового продукта при условии что он существует
                    {
                        SendProducts.Add(new ProductsInOrder(new(Product[i], Product[i + 1], int.Parse(Product[i + 2])), int.Parse(Product[i + 3])));
                        i += 3;
                    }
                }
                var date = Product[^1].Split('.'); // Разбитие даты 
                SendOrder = new(ref SendProducts, new DateTime(int.Parse(date[2]), int.Parse(date[1]), int.Parse(date[0]))); // Создание заказа на основе чтения
                FileOrders.Add(number, SendOrder); // Добавление заказа в список заказов
            }
            return FileOrders; // Возвращение словаря заказов
        }
    }
}