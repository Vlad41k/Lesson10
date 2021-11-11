using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace VariantA
{
    class DatabaseManagement
    {
        public static void CreateDatabaseFile(string filename) // Создание файла базы данных 
        {
            DirectoryInfo dirInfo = new(@"database");
            if (!dirInfo.Exists) { dirInfo.Create(); } // Создание директории
            StringBuilder path = new("");
            path.Append(@"database\" + filename); // Определение пути
            using FileStream fstream = new(path.ToString(), FileMode.Create);// Создание файла
                                                                             //  if (!File.Exists(path.ToString())) { File.Create(path.ToString()).Close(); } // Создание файла
        }
        public static void DeleteDatabaseFile(string filename) // Удаление файла базы данных 
        {
            StringBuilder path = new("");
            path.Append(@"database\" + filename); // Определение пути
            if (File.Exists(path.ToString())) { File.Delete(path.ToString()); } // Создание файла
        }
    }
}