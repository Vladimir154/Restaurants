using System;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Core.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public Product Product { get; set; }

        [Required]
        public int Count { get; set; }
        public DateTime OrderedDate { get; set; }

        public Order()
        {
            OrderedDate = DateTime.Now;
        }
    }
}
