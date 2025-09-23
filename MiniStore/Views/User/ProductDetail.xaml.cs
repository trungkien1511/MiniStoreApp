using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniStore.Views.User
{
    /// <summary>
    /// Interaction logic for ProductDetail.xaml
    /// </summary>
    public partial class ProductDetail : Page
    {
        private Product _product;

        public ProductDetail(Product product)
        {
            InitializeComponent();
            _product = product;

            // Binding dữ liệu trực tiếp lên UI
            ProductName.Text = _product.Name;
            ProductPrice.Text = _product.Price.ToString("N0") + " đ";

            if (_product.StockQuantity == 0)
            {
                ProductStatus.Text = "Hết hàng";
                ProductStatus.Foreground = Brushes.Red;

                
            }
            else
            {
                ProductStatus.Text = "Còn hàng";
                ProductStatus.Foreground = Brushes.Green;
            }

        }

        private void BtnMinus_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TxtQuantity.Text, out int qty))
            {
                if (qty > 1) // số lượng tối thiểu là 1
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
                qty++; // tăng 1
                TxtQuantity.Text = qty.ToString();
            }
        }



    }
}
