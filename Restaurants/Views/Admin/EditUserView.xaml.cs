using GalaSoft.MvvmLight.Messaging;
using Restaurants.Core.Models;
using Restaurants.ViewModels;
using Restaurants.ViewModels.Admin;

namespace Restaurants.Views
{
    /// <summary>
    /// Interaction logic for EditUserView.xaml
    /// </summary>
    public partial class EditUserView
    {
        public EditUserView(User user)
        {
            InitializeComponent();
            ((EditUserViewModel)DataContext).User = user;
            Messenger.Default.Send(new NotificationMessage(this, "RaisePropertyChanged"));
            Messenger.Default.Register<NotificationMessage>(this, (nm) =>
            {
                if (nm.Notification == "CloseWindowsBoundToMe")
                {
                    if (nm.Sender == this.DataContext)
                        this.Close();
                }
            });
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
