using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ScheduleWindow.xaml
    /// </summary>
    public partial class ScheduleWindow : HC.Window
    {
        public ScheduleWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<ScheduleViewModel>();
        }
    }
}