using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Unicode;
using System.Text.Encodings.Web;


namespace VariantA
{
    class JSONManagement
    {
        public static void JSONSerialize(Dictionary<int, Order> orders) // Сериализация
        {
            try
            {
                string fileName = "orders.json";
                string json = JsonSerializer.Serialize(orders,
                    new JsonSerializerOptions()
                    {
                        WriteIndented = true, // Добавление пробелов при сериализации 
                        IncludeFields = true, // Сериализация полей
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) // Сериализация кириллицы
                    });
                File.WriteAllText(fileName, json); // Запись в файл 
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static Dictionary<int, Order> JSONDeSerialize() // Десериализация
        {
            Dictionary<int, Order> Orders = new();
            try
            {
                string fileName = "orders.json";
                string json = File.ReadAllText(fileName); // Считывание с файла
                Orders = JsonSerializer.Deserialize<Dictionary<int, Order>>(json,
                    new JsonSerializerOptions()
                    {
                        WriteIndented = true, // Добавление пробелов при сериализации 
                        IncludeFields = true // Сериализация полей
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return Orders;
        }
    }
}