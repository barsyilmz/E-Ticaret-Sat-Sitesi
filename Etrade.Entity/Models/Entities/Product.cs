using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Etrade.Entity.Models.Entities
{
    public class Product:Entity
    {
        public Product():base()
        {
        }

        public Product(string name, Category category, string ımage, int stock, decimal price, bool ısHome):base(name)
        {
            
            Category = category;
            Image = ımage;
            Stock = stock;
            Price = price;
            IsHome = ısHome;
        }

        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string? Image { get; set; }
        public int Stock { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        public bool IsHome { get; set; }
    }
}
