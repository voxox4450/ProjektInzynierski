using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Interaction logic for CreateUserWindow.xaml
    /// </summary>
    public partial class CreateUserWindow : HC.Window
    {
        public CreateUserWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<CreateUserViewModel>();
        }
    }
}
