using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public int Name { get; set; }

        [Required]
        public double Price { get; set; }

        public ICollection<Order> Orders { get; set; }

        public Product()
        {
            Orders = new List<Order>();
        }
    }
}
