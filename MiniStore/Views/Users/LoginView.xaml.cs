using System.Windows;
using MiniStore.ViewModels;

namespace MiniStore.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
            this.DataContext = new LoginViewModel() ;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = txtPassword.Password;
            }
        }
    }
}
