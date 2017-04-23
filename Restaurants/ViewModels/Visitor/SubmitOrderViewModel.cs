using Restaurants.Core.Data;
using Restaurants.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Restaurants.ViewModels.Visitor
{
    class SubmitOrderViewModel : UserInputBaseViewModel
    {
        private List<string> _products;
        public List<string> Products
        {
            get
            {
                return DbContext.Products.Select(u => u.Name).ToList();
            }
            set
            {
                _products = value;
            }
        }

        string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
                RaisePropertyChanged("Name");
            }
        }

        string _count;
        public string Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
                RaisePropertyChanged("Count");
            }
        }


        protected override void Confirm(object param)
        {
            if (!Validate())
            {
                MessageBox.Show("Проверьте корректность введенных данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var dbContext = new AppDbContext())
            {
                dbContext.Orders.Add(new Core.Models.Order
                {
                    Username = AuthenticationService.CurrentUserName,
                    Product = DbContext.Products.FirstOrDefault(p => p.Name == Name),
                    Count = Convert.ToInt32(Count)
                });

                dbContext.SaveChanges();
            }

            NotifyWindowToClose();
        }

        bool Validate()
        {
            return !(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Count))
                   && (int.TryParse(Count, out int a));
        }
    }
}
