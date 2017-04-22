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
        
        public new string Username
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

        public new string Password
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

        public new string Role
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
