using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;
namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : HC.Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<RegisterViewModel>();
        }


    }
}
