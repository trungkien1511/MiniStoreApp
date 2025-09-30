using MiniStore.Helpers;
using MiniStore.Views;
using MiniStore.Views.User;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MiniStore.ViewModels.User
{
    public class CartViewModel : INotifyPropertyChanged
    {
        private readonly MiniStoreEntities _db = new MiniStoreEntities();
        private readonly int CurrentUserId = CurrentUser.UserID; // dùng user thực tế

        public ObservableCollection<CartItemViewModel> Items { get; set; } = new ObservableCollection<CartItemViewModel>();

        private string _checkoutButtonText;
        public string CheckoutButtonText
        {
            get => _checkoutButtonText;
            set
            {
                _checkoutButtonText = value;
                OnPropertyChanged(nameof(CheckoutButtonText));
            }
        }

        private decimal _subtotal;
        public decimal Subtotal
        {
            get => _subtotal;
            set
            {
                if (_subtotal != value)
                {
                    _subtotal = value;
                    OnPropertyChanged(nameof(Subtotal));
                    OnPropertyChanged(nameof(Total));
                }
            }
        }

        private Visibility _emptyCartVisibility = Visibility.Collapsed;
        public Visibility EmptyCartVisibility
        {
            get => _emptyCartVisibility;
            set
            {
                _emptyCartVisibility = value;
                OnPropertyChanged(nameof(EmptyCartVisibility));
            }
        }

        private Visibility _rightPanelVisibility = Visibility.Visible;
        public Visibility RightPanelVisibility
        {
            get => _rightPanelVisibility;
            set
            {
                _rightPanelVisibility = value;
                OnPropertyChanged(nameof(RightPanelVisibility));
            }
        }

        public decimal Total => Subtotal + 30000m; // cộng phí ship

        // Command
        public ICommand IncreaseCommand { get; }
        public ICommand DecreaseCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand CheckoutCommand { get; }

        public CartViewModel()
        {
            IncreaseCommand = new RelayCommand(param =>
            {
                if (param is int cartDetailId)
                    IncreaseQuantity(cartDetailId);
            });

            DecreaseCommand = new RelayCommand(param =>
            {
                if (param is int cartDetailId)
                    DecreaseQuantity(cartDetailId);
            });

            DeleteCommand = new RelayCommand(param =>
            {
                if (param is int cartDetailId)
                    DeleteItem(cartDetailId);
            });

            CheckoutCommand = new RelayCommand(_ => Checkout());

            UpdateEmptyCartMessage();
            LoadCart();
        }

        private void UpdateEmptyCartMessage()
        {
            if (Items == null || Items.Count == 0)
            {
                EmptyCartVisibility = Visibility.Visible;
                RightPanelVisibility = Visibility.Collapsed;
            }
            else
            {
                EmptyCartVisibility = Visibility.Collapsed;
                RightPanelVisibility = Visibility.Visible;
            }
        }

        private void LoadCart()
        {
            Items.Clear();

            var cart = _db.Carts.FirstOrDefault(c => c.UserID == CurrentUserId);
            if (cart == null) return;

            var details = _db.CartDetails.Where(cd => cd.CartID == cart.CartID).ToList();
            foreach (var cd in details)
            {
                Items.Add(new CartItemViewModel
                {
                    CartDetailID = cd.CartDetailID,
                    ProductID = cd.Product.ProductID,
                    ProductName = cd.Product.Name,
                    ImagePath = cd.Product.ImageUrl,
                    UnitPrice = cd.UnitPrice,
                    Quantity = cd.Quantity
                });
            }

            UpdateSummary();
            UpdateEmptyCartMessage();

        }

        private void IncreaseQuantity(int cartDetailId)
        {
            var item = Items.FirstOrDefault(x => x.CartDetailID == cartDetailId);
            if (item == null) return;

            item.Quantity++;
            SaveQuantityToDb(item);
            UpdateSummary();
        }

        private void DecreaseQuantity(int cartDetailId)
        {
            var item = Items.FirstOrDefault(x => x.CartDetailID == cartDetailId);
            if (item == null) return;

            if (item.Quantity > 1)
            {
                item.Quantity--;
                SaveQuantityToDb(item);
            }
            else
            {
                DeleteItem(cartDetailId);
            }

            UpdateSummary();
            UpdateEmptyCartMessage();
        }

        private void DeleteItem(int cartDetailId)
        {
            var item = Items.FirstOrDefault(x => x.CartDetailID == cartDetailId);
            if (item == null) return;

            Items.Remove(item);
            var detail = _db.CartDetails.Find(cartDetailId);
            if (detail != null)
            {
                _db.CartDetails.Remove(detail);
                _db.SaveChanges();
            }
            UpdateSummary();
            UpdateEmptyCartMessage();
        }

        private void SaveQuantityToDb(CartItemViewModel vm)
        {
            var entity = _db.CartDetails.Find(vm.CartDetailID);
            if (entity != null)
            {
                entity.Quantity = vm.Quantity;
                _db.SaveChanges();
            }
        }

        private void UpdateSummary()
        {
            Subtotal = Items.Sum(i => i.Total);

            CheckoutButtonText = $"Thanh toán: {Total:N0} đ";
        }

        private void Checkout()
        {
            if (Items.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (Application.Current.MainWindow is MainLayout mainWindow)
            {
                var orderPage = new OrderView(Items, Total);
                mainWindow.MainFrame.Navigate(orderPage);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
