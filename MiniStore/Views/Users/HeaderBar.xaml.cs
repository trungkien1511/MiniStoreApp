using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MiniStore.Helpers;
using MiniStore.ViewModels.User;

namespace MiniStore.Views.User
{
    public partial class HeaderBar : UserControl
    {
        public HeaderBar()
        {
            InitializeComponent();

            // Gọi sự kiện Loaded để khi UserControl hiển thị thì nạp danh mục
            this.DataContext = new HeaderBarViewModel();
        }
    }
}
