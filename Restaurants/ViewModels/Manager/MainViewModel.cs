using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using Restaurants.Core.Data;
using Restaurants.Core.Models;
using Restaurants.Helpers;
using Restaurants.Views;
using Restaurants.Views.Manager;

namespace Restaurants.ViewModels.Manager
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
            AddProductCommand = new RelayCommand(AddProduct);
            EditProductCommand = new RelayCommand(EditProduct);
            DeleteProductCommand = new RelayCommand(DeleteProduct);
            OpenSettingsCommand = new RelayCommand(OpenSettings);
            LogoutCommand = new RelayCommand(Logout);
            ExportToExcelCommand = new RelayCommand(ExportToExcel);
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
                    Products = new ObservableCollection<Product>(_dbContext.Products.Local.Where(p => RegularExpressions.RegularExpressions.IsMatch(p.Name, $"^{value}")));
                    RaisePropertyChanged("FilterQuery");
                }
            }
        }
        #endregion

        #region Commands 
        public RelayCommand AddProductCommand { get; set; }
        void AddProduct(object parameter)
        {
            // показать окно добавления пользователя
            new AddProductView().ShowDialog();

            // подгрузить добавленые данные
            _dbContext.Products.Load();
            Products = _dbContext.Products.Local;
            RaisePropertyChanged("Products");
        }

        public RelayCommand EditProductCommand { get; set; }
        void EditProduct(object parameter)
        {
            if (SelectedProduct == null && !(SelectedProduct is User))
            {
                MessageBox.Show("Cначала выберите продукт, который хотите изменить", "Помощь", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            // показать окно редактирования данных пользователя
            new EditProductView(SelectedProduct as Product).ShowDialog();

            _dbContext.Products.Load();
            Products = new ObservableCollection<Product>(_dbContext.Products);
            RaisePropertyChanged("Users");
        }

        public RelayCommand DeleteProductCommand { get; set; }
        async void DeleteProduct(object parameter)
        {
            var product = SelectedProduct as Product;
            if (product == null || product.Name == null)
            {
                MessageBox.Show("Сначала выберите продукт, который хотите удалить", "Помощь", MessageBoxButton.OK, MessageBoxImage.Asterisk);
                return;
            }

            var deleteConfirmed = MessageBox.Show($"Удалить продукт\nName: {product.Name}",
                                                "Вы уверены?", MessageBoxButton.YesNo, MessageBoxImage.Asterisk);

            if (deleteConfirmed == MessageBoxResult.No)
                return;

            // удаление пользователя
            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            // обновление таблицы
            _dbContext.Products.Load();
            Products = new ObservableCollection<Product>(_dbContext.Products);
            RaisePropertyChanged("Products");
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

        public RelayCommand ExportToExcelCommand { get; set; }
        void ExportToExcel(object parameter)
        {
            new ExportToExcel<Product>()
            {
                dataToPrint = new List<Product>(Products)
            }.GenerateReport();
        }
        #endregion

    }
}
