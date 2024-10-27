using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Interaction logic for WorkHoursWindow.xaml
    /// </summary>
    public partial class WorkHoursWindow : HC.Window
    {
        public WorkHoursWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<WorkHoursViewModel>();
        }
    }
}
