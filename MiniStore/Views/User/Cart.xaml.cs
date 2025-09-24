using MiniStore.ViewModels;
using MiniStore.ViewModels.User;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MiniStore.Helpers;

namespace MiniStore.Views.User
{
    public partial class Cart : Page
    {
        private MiniStoreEntities db = new MiniStoreEntities();
        private int CurrentUserId = CurrentUser.UserID;

        public ObservableCollection<CartItemViewModel> Items { get; set; }

        public Cart()
        {
            InitializeComponent();
            LoadCart();
        }

        private void LoadCart()
        {
            var cart = db.Carts.FirstOrDefault(c => c.UserID == CurrentUserId);
            if (cart == null)
            {
                Items = new ObservableCollection<CartItemViewModel>();
                CartList.ItemsSource = Items;
                UpdateEmptyCartMessage();
                return;
            }

            Items = new ObservableCollection<CartItemViewModel>(
                db.CartDetails
                  .Where(cd => cd.CartID == cart.CartID)
                  .Select(cd => new CartItemViewModel
                  {
                      CartDetailID = cd.CartDetailID,
                      ProductID = cd.Product.ProductID,
                      ProductName = cd.Product.Name,
                      ImagePath = cd.Product.ImageUrl,
                      UnitPrice = cd.UnitPrice,
                      Quantity = cd.Quantity
                  })
                  .ToList()
            );

            CartList.ItemsSource = Items;
            UpdateSummary();
            UpdateEmptyCartMessage();
        }

        private void UpdateEmptyCartMessage()
        {
            if (Items == null || Items.Count == 0)
            {
                EmptyCartMessage.Visibility = Visibility.Visible;
                RightPanel.Visibility = Visibility.Collapsed;
            }
            else
            {
                EmptyCartMessage.Visibility = Visibility.Collapsed;
                RightPanel.Visibility = Visibility.Visible;
            }
        }


        private void BtnIncrease_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var item = Items.FirstOrDefault(x => x.CartDetailID == id);
                if (item != null)
                {
                    item.Quantity++;
                    SaveQuantityToDb(item);
                    UpdateSummary();
                }
            }
        }

        private void BtnDecrease_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is int id)
            {
                var item = Items.FirstOrDefault(x => x.CartDetailID == id);
                if (item != null && item.Quantity > 1)
                {
                    item.Quantity--;
                    SaveQuantityToDb(item);
                    UpdateSummary();
                }
                else if (item != null && item.Quantity == 1)
                {
                    Items.Remove(item);
                    var detail = db.CartDetails.Find(item.CartDetailID);
                    if (detail != null)
                    {
                        db.CartDetails.Remove(detail);
                        db.SaveChanges();
                    }
                    UpdateSummary();
                }
            }
        }

        private void Btn_Delete(object sender, RoutedEventArgs e)
        {
            // Lấy CartDetailID từ Tag
            if (sender is Button btn && btn.Tag is int id)
            {
                // Tìm item trong ObservableCollection
                var item = Items.FirstOrDefault(x => x.CartDetailID == id);
                if (item != null)
                {
                    // Xóa khỏi collection (UI sẽ tự cập nhật)
                    Items.Remove(item);

                    // Xóa khỏi database
                    var detail = db.CartDetails.Find(item.CartDetailID);
                    if (detail != null)
                    {
                        db.CartDetails.Remove(detail);
                        db.SaveChanges();
                    }

                    // Cập nhật tổng tiền
                    UpdateSummary();
                }
            }
        }


        private void SaveQuantityToDb(CartItemViewModel vm)
        {
            var entity = db.CartDetails.Find(vm.CartDetailID);
            if (entity != null)
            {
                entity.Quantity = vm.Quantity;
                db.SaveChanges();
            }
        }

        private float UpdateSummary()
        {
            decimal subtotal = Items.Sum(i => i.Total);
            txtSubtotalValue.Text = $"{subtotal:N0} đ";
            txtTotalValue.Text = $"{subtotal + 30000:N0} đ"; // cộng phí ship
            btnCheckout.Content = $"THANH TOÁN ({subtotal + 30000:N0} đ)";
            return (float)subtotal;
        }


        private void btnCheckout_Click(object sender, RoutedEventArgs e)
        {
            if (Items == null || Items.Count == 0)
            {
                MessageBox.Show("Giỏ hàng trống!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Lấy tổng tiền từ UpdateSummary
            float subtotal = UpdateSummary();

            // Lấy MainWindow / MainFrame
            if (Application.Current.MainWindow is MainLayout mainWindow)
            {
                // Tạo OrderPage, truyền Items và subtotal
                var orderPage = new OrderView(Items, (decimal)subtotal);

                // Điều hướng sang Order
                mainWindow.MainFrame.Navigate(orderPage);
            }
        }

    }
}
