using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Globalization;
using Order_App;

namespace Order_app
{
    class Entry
    {
        static string connectionString = @"Data Source=Order_App.db";
        static void Main(string[] args)
        {
            using (var connection = new SqliteConnection(connectionString)) 
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS order_table (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date Text,
                        Quantity INTEGER,
                        Price INTEGER)";


                tableCmd.ExecuteNonQuery();

                connection.Close();
            }

            Menu menu = new Menu();
            menu.RunMenuUI();
        }
    }
}