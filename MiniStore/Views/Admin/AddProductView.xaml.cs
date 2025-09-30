using System.Windows.Controls;
using MiniStore.ViewModels.Admin;

namespace MiniStore.Views.Admin
{
    public partial class AddProductView : UserControl
    {
        public AddProductView()
        {
            InitializeComponent();
            DataContext = new AddProductViewModel();
        }

    }
}
