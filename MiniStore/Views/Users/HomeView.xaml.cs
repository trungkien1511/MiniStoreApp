using MiniStore.ViewModels.User;
using System.Windows.Controls;
using MiniStore.ViewModels.User;

namespace MiniStore.Views.User
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home()
        {
            InitializeComponent();

            // Gán ViewModel cho DataContext
            this.DataContext = new HomeViewModel();
        }
    }
}
