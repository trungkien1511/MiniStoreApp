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
    public partial class Home : UserControl
    {
        public ObservableCollection<Product> Products { get; set; }

        public Home()
        {
            InitializeComponent();

            this.DataContext = new HomeViewModel();

        }
    }
}
