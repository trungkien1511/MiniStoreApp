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

        private void Category_Click(object sender, RoutedEventArgs e)
        {
            if (sender is MenuItem menuItem && menuItem.Tag is Category category)
            {
                // Lấy MainWindow để gọi Home Page
                var mainWindow = (MainLayout)Application.Current.MainWindow;
                if (mainWindow.MainFrame.Content is Home homePage)
                {
                    if (category.Name == "Tất cả sản phẩm") // kiểm tra tên category
                    {
                        homePage.LoadProducts(); // load tất cả sản phẩm
                    }
                    else
                    {
                        homePage.LoadProductsByCategory(category.CategoryID); // load theo category
                    }
                }
            }
        }

        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Nếu nhấn Enter mới tìm
            if (e.Key != Key.Enter) return;

            string keyword = SearchTextBox.Text.Trim();
            if (string.IsNullOrEmpty(keyword)) return;

            // Lấy MainWindow để truy cập Home
            if (Application.Current.MainWindow is MainLayout mainWindow &&
                mainWindow.MainFrame.Content is Home homePage)
            {
                homePage.SearchProductsByName(keyword);
            }
        }

    }
}
