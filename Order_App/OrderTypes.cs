using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order_App
{
    public class OrderTypes
    {
        private int _id { get; set; }
        private DateTime _date { get; set; }
        private int _quantity { get; set; }
        private int _price { get; set; }

        public int Id 
        { 
            get { return _id; } 
            set { _id = value; } 
        }

        public DateTime Date  
        { 
            get { return _date;}
            set { _date = value; } 
        }

        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public int Price
        {
            get { return _price; }
            set{ _price = value; }
        }
    }
}
