using Harmonogram.Wpf.ViewModels;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.Views
{
    /// <summary>
    /// Logika interakcji dla klasy WorkBlockEditorWindow.xaml
    /// </summary>
    public partial class WorkBlockEditorWindow : HC.Window
    {
        public WorkBlockEditorWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            Step1.DataContext = DataContext;
            Step2.DataContext = DataContext;

            WorkBlockEditorViewModel? viewModel = DataContext as WorkBlockEditorViewModel;

            if (viewModel is null)
                return;

            viewModel.OnRequestNextStep += (s, e) => StepBar.Next();
            viewModel.OnRequestPrevioustStep += (s, e) => StepBar.Prev();
        }
    }
}