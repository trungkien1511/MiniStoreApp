using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MiniStore.ViewModels.Admin
{
    public class OrderManagementViewModel : INotifyPropertyChanged
    {
        private readonly MiniStoreEntities _db = new MiniStoreEntities();

        private Order _selectedOrder;

        public ObservableCollection<Order> Orders { get; set; }

        public Order SelectedOrder
        {
            get => _selectedOrder;
            set { _selectedOrder = value; OnPropertyChanged(nameof(SelectedOrder)); }
        }

        // Commands
        public ICommand LoadOrdersCommand { get; }
        public ICommand ViewOrderDetailsCommand { get; }
        public ICommand UpdateStatusCommand { get; }

        public OrderManagementViewModel()
        {
            // Load dữ liệu ban đầu
            Orders = new ObservableCollection<Order>(_db.Orders
                .OrderByDescending(o => o.OrderDate)
                .ToList());

            // Khởi tạo command
            
        }

        

        

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
