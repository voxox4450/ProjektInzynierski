using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Wpf.ViewModels.ListViewModels;
using Harmonogram.Wpf.Views;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;


namespace Harmonogram.Wpf.ViewModels

{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly LoginViewModel _loginViewModel;
        private readonly IWorkBlockService _workBlockService;

        public MainViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _loginViewModel = Ioc.Default.GetRequiredService<LoginViewModel>();
            _workBlockService = Ioc.Default.GetRequiredService<IWorkBlockService>();

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
        private int _hoursDayCount = 0;

        [ObservableProperty]
        private double _moneyCount = 0.0;

        [ObservableProperty]
        private string _nextShiftDate = "Dzień Wolny";

        [ObservableProperty]
        private DateTime _startShift = new(0001, 01, 01, 01, 01, 0);

        [ObservableProperty]
        private DateTime _endShift = new(0001, 01, 01, 01, 01, 0);

        [ObservableProperty]
        private string _workHours = default!;

        [ObservableProperty]
        //TODO: z całego okresu w miesiacu
        private string _totalHours = "16,82";
        [ObservableProperty]
        //TODO: z całego okresu w miesiacu
        private string _totalAmount = "2334,56";

        public ObservableCollection<WorkBlockListViewModel> WorkBlocks { get; set; } = [];

        private void LoadWorkBlock(int userId)
        {
            var workBlocks = _workBlockService.GetByUserId(userId);
            var workBlocksVms = new List<WorkBlockListViewModel>();

            foreach (var workBlock in workBlocks)
            {
                workBlocksVms.Add(CreateWorkBlockVm(workBlock));
            }
            WorkBlocks = new ObservableCollection<WorkBlockListViewModel>(workBlocksVms);
        }

        private static WorkBlockListViewModel CreateWorkBlockVm(WorkBlock workBlock)
        {
            return new WorkBlockListViewModel(workBlock);
        }

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
        private void Close()
        {
            Application.Current.Shutdown();
        }


        private void LoadData(User user)
        {
            UserName = user.Name;
            LoadWorkBlock(user.Id);

            foreach (var workBlock in WorkBlocks)
            {
                if (workBlock.IsToday)
                {

                    NextShiftDate = workBlock.Date.ToString("D", new CultureInfo("pl-PL"))!;
                    HoursDayCount = workBlock.EndHour - workBlock.StartHour;
                    WorkHours = $"{workBlock.StartHourFormatted} - {workBlock.EndHourFormatted}";
                }
            }
            MoneyCount = user.PaymentPerHour * HoursDayCount;



            double hoursCount = (double)HoursDayCount;
            Amount = Math.Round(user.PaymentPerHour * hoursCount + user.PaymentPerHour * (hoursCount + 1) + user.PaymentPerHour * (hoursCount + 3), 2).ToString("F2");
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