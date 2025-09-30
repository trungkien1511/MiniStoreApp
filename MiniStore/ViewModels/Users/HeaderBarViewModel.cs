using MiniStore.Helpers;
using MiniStore.Views;
using MiniStore.Views.User;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace MiniStore.ViewModels.User
{
    public class HeaderBarViewModel : INotifyPropertyChanged
    {
        private readonly MiniStoreEntities _db = new MiniStoreEntities();

        public ObservableCollection<Category> Categories { get; set; } = new ObservableCollection<Category>();

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        public string UserName => CurrentUser.UserName;

        public ICommand CategorySelectedCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand OpenCartCommand { get; }
        public ICommand SearchCommand { get; }

        public HeaderBarViewModel()
        {
            LoadCategories();

            CategorySelectedCommand = new RelayCommand(param =>
            {
                if (param is Category category)
                    NavigateToHome(category);
            });

            OpenCartCommand = new RelayCommand(_ =>
            {
                if (Application.Current.MainWindow is MainLayout mainWindow)
                {
                    mainWindow.MainFrame.Navigate(new CartView());
                }
            });


            LogoutCommand = new RelayCommand(_ =>
            {
                var result = MessageBox.Show("Bạn có chắc muốn đăng xuất không?",
                                             "Xác nhận",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    CurrentUser.UserID = 0;
                    var loginWindow = new LoginView();
                    loginWindow.Show();
                    Application.Current.MainWindow.Close();
                    Application.Current.MainWindow = loginWindow;
                }
            });

            SearchCommand = new RelayCommand(_ =>
            {
                if (!string.IsNullOrWhiteSpace(SearchText))
                {
                    var mainWindow = Application.Current.MainWindow as MainLayout;
                    if (mainWindow?.MainFrame.Content is Home homePage &&
                        homePage.DataContext is HomeViewModel homeVM)
                    {
                        homeVM.SearchProductsByName(SearchText);
                    }
                    else
                    {
                        var homePageNew = new Home();
                        mainWindow?.MainFrame.Navigate(homePageNew);

                        homePageNew.Loaded += (s, e) =>
                        {
                            if (homePageNew.DataContext is HomeViewModel homeVM2)
                                homeVM2.SearchProductsByName(SearchText);
                        };
                    }
                }
            });
        }

        private void LoadCategories()
        {
            Categories.Clear();
            foreach (var cat in _db.Categories.ToList())
                Categories.Add(cat);
        }

        private void NavigateToHome(Category category)
        {
            var mainWindow = Application.Current.MainWindow as MainLayout;
            if (mainWindow == null) return;

            if (mainWindow.MainFrame.Content is Home homePage &&
                homePage.DataContext is HomeViewModel homeVM)
            {
                if (category.Name == "Tất cả sản phẩm")
                    homeVM.LoadProducts();
                else
                    homeVM.LoadProductsByCategory(category.CategoryID);
            }
            else
            {
                var homePageNew = new Home();
                mainWindow.MainFrame.Navigate(homePageNew);

                homePageNew.Loaded += (s, e) =>
                {
                    if (homePageNew.DataContext is HomeViewModel homeVM2)
                    {
                        if (category.Name == "Tất cả sản phẩm")
                            homeVM2.LoadProducts();
                        else
                            homeVM2.LoadProductsByCategory(category.CategoryID);
                    }
                };
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }
}
