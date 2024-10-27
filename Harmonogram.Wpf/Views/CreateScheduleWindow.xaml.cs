using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Interaction logic for CreateScheduleWindow.xaml
    /// </summary>
    public partial class CreateScheduleWindow : HC.Window
    {
        public CreateScheduleWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<CreateScheduleViewModel>();
        }
    }
}
