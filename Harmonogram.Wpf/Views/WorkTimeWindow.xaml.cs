using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Interaction logic for WorkHoursWindow.xaml
    /// </summary>
    public partial class WorkTimeWindow : HC.Window
    {
        public WorkTimeWindow()
        {
            InitializeComponent();
            DataContext = new WorkTimeViewModel();
        }
    }
}
