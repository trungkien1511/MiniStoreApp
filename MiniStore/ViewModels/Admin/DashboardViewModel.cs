using MiniStore.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace MiniStore.ViewModels.Admin
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly MiniStoreEntities _db = new MiniStoreEntities();

        private decimal _totalRevenue;
        private int _totalOrders;
        private string _revenueChange;
        private string _newOrdersToday;
        private int _newCustomers;

        public decimal TotalRevenue
        {
            get => _totalRevenue;
            set { _totalRevenue = value; OnPropertyChanged(nameof(TotalRevenue)); OnPropertyChanged(nameof(TotalRevenueDisplay)); }
        }

        public string TotalRevenueDisplay => TotalRevenue.ToString("N0") + " đ";

        public int TotalOrders
        {
            get => _totalOrders;
            set { _totalOrders = value; OnPropertyChanged(nameof(TotalOrders)); }
        }

        public string RevenueChange
        {
            get => _revenueChange;
            set { _revenueChange = value; OnPropertyChanged(nameof(RevenueChange)); }
        }

        public string NewOrdersToday
        {
            get => _newOrdersToday;
            set { _newOrdersToday = value; OnPropertyChanged(nameof(NewOrdersToday)); }
        }

        public int NewCustomers
        {
            get => _newCustomers;
            set { _newCustomers = value; OnPropertyChanged(nameof(NewCustomers)); }
        }

        public ObservableCollection<TopSellingProduct> TopSellingProducts { get; set; }
        public ObservableCollection<RecentOrder> RecentOrders { get; set; }

        public DashboardViewModel()
        {
            TopSellingProducts = new ObservableCollection<TopSellingProduct>();
            RecentOrders = new ObservableCollection<RecentOrder>();
            LoadDashboardData();
        }

        private void LoadDashboardData()
        {
            // Tổng doanh thu
            TotalRevenue = _db.Orders.Sum(o => (decimal?)o.TotalAmount) ?? 0;

            // Tổng số đơn hàng
            TotalOrders = _db.Orders.Count();
            
            // Số khách hàng mới trong tháng
            var firstDayOfMonth = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            NewCustomers = _db.Users.Count(u => u.CreatedAt >= firstDayOfMonth);

            var topProducts = _db.OrderDetails
                                 .GroupBy(od => od.Product.Name)
                                 .Select(g => new TopSellingProduct
                                 {
                                     Name = g.Key,
                                     Amount = (int)g.Sum(od => od.Quantity)
                                 })
                                 .OrderByDescending(p => p.Amount)
                                 .Take(5)
                                 .ToList();

            TopSellingProducts.Clear();
            foreach (var p in topProducts)
                TopSellingProducts.Add(p);

            // --- Đơn hàng gần đây ---
            var recentOrders = _db.Orders
                      .OrderByDescending(o => o.OrderDate)
                      .Take(5)
                      .ToList() // quan trọng: đưa về List trước khi thao tác format
                      .Select(o => new RecentOrder
                      {
                          OrderID = o.OrderID,
                          TotalAmount = o.TotalAmount,
                          Status = o.Status,
                          OrderDate = o.OrderDate
                      })
                      .ToList();

            RecentOrders.Clear();
            foreach (var r in recentOrders)
                RecentOrders.Add(r);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
