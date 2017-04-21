using Restaurants.Core.Data;
using Restaurants.Helpers;

namespace Restaurants.ViewModels
{
    class InputViewModel : ViewModelBase
    {
        private readonly AppDbContext _dbContext;

        public InputViewModel()
        {
            _dbContext = new AppDbContext();

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
        async void Confirm(object param)
        {
            _dbContext.Users.Add(new Core.Models.User
            {
                Username = Username,
                Password = Password,
                Role = Role
            });

            await _dbContext.SaveChangesAsync();
            NotifyWindowToClose();
        }

        public RelayCommand CancelCommand { get; set; }
        void Cancel(object param)
        {
            NotifyWindowToClose();
        }
    }
}
