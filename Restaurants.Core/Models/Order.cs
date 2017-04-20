using System;
using System.Collections.Generic;

namespace Restaurants.Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public DateTime OrderedDate { get; set; }
        public ICollection<Product> OrderedProducts { get; set; }

        public Order()
        {
            OrderedProducts = new List<Product>();
        }
    }
}
