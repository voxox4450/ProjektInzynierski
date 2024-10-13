using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Wpf.Views;
using MvvmDialogs;
using System.Windows;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly LoginViewModel _loginViewModel;

        [ObservableProperty]
        private User? _isLogged;

        public MainViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _loginViewModel = Ioc.Default.GetRequiredService<LoginViewModel>();
            _loginViewModel.IsLoggedIn += OnLoggedIn;



        }

        [RelayCommand]
        private static void Hide()
        {
            // Hide the current window
            Application.Current.MainWindow.Hide();
        }


        [RelayCommand]
        private void OpenLogin()
        {
            if (IsLogged == null)
            {
                _dialogService.ShowDialog<LoginWindow>(this, _loginViewModel);
                
            }

            OnLoggedIn(this, _loginViewModel.userToLogged);



        }

        

        private void OnLoggedIn(object? sender, User user)
        {
            IsLogged = user;
        }



        [RelayCommand]
        private void OpenAdmin()
        {
            if(IsLogged.IsAdmin) {
            Hide();
            var dialogViewModel = new AdminViewModel();
            _dialogService.ShowDialog<AdminWindow>(this, dialogViewModel);
            }


        }


    }
}
