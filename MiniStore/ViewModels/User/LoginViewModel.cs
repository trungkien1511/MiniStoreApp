using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using MiniStore.Models;
using MiniStore.Data;

namespace MiniStore.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private string _phone;
        private string _password;

        public string Phone
        {
            get => _phone;
            set { _phone = value; OnPropertyChanged(nameof(Phone)); }
        }

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

        private void Login()
        {
            if (string.IsNullOrWhiteSpace(Phone) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Vui lòng nhập số điện thoại và mật khẩu!");
                return;
            }

            try
            {
                string query = @"SELECT COUNT(*) 
                                 FROM Users 
                                 WHERE Username = @username AND PasswordHash = @password";

                int count = (int)DatabaseHelper.ExecuteScalar(query, cmd =>
                {
                    cmd.Parameters.AddWithValue("@username", Phone);
                    cmd.Parameters.AddWithValue("@password", Password);
                });

                MessageBox.Show(count > 0
                    ? "Đăng nhập thành công!"
                    : "Sai số điện thoại hoặc mật khẩu!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi đăng nhập: {ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
