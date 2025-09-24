using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MiniStore.Helpers;
using MiniStore;


namespace MiniStore.Views.User
{
    public partial class ProductDetail : Page
    {
        private Product _product;

        public ProductDetail(Product product)
        {
            InitializeComponent();
            _product = product;

            LoadProduct();
        }

        /// <summary>
        /// Binding dữ liệu sản phẩm lên UI
        /// </summary>
        private void LoadProduct()
        {
            ProductName.Text = _product.Name;
            ProductPrice.Text = _product.Price.ToString("N0") + " đ";

            // Nếu có ảnh trong DB thì thay ảnh mặc định
            if (!string.IsNullOrEmpty(_product.ImageUrl))
            {
                ProductImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(_product.ImageUrl, UriKind.RelativeOrAbsolute));
            }

            if (_product.StockQuantity == 0)
            {
                ProductStatus.Text = "Hết hàng";
                ProductStatus.Foreground = Brushes.Red;
                btnCart.IsEnabled = false;
                btnCart.Opacity = 0.5;
            }
            else
            {
                ProductStatus.Text = "Còn hàng";
                ProductStatus.Foreground = Brushes.Green;
                btnCart.IsEnabled = true;
                btnCart.Opacity = 1;
            }
        }

        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TxtQuantity.Text, out int qty))
            {
                if (qty > 1)
                {
                    qty--;
                    TxtQuantity.Text = qty.ToString();
                }
            }
        }

        private void BtnPlus_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TxtQuantity.Text, out int qty))
            {
                if (qty < _product.StockQuantity)
                {
                    qty++;
                    TxtQuantity.Text = qty.ToString();
                }
                else
                {
                    MessageBox.Show("Số lượng vượt quá hàng tồn!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void Cart_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TxtQuantity.Text, out int qty) && qty > 0)
            {
                using (var db = new MiniStoreEntities())
                {
                    int userId = 1; // Use the correct class that contains the UserID property

                    // 1. Tìm giỏ hàng của user
                    var cart = db.Carts.FirstOrDefault(c => c.UserID == userId);

                    if (cart == null)
                    {
                        // Nếu chưa có giỏ hàng -> tạo mới
                        cart = new MiniStore.Cart
                        {
                            UserID = 1,
                            CreatedAt = DateTime.Now
                        };
                        db.Carts.Add(cart);
                        db.SaveChanges(); // lưu để có CartID
                    }

                    // 2. Kiểm tra sản phẩm đã có trong giỏ chưa
                    var cartDetail = db.CartDetails
                                       .FirstOrDefault(cd => cd.CartID == cart.CartID
                                                          && cd.ProductID == _product.ProductID);

                    if (cartDetail != null)
                    {
                        // Nếu đã có thì cộng thêm số lượng
                        cartDetail.Quantity += qty;
                    }
                    else
                    {
                        // Nếu chưa có -> thêm mới
                        cartDetail = new CartDetail
                        {
                            CartID = cart.CartID,
                            ProductID = _product.ProductID,
                            Quantity = qty,
                            UnitPrice = _product.Price
                        };
                        db.CartDetails.Add(cartDetail);
                    }

                    // 3. Lưu thay đổi
                    db.SaveChanges();
                }

                MessageBox.Show("Sản phẩm đã được thêm vào giỏ!",
                                "Thông báo",
                                MessageBoxButton.OK,
                                MessageBoxImage.Information);
            }
        }

    }
}
