using MiniStore.ViewModels;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using MiniStore.Views.Admin;
public class AdminLayoutViewModel : INotifyPropertyChanged
{
    private UserControl _currentView;
    public UserControl CurrentView
    {
        get => _currentView;
        set { _currentView = value; OnPropertyChanged(nameof(CurrentView)); }
    }

    private string _activeMenu;
    public string ActiveMenu
    {
        get => _activeMenu;
        set { _activeMenu = value; OnPropertyChanged(nameof(ActiveMenu)); }
    }

    public ICommand ShowDashboardCommand { get; }
    public ICommand ShowProductsCommand { get; }
    public ICommand ShowUsersCommand { get; }
    public ICommand ShowOrdersCommand { get; }

    public AdminLayoutViewModel()
    {
        ShowDashboardCommand = new RelayCommand(_ => { CurrentView = new Dashboard(); ActiveMenu = "Dashboard"; });
        ShowProductsCommand = new RelayCommand(_ => { CurrentView = new ProductManagement(); ActiveMenu = "Products"; });
        ShowUsersCommand = new RelayCommand(_ => { CurrentView = new UserManagement(); ActiveMenu = "Users"; });
        ShowOrdersCommand = new RelayCommand(_ => { CurrentView = new OrderManagement(); ActiveMenu = "Orders"; });

        // Default
        ShowDashboardCommand.Execute(null);
    }

    public event PropertyChangedEventHandler PropertyChanged;
    private void OnPropertyChanged(string propName) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
}
