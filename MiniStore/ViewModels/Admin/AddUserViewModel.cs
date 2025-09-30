using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MiniStore.ViewModels.Admin
{
    public class AddUserViewModel : INotifyPropertyChanged
    {
        private string _username;
        private string _password;
        private string _fullName;
        private int _selectedRoleId;
        private string _phoneNumber;
        private string _address;
        private DateTime? _dateOfBirth;

        private readonly MiniStoreEntities _db = new MiniStoreEntities();

        public string Username
        {
            get => _username;
            set { _username = value; OnPropertyChanged(nameof(Username)); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; OnPropertyChanged(nameof(Password)); }
        }

        public string FullName
        {
            get => _fullName;
            set { _fullName = value; OnPropertyChanged(nameof(FullName)); }
        }

        public int SelectedRoleId
        {
            get => _selectedRoleId;
            set { _selectedRoleId = value; OnPropertyChanged(nameof(SelectedRoleId)); }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set { _phoneNumber = value; OnPropertyChanged(nameof(PhoneNumber)); }
        }

        public string Address
        {
            get => _address;
            set { _address = value; OnPropertyChanged(nameof(Address)); }
        }

        public DateTime? DateOfBirth
        {
            get => _dateOfBirth;
            set { _dateOfBirth = value; OnPropertyChanged(nameof(DateOfBirth)); }
        }

        public ObservableCollection<Role> Roles { get; set; }

        public ICommand SaveUserCommand { get; }
        public ICommand CancelCommand { get; }

        public AddUserViewModel()
        {
            Roles = new ObservableCollection<Role>(_db.Roles.ToList());
            SelectedRoleId = Roles.FirstOrDefault()?.RoleID ?? 2;

            SaveUserCommand = new RelayCommand(SaveUser);
            CancelCommand = new RelayCommand(ClearForm);
        }

        private void SaveUser(object obj)
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Username và Password không được để trống.");
                return;
            }

            if (_db.Users.Any(u => u.Username == Username))
            {
                MessageBox.Show("Username đã tồn tại.");
                return;
            }

            string passwordHash = HashPasswordMD5(Password);

            var newUser = new MiniStore.User
            {
                Username = Username,
                PasswordHash = passwordHash,
                FullName = FullName,
                RoleID = SelectedRoleId,
                PhoneNumber = PhoneNumber,
                Address = Address,
                DateOfBirth = DateOfBirth,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _db.Users.Add(newUser);
            _db.SaveChanges();

            MessageBox.Show("Thêm user thành công!");
            ClearForm(null);
        }

        private void ClearForm(object obj)
        {
            Username = "";
            Password = "";
            FullName = "";
            SelectedRoleId = Roles.FirstOrDefault()?.RoleID ?? 2;
        }

        private string HashPasswordMD5(string password)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
