using System.Windows;
using System.Windows.Controls;

namespace MiniStore.Views.User
{
    public partial class MainLayout : Window
    {
        public MainLayout()
        {
            InitializeComponent();
            MainFrame.Navigate(new Home());
        }
    }
}
