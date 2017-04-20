using Restaurants.Core.Data;
using Restaurants.Core.Models;

namespace Restaurants.Core.Services
{
    public class DatabaseService
    {
        private readonly AppDbContext _dbContext;

        public DatabaseService()
        {
            _dbContext = new AppDbContext();
        }

        public async void AddUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}
