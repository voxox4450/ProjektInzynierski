using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Interaction logic for ManageUserWindow.xaml
    /// </summary>
    public partial class ManageUserWindow : HC.Window
    {
        public ManageUserWindow()
        {
            InitializeComponent();
            DataContext = new ManageUserViewModel();
        }
    }
}
