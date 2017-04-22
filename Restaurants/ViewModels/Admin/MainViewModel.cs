using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Restaurants.Core.Data;
using Restaurants.Core.Models;
using Restaurants.Helpers;
using Restaurants.Views;

namespace Restaurants.ViewModels.Admin
{
    class MainViewModel : ViewModelBase
    {
        private readonly AppDbContext _dbContext;

        public MainViewModel()
        {
            _dbContext = new AppDbContext();
            _dbContext.Users.Load();
            Users = _dbContext.Users.Local;

            AddUserCommand = new RelayCommand(AddUser);
            EditUserCommand = new RelayCommand(EditUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
        }

        #region Fields
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                _users = value;
                RaisePropertyChanged("Users");
            }
        }

        object _selectedUser;
        public object SelectedUser
        {
            get
            {
                return _selectedUser;
            }
            set
            {
                if (_selectedUser != value)
                {
                    _selectedUser = value;
                    RaisePropertyChanged("SelectedUser");
                }
            }
        }

        string _filterQuery;
        public string FilterQuery
        {
            get
            {
                return _filterQuery;
            }
            set
            {
                if (_filterQuery != value)
                {
                    _filterQuery = value;
                    Users = new ObservableCollection<User>(_dbContext.Users.Local.Where(u => u.Username.Contains(value)));
                    RaisePropertyChanged("FilterQuery");
                }
            }
        }
        #endregion

        #region Commands 
        public RelayCommand AddUserCommand { get; set; }
        void AddUser(object parameter)
        {
            new AddUserView().ShowDialog();

            _dbContext.Users.Load();
            Users = _dbContext.Users.Local;
            RaisePropertyChanged("Users");
        }

        public RelayCommand EditUserCommand { get; set; }
        void EditUser(object parameter)
        {
            if (SelectedUser == null && !(SelectedUser is User))
            {
                MessageBox.Show("Cначала выберите пользователя, которого хотите изменить", "Помощь", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }


            new EditUserView(SelectedUser as User).ShowDialog();

            _dbContext.Users.Load();
            Users = new ObservableCollection<User>(_dbContext.Users);
            RaisePropertyChanged("Users");
        }

        public RelayCommand DeleteUserCommand { get; set; }
        async void DeleteUser(object parameter)
        {
            var user = SelectedUser as User;
            if (user == null || user.Username == null)
            {
                MessageBox.Show("Сначала выберите пользователя, которого хотите удалить", "Помощь", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            var deleteConfirmed = MessageBox.Show($"Удалить пользователя\n\nUsername: {user.Username}\nRole: {user.Role}",
                                                "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

            if (deleteConfirmed == MessageBoxResult.No)
                return;

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            _dbContext.Users.Load();
            Users = new ObservableCollection<User>(_dbContext.Users);
            RaisePropertyChanged("Users");
        }
        #endregion

    }
}
