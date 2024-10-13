using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Interaction logic for AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : HC.Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<AdminViewModel>();
        }
    }
}
