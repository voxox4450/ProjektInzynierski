using CommunityToolkit.Mvvm.DependencyInjection;
using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Interaction logic for WorkBlockCreatorWindow.xaml
    /// </summary>
    public partial class WorkBlockCreatorWindow : HC.Window
    {
        public WorkBlockCreatorWindow()
        {
            InitializeComponent();
            DataContext = Ioc.Default.GetRequiredService<WorkBlockCreatorViewModel>();
        }
    }
}
