using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_App
{
    class OrderManager
    {
        static string connectionString = @"Data Source=Order_App.db";
        private int finalNumberInput;
        private Menu menu;
        internal int GetUserQuantity(string message)//Gets an int output from a user with validation checks. **Code snippet**
        {
            Console.WriteLine(message);//Display written message from this function. Less clutter

            string numberInput = Console.ReadLine();

            if (numberInput == "0")
            {
                menu = new Menu();
                menu.RunMenuUI();
            }//brings the user back to the menu

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)//Checks if input can convert to int and it's not negative.
            {
                Console.WriteLine("\n\n Invalid number.\n\n");
                numberInput = Console.ReadLine();
            }

            finalNumberInput = Convert.ToInt32(numberInput);

            return finalNumberInput;
        }


        internal string GetUserDate()//Gets a date output from a user with validation checks. **Code snippet**
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.\n\n");

            string dateInput = Console.ReadLine();

            if (dateInput == "0")
            {
                menu = new Menu();
                menu.RunMenuUI();
            }//brings the user back to the menu

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))//If date cannot be parsed as the exact format, repeat.
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main menu:\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        internal int GetUserPrice()
        {
            int price = finalNumberInput * 50;//A temporary price number.
            return price;
        }

        public void Insert()
        {
            string dateInput = GetUserDate();

            int orderQuantity = GetUserQuantity("\n\nPlease write the amount of the item you want to buy.\n\n");

            int orderPrice = GetUserPrice();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"INSERT INTO order_table(date,quantity,price) VALUES ('{dateInput}','{orderQuantity}','{orderPrice}')";

                tableCmd.Parameters.AddWithValue("@date", dateInput);
                tableCmd.Parameters.AddWithValue("@quantity", orderQuantity);
                tableCmd.Parameters.AddWithValue("@price", orderPrice);

                tableCmd.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void GetRecords()
        {
            Console.Clear();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    $"SELECT * FROM order_table";

                List<Orders> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if(reader.HasRows) 
                {
                    while(reader.Read()) 
                    {
                        tableData.Add(
                        new Orders
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2),
                            Price = reader.GetInt32(3),
                        });

                    }
                }
                else
                {
                    Console.WriteLine("No rows found.");
                } 

                connection.Close();

                Console.WriteLine("--------------------------");
                foreach (var orders in tableData)//gets every object in the database **code snippet*
                {
                    Console.WriteLine($"ID: {orders.Id} - DATE: {orders.Date.ToString("dd-MMM-yyyy")} - QUANTITY: {orders.Quantity} - PRICE: {orders.Price}");
                }
                Console.WriteLine("--------------------------\n");
            }
        }

        public void Delete()
        {
            Console.Clear();
            GetRecords();
            var recordId = GetUserQuantity("\n\nWrite the Id of the record you want to delete.");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"DELETE from order_table WHERE Id = '{recordId}'";

                int rowCount = tableCmd.ExecuteNonQuery();

                if(rowCount == 0)// 0 means if no rows were affected by the command, retry. If user inputs 1 and Id 1 exists, it will tell the user it was deleted
                    //Otherwise it will ask the user to retry.
                {
                    Console.WriteLine($"\n\nRecord with Id {recordId} does not exist. \n\n");
                    Delete();
                }
                
                Console.WriteLine($"\n\nRecord with Id {recordId} was deleted. \n\n");

                connection.Close();

            }
            
        }

    }

    public class Orders
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
    }

            
}
