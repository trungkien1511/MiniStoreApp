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
using System.Windows.Shapes;

namespace MiniStore.Views.User
{
    /// <summary>
    /// Interaction logic for OrderSuccess.xaml
    /// </summary>
    public partial class OrderSuccess : Page
    {
        public OrderSuccess(string orderNumber, decimal totalAmount)
        {
            InitializeComponent();
            txtOrderInfo.Text = $"Mã đơn hàng: {orderNumber}\nTổng thanh toán: {totalAmount:N0} ₫";
        }

        private void BtnBackHome_Click(object sender, RoutedEventArgs e)
        {
            if (Application.Current.MainWindow is MainLayout mainWindow)
            {
                mainWindow.MainFrame.Navigate(new Home());
            }
        }
    }

}
