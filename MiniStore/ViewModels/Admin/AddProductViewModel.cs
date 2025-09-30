using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MiniStore.ViewModels.Admin
{
    public class AddProductViewModel : INotifyPropertyChanged
    {
        private string _productName;
        private decimal _price;
        private int _stockQuantity;
        private int _selectedCategoryId;
        private int _selectedSupplierId;

        private readonly MiniStoreEntities _db = new MiniStoreEntities();

        public string ProductName
        {
            get => _productName;
            set { _productName = value; OnPropertyChanged(nameof(ProductName)); }
        }

        public int SelectedSupplierId
        {
            get => _selectedSupplierId;
            set { _selectedSupplierId = value; OnPropertyChanged(nameof(SelectedSupplierId)); }
        }

        public decimal Price
        {
            get => _price;
            set { _price = value; OnPropertyChanged(nameof(Price)); }
        }

        public int StockQuantity
        {
            get => _stockQuantity;
            set { _stockQuantity = value; OnPropertyChanged(nameof(StockQuantity)); }
        }

        public int SelectedCategoryId
        {
            get => _selectedCategoryId;
            set { _selectedCategoryId = value; OnPropertyChanged(nameof(SelectedCategoryId)); }
        }

        public ObservableCollection<Category> Categories { get; set; }
        public ObservableCollection<Supplier> Suppliers { get; set; }

        public ICommand SaveProductCommand { get; }
        public ICommand ResetCommand { get; }

        public AddProductViewModel()
        {
            Categories = new ObservableCollection<Category>(_db.Categories.OrderBy(c => c.Name).ToList());
            Suppliers = new ObservableCollection<Supplier>(_db.Suppliers.OrderBy(s => s.Name).ToList());

            SaveProductCommand = new RelayCommand(SaveProduct);
            ResetCommand = new RelayCommand(ResetForm);
        }

        private void SaveProduct(object obj)
        {
            if (string.IsNullOrWhiteSpace(ProductName) || Price <= 0 || StockQuantity < 0 || SelectedCategoryId == 0)
            {
                MessageBox.Show("Vui lòng nhập thông tin hợp lệ.");
                return;
            }

            
            var product = new Product
            {
                Name = ProductName.Trim(),
                Price = Price,
                StockQuantity = StockQuantity,
                CategoryID = SelectedCategoryId,
                SupplierID = SelectedSupplierId, 
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _db.Products.Add(product);
            _db.SaveChanges();
            

            MessageBox.Show("Thêm sản phẩm thành công!");
            ResetForm(null);
        }

        private void ResetForm(object obj)
        {
            ProductName = string.Empty;
            Price = 0;
            StockQuantity = 0;
            SelectedCategoryId = Categories.Any() ? Categories.First().CategoryID : 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
