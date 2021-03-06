﻿using System.Collections.Generic;
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

            // инициализация команд
            AddUserCommand = new RelayCommand(AddUser);
            EditUserCommand = new RelayCommand(EditUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
            LogoutCommand = new RelayCommand(Logout);
            ExportToExcelCommand = new RelayCommand(ExportToExcel);
        }

        #region Fields
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get
            {
                return _users;
            }
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
                    Users = new ObservableCollection<User>(_dbContext.Users.Local.Where(u => RegularExpressions.RegularExpressions.IsMatch(u.Username, $"^{value}")));
                    RaisePropertyChanged("FilterQuery");
                }
            }
        }
        #endregion

        #region Commands 
        public RelayCommand AddUserCommand { get; set; }
        void AddUser(object parameter)
        {
            // показать окно добавления пользователя
            new AddUserView().ShowDialog();

            // подгрузить добавленые данные
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

            // показать окно редактирования данных пользователя
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

            // удаление пользователя
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();

            // обновление таблицы
            _dbContext.Users.Load();
            Users = new ObservableCollection<User>(_dbContext.Users);
            RaisePropertyChanged("Users");
        }

        public RelayCommand OpenSettingsCommand { get; set; }
        void OpenSettings(object parameter)
        {
            // открытие окна настроек
            new SettingsView().ShowDialog();
            RaisePropertyChanged("WindowBackgroundColor");
            RaisePropertyChanged("BorderBackgroundColor");
            RaisePropertyChanged("FontColor");
        }

        public RelayCommand LogoutCommand { get; set; }
        void Logout(object parameter)
        {
            NotifyWindowToHide();
            new LoginView().Show();
        }
        #endregion

        public RelayCommand ExportToExcelCommand { get; set; }
        void ExportToExcel(object parameter)
        {
            new ExportToExcel<User>()
            {
                dataToPrint = new List<User>(Users)
            }.GenerateReport();
        }
    }
}

