using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Wpf.Models;
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

        private readonly Const _conts;

        public MainViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _loginViewModel = Ioc.Default.GetRequiredService<LoginViewModel>();
            _workBlockService = Ioc.Default.GetRequiredService<IWorkBlockService>();

            _conts = Ioc.Default.GetRequiredService<Const>();

            _loginViewModel.IsLoggedIn += OnLoggedIn;

            startOfWeek = _conts.SetStartOfWeek();
            endOfWeek = _conts.SetEndOfWeek();
            startOfMonth = _conts.SetStartOfMonth();
            endOfMonth = _conts.SetEndOfMonth();
            InitializeVariables();
        }

        private bool CanExecuteAdminCommands => IsLogged?.IsAdmin == true;

        [ObservableProperty]
        private User _isLogged;

        [ObservableProperty]
        private Visibility _openAdminVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private string _userName = string.Empty!;
        [ObservableProperty]
        private string _totalWeekAmount = string.Empty!;

        [ObservableProperty]
        private int _hoursDayCount = 0;

        [ObservableProperty]
        private double _moneyDayCount = 0.0;

        [ObservableProperty]
        private string statusOfWork = "Dzień Wolny";

        [ObservableProperty]
        private DateTime _startShift = new(0001, 01, 01, 01, 01, 0);

        [ObservableProperty]
        private DateTime _endShift = new(0001, 01, 01, 01, 01, 0);

        [ObservableProperty]
        private string _workDayHours = default!;

        [ObservableProperty]
        private string _totalMonthHours = "00,00";
        [ObservableProperty]
        private string _totalMonthAmount = "00,00";

        private DateTime startOfWeek { get; set; }
        private DateTime endOfWeek { get; set; }
        private DateTime startOfMonth { get; set; }
        private DateTime endOfMonth { get; set; }
        public ObservableCollection<WorkBlockListViewModel> WorkBlocks { get; set; } = [];

        [ObservableProperty]
        private ObservableCollection<bool> _toWork = [];

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
            if (IsLogged is null)
            {
                return;
            }
            var dialogViewModel = new WorkTimeViewModel(IsLogged);
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

            double totalHoursWeek = 0;
            double totalHoursMonth = 0;
            double totalEarningsMonth = 0;
            foreach (var workBlock in WorkBlocks)
            {
                if (workBlock.IsToday)
                {
                    StatusOfWork = workBlock.Date.ToString("D", new CultureInfo("pl-PL"))!;
                    HoursDayCount = workBlock.EndHour - workBlock.StartHour;
                    WorkDayHours = $"{workBlock.StartHourFormatted} - {workBlock.EndHourFormatted}";
                }


                if (workBlock.Date >= startOfWeek && workBlock.Date <= endOfWeek)
                {
                    int dayIndex = (int)workBlock.Date.DayOfWeek;

                    dayIndex = (dayIndex == 0) ? 6 : dayIndex - 1;

                    if (dayIndex >= 0 && dayIndex < ToWork.Count)
                    {
                        ToWork[dayIndex] = true;
                    }

                    totalHoursWeek += workBlock.EndHour - workBlock.StartHour;
                }

                if (workBlock.Date >= startOfMonth && workBlock.Date <= DateTime.Today && workBlock.Date <= endOfMonth)
                {
                    double hoursWorked = workBlock.EndHour - workBlock.StartHour;


                    if (workBlock.Date.Date == DateTime.Today)
                    {
                        int currentHour = DateTime.Now.Hour;
                        if (workBlock.EndHour > currentHour)
                        {
                            hoursWorked = Math.Max(0, currentHour - workBlock.StartHour);
                        }
                    }

                    totalHoursMonth += hoursWorked;
                    totalEarningsMonth += hoursWorked * user.PaymentPerHour;
                }
            }
            MoneyDayCount = user.PaymentPerHour * HoursDayCount;
            var totalEarningsWeeks = totalHoursWeek * user.PaymentPerHour;
            TotalWeekAmount = Math.Round(totalEarningsWeeks, 2).ToString("F2");


            TotalMonthHours = totalHoursMonth.ToString("F2");
            TotalMonthAmount = totalEarningsMonth.ToString("F2");
        }

        private void InitializeVariables()
        {
            ToWork = new ObservableCollection<bool>
            {
                false,
                false,
                false,
                false,
                false,
                false,
                false,
            };
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