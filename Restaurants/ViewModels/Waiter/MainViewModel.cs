using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Restaurants.Core.Data;
using Restaurants.Core.Models;
using Restaurants.Helpers;
using Restaurants.Views;
using Restaurants.Views.Manager;

namespace Restaurants.ViewModels.Waiter
{
    class MainViewModel : ViewModelBase
    {
        private readonly AppDbContext _dbContext;

        public MainViewModel()
        {
            _dbContext = new AppDbContext();
            _dbContext.Orders.Load();
            Orders = _dbContext.Orders.Local;

            // инициализация команд
            AcceptOrderCommand = new RelayCommand(AcceptOrder);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
            LogoutCommand = new RelayCommand(Logout);
        }

        #region Fields
        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get
            {
                return _orders;
            }
            set
            {
                _orders = value;
                RaisePropertyChanged("Orders");
            }
        }

        object _selectedOrder;
        public object SelectedOrder
        {
            get
            {
                return _selectedOrder;
            }
            set
            {
                if (_selectedOrder != value)
                {
                    _selectedOrder = value;
                    RaisePropertyChanged("SelectedOrder");
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
                    Orders = new ObservableCollection<Order>(_dbContext.Orders.Local.Where(o => RegularExpressions.RegularExpressions.IsMatch(o.ProductName, $"^{value}")));
                    RaisePropertyChanged("FilterQuery");
                }
            }
        }
        #endregion

        #region Commands 
        public RelayCommand AcceptOrderCommand { get; set; }
        async void AcceptOrder(object parameter)
        {
            var order = SelectedOrder as Order;
            if (order == null || order.Username == null)
            {
                MessageBox.Show("Сначала выберите заказ, который хотите принять", "Помощь", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            var orderAccepted = MessageBox.Show($"Принять заказ\n\nUsername: {order.Username}\nProduct: {order.ProductName}\nCount: {order.Count}",
                                                "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

            if (orderAccepted == MessageBoxResult.No)
                return;

            _dbContext.Orders.Remove(order);
            await _dbContext.SaveChangesAsync();

            // обновление таблицы
            _dbContext.Orders.Load();
            Orders = new ObservableCollection<Order>(_dbContext.Orders);
            RaisePropertyChanged("Orders");
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

    }
}
