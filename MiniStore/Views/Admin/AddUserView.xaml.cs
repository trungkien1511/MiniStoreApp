using MiniStore.ViewModels.Admin;
using System.Windows;
using System.Windows.Controls;

namespace MiniStore.Views.Admin
{
    public partial class AddUserView : UserControl
    {
        public AddUserView()
        {
            InitializeComponent();
            this.DataContext = new AddUserViewModel(); // gán VM ở đây
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is AddUserViewModel vm)
            {
                vm.Password = (sender as PasswordBox).Password;
            }
        }

    }
}
