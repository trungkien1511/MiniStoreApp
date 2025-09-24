using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace MiniStore.Views.User
{
    public partial class HeaderBar : UserControl
    {
        public HeaderBar()
        {
            InitializeComponent();

            // Gọi sự kiện Loaded để khi UserControl hiển thị thì nạp danh mục
            this.Loaded += HeaderBar_Loaded;
        }

        private void CartButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainLayout)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new Cart());
        }

        private void HeaderBar_Loaded(object sender, RoutedEventArgs e)
        {
            using (var db = new MiniStoreEntities())
            {
                var categories = db.Categories.ToList();

                CategoryMenu.Items.Clear();

                foreach (var cat in categories)
                {
                    var item = new MenuItem
                    {
                        Header = cat.Name,
                        Tag = cat
                    };

                    item.Click += Category_Click;
                    CategoryMenu.Items.Add(item);
                }
            }
        }

        private void EnsureHome(Action<Home> action)
        {
            if (Application.Current.MainWindow is MainLayout mainWindow)
            {
                void RunAction(Home home) => action?.Invoke(home);

                if (mainWindow.MainFrame.Content is Home homePage)
                {
                    // Nếu đang ở Home → thực hiện ngay
                    RunAction(homePage);
                }
                else
                {
                    // Nếu không phải Home → tạo mới, Navigate và gọi action khi Loaded
                    var homePageNew = new Home();
                    mainWindow.MainFrame.Navigate(homePageNew);
                    homePageNew.Loaded += (s, e) => RunAction(homePageNew);
                }
            }
        }

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Tag is Category category)
            {
                EnsureHome(home =>
                {
                    if (string.Equals(category.Name, "Tất cả sản phẩm", StringComparison.OrdinalIgnoreCase))
                        home.LoadProducts();
                    else
                        home.LoadProductsByCategory(category.CategoryID);
                });
            }
        }
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;

            string keyword = SearchTextBox.Text.Trim();
            if (string.IsNullOrEmpty(keyword)) return;

            EnsureHome(home =>
            {
                home.SearchProductsByName(keyword);
            });
        }


    }
}
