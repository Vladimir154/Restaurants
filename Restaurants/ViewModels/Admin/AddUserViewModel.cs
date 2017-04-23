using Restaurants.Core.Services;
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
            if(!AuthenticationService.Register(Username, Password))
            {
                MessageBox.Show("Имя занято", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NotifyWindowToClose();
        }

        bool Validate()
        {
            return !(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Role));
        }
    }
}
