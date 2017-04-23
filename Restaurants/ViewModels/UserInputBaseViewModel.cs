using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restaurants.Core.Data;
using Restaurants.Helpers;

namespace Restaurants.ViewModels
{
    abstract class UserInputBaseViewModel : ViewModelBase
    {
        protected readonly AppDbContext DbContext;

        protected UserInputBaseViewModel()
        {
            DbContext = new AppDbContext();

            ConfirmCommand = new RelayCommand(Confirm);
            CancelCommand = new RelayCommand(Cancel);
        }

        public RelayCommand ConfirmCommand { get; set; }
        protected abstract void Confirm(object param);

        public RelayCommand CancelCommand { get; set; }
        protected void Cancel(object param)
        {
            NotifyWindowToClose();
        }

    }
}
