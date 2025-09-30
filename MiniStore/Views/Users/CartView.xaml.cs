using MiniStore.ViewModels;
using MiniStore.ViewModels.User;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MiniStore.Helpers;

namespace MiniStore.Views.User
{
    public partial class CartView : Page
    {
        private readonly MiniStoreEntities _db = new MiniStoreEntities();
        private int CurrentUserId = 1;

        public ObservableCollection<CartItemViewModel> Items { get; set; }

        public CartView()
        {
            InitializeComponent();
            //LoadCart();
            this.DataContext = new CartViewModel();
        }

       

    }
}
