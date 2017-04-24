using Restaurants.Core.Data;
using Restaurants.Core.Services;
using System;
using System.Windows;

namespace Restaurants.ViewModels.Manager
{
    class AddProductViewModel : UserInputBaseViewModel
    {
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

        string _price;
        public string Price
        {
            get
            {
                return _price;
            }
            set
            {
                _price = value;
                RaisePropertyChanged("Price");
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

            using(var dbContext = new AppDbContext())
            {
                dbContext.Products.Add(new Core.Models.Product
                {
                    Name = Name,
                    Price = Convert.ToDouble(Price),
                    Count = Convert.ToInt32(Count)
                });

                dbContext.SaveChanges();
            }

            NotifyWindowToClose();
        }

        bool Validate()
        {
            int a;
            double b;
            return !(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Price) || string.IsNullOrEmpty(Count))
                   && (int.TryParse(Count, out a) && double.TryParse(Price, out b));
        }
    }
}
