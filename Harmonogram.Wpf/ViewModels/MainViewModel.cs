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
        private Visibility _openAdminVisibility = Visibility.Collapsed; // Domyślnie ukryte


        public MainViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _loginViewModel = Ioc.Default.GetRequiredService<LoginViewModel>();
            _loginViewModel.IsLoggedIn += OnLoggedIn;
        }

        [ObservableProperty]
        private User _isLogged;
        private bool CanExecuteAdminCommands => IsLogged?.IsAdmin == true;


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
        private void Close()
        {
            Application.Current.Shutdown();
        }

        [RelayCommand]
        private void OpenWorkHoursPanel()
        {
            var dialogViewModel = new WorkHoursViewModel();
            _dialogService.ShowDialog<WorkHoursWindow>(this, dialogViewModel);
        }

        [RelayCommand]
        private void OpenSchedulePanel()
        {
            var dialogViewModel = new SchedulePanelViewModel();
            _dialogService.ShowDialog<SchedulePanelWindow>(this, dialogViewModel);
        }
        [RelayCommand(CanExecute = nameof(CanExecuteAdminCommands))]
        private void OpenCreateSchedule()
        {
            var dialogViewModel = new CreateScheduleViewModel();
            _dialogService.ShowDialog<CreateScheduleWindow>(this, dialogViewModel);
        }
        [RelayCommand(CanExecute = nameof(CanExecuteAdminCommands))]
        private void OpenCreateUser()
        {
            var dialogViewModel = new CreateUserViewModel();
            _dialogService.ShowDialog<CreateUserWindow>(this, dialogViewModel);
        }

        //Event hanlders

        private void OnLoggedIn(object? sender, User user)
        {
            IsLogged = user;
            if (IsLogged == null)
            {
                Close();
            }
            else
            {
                // Ustaw widoczność na podstawie roli
                OpenAdminVisibility = IsLogged.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
            }
        }


    }

}
