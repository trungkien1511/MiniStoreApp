using System.Windows;
using System.Windows.Controls;

namespace MiniStore.Views.User
{
    public partial class MainLayout : Window
    {
        public MainLayout()
        {
            InitializeComponent();

            // Tạo instance của Home và gán vào ContentControl
            Home homePage = new Home();
            MainContent.Content = homePage;
        }
    }
}
