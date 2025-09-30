using MiniStore.Helpers;
using MiniStore.Views.User;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MiniStore.ViewModels.User
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        private readonly MiniStoreEntities _db = new MiniStoreEntities();

        public ObservableCollection<Product> Products { get; set; } = new ObservableCollection<Product>();

        private Product _selectedProduct;
        public Product SelectedProduct
        {
            get => _selectedProduct;
            set { _selectedProduct = value; OnPropertyChanged(nameof(SelectedProduct)); }
        }

        // Commands
        public ICommand LoadProductsCommand { get; }
        public ICommand LoadProductsByCategoryCommand { get; }
        public ICommand SearchProductsCommand { get; }
        public ICommand AddToCartCommand { get; }
        public ICommand ViewProductDetailCommand { get; }

        public HomeViewModel()
        {
            LoadProductsCommand = new RelayCommand(_ => LoadProducts());
            LoadProductsByCategoryCommand = new RelayCommand(param => LoadProductsByCategory((int)param));
            SearchProductsCommand = new RelayCommand(param => SearchProductsByName(param?.ToString() ?? ""));
            AddToCartCommand = new RelayCommand(param =>
            {
                if (param is Product product)
                    CartHelper.AddToCart(CurrentUser.UserID, product.ProductID, 1, product.Price);
            });
            ViewProductDetailCommand = new RelayCommand(param =>
            {
                if (param is Product product)
                {
                    var detailPage = new ProductDetailView(product);
                    var mainWindow = System.Windows.Application.Current.MainWindow as MainLayout;
                    mainWindow?.MainFrame.Navigate(detailPage);
                }
            });

            // Load all products initially
            LoadProducts();
        }

        public void LoadProducts()
        {
            Products.Clear();
            foreach (var p in _db.Products.ToList())
                Products.Add(p);
        }

        public void LoadProductsByCategory(int categoryId)
        {
            Products.Clear();
            foreach (var p in _db.Products.Where(p => p.CategoryID == categoryId))
                Products.Add(p);
        }

        public void SearchProductsByName(string keyword)
        {
            Products.Clear();
            foreach (var p in _db.Products.Where(p => p.Name.Contains(keyword)))
                Products.Add(p);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
