using MiniStore.Helpers;
using MiniStore.Models;
using MiniStore.Views.User;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace MiniStore.Views
{
    public partial class LoginView : Window
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            // Hash password (nếu bạn lưu ở DB là hash MD5 như trước)
            string hashedPassword = GetMd5Hash(password);

            using (var db = new MiniStoreEntities())
            {
                var user = db.Users
                             .FirstOrDefault(u => u.Username == username
                                               && u.PasswordHash == hashedPassword
                                               && u.IsActive == true);

                if (user != null)
                {
                    MessageBox.Show($"Đăng nhập thành công! Xin chào {user.FullName}");
                    CurrentUser.UserID = user.UserID;
                    CurrentUser.UserName = user.Username;

                    var mainWindow = new MainLayout();
                    mainWindow.Show();

                    // Đóng cửa sổ LoginView hiện tại
                    Application.Current.MainWindow.Close();
                    Application.Current.MainWindow = mainWindow;

                }
                else
                {
                    MessageBox.Show("Sai tên đăng nhập hoặc mật khẩu!");
                }
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
    }
}
