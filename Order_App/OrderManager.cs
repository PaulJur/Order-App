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
        private int finalTypeInput;
        private string materialName;
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

       

        internal string GetUserType()
        {
            
            Console.Clear();

            Console.WriteLine("1: Wood Price: 125");
            Console.WriteLine("2: Metal Price: 250 ");
            Console.WriteLine("3: Plastic: 75");
            Console.WriteLine("4: Glass: 300");
            Console.WriteLine("--------------------------------------\n");

            int typeInput = GetUserQuantity("\n\nPlease choose your material type.\n");

            new OrderTypes
            {
                MaterialType = materialName
            };


            switch (typeInput)
            {
                case 1:
                    finalTypeInput = 125;
                    return materialName = "Wood";
                case 2:
                    finalTypeInput = 250;
                    return materialName = "Metal";
                case 3:
                    finalTypeInput = 75;
                    return materialName = "Plastic";
                case 4:
                    finalTypeInput = 300;
                    return materialName = "Glass";
                default:
                    Console.WriteLine("\nInvalid input. Choose between 1-4.\n");
                    break;
            }

            return materialName;
 
        }

        internal int GetUserPrice()
        {
            int price = finalTypeInput * finalNumberInput;
            return price;
        }

        public void Insert()
        {
            string dateInput = GetUserDate();

            string orderMaterialType = GetUserType();

            int orderQuantity = GetUserQuantity("\n\nPlease write the amount of the material you want to buy.\n\n");

            int orderPrice = GetUserPrice();

            

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();

                tableCmd.CommandText =
                    "INSERT INTO order_table (date, quantity, Type, price) VALUES (@date, @quantity, @type, @price)";

                tableCmd.Parameters.AddWithValue("@date", dateInput);
                tableCmd.Parameters.AddWithValue("@quantity", orderQuantity);
                tableCmd.Parameters.AddWithValue("@type", orderMaterialType);
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

                List<OrderTypes> tableData = new();

                SqliteDataReader reader = tableCmd.ExecuteReader();

                if(reader.HasRows) 
                {
                    while(reader.Read()) 
                    {
                        tableData.Add(
                        new OrderTypes()
                        {
                            Id = reader.GetInt32(0),
                            Date = DateTime.ParseExact(reader.GetString(1), "dd-MM-yy", new CultureInfo("en-US")),
                            Quantity = reader.GetInt32(2),
                            MaterialType = reader.GetString(3),
                            Price = reader.GetInt32(4),
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
                    Console.WriteLine($"ID: {orders.Id} - DATE: {orders.Date.ToString("dd-MMM-yyyy")} - QUANTITY: {orders.Quantity} - TYPE: {orders.MaterialType}  - PRICE: {orders.Price}");
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

        public void Update()
        {
            Console.Clear();
            GetRecords();

            var getId = GetUserQuantity("\n\nType the Id of the record you want to update or type 0 to go back to the main menu.\n\n");

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    $"SELECT EXISTS(SELECT 1 FROM order_table WHERE Id = {getId})";
                tableCmd.Parameters.AddWithValue("@Id", getId);
                int checkQuery = Convert.ToInt32(tableCmd.ExecuteScalar());//returns the value from the database

                if(checkQuery== 0) 
                {
                    Console.WriteLine($"\n\nRecord with Id {getId} does not exist.\n\n");
                    connection.Close();
                    Update();
                
                }

                string date = GetUserDate();

                int quantity = GetUserQuantity("\n\nUpdate the amount of stock you want to buy.\n\n");

                string materialType = GetUserType();

                int price = GetUserPrice();

                var tableCommand = connection.CreateCommand();



                tableCommand.CommandText =
                    "UPDATE order_table SET date = @date, quantity = @quantity, type = @type, price = @price WHERE Id = @getId";
                tableCommand.Parameters.AddWithValue("@date",date);
                tableCommand.Parameters.AddWithValue("@quantity", quantity);
                tableCmd.Parameters.AddWithValue("@type", materialType);
                tableCommand.Parameters.AddWithValue("@id", getId);
                tableCommand.ExecuteNonQuery();

                connection.Close();
            }

        }

    }         
}
