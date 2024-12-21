using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using Harmonogram.Wpf.ViewModels.ListViewModels;
using MvvmDialogs;
using System.Collections.ObjectModel;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class WorkTimeViewModel : ObservableValidator, IModalDialogViewModel
    {
        private readonly IWorkBlockService _workBlockService;

        private readonly Constants _conts;
        public WorkTimeViewModel(User? user = null)
        {
            _conts = Ioc.Default.GetRequiredService<Constants>();
            _workBlockService = Ioc.Default.GetRequiredService<IWorkBlockService>();
            User = user;
            StartOfMonth = _conts.SetStartOfMonth();
            EndOfMonth = _conts.SetEndOfMonth();
            UpdateCurrentContent(StartOfMonth, EndOfMonth);
            if (user is null) { return; }
            LoadSchedule();
            LoadData();
        }


        [ObservableProperty]
        private User _user;

        [ObservableProperty]
        private bool? _dialogResult;

        [ObservableProperty]
        private string _currentContent;

        [ObservableProperty]
        private int _totalHoursMonthCount = 0;

        [ObservableProperty]
        private double _totalMoneyMonthCount = 0.0;

        [ObservableProperty]
        private ObservableCollection<WorkBlockListViewModel> _schedules = [];

        [ObservableProperty]
        private string _salary;

        [ObservableProperty]
        private int _hourAmount;

        [ObservableProperty]
        private string _hourRange;

        [ObservableProperty]
        private string _day;


        [ObservableProperty]
        private DateTime _startOfMonth;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RightArrowCommand))]
        private DateTime _endOfMonth;


        private void LoadSchedule()
        {
            if (User is null)
            {
                return;
            }
            var workBlocks = _workBlockService.GetByUserId(User.Id);
            var workBlocksVms = new List<WorkBlockListViewModel>();

            foreach (var workBlock in workBlocks)
            {
                var workBlockVm = CreateWorkBlockVm(workBlock);

                if (workBlockVm.Date >= StartOfMonth && workBlockVm.Date <= EndOfMonth)
                {
                    var workEndDate = workBlockVm.Date.AddHours(workBlockVm.EndHour);
                    if (workEndDate <= DateTime.Now)
                    {
                        workBlocksVms.Add(workBlockVm);
                    }
                }
            }
            workBlocksVms.Sort((x, y) => x.Date.CompareTo(y.Date));
            Schedules = new ObservableCollection<WorkBlockListViewModel>(workBlocksVms);
        }

        private static WorkBlockListViewModel CreateWorkBlockVm(WorkBlock workBlock)
        {
            return new WorkBlockListViewModel(workBlock);
        }

        private void LoadData()
        {
            int totalHoursMonth = 0;
            double totalEarningsMonth = 0;

            foreach (var schedule in Schedules)
            {

                if (schedule.Date >= StartOfMonth && schedule.Date <= DateTime.Today && schedule.Date <= EndOfMonth)
                {
                    int hoursWorked = schedule.EndHour - schedule.StartHour;


                    if (schedule.Date.Date == DateTime.Today)
                    {
                        int currentHour = DateTime.Now.Hour;
                        if (schedule.EndHour > currentHour)
                        {
                            hoursWorked = Math.Max(0, currentHour - schedule.StartHour);
                        }
                    }
                    totalHoursMonth += hoursWorked;
                    totalEarningsMonth += hoursWorked * User.PaymentPerHour;
                }
            }
            TotalHoursMonthCount = totalHoursMonth;
            TotalMoneyMonthCount = totalEarningsMonth;
            RightArrowCommand.NotifyCanExecuteChanged();
            LeftArrowCommand.NotifyCanExecuteChanged();
        }


        [RelayCommand]
        private void Close()
        {
            DialogResult = true;
        }

        [RelayCommand(CanExecute = nameof(CanGoToNextMonth))]
        private void RightArrow()
        {
            StartOfMonth = StartOfMonth.AddMonths(1);
            EndOfMonth = EndOfMonth.AddMonths(1);
            EndOfMonth = new DateTime(StartOfMonth.Year, StartOfMonth.Month, 1)
                .AddMonths(1)
                .AddDays(-1);
            UpdateCurrentContent(StartOfMonth, EndOfMonth);
            LoadSchedule();
            LoadData();
        }
        private bool CanGoToNextMonth()
        {

            return EndOfMonth.DayOfYear < DateTime.Today.DayOfYear
                && StartOfMonth.AddMonths(1).Year <= DateTime.Today.Year;
        }

        private bool CanGoToPreviousMonth()
        {
            var currentDate = DateTime.Today;
            if (StartOfMonth.AddMonths(-1).Year < 2024)
                return false;
            return true;

        }

        [RelayCommand(CanExecute = nameof(CanGoToPreviousMonth))]
        private void LeftArrow()
        {
            StartOfMonth = StartOfMonth.AddMonths(-1);
            EndOfMonth = EndOfMonth.AddMonths(-1);
            EndOfMonth = new DateTime(StartOfMonth.Year, StartOfMonth.Month, 1)
                .AddMonths(1)
                .AddDays(-1);
            UpdateCurrentContent(StartOfMonth, EndOfMonth);
            LoadSchedule();
            LoadData();
        }

        private void UpdateCurrentContent(DateTime startMonth, DateTime endMonth)
        {
            CurrentContent = $"{startMonth:dd.MM.yyyy} - {endMonth:dd.MM.yyyy}";
        }

    }
}
