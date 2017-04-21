using System;
using System.Collections.ObjectModel;
using Restaurants.Core.Models;
using Restaurants.Helpers;
using Restaurants.Core.Data;
using System.Linq;

namespace Restaurants.ViewModels
{
    class MainViewModel : ViewModelBase
    {
        public ObservableCollection<User> People { get; set; }
        private readonly AppDbContext _dbContext;

        public MainViewModel()
        {
            _dbContext = new AppDbContext();
            People = _dbContext.Users.Local;

            TextProperty1 = "Type here";

            AddUserCommand = new RelayCommand(AddUser);
        }

        object _SelectedPerson;
        public object SelectedPerson
        {
            get
            {
                return _SelectedPerson;
            }
            set
            {
                if (_SelectedPerson != value)
                {
                    _SelectedPerson = value;
                    RaisePropertyChanged("SelectedPerson");
                }
            }
        }

        string _TextProperty1;
        public string TextProperty1
        {
            get
            {
                return _TextProperty1;
            }
            set
            {
                if (_TextProperty1 != value)
                {
                    _TextProperty1 = value;
                    RaisePropertyChanged("TextProperty1");
                }
            }
        }

        public RelayCommand AddUserCommand { get; set; }

        void AddUser(object parameter)
        {
            if (parameter == null) return;
            _dbContext.Users.Add(new User { });
        }

    }
}
