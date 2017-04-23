using Restaurants.Core.Data;
using Restaurants.Core.Models;
using System.Threading.Tasks;
using System.Linq;
using Restaurants.Core.Helpers;
using Restaurants.Core.Enums;

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
                    Role = "visitor"
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

        public static RoleEnum GetRole(string username)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == username);

            switch (user?.Role)
            {
                case "manager":
                    return RoleEnum.Manager;
                case "admin":
                    return RoleEnum.Admin;
                case "waiter":
                    return RoleEnum.Waiter;
                default:
                    return RoleEnum.Visitor;
            }
        }
    }
}
