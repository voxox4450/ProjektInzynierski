using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Interaction logic for SchedulePanelWindow.xaml
    /// </summary>
    public partial class SchedulePanelWindow : HC.Window
    {
        public SchedulePanelWindow()
        {
            InitializeComponent();
            DataContext = new SchedulePanelViewModel();
        }
    }
}
