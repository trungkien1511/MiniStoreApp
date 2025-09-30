using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using UserEntity = MiniStore.User;

namespace MiniStore.ViewModels.Admin
{
    public class UserManagementViewModel : INotifyPropertyChanged
    {
        private readonly MiniStoreEntities _db = new MiniStoreEntities();

        private UserEntity _selectedUser;

        public ObservableCollection<UserEntity> Users { get; set; }
        public ObservableCollection<Role> Roles { get; set; }

        public UserEntity SelectedUser
        {
            get => _selectedUser;
            set { _selectedUser = value; OnPropertyChanged(nameof(SelectedUser)); }

        }

        public ICommand LoadUsersCommand { get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand AddUserCommand { get; }

        public UserManagementViewModel()
        {
            LoadUsers();


            DeleteUserCommand = new RelayCommand(DeleteUser, _ => SelectedUser != null);
            
        }

        private void LoadUsers()
        {
            
             // Include Role để DataGrid có thể bind Role.Name
                Users = new ObservableCollection<UserEntity>(
                    _db.Users
                      .Include("Role")  // hoặc db.Users.Include(u => u.Role) nếu dùng EF 6+
                      .OrderBy(u => u.Username)
                      .ToList()
                );
            
            OnPropertyChanged(nameof(Users));

        }


        private void DeleteUser(object obj)
        {
            if (SelectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn user để xóa.");
                return;
            }

            var confirm = MessageBox.Show(
                $"Bạn có chắc muốn xóa user '{SelectedUser.Username}' cùng tất cả dữ liệu liên quan?",
                "Xác nhận",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                using (var db = new MiniStoreEntities())
                {
                    // 1️⃣ Lấy User mới từ context mới
                    var user = db.Users.FirstOrDefault(u => u.UserID == SelectedUser.UserID);
                    if (user == null)
                    {
                        MessageBox.Show("User không tồn tại.");
                        return;
                    }

                    // 2️⃣ Xóa OrderDetails
                    var orders = db.Orders.Where(o => o.UserID == user.UserID).ToList();
                    var orderIds = orders.Select(o => o.OrderID).ToList();
                    var orderDetails = db.OrderDetails.Where(od => orderIds.Contains((int)od.OrderID)).ToList();
                    db.OrderDetails.RemoveRange(orderDetails);

                    // 3️⃣ Xóa Orders
                    db.Orders.RemoveRange(orders);

                    // 4️⃣ Xóa CartDetails
                    var carts = db.Carts.Where(c => c.UserID == user.UserID).ToList();
                    var cartIds = carts.Select(c => c.CartID).ToList();
                    var cartDetails = db.CartDetails.Where(cd => cartIds.Contains(cd.CartID)).ToList();
                    db.CartDetails.RemoveRange(cartDetails);

                    // 5️⃣ Xóa Carts
                    db.Carts.RemoveRange(carts);

                    // 6️⃣ Cuối cùng xóa User
                    db.Users.Remove(user);

                    // Lưu thay đổi
                    db.SaveChanges();

                    

                    MessageBox.Show("Xóa user và tất cả dữ liệu liên quan thành công!");
                }

                // Cập nhật lại danh sách trong ViewModel
                Users.Remove(SelectedUser);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xóa user: {ex.Message}");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
