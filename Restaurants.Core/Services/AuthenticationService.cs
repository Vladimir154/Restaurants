using Restaurants.Core.Data;
using Restaurants.Core.Models;
using System.Threading.Tasks;
using System.Linq;
using Restaurants.Core.Helpers;

namespace Restaurants.Core.Services
{
    public class AuthenticationService
    {
        private static readonly AppDbContext _dbContext = new AppDbContext();

        public static bool Register(string username, string password)
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

            _dbContext.SaveChanges();
            return true;
        }

        public static bool Login(string username, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            if (user == null)
                return false;

            return PasswordEncryptionHelper.VerifyHash(password, "SHA512", user.Password);
        }
    }
}
