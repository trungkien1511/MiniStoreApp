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

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack)
            {
                MainFrame.GoBack();
            }
        }

        private void Forward_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoForward)
            {
                MainFrame.GoForward();
            }
        }

    }
}
