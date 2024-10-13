using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Wpf.Views;
using MvvmDialogs;
using System.Windows;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class AdminViewModel : ObservableObject, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        
        public AdminViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
        }


        [ObservableProperty]
        private bool? _dialogResult;

        public void Close()
        {
            DialogResult = true;
        }



        [RelayCommand]
        public void Show()
        {
            Close();
            Application.Current.MainWindow.Show();
            
        }

        [RelayCommand]
        public void Register()
        {
            var dialogViewModel = new RegisterViewModel();
            _dialogService.ShowDialog<RegisterWindow>(this, dialogViewModel);
        }


        
        

    }
}
