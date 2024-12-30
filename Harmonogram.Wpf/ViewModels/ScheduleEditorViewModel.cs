using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using Harmonogram.Wpf.Views;
using Microsoft.IdentityModel.Tokens;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.Windows;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class ScheduleEditorViewModel : ObservableValidator, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IScheduleService _scheduleService;
        private readonly IUserService _userService;
        private readonly IWorkBlockService _workBlockService;
        private readonly IColorService _colorService;
        private readonly Constants _constants;
        private readonly IDayService _dayService;

        public event EventHandler? OnRequestNextStep;

        public event EventHandler? OnRequestPrevioustStep;

        public ScheduleEditorViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _scheduleService = Ioc.Default.GetRequiredService<IScheduleService>();
            _userService = Ioc.Default.GetRequiredService<IUserService>();
            _workBlockService = Ioc.Default.GetRequiredService<IWorkBlockService>();
            _colorService = Ioc.Default.GetRequiredService<IColorService>();
            _constants = Ioc.Default.GetRequiredService<Constants>();
            _dayService = Ioc.Default.GetRequiredService<IDayService>();

            _workBlockService.WorkBlockAdded += OnWorkBlockAdded!;
            _workBlockService.WorkBlockUpdated += OnWorkBlockUpdated!;
            _workBlockService.WorkBlockDeleted += OnWorkBlockDeleted!;

            InitializeVariables();
            LoadSchedules();
            LoadUsers();
            ValidateAllProperties();
        }

        #region Fields & Properties

        [ObservableProperty]
        private ObservableCollection<ScheduleViewModel> _schedules = [];

        public ScheduleViewModel SelectedSchedule
        {
            get => _selectedSchedule;
            set
            {
                SetProperty(ref _selectedSchedule, value);
                ValidateProperty(_selectedSchedule);
                NextStepCommand.NotifyCanExecuteChanged();
                LoadWorkBlocks();
            }
        }

        private ScheduleViewModel _selectedSchedule;

        [ObservableProperty]
        private double _windowWidth = Application.Current.MainWindow.ActualWidth;

        [ObservableProperty]
        private ObservableCollection<double> _columnWidth = [];

        [ObservableProperty]
        private ObservableCollection<UserViewModel> _users = [];

        [ObservableProperty]
        private IEnumerable<string> _workHours;

        [ObservableProperty]
        private ObservableCollection<DateTime> _dayDates = [];

        [ObservableProperty]
        private Dictionary<int, ObservableCollection<WorkBlockViewModel>> _workBlockViewModels = [];

        [ObservableProperty]
        private bool? _dialogResult;

        [ObservableProperty]
        private ObservableCollection<Visibility> _userControlsVisibility;

        public int? Step
        {
            get => _step;
            set
            {
                SetProperty(ref _step, value);
                ToggleVisibility();
            }
        }

        private int? _step;

        #endregion Fields & Properties

        private bool CanNextStepBeExecuted()
        {
            return Step switch
            {
                1 => !Schedules.IsNullOrEmpty() && SelectedSchedule != null,
                2 => !SelectedSchedule!.Name.IsNullOrEmpty() && SelectedSchedule.StartDate > DateTime.Today && SelectedSchedule.StartDate.DayOfWeek.Equals(DayOfWeek.Monday),
                3 => !Users.IsNullOrEmpty() && Users.Any(u => u.IsChecked),
                _ => false,
            };
        }

        private bool CanPreviousStepBeExecuted()
        {
            return Step > 1;
        }

        private void InitializeVariables()
        {
            UserControlsVisibility =
                [
                    Visibility.Visible,
                    Visibility.Hidden,
                    Visibility.Hidden,
                    Visibility.Hidden
                ];
            Step = 1;
            //StartingDate = _constants.SetStartDate();
            //EndingDate = _constants.SetEndDate();
            WorkHours = _constants.SetHours();
            ColumnWidth = new ObservableCollection<double>
            {
                50,
                (WindowWidth - 50) / 7,
                (WindowWidth - 50) / 7,
                (WindowWidth - 50) / 7,
                (WindowWidth - 50) / 7,
                (WindowWidth - 50) / 7,
                (WindowWidth - 50) / 7,
                (WindowWidth - 50) / 7
            };
        }

        [RelayCommand(CanExecute = nameof(CanNextStepBeExecuted))]
        private void NextStep()
        {
            Step++;
            ReloadVariables();
            OnRequestNextStep?.Invoke(this, new EventArgs());
        }

        [RelayCommand(CanExecute = nameof(CanPreviousStepBeExecuted))]
        private void PreviousStep()
        {
            Step--;
            ReloadVariables();
            OnRequestPrevioustStep?.Invoke(this, new EventArgs());
        }

        private void ReloadVariables()
        {
            if (Step == 3)
            {
                foreach (var user in Users)
                {
                    user.IsChecked = SelectedSchedule.Users.Any(u => u.Id == user.Id);
                }
            }
            if (Step == 4)
            {
                for (int i = 0; i < 7; i++)
                {
                    DayDates.Add(SelectedSchedule.StartDate.AddDays(i));
                }
            }
        }

        [RelayCommand]
        private void ToggleVisibility()
        {
            if (!Step.HasValue)
            {
                return;
            }

            for (int i = 0; i < UserControlsVisibility.Count; i++)
            {
                UserControlsVisibility[i] = Visibility.Hidden;
            }
            UserControlsVisibility[Step.Value - 1] = Visibility.Visible;
        }

        private void LoadSchedules()
        {
            var schedules = _scheduleService.GetAllEditable();
            var schedulesViewModels = new List<ScheduleViewModel>();
            foreach (var schedule in schedules)
            {
                schedulesViewModels.Add(CreateScheduleViewModel(schedule));
            }
            Schedules = new ObservableCollection<ScheduleViewModel>(schedulesViewModels);
        }

        private static ScheduleViewModel CreateScheduleViewModel(Schedule schedule) => new ScheduleViewModel(schedule);

        private void LoadUsers()
        {
            var users = _userService.GetAll();
            var usersViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                usersViewModels.Add(CreateUserViewModel(user));
            }
            Users = new ObservableCollection<UserViewModel>(usersViewModels);
        }

        private void LoadWorkBlocks()
        {
            WorkBlockViewModels = new Dictionary<int, ObservableCollection<WorkBlockViewModel>>
            {
                { _dayService.GetDayId("Poniedziałek"), new ObservableCollection<WorkBlockViewModel>() },
                { _dayService.GetDayId("Wtorek"), new ObservableCollection<WorkBlockViewModel>() },
                { _dayService.GetDayId("Środa"), new ObservableCollection<WorkBlockViewModel>() },
                { _dayService.GetDayId("Czwartek"), new ObservableCollection<WorkBlockViewModel>() },
                { _dayService.GetDayId("Piątek"), new ObservableCollection<WorkBlockViewModel>() },
                { _dayService.GetDayId("Sobota"), new ObservableCollection<WorkBlockViewModel>() },
                { _dayService.GetDayId("Niedziela"), new ObservableCollection<WorkBlockViewModel>() },
            };
            foreach (var workBlock in SelectedSchedule.WorkBlocks)
            {
                var workBlocksCountByDay = WorkBlockViewModels[workBlock.DayId].Count;
                double newWidth = ColumnWidth[workBlock.DayId] / (workBlocksCountByDay + 1);
                var iterator = 0;
                foreach (var wbvm in WorkBlockViewModels[workBlock.DayId])
                {
                    iterator++;
                    wbvm.Width = newWidth;
                    wbvm.Margin = new Thickness(iterator * newWidth, 0, 0, 0);
                }
                workBlock.LoadBlock(newWidth);
                WorkBlockViewModels[workBlock.DayId].Add(workBlock);
            }
        }

        private void ReloadWorkBlocks(int dayId)
        {
            var overlappingGroups = new ObservableCollection<ObservableCollection<WorkBlockViewModel>>();

            foreach (var wbvm in WorkBlockViewModels[dayId])
            {
                var overlappingGroup = overlappingGroups.FirstOrDefault(group => group.Any(
                    g => !(g.EndHour <= wbvm.StartHour || wbvm.EndHour <= g.StartHour)
                ));

                if (overlappingGroup != null)
                {
                    overlappingGroup.Add(wbvm);
                }
                else
                {
                    overlappingGroups.Add(new ObservableCollection<WorkBlockViewModel> { wbvm });
                }
            }
            foreach (var group in overlappingGroups)
            {
                double newWidth = ColumnWidth[dayId] / group.Count;
                for (int i = 0; i < group.Count; i++)
                {
                    group[i].LoadBlock(newWidth);
                    group[i].Margin = new Thickness(i * newWidth, 0, 0, 0);
                }
            }
        }

        private static UserViewModel CreateUserViewModel(User user) => new UserViewModel(user);

        private static WorkBlockViewModel CreateWorkBlockViewModel(WorkBlock workBlock, User user) => new(workBlock, user);

        [RelayCommand]
        private void OpenWorkBlockCreator(object parameter)
        {
            string? day = parameter as string;
            if (day == null)
            {
                return;
            }

            var dialogViewModel = new WorkBlockCreatorViewModel();
            var checkedUsers = Users.Where(user => user.IsChecked).ToList();
            dialogViewModel.CheckedUsers = new ObservableCollection<UserViewModel>(checkedUsers);
            dialogViewModel.SelectedUser = dialogViewModel.CheckedUsers[0];
            dialogViewModel.Day = day;
            dialogViewModel.Date = DayDates[_dayService.GetDayId(day) - 1];
            _dialogService.ShowDialog<WorkBlockCreatorWindow>(this, dialogViewModel);
        }

        [RelayCommand]
        private void OpenWorkBlockEditor(object parameter)
        {
            if (parameter is not string day)
            {
                return;
            }
            var dialogViewModel = new WorkBlockEditorViewModel();
            var checkedUsers = Users.Where(user => user.IsChecked).ToList();
            var workBlocks = WorkBlockViewModels[_dayService.GetDayId(day)].ToList();

            dialogViewModel.CheckedUsers = new ObservableCollection<UserViewModel>(checkedUsers);
            dialogViewModel.Day = day;
            dialogViewModel.Date = DayDates[_dayService.GetDayId(day) - 1];
            dialogViewModel.WorkBlocks = new ObservableCollection<WorkBlockViewModel>(workBlocks);
            _dialogService.ShowDialog<WorkBlockEditorWindow>(this, dialogViewModel);
        }

        [RelayCommand]
        private void Save()
        {
            if (!WorkBlockViewModels.Values.Any(v => v.Any()))
            {
                HC.MessageBox.Show("Harmonogram musi zawierać conajmniej jeden blok pracy!", "Ostrzeżenie", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SelectedSchedule.Schedule.EndDate = SelectedSchedule.StartDate.AddDays(6);

            SelectedSchedule.Schedule.Users = Users
                .Where(u => u.IsChecked)
                .Select(cu => cu.User)
                .ToList();

            SelectedSchedule.Schedule.WorkBlocks = WorkBlockViewModels.Values
                .SelectMany(collection => collection)
                .Select(viewModel => viewModel.WorkBlock)
                .Where(wb => wb != null)
                .ToList();

            SelectedSchedule.Schedule.ModifiedOn = DateTime.Now;

            _scheduleService.Update(SelectedSchedule.Schedule);

            DialogResult = true;
        }

        [RelayCommand]
        private void Exit()
        {
            var result = HC.MessageBox.Show(
            "Czy na pewno chcesz wyjść?\nWszystkie Twoje zmiany zostaną utracone.",
            "Potwierdzenie wyjścia",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                DialogResult = true;
            }
        }

        private void OnWorkBlockAdded(object sender, WorkBlock workBlock)
        {
            var workBlockViewModel = CreateWorkBlockViewModel(workBlock, Users.First(user => user.Id == workBlock.UserId).User);

            WorkBlockViewModels[workBlock.DayId].Add(workBlockViewModel);
            ReloadWorkBlocks(workBlock.DayId);
            workBlockViewModel.LoadBlock(workBlockViewModel.Width);
        }

        private void OnWorkBlockUpdated(object sender, WorkBlock workBlock)
        {
            var workBlockViewModel = WorkBlockViewModels[workBlock.DayId].First(wbvm => wbvm.WorkBlock.Id == workBlock.Id);
            ReloadWorkBlocks(workBlock.DayId);
            workBlockViewModel.Update(workBlock);
            workBlockViewModel.LoadBlock(workBlockViewModel.Width);
        }

        private void OnWorkBlockDeleted(object sender, WorkBlock workBlock)
        {
            var workBlockViewModel = WorkBlockViewModels[workBlock.DayId].First(wbvm => wbvm.WorkBlock.Id == workBlock.Id);
            WorkBlockViewModels[workBlock.DayId].Remove(workBlockViewModel);
            ReloadWorkBlocks(workBlock.DayId);
        }
    }
}