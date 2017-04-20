namespace Restaurants.Core.Data
{
    using Restaurants.Core.Models;
    using System.Data.Entity;

    public class AppDbContext : DbContext
    { 
        public AppDbContext()
            : base("name=RestaurantDB")
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
    }
}