using Restaurants.Core.Data;
using Restaurants.Core.Models;
using System.Threading.Tasks;
using System.Linq;
using Restaurants.Core.Helpers;

namespace Restaurants.Core.Services
{
    public class AuthenticationService
    {
        private static AppDbContext _dbContext = new AppDbContext();

        public static async Task<bool> Register(string username, string password)
        {
            if (_dbContext.Users.FirstOrDefault(u => u.Username == username) != null)
                return false;

            _dbContext.Users.Add(
                new User
                {
                    Username = username,
                    Password = PasswordEncryptionHelper.ComputeHash(password, "SHA512", null),
                    Role = "user"
                });

            await _dbContext.SaveChangesAsync();
            return true;
        }

        public static async Task<bool> Login(string username, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return false;

            if(PasswordEncryptionHelper.VerifyHash(password, "SHA512", user.Password))
                return true;

            return false;
        }
    }
}
