using MiniStore.ViewModels.User;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MiniStore.Helpers;
namespace MiniStore.Views.User
{
    /// <summary>
    /// Interaction logic for Order.xaml
    /// </summary>
    public partial class OrderView : Page
    {   public ObservableCollection<CartItemViewModel> CartItems { get; set; }
        public decimal Subtotal { get; set; }
        private MiniStoreEntities db = new MiniStoreEntities();
        int CurrentUserId = CurrentUser.UserID;

        public OrderView()
        {
            InitializeComponent();
        }

        public OrderView(ObservableCollection<CartItemViewModel> items, decimal subtotal)
        {
            InitializeComponent();

            CartItems = new ObservableCollection<CartItemViewModel>(items);
            Subtotal = subtotal;

            // Cập nhật UI
            txtSubtotal.Text = $"{Subtotal:N0} ₫";
            txtShippingFee.Text = $"{30000:N0} ₫";
            txtTotal.Text = $"{Subtotal} ₫";
        }

        private void btnConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Vui lòng nhập họ tên!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtPhone.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Vui lòng nhập địa chỉ!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                txtAddress.Focus();
                return;
            }

            try
            {
                decimal shippingFee = 30000; // Nếu muốn tính phí
                decimal totalAmount = Subtotal + shippingFee;

                // Tạo Order mới
                var order = new Order
                {
                    UserID = CurrentUserId,
                    OrderDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    Status = "Pending",
                    PaymentMethod = rbCOD.IsChecked == true ? "COD" : "VNPAY",
                    Address = txtAddress.Text.Trim(),
                    Note = txtNote.Text.Trim(),
                    PhoneNumber = txtPhone.Text.Trim()
                };

                db.Orders.Add(order);
                db.SaveChanges(); // Lưu để order.OrderID có giá trị

                // Thêm OrderDetail từ CartItems
                foreach (var item in CartItems)
                {
                    MessageBox.Show($"ProductID: {item.ProductID}\n" +
                    $"ProductName: {item.ProductName}\n" +
                    $"UnitPrice: {item.UnitPrice}\n" +
                    $"Quantity: {item.Quantity}");
                    var orderDetail = new OrderDetail
                    {
                        OrderID = order.OrderID,       // phải dùng order.OrderID
                        ProductID = item.ProductID,    // phải dùng ProductID thực
                        ProductName = item.ProductName,
                        UnitPrice = item.UnitPrice,
                        Quantity = item.Quantity
                    };
                    db.OrderDetails.Add(orderDetail);
                }

                db.SaveChanges();

                var cart = db.Carts.FirstOrDefault(c => c.UserID == 1); // dùng ID người dùng hiện tại
                if (cart != null)
                {
                    // Xóa tất cả CartDetails trước
                    var cartDetails = db.CartDetails.Where(cd => cd.CartID == cart.CartID).ToList();
                    foreach (var cd in cartDetails)
                    {
                        db.CartDetails.Remove(cd);
                    }

                    // Xóa Cart
                    db.Carts.Remove(cart);

                    db.SaveChanges();
                }

                // Chuyển sang trang thành công
                if (Application.Current.MainWindow is MainLayout mainWindow)
                {
                    mainWindow.MainFrame.Navigate(new OrderSuccess(order.OrderID.ToString(), totalAmount));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException?.Message ?? ex.Message);
            }
        }


    }
}
