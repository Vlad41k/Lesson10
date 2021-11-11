using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Unicode;
using System.Text.Encodings.Web;
using System.Text;


namespace VariantA
{
    class JSONManagement
    {
        public static void JSONSerialize(Dictionary<int, Order> orders)
        {
            try
            {
                string fileName = "orders.json";
                string json = JsonSerializer.Serialize(orders,
                    new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                        IncludeFields = true,
                        Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic) // Чтоб читало кириллицу
                    });
                File.WriteAllText(fileName, json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static Dictionary<int, Order> JSONDeSerialize()
        {
            Dictionary<int, Order> Orders = new();
            try
            {
                string fileName = "orders.json";
                string json = File.ReadAllText(fileName);
                Orders = JsonSerializer.Deserialize<Dictionary<int, Order>>(json,
                    new JsonSerializerOptions()
                    {
                        WriteIndented = true,
                        IncludeFields = true
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