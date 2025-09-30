using MiniStore.ViewModels.Admin;
using System.Windows.Controls;

namespace MiniStore.Views.Admin
{
    public partial class OrderManagement : UserControl
    {
        public OrderManagement()
        {
            InitializeComponent();
            this.DataContext = new OrderManagementViewModel();
        }
    }
}
