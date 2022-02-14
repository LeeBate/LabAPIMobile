using System;
using System.Collections.Generic;
using System.Text;

namespace LabXaml.Models
{
    class Order
    {
        public int ID { get; set; }  
        public string Username { get; set; }
        public int ProdID { get; set; }
        public string Title { get; set; }
        public float Price { get; set; }
       
    }
}
