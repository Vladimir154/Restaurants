using System.Windows;

namespace Restaurants.ViewModels.Admin
{
    class AddUserViewModel : UserInputBaseViewModel
    {
        protected override void Confirm(object param)
        {
            if (!Validate())
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // добавляем пользователя в БД
            DbContext.Users.Add(new Core.Models.User
            {
                Username = Username,
                Password = Password,
                Role = Role
            });

            DbContext.SaveChanges();
            NotifyWindowToClose();
        }

        bool Validate()
        {
            return !(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Role));
        }
    }
}
