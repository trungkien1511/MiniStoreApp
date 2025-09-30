using System;
using System.Linq;
using System.Windows;

namespace MiniStore.Helpers
{
    public static class CartHelper
    {
        public static void AddToCart(int userId, int productId, int quantity, decimal price)
        {
            if (quantity <= 0)
            {
                MessageBox.Show("Số lượng phải lớn hơn 0!",
                                "Lỗi",
                                MessageBoxButton.OK,
                                MessageBoxImage.Warning);
                return;
            }

            using (var db = new MiniStoreEntities())
            {
                // 1. Tìm giỏ hàng của user
                var cart = db.Carts.FirstOrDefault(c => c.UserID == userId);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        UserID = userId,
                        CreatedAt = DateTime.Now
                    };
                    db.Carts.Add(cart);
                    db.SaveChanges(); // cần CartID
                }

                // 2. Kiểm tra sản phẩm đã có trong giỏ chưa
                var cartDetail = db.CartDetails
                                   .FirstOrDefault(cd => cd.CartID == cart.CartID
                                                      && cd.ProductID == productId);

                if (cartDetail != null)
                {
                    // Nếu đã có thì cộng thêm số lượng
                    cartDetail.Quantity += quantity;
                }
                else
                {
                    // Nếu chưa có thì thêm mới
                    cartDetail = new CartDetail
                    {
                        CartID = cart.CartID,
                        ProductID = productId,
                        Quantity = quantity,
                        UnitPrice = price
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
