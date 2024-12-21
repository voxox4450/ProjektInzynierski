using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using Harmonogram.Wpf.ViewModels.ListViewModels;
using Harmonogram.Wpf.Views;
using MvvmDialogs;
using System.Collections.ObjectModel;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class SchedulePanelViewModel : ObservableObject, IModalDialogViewModel
    {
        private readonly IWorkBlockService _workBlockService;
        private readonly IDialogService _dialogService;
        private readonly Constants _conts;

        public SchedulePanelViewModel(User? user = null)
        {
            _conts = Ioc.Default.GetRequiredService<Constants>();
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _workBlockService = Ioc.Default.GetRequiredService<IWorkBlockService>();

            User = user;
            StartOfWeek = _conts.SetStartOfWeek();
            EndOfWeek = _conts.SetEndOfWeek();

            UpdateCurrentContent(StartOfWeek, EndOfWeek);
            LoadSchedule();
            LoadData();
        }

        [ObservableProperty]
        private User _user;

        [ObservableProperty]
        private bool? _dialogResult;

        [ObservableProperty]
        private DateTime _startOfWeek;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(RightArrowCommand))]
        private DateTime _endOfWeek;

        [ObservableProperty]
        private string _salary;

        [ObservableProperty]
        private int _hourAmount;

        [ObservableProperty]
        private string _hourRange;

        [ObservableProperty]
        private string _day;

        [ObservableProperty]
        private string _currentContent;

        [ObservableProperty]
        private int _totalHoursWeekCount = 0;

        [ObservableProperty]
        private double _totalMoneyWeekCount = 0.0;

        [ObservableProperty]
        private ObservableCollection<WorkBlockListViewModel> _schedules = [];

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(OpenPropertiesCommand))]
        private WorkBlockListViewModel? _selectedSchedule;

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

                if (workBlockVm.Date >= StartOfWeek && workBlockVm.Date <= EndOfWeek)
                {
                    workBlocksVms.Add(workBlockVm);
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
            int totalHoursWeek = 0;
            double totalEarningsWeek = 0;

            foreach (var schedule in Schedules)
            {
                int hoursWorked = schedule.EndHour - schedule.StartHour;

                totalHoursWeek += hoursWorked;
                totalEarningsWeek += hoursWorked * User.PaymentPerHour;
            }

            TotalHoursWeekCount = totalHoursWeek;
            TotalMoneyWeekCount = totalEarningsWeek;
            RightArrowCommand.NotifyCanExecuteChanged();
            LeftArrowCommand.NotifyCanExecuteChanged();
        }

        private void UpdateCurrentContent(DateTime startWeek, DateTime endWeek)
        {
            CurrentContent = $"{startWeek:dd.MM.yyyy} - {endWeek:dd.MM.yyyy}";
        }

        [RelayCommand(CanExecute = nameof(CanGoToNextMonth))]
        private void RightArrow()
        {
            StartOfWeek = StartOfWeek.AddDays(7);
            EndOfWeek = EndOfWeek.AddDays(7);
            UpdateCurrentContent(StartOfWeek, EndOfWeek);
            LoadSchedule();
            LoadData();
        }

        [RelayCommand(CanExecute = nameof(CanGoToPreviousMonth))]
        private void LeftArrow()
        {
            StartOfWeek = StartOfWeek.AddDays(-7);
            EndOfWeek = EndOfWeek.AddDays(-7);
            UpdateCurrentContent(StartOfWeek, EndOfWeek);
            LoadSchedule();
            LoadData();
        }

        private bool CanGoToNextMonth()
        {
            return EndOfWeek < DateTime.Today.AddDays(7);
        }

        private bool CanGoToPreviousMonth()
        {
            var minimumAllowedDate = new DateTime(2024, 1, 1);
            return StartOfWeek.AddDays(-7) >= minimumAllowedDate;
        }

        [RelayCommand]
        private void Close()
        {
            DialogResult = true;
        }

        [RelayCommand(CanExecute = nameof(IsSelectedSchedule))]
        private void OpenProperties()
        {
            if (SelectedSchedule is null)
            {
                return;
            }
            var dialogViewModel = new PropertiesListViewModel(SelectedSchedule);

            _dialogService.ShowDialog<PropertiesListWindow>(this, dialogViewModel);
        }

        private bool IsSelectedSchedule()
        {
            return SelectedSchedule is not null;
        }
    }
}
