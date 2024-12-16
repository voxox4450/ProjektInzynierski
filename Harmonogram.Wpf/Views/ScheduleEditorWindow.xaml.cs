using HandyControl.Controls;
using Harmonogram.Wpf.ViewModels;

using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy ScheduleEditorWindow.xaml
    /// </summary>
    public partial class ScheduleEditorWindow : HC.Window
    {
        public ScheduleEditorWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Step1.DataContext = DataContext;
            Step2.DataContext = DataContext;
            Step3.DataContext = DataContext;
            Step4.DataContext = DataContext;

            ScheduleEditorViewModel? viewModel = DataContext as ScheduleEditorViewModel;

            if (viewModel is null)
                return;

            viewModel.OnRequestNextStep += (s, e) => StepBar.Next();
            viewModel.OnRequestPrevioustStep += (s, e) => StepBar.Prev();
        }
    }
}