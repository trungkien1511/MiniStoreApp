using MiniStore.ViewModels;
using System.Windows.Controls;

namespace MiniStore.Views.User
{
    public partial class ProductDetailView : Page
    {
        public ProductDetailView(Product product)
        {
            InitializeComponent();
            DataContext = new ProductDetailViewModel(product);
        }
    }
}
