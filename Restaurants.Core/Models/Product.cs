using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restaurants.Core.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int Count { get; set; }
    }
}
