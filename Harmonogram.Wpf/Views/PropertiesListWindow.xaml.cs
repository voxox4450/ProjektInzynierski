using Harmonogram.Wpf.ViewModels.ListViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Interaction logic for PropertiesListWindow.xaml
    /// </summary>
    public partial class PropertiesListWindow : HC.Window
    {
        public PropertiesListWindow()
        {
            InitializeComponent();
            DataContext = new PropertiesListViewModel();
        }
    }
}
