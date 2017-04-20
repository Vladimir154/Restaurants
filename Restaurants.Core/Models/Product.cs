using System.Collections.Generic;

namespace Restaurants.Core.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public double Price { get; set; }
        public ICollection<Order> Orders { get; set; }

        public Product()
        {
            Orders = new List<Order>();
        }
    }
}
