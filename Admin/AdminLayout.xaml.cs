using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

// Đảm bảo namespace này khớp với project của bạn
namespace MiniStore.Views.Admin
{
    /// <summary>
    /// Interaction logic for AdminLayout.xaml
    /// </summary>
    public partial class AdminLayout : Window
    {
        // Danh sách để quản lý tất cả các nút điều hướng
        private readonly List<Button> _menuButtons;

        public AdminLayout()
        {
            InitializeComponent();

            // Thêm các nút từ XAML vào danh sách để dễ dàng quản lý
            _menuButtons = new List<Button>
            {
                DashboardButton,
                ProductButton,
                UserButton,
                OrderButton
            };

            // Thiết lập trạng thái ban đầu khi cửa sổ được mở
            // Mặc định, chọn Dashboard
            SetActiveButton(DashboardButton);
            MainContent.Content = new Dashboard();
        }

        /// <summary>
        /// Phương thức trung tâm để cập nhật trạng thái của các nút menu.
        /// </summary>
        /// <param name="clickedButton">Nút vừa được nhấn.</param>
        private void SetActiveButton(Button clickedButton)
        {
            // 1. Reset trạng thái của tất cả các nút về "không được chọn"
            // Bằng cách set IsEnabled = true, chúng ta vô hiệu hóa DataTrigger "active"
            foreach (var button in _menuButtons)
            {
                button.IsEnabled = true;
            }

            // 2. Kích hoạt trạng thái "active" cho nút vừa được nhấn
            // Bằng cách set IsEnabled = false, DataTrigger trong XAML sẽ được kích hoạt,
            // thay đổi giao diện của nút (hiển thị indicator, đổi màu nền/chữ).
            if (clickedButton != null)
            {
                clickedButton.IsEnabled = false;
            }
        }

        // --- CÁC HÀM XỬ LÝ SỰ KIỆN CLICK CHO TỪNG NÚT ---

        private void NavigateToDashboard_Click(object sender, RoutedEventArgs e)
        {
            // Cập nhật trạng thái active cho nút Dashboard
            SetActiveButton(sender as Button);
            // Hiển thị UserControl Dashboard trong khu vực nội dung chính
            MainContent.Content = new Dashboard();
        }

        private void NavigateToProducts_Click(object sender, RoutedEventArgs e)
        {
            // Cập nhật trạng thái active cho nút Quản lý sản phẩm
            SetActiveButton(sender as Button);
            // Hiển thị UserControl ProductManagement
            MainContent.Content = new ProductManagement();
        }

        private void NavigateToUsers_Click(object sender, RoutedEventArgs e)
        {
            // Cập nhật trạng thái active cho nút Quản lý người dùng
            SetActiveButton(sender as Button);
            // Hiển thị UserControl UserManagement
            MainContent.Content = new UserManagement();
        }

        private void NavigateToOrders_Click(object sender, RoutedEventArgs e)
        {
            // Cập nhật trạng thái active cho nút Quản lý đơn hàng
            SetActiveButton(sender as Button);
            // Hiển thị UserControl OrderManagement
            MainContent.Content = new OrderManagement();
        }
    }
}