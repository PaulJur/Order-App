using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_App
{
     class Menu
    {
        private OrderManager orderManager;

        public Menu()
        {
            orderManager = new OrderManager();
        }

        public void RunMenuUI()
        {
            Console.Clear();
            bool exitApp = false;

            while (exitApp == false)
            {
                Console.WriteLine("\nMade by Paulius Jurgelis");
                Thread.Sleep(1000);
                Console.WriteLine("\n\nMAIN MENU");
                Console.WriteLine("\nWhat would you like to do?");
                Console.WriteLine("\nType 0 to exit the application.");
                Console.WriteLine("Type 1 to View All Records.");
                Console.WriteLine("Type 2 to Insert Records.");
                Console.WriteLine("Type 3 to Update Records.");
                Console.WriteLine("Type 4 to Delete Records.");
                Console.WriteLine("--------------------------------------");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "0":
                        Console.WriteLine("\nGoodbye!\n");
                        exitApp = true;
                        Environment.Exit(0);
                        break;
                    case "1":
                        //ViewRecords()
                        break;
                    case "2":
                        //InsertRecords()
                        break;
                    case "3":
                        //UpdateRecords()
                        break;
                    case "4":
                        //DeleteRecords()
                        break;
                    default:
                        Console.WriteLine("\nInvalid input. Choose 0-4");
                        break;
                }

            }

        }

    }
}
