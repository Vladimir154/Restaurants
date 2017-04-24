using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Restaurants.Core.Models;
using System;

namespace Restaurants.ViewModels.Manager
{
    class EditProductViewModel : UserInputBaseViewModel
    {
        public EditProductViewModel()
        {
            Messenger.Default.Register<NotificationMessage>(this, (nm) =>
            {
                if (nm.Notification == "RaisePropertyChanged")
                {
                    RaisePropertyChanged("Name");
                    RaisePropertyChanged("Price");
                    RaisePropertyChanged("Count");
                }
            });

        }

        public Product Product { get; set; }

        public string Name
        {
            get
            {
                return Product.Name;
            }
            set
            {
                Product.Name = value;
            }
        }

        public string Price
        {
            get
            {
                return Product.Price.ToString();
            }
            set
            {
                Product.Price = Convert.ToDouble(value);
            }
        }

        public string Count
        {
            get
            {
                return Product.Count.ToString();
            }
            set
            {
                Product.Count = Convert.ToInt32(value);
            }
        }

        protected override void Confirm(object param)
        {
            if (!Validate())
            {
                MessageBox.Show("Проверьте корректность введенных данных", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // изменение пользователя в БД
            DbContext.Entry(Product).State = System.Data.Entity.EntityState.Modified;
            DbContext.SaveChanges();
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
