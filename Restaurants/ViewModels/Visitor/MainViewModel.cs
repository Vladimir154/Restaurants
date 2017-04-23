using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Restaurants.Core.Data;
using Restaurants.Core.Models;
using Restaurants.Helpers;
using Restaurants.Views;
using Restaurants.Views.Manager;
using Restaurants.Views.Visitor;

namespace Restaurants.ViewModels.Visitor
{
    class MainViewModel : ViewModelBase
    {
        private readonly AppDbContext _dbContext;

        public MainViewModel()
        {
            _dbContext = new AppDbContext();
            _dbContext.Products.Load();
            Products = _dbContext.Products.Local;

            // инициализация команд
            SubmitOrderCommand = new RelayCommand(SubmitOrder);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
            LogoutCommand = new RelayCommand(Logout);
        }

        #region Fields
        private ObservableCollection<Product> _products;
        public ObservableCollection<Product> Products
        {
            get
            {
                return _products;
            }
            set
            {
                _products = value;
                RaisePropertyChanged("Products");
            }
        }

        object _selectedProduct;
        public object SelectedProduct
        {
            get
            {
                return _selectedProduct;
            }
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    RaisePropertyChanged("SelectedProduct");
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
                    Products = new ObservableCollection<Product>(_dbContext.Products.Local.Where(p => p.Name.Contains(value)));
                    RaisePropertyChanged("FilterQuery");
                }
            }
        }
        #endregion

        #region Commands 
        public RelayCommand SubmitOrderCommand { get; set; }
        void SubmitOrder(object parameter)
        {
            // показать окно добавления пользователя
            new SubmitOrderView().ShowDialog();
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
