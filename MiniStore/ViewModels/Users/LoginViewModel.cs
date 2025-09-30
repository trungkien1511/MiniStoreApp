using MiniStore.Helpers;
using MiniStore.Views.Admin;
using MiniStore.Views.User;
using System;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace MiniStore.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        // Password không binding trực tiếp (vì PasswordBox không hỗ trợ Password binding)
        // Bạn có thể xử lý qua AttachedProperty hoặc sự kiện PasswordChanged trong View
        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login(object obj)
        {
            try
            {
                if (string.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                    return;
                }

                string hashedPassword = GetMd5Hash(Password);

                using (var db = new MiniStoreEntities())
                {
                    var user = db.Users
                                 .FirstOrDefault(u => u.Username == Username
                                                   && u.PasswordHash == hashedPassword
                                                   && u.IsActive == true);

                    if (user != null)
                    {
                        MessageBox.Show($"Đăng nhập thành công! Xin chào {user.FullName}");

                        CurrentUser.UserID = user.UserID;
                        CurrentUser.UserName = user.Username;

                        Window windowToShow;
                        if (user.RoleID == 1) // Admin
                            windowToShow = new AdminLayout();
                        else
                            windowToShow = new MainLayout();

                        windowToShow.Show();

                        // Đóng LoginView (truyền qua obj nếu cần)
                        if (obj is Window loginWindow)
                            loginWindow.Close();

                        Application.Current.MainWindow = windowToShow;
                    }
                    else
                    {
                        MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
                    }
                }
            }
            catch (Exception ex)
            {
                // Nếu có InnerException thì hiển thị chi tiết hơn
                MessageBox.Show($"Đăng nhập thất bại!\nLỗi: {ex.InnerException?.Message ?? ex.Message}",
                                "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private string GetMd5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
