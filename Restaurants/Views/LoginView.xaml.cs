using Restaurants.Core.Services;
using System.Windows;

namespace Restaurants.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void RegisterBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!await AuthenticationService.Register(UsernameTxt.Text, PasswordTxt.Password))
            {
                MessageBox.Show("Имя уже занято", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            new MainWindow().Show();
            this.Close();
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!await AuthenticationService.Login(UsernameTxt.Text, PasswordTxt.Password))
            {
                MessageBox.Show("Неверное имя пользователя или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            new MainWindow().Show();
            this.Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
