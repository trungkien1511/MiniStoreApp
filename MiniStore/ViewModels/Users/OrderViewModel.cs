using MiniStore.Helpers;
using MiniStore.Views.User;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace MiniStore.ViewModels.User
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private readonly MiniStoreEntities _db = new MiniStoreEntities();
        private readonly int _currentUserId = CurrentUser.UserID;

        public ObservableCollection<CartItemViewModel> CartItems { get; set; }

        private string _fullName;
        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(); }
        }

        private string _phone;
        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(); }
        }

        private string _address;
        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(); }
        }

        private string _note;
        public string Note
        {
            get => _note;
            set { _note = value; OnPropertyChanged(); }
        }

        public decimal Subtotal { get; set; }
        public decimal ShippingFee => 30000;
        public decimal Total => Subtotal + ShippingFee;

        private string _paymentMethod = "COD";
        public string PaymentMethod
        {
            get => _paymentMethod;
            set { _paymentMethod = value; OnPropertyChanged(); }
        }

        public ICommand ConfirmOrderCommand { get; }

        public OrderViewModel(ObservableCollection<CartItemViewModel> items, decimal subtotal)
        {
            CartItems = new ObservableCollection<CartItemViewModel>(items);
            Subtotal = subtotal;

            ConfirmOrderCommand = new RelayCommand(_ => ConfirmOrder());
        }

        private void ConfirmOrder()
        {
            // Validate
            if (string.IsNullOrWhiteSpace(FullName))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(Phone))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(Address))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var order = new Order
                {
                    UserID = _currentUserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = Total,
                    Status = "Complete",
                    PaymentMethod = PaymentMethod,
                    Address = Address.Trim(),
                    Note = Note?.Trim(),
                    PhoneNumber = Phone.Trim(),
                    NameOfOrderer = FullName.Trim()
                };

                Console.WriteLine(order.UserID);

                _db.Orders.Add(order);
                _db.SaveChanges();

                foreach (var item in CartItems)
                {
                    var orderDetail = new OrderDetail
                    {
                        OrderID = order.OrderID,
                        ProductID = item.ProductID,
                        ProductName = item.ProductName,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity
                    };
                    _db.OrderDetails.Add(orderDetail);

                    var product = _db.Products.FirstOrDefault(p => p.ProductID == item.ProductID);
                    if (product != null)
                    {
                        product.StockQuantity -= item.Quantity;
                        if (product.StockQuantity < 0) product.StockQuantity = 0;
                    }
                }

                _db.SaveChanges();

                // Xóa giỏ hàng
                var cart = _db.Carts.FirstOrDefault(c => c.UserID == _currentUserId);
                if (cart != null)
                {
                    var cartDetails = _db.CartDetails.Where(cd => cd.CartID == cart.CartID).ToList();
                    foreach (var cd in cartDetails)
                        _db.CartDetails.Remove(cd);

                    _db.Carts.Remove(cart);
                    _db.SaveChanges();
                }

                // Chuyển sang trang thành công
                if (Application.Current.MainWindow is MainLayout mainWindow)
                {
                    mainWindow.MainFrame.Navigate(new OrderSuccess(order.OrderID.ToString(), Total));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
