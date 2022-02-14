using System;
using System.Collections.Generic;
using System.Text;

namespace LabXaml.Models
{
    class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public Uri Image { get; set; }
    }
}
