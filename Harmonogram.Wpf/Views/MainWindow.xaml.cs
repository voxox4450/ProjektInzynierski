using HC = HandyControl.Controls;
using Harmonogram.Wpf.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;

namespace Harmonogram.Wpf
{
    public partial class MainWindow : HC.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<MainViewModel>();
        }


    }
}