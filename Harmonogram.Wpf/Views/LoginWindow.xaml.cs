using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    public partial class LoginWindow : HC.Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<LoginViewModel>();
        }

    }
}
