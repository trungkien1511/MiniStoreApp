using MiniStore.ViewModels.User;
using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace MiniStore.Views.User
{
    public partial class OrderView : Page
    {
        public OrderView(ObservableCollection<CartItemViewModel> items, decimal subtotal)
        {
            InitializeComponent();
            DataContext = new OrderViewModel(items, subtotal);
        }
    }
}
