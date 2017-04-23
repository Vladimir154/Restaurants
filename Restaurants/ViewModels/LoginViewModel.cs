using GalaSoft.MvvmLight.Messaging;
using NLog;
using Restaurants.Core.Enums;
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

        private static Logger logger = LogManager.GetCurrentClassLogger();

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
                return _password;
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
            // авторизация через сервис
            if (!AuthenticationService.Login(Username, Password))
            {
                MessageBox.Show("Неверное имя пользователя или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn($"Invalid attempt to login by user {Username}");
                return;
            }

            logger.Info($"User '{Username}' logged in");

            switch (AuthenticationService.GetRole(Username))
            {
                case RoleEnum.Manager:
                    new Views.Manager.MainView().Show();
                    break;
                case RoleEnum.Waiter:
                    new MainWindow().Show();
                    break;
                case RoleEnum.Admin:
                    new MainWindow().Show();
                    break;
                default:
                    new Views.Visitor.MainView().Show();
                    break;
            }
            NotifyWindowToClose();
        }

        public RelayCommand RegisterCommand { get; set; }
        void Register(object parameter)
        {
            if (!Validate())
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // регистрация через сервис
            if (!AuthenticationService.Register(Username, Password))
            {
                MessageBox.Show("Имя уже занято", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                logger.Warn($"Invalid attempt to register nickname {Username}");
                return;
            }

            logger.Info($"User {Username} registered");

            switch(AuthenticationService.GetRole(Username))
            {
                case RoleEnum.Manager:
                    new Views.Manager.MainView().Show();
                    break;
                case RoleEnum.Waiter:
                    new MainWindow().Show();
                    break;
                case RoleEnum.Admin:
                    new MainWindow().Show();
                    break;
                default:
                    new Views.Visitor.MainView().Show();
                    break;
            }

            NotifyWindowToClose();
        }

        bool Validate()
        {
            return !(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password));
        }
    }
}
