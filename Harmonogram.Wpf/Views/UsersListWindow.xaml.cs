using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;
namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Interaction logic for UsersListWindow.xaml
    /// </summary>
    public partial class UsersListWindow : HC.Window
    {
        public UsersListWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<UsersListViewModel>();
        }
    }
}
