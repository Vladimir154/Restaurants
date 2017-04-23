using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Restaurants.Core.Models;

namespace Restaurants.ViewModels.Admin
{
    class EditUserViewModel : UserInputBaseViewModel
    {
        public EditUserViewModel()
        {
            Messenger.Default.Register<NotificationMessage>(this, (nm) =>
            {
                if (nm.Notification == "RaisePropertyChanged")
                {
                    RaisePropertyChanged("Username");
                    RaisePropertyChanged("Password");
                    RaisePropertyChanged("Role");
                }
            });

        }

        public User User { get; set; }
        
        public string Username
        {
            get
            {
                 return User.Username;
            }
            set
            {
                User.Username = value;
            }
        }

        public string Password
        {
            get
            {
                return User.Password;
            }
            set
            {
                User.Password = value;
            }
        }

        public string Role
        {
            get
            {
                return User.Role;
            }
            set
            {
                User.Role = value;
            }
        }

        protected override void Confirm(object param)
        {
            if (!Validate())
            {
                MessageBox.Show("Заполните все поля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if(Role != "visitor" && Role != "manager" && Role != "admin")
            {
                MessageBox.Show("Возможные роли:\n\nvisitor\nmanager\nadmin", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // изменение пользователя в БД
            DbContext.Entry(User).State = System.Data.Entity.EntityState.Modified;
            DbContext.SaveChanges();
            NotifyWindowToClose();
        }

        bool Validate()
        {
            return !(string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(Role));
        }

    }
}
