using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MiniStore.Data;

namespace MiniStore.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _phone;
        private string _password;
        private string _phoneError;
        private string _passwordError;

        public string Phone
        {
            get => _phone;
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
                PhoneError = string.Empty;
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                PasswordError = string.Empty;
            }
        }

        public string PhoneError
        {
            get => _phoneError;
            set { _phoneError = value; OnPropertyChanged(nameof(PhoneError)); }
        }

        public string PasswordError
        {
            get => _passwordError;
            set { _passwordError = value; OnPropertyChanged(nameof(PasswordError)); }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            if (string.IsNullOrEmpty(Phone))
            {
                PhoneError = "Số điện thoại không được để trống!";
                return;
            }

            if (string.IsNullOrEmpty(Password))
            {
                PasswordError = "Mật khẩu không được để trống!";
                return;
            }

            try
            {
                // Check tài khoản + mật khẩu
                string query = @"SELECT COUNT(*) 
                                 FROM Users 
                                 WHERE Username = @username AND PasswordHash = @password";

                int count = (int)DatabaseHelper.ExecuteScalar(query, cmd =>
                {
                    cmd.Parameters.AddWithValue("@username", Phone);
                    cmd.Parameters.AddWithValue("@password", Password);
                });

                if (count > 0)
                {
                    MessageBox.Show("Đăng nhập thành công!");
                    // 👉 Tại đây bạn có thể mở trang MainWindow hoặc Dashboard
                }
                else
                {
                    MessageBox.Show("Sai số điện thoại hoặc mật khẩu!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi kết nối: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
