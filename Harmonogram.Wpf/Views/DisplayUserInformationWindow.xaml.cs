using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;
namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Interaction logic for DisplayUserInformationWindow.xaml
    /// </summary>
    public partial class DisplayUserInformationWindow : HC.Window
    {
        public DisplayUserInformationWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<DisplayUserInformationViewModel>();
        }
    }
}