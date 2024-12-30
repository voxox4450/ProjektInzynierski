using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using HandyControl.Tools.Extension;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using Harmonogram.Wpf.Views;
using Microsoft.IdentityModel.Tokens;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class ScheduleCreatorViewModel : ObservableValidator, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IUserService _userService;
        private readonly IDayService _dayService;
        private readonly IWorkBlockService _workBlockService;
        private readonly IColorService _colorService;
        private readonly IScheduleService _scheduleService;
        private readonly Constants _constants;

        public event EventHandler? OnRequestNextStep;

        public event EventHandler? OnRequestPrevioustStep;

        public ScheduleCreatorViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _userService = Ioc.Default.GetRequiredService<IUserService>();
            _dayService = Ioc.Default.GetRequiredService<IDayService>();
            _workBlockService = Ioc.Default.GetRequiredService<IWorkBlockService>();
            _scheduleService = Ioc.Default.GetRequiredService<IScheduleService>();
            _colorService = Ioc.Default.GetRequiredService<IColorService>();
            _constants = Ioc.Default.GetRequiredService<Constants>();
            _workBlockService.WorkBlockAdded += OnWorkBlockAdded!;
            _workBlockService.WorkBlockUpdated += OnWorkBlockUpdated!;
            _workBlockService.WorkBlockDeleted += OnWorkBlockDeleted!;

            InitializeVariables();
            LoadUsers();
            ValidateAllProperties();
        }

        [Required(ErrorMessage = "Data rozpoczęcia jest wymagana.")]
        public DateTime StartingDate
        {
            get => _startingDate;
            set
            {
                SetProperty(ref _startingDate, value);
                ValidateProperty(_startingDate);
                NextStepCommand.NotifyCanExecuteChanged();
            }
        }

        private DateTime _startingDate;

        [Required(ErrorMessage = "Nazwa harmonogramu jest wymagana.")]
        public string ScheduleName
        {
            get => _scheduleName;
            set
            {
                SetProperty(ref _scheduleName, value);
                ValidateProperty(_scheduleName);
                NextStepCommand.NotifyCanExecuteChanged();
            }
        }

        private string _scheduleName;

        [ObservableProperty]
        private double _windowWidth = Application.Current.MainWindow.ActualWidth;

        [ObservableProperty]
        private IEnumerable<string> _workHours;

        [ObservableProperty]
        private ObservableCollection<double> _columnWidth = [];

        [ObservableProperty]
        private ObservableCollection<DateTime> _dayDates = [];

        [ObservableProperty]
        private ObservableCollection<UserViewModel> _users = [];

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
                NextStepCommand.NotifyCanExecuteChanged();
                PreviousStepCommand.NotifyCanExecuteChanged();
            }
        }

        private int? _step;

        private bool CanNextStepBeExecuted()
        {
            return Step switch
            {
                1 => StartingDate > DateTime.Today && StartingDate.DayOfWeek.Equals(DayOfWeek.Monday),
                2 => !ScheduleName.IsNullOrEmpty(),
                3 => !Users.IsNullOrEmpty() && Users.Any(u => u.IsChecked),
                _ => false,
            };
        }

        private bool CanPreviousStepBeExecuted()
        {
            return Step > 1;
        }

        [RelayCommand(CanExecute = nameof(CanNextStepBeExecuted))]
        private void NextStep()
        {
            Step++;
            ReloadVariables();
            OnRequestNextStep?.Invoke(this, EventArgs.Empty);
        }

        [RelayCommand(CanExecute = nameof(CanPreviousStepBeExecuted))]
        private void PreviousStep()
        {
            Step--;
            ReloadVariables();
            OnRequestPrevioustStep?.Invoke(this, EventArgs.Empty);
        }

        private void ReloadVariables()
        {
            if (Step == 2)
            {
                ScheduleName = _constants.SetBaseName(StartingDate);
            }
            if (Step == 4)
            {
                for (int i = 0; i < 7; i++)
                {
                    DayDates.Add(StartingDate.AddDays(i));
                }
            }
        }

        private void LoadUsers()
        {
            var users = _userService.GetAll();
            var usersViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userViewModel = CreateUserViewModel(user);
                userViewModel.PropertyChanged += UserViewModel_PropertyChanged;
                usersViewModels.Add(userViewModel);
            }
            Users = new ObservableCollection<UserViewModel>(usersViewModels);
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
                    group[i].Margin = new Thickness(i * newWidth, 0, 0, 0);
                    group[i].LoadBlock(newWidth);
                }
            }
        }

        private static UserViewModel CreateUserViewModel(User user) => new(user);

        private static WorkBlockViewModel CreateWorkBlockViewModel(WorkBlock workBlock, User user) => new(workBlock, user);

        private void InitializeVariables()
        {
            UserControlsVisibility =
                [
                    Visibility.Visible,
                    Visibility.Hidden,
                    Visibility.Hidden,
                    Visibility.Hidden
                ];

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
            Step = 1;
            StartingDate = _constants.SetStartDate();
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

        [RelayCommand]
        private void OpenWorkBlockCreator(object parameter)
        {
            if (parameter is not string day)
            {
                return;
            }

            var dialogViewModel = new WorkBlockCreatorViewModel();
            var checkedUsers = Users.Where(user => user.IsChecked).ToList();

            dialogViewModel.CheckedUsers = new ObservableCollection<UserViewModel>(checkedUsers);
            dialogViewModel.Day = day;
            dialogViewModel.Date = DayDates[_dayService.GetDayId(day) - 1];
            dialogViewModel.SelectedUser = dialogViewModel.CheckedUsers[0];
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

            var schedule = new Schedule
            {
                Name = ScheduleName,
                StartDate = StartingDate,
                EndDate = StartingDate.AddDays(6),
                Users = Users.Where(user => user.IsChecked).Select(user => user.User).ToList(),
                WorkBlocks = WorkBlockViewModels.Values
                    .SelectMany(collection => collection)
                    .Select(viewModel => viewModel.WorkBlock)
                    .Where(wb => wb != null)
                    .ToList()
            };
            _scheduleService.Add(schedule);
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
            workBlockViewModel.User = workBlock.User;
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

        private void UserViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(UserViewModel.IsChecked))
            {
                NextStepCommand.NotifyCanExecuteChanged();
            }
        }
    }
}