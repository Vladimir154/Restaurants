using Restaurants.Core.Services;
using System.Windows;

namespace Restaurants.ViewModels.Admin
{
    class AddUserViewModel : UserInputBaseViewModel
    {
        string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                _username = value;
                RaisePropertyChanged("Username");
            }
        }

        string _password;
        public string Password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
                RaisePropertyChanged("Password");
            }
        }

        string _role;
        public string Role
        {
            get
            {
                return _role;
            }
            set
            {
                _role = value;
                RaisePropertyChanged("Role");
            }
        }


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

            if (Role != "visitor" && Role != "manager" && Role != "admin")
            {
                MessageBox.Show("Возможные роли:\n\nvisitor\nmanager\nadmin", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
