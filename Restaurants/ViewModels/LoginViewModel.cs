using GalaSoft.MvvmLight.Messaging;
using Restaurants.Core.Services;
using Restaurants.Helpers;
using System.Windows;

namespace Restaurants.ViewModels
{
    class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
            RegisterCommand = new RelayCommand(Register);

            Username = "";
            Password = "";
        }

        string _username;
        public string Username
        {
            get
            {
                return _username;
            }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    RaisePropertyChanged("Username");
                }
            }
        }

        string _password;
        public string Password
        {
            get
            {
                return new string('*', _password.Length);
            }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    RaisePropertyChanged("Password");
                }
            }
        }

        public RelayCommand LoginCommand { get; set; }
        void Login(object parameter)
        {
            if (!AuthenticationService.Login(Username, Password))
            {
                MessageBox.Show("Неверное имя пользователя или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            new MainWindow().Show();
            NotifyWindowToClose();
        }

        public RelayCommand RegisterCommand { get; set; }
        void Register(object parameter)
        {
            if (!AuthenticationService.Register(Username, Password))
            {
                MessageBox.Show("Имя уже занято", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            new MainWindow().Show();
            NotifyWindowToClose();
        }
    }
}
