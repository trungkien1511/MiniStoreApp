using MiniStore.ViewModels.Admin;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MiniStore.Views.Admin
{
    public partial class ProductManagement : UserControl
    {
        // ObservableCollection để binding DataGrid
        public ObservableCollection<Product> Products { get; set; }

        public ProductManagement()
        {
            InitializeComponent();
            this.DataContext = new ProductManagementViewModel(); ; // Binding trực tiếp vào UserControl

        }

        private void AddProductButton_Click(object sender, RoutedEventArgs e)
        {
            // Lấy Window cha là AdminLayout
            var adminLayout = Window.GetWindow(this) as AdminLayout;
            if (adminLayout != null)
            {
                // Hiển thị AddProductView trong MainContent
                adminLayout.MainContent.Content = new AddProductView();
            }
        }

    }
}
