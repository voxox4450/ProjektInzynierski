using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Tools.Extension;
using Harmonogram.Common.Entities;
using Harmonogram.Wpf.Views;
using MvvmDialogs;
using System.Globalization;
using System.Windows;

namespace Harmonogram.Wpf.ViewModels

{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly LoginViewModel _loginViewModel;

        public MainViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _loginViewModel = Ioc.Default.GetRequiredService<LoginViewModel>();

            _loginViewModel.IsLoggedIn += OnLoggedIn;
        }

        private bool CanExecuteAdminCommands => IsLogged?.IsAdmin == true;

        [ObservableProperty]
        private User _isLogged;

        [ObservableProperty]
        private Visibility _openAdminVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private string _userName = string.Empty!;

        [ObservableProperty]
        private string _amount = string.Empty!;

        [ObservableProperty]
        private int _hoursCount = 0;

        [ObservableProperty]
        private double _moneyCount = 0.0;

        [ObservableProperty]
        private string _nextShiftDate = DateTime.Today.ToString("D", new CultureInfo("pl-PL"))!;

        [ObservableProperty]
        private DateTime _startShift = new(2024, 11, 15, 10, 15, 0);

        [ObservableProperty]
        private DateTime _endShift = new(2024, 11, 15, 17, 45, 0);

        [ObservableProperty]
        private string _workHours = default!;

        [ObservableProperty]
        private string _totalHours = "16,82";

        [ObservableProperty]
        private string _totalAmount = "2334,56";

        [RelayCommand]
        private static void Hide()
        {
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

        [RelayCommand]
        private void OpenWorkHoursPanel()
        {
            var dialogViewModel = new WorkTimeViewModel();
            _dialogService.ShowDialog<WorkTimeWindow>(this, dialogViewModel);
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
            var dialogViewModel = new ScheduleCreatorViewModel();
            _dialogService.ShowDialog<ScheduleCreatorWindow>(this, dialogViewModel);
        }

        [RelayCommand(CanExecute = nameof(CanExecuteAdminCommands))]
        private void OpenCreateUser()
        {
            var dialogViewModel = new CreateUserViewModel();
            _dialogService.ShowDialog<CreateUserWindow>(this, dialogViewModel);
        }

        [RelayCommand(CanExecute = nameof(CanExecuteAdminCommands))]
        private void OpenListUsers()
        {
            var dialogViewModel = new UsersListViewModel();
            _dialogService.ShowDialog<UsersListWindow>(this, dialogViewModel);
        }

        //New
        [RelayCommand(CanExecute = nameof(CanExecuteAdminCommands))]
        private void OpenScheduleEditor()
        {
            var dialogViewModel = new ScheduleEditorViewModel();
            _dialogService.ShowDialog<ScheduleEditorWindow>(this, dialogViewModel);
        }

        private static void Close() => Application.Current.Shutdown();

        private void LoadData(User user)
        {
            UserName = user.Name;
            WorkHours = $"{StartShift:HH:mm} - {EndShift:HH:mm}";
            HoursCount = 5;
            MoneyCount = user.PaymentPerHour * HoursCount;
            double hoursCount = (double)HoursCount;
            Amount = Math.Round((user.PaymentPerHour * hoursCount) + (user.PaymentPerHour * (hoursCount + 1)) + (user.PaymentPerHour * (hoursCount + 3)), 2).ToString("F2");
        }

        private void OnLoggedIn(object? sender, User user)
        {
            IsLogged = user;
            if (IsLogged == null)
            {
                Close();
            }
            else
            {
                LoadData(user);

                OpenAdminVisibility = IsLogged.IsAdmin ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}