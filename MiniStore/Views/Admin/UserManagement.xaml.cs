using MiniStore.ViewModels.Admin;
using System.Windows;
using System.Windows.Controls;

namespace MiniStore.Views.Admin
{
    public partial class UserManagement : UserControl
    {
        public UserManagement()
        {
            InitializeComponent();
            this.DataContext = new UserManagementViewModel();
        }
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy Window cha là AdminLayout
            var adminLayout = Window.GetWindow(this) as AdminLayout;
            if (adminLayout != null)
            {
                // Hiển thị AddProductView trong MainContent
                adminLayout.MainContent.Content = new AddUserView();
            }
        }
    }


}
