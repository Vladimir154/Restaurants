using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurants.Core.Data;
using Restaurants.Helpers;

namespace Restaurants.ViewModels
{
    abstract class UserInputBaseViewModel : ViewModelBase
    {
        protected readonly AppDbContext DbContext;

        protected UserInputBaseViewModel()
        {
            DbContext = new AppDbContext();

            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
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

        public RelayCommand ConfirmCommand { get; set; }
        protected abstract void Confirm(object param);

        public RelayCommand CancelCommand { get; set; }
        protected void Cancel(object param)
        {
            NotifyWindowToClose();
        }

    }
}
