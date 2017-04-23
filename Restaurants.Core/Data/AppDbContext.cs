namespace Restaurants.Core.Data
{
    using Restaurants.Core.Helpers;
    using Restaurants.Core.Models;
    using System.Data.Entity;

    public class AppDbContext : DbContext
    {
        public AppDbContext()
            : base("RestaurantsDB")
        {
            Database.SetInitializer(new DropCreateIfModelChangesInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }

        public void Seed(AppDbContext context)
        {
            context.Users.Add(new User
            {
                Username = "Admin",
                Password = PasswordEncryptionHelper.ComputeHash("1111", "SHA512", null),
                Role = "admin"
            });
        }

        public class DropCreateIfModelChangesInitializer : DropCreateDatabaseIfModelChanges<AppDbContext>
        {
            protected override void Seed(AppDbContext context)
            {
                context.Seed(context);

                base.Seed(context);
            }
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
    }
}