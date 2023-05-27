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
        private Menu menu;
        internal int GetUserQuantity(string message)
        {
            Console.WriteLine(message);//Display written message from this function. Less clutter

            string numberInput = Console.ReadLine();

            if (numberInput == "0") menu.RunMenuUI();//brings the user back to the menu

            while (!Int32.TryParse(numberInput, out _) || Convert.ToInt32(numberInput) < 0)//Checks if input can convert to int and it's not negative.
            {
                Console.WriteLine("\n\n Invalid number.\n\n");
                numberInput= Console.ReadLine();
            }

            int finalNumberInput = Convert.ToInt32(numberInput);

            return finalNumberInput;
        }

        internal string GetUserDate()
        {
            Console.WriteLine("\n\nPlease insert the date: (Format: dd-mm-yy). Type 0 to return to main menu.\n\n");

            string dateInput = Console.ReadLine();

            if (dateInput == "0") menu.RunMenuUI();//brings the user back to the menu

            while (!DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))//If date cannot be parsed as the exact format, repeat.
            {
                Console.WriteLine("\n\nInvalid date. (Format: dd-mm-yy). Type 0 to return to main menu:\n\n");
                dateInput = Console.ReadLine();
            }

            return dateInput;
        }

        

    }
}
