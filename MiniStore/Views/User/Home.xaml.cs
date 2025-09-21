using MiniStore.Models;
using MiniStore.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;




namespace MiniStore.Views.User
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public ObservableCollection<Product> Products { get; set; }

        public Home()
        {
            InitializeComponent();
        }

        private void ProductCard_Click(object sender, RoutedEventArgs e)
        {

            var mainWindow = (MainLayout)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new Cart());

        }
        
    }
}
