using MiniStore.Views.Admin;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MiniStore.ViewModels.Admin
{
    public class ProductManagementViewModel : INotifyPropertyChanged
    {
        private readonly MiniStoreEntities _db = new MiniStoreEntities();

        private Product _selectedProduct;

        public ObservableCollection<Product> Products { get; set; }
        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Supplier> Suppliers { get; set; }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(nameof(SelectedProduct)); }
        }

        private UserControl _currentChildView;
        public UserControl CurrentChildView
        {
            get => _currentChildView;
            set { _currentChildView = value; OnPropertyChanged(nameof(CurrentChildView)); }
        }


        public ICommand DeleteProductCommand { get; }
        public ICommand AddProductCommand { get; }

        public ProductManagementViewModel()
        {
            Products = new ObservableCollection<Product>(
                _db.Products.Include(p => p.Category)
                            .Include(p => p.Supplier)
                            .OrderBy(p => p.Name)
                            .ToList());

            Categories = new ObservableCollection<Category>(_db.Categories.OrderBy(c => c.Name).ToList());
            Suppliers = new ObservableCollection<Supplier>(_db.Suppliers.OrderBy(s => s.Name).ToList());

            DeleteProductCommand = new RelayCommand(DeleteProduct, _ => SelectedProduct != null);
            AddProductCommand = new RelayCommand(_ => CurrentChildView = new AddProductView());
        }

        private void DeleteProduct(object obj)
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm để xóa.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa sản phẩm '{SelectedProduct.Name}'?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                var entity = _db.Products.FirstOrDefault(p => p.ProductID == SelectedProduct.ProductID);
                if (entity != null)
                {
                    _db.Products.Remove(entity);
                    _db.SaveChanges();
                }

                Products.Remove(SelectedProduct);
                MessageBox.Show("Xóa sản phẩm thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa sản phẩm: {ex.Message}");
            }
        }

       

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
