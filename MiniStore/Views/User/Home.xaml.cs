using MiniStore.Models;
using MiniStore.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Linq;





namespace MiniStore.Views.User
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public ObservableCollection<Product> Products { get; set; }
        private MiniStoreEntities db = new MiniStoreEntities();


        public Home()
        {
            InitializeComponent();
            LoadProducts();
        }

        public void LoadProducts()
        {
            var products = db.Products.ToList(); // Lấy toàn bộ dữ liệu từ bảng Products
            ProductItemsControl.ItemsSource = products;
        }

        private void ProductCard_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button?.Tag is Product selectedProduct)
            {
                // Điều hướng sang ProductDetail, truyền sản phẩm vào constructor
                var detailPage = new ProductDetail(selectedProduct);
                ((MainLayout)Application.Current.MainWindow).MainFrame.Navigate(detailPage);
            }
        }

        public void LoadProductsByCategory(int categoryId)
        {
            var products = db.Products
                                .Where(p => p.CategoryID == categoryId)
                                .ToList();
            ProductItemsControl.ItemsSource = products;
            
        }

        public void SearchProductsByName(string keyword)
        {
            var products = db.Products
                             .Where(p => p.Name.Contains(keyword))
                             .ToList();

            ProductItemsControl.ItemsSource = products;
        }



    }
}
