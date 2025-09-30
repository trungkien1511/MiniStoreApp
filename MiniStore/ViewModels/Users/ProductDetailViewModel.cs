using MiniStore.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace MiniStore.ViewModels
{
    public class ProductDetailViewModel : INotifyPropertyChanged
    {
        private Product _product;
        private int _quantity;

        public ProductDetailViewModel(Product product)
        {
            _product = product;
            _quantity = 1;

            IncreaseQuantityCommand = new RelayCommand(_ => IncreaseQuantity());
            DecreaseQuantityCommand = new RelayCommand(_ => DecreaseQuantity());
            AddToCartCommand = new RelayCommand(_ => AddToCart(), _ => CanAddToCart);
        }

        public string ProductName => _product.Name;
        public string ProductPrice => _product.Price.ToString("N0") + " đ";
        public string ProductImage => _product.ImageUrl;
        public string ProductStatus => _product.StockQuantity == 0 ? "Hết hàng" : "Còn hàng";
        public Brush ProductStatusColor => _product.StockQuantity == 0 ? Brushes.Red : Brushes.Green;
        public bool CanAddToCart => _product.StockQuantity > 0;

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    OnPropertyChanged();
                }
            }
        }

        // Commands
        public ICommand IncreaseQuantityCommand { get; }
        public ICommand DecreaseQuantityCommand { get; }
        public ICommand AddToCartCommand { get; }

        private void IncreaseQuantity()
        {
            if (Quantity < _product.StockQuantity)
                Quantity++;
            else
                MessageBox.Show("Số lượng vượt quá hàng tồn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void DecreaseQuantity()
        {
            if (Quantity > 1)
                Quantity--;
        }

        private void AddToCart()
        {
            if (Quantity > 0)
            {
                CartHelper.AddToCart(CurrentUser.UserID, _product.ProductID, Quantity, _product.Price);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
