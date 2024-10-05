using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy WorkTimeWindow.xaml
    /// </summary>
    public partial class WorkTimeWindow : HC.Window
    {
        public WorkTimeWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<WorkTimeViewModel>();
        }
    }
}