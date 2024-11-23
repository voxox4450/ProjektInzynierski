using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Wpf.Views;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.Windows;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class ScheduleCreatorViewModel : ObservableValidator, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IUserService _userService;
        private readonly IDayService _dayService;
        private readonly IWorkBlockService _workBlockService;

        private readonly IScheduleService _scheduleService;
        private readonly IConstants _constants;

        public event EventHandler? OnRequestNextStep;

        public event EventHandler? OnRequestPrevioustStep;
        public ScheduleCreatorViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _userService = Ioc.Default.GetRequiredService<IUserService>();
            _dayService = Ioc.Default.GetRequiredService<IDayService>();
            _workBlockService = Ioc.Default.GetRequiredService<IWorkBlockService>();
            _scheduleService = Ioc.Default.GetRequiredService<IScheduleService>();
            _constants = Ioc.Default.GetRequiredService<IConstants>();
            _workBlockService.WorkBlockAdded += OnWorkBlockAdded!;
            InitializeVariables();
            LoadUsers();
        }
        //public DateTime StartingDate
        //{
        //    get => _startingDate;
        //    set
        //    {
        //        SetProperty(ref _startingDate, value);
        //        ValidateProperty(_startingDate);
        //    }
        //}
        [ObservableProperty]
        //[NotifyDataErrorInfo]
        private DateTime _startingDate;

        //private DateTime _startingDate;

        //public DateTime EndingDate
        //{
        //    get => _endingDate;
        //    set
        //    {
        //        SetProperty(ref _endingDate, value);
        //        ValidateProperty(_endingDate);
        //    }
        //}
        [ObservableProperty]
        //[NotifyDataErrorInfo]
        private DateTime _endingDate;

        //public string ScheduleName
        //{
        //    get => _scheduleName;
        //    set
        //    {
        //        SetProperty(ref _scheduleName, value);
        //        ValidateProperty(_scheduleName);
        //    }
        //}
        [ObservableProperty]
        //[NotifyDataErrorInfo]
        private string _scheduleName;

        [ObservableProperty]
        private IEnumerable<string> _workHours;

        [ObservableProperty]
        private ObservableCollection<string> _dayDates = [];

        [ObservableProperty]
        private ObservableCollection<UserViewModel> _users = [];

        [ObservableProperty]
        private Dictionary<string, ObservableCollection<WorkBlockViewModel>> _workBlockViewModels = [];

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
        [RelayCommand]
        private void NextStep()
        {
            if (Step < 4)
            {
                Step++;
                ReloadVariables();
            }
            OnRequestNextStep?.Invoke(this, new EventArgs());
        }

        [RelayCommand]
        private void PreviousStep()
        {
            if (Step > 1)
            {
                Step--;
                ReloadVariables();
            }
            OnRequestPrevioustStep?.Invoke(this, new EventArgs());
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
                    DayDates.Add(StartingDate.AddDays(i).ToString("dd.MM"));
                }
            }
        }

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

        private static UserViewModel CreateUserViewModel(User user) => new UserViewModel(user);

        private static WorkBlockViewModel CreateWorkBlockViewModel(WorkBlock workBlock, User user) => new WorkBlockViewModel(workBlock, user);

        private void InitializeVariables()
        {
            UserControlsVisibility =
                [
                    Visibility.Visible,
                    Visibility.Hidden,
                    Visibility.Hidden,
                    Visibility.Hidden
                ];

            _workBlockViewModels = new Dictionary<string, ObservableCollection<WorkBlockViewModel>>
        {
            { "Poniedziałek", new ObservableCollection<WorkBlockViewModel>() },
            { "Wtorek", new ObservableCollection<WorkBlockViewModel>() },
            { "Środa", new ObservableCollection<WorkBlockViewModel>() },
            { "Czwartek", new ObservableCollection<WorkBlockViewModel>() },
            { "Piątek", new ObservableCollection<WorkBlockViewModel>() },
            { "Sobota", new ObservableCollection<WorkBlockViewModel>() },
            { "Niedziela", new ObservableCollection<WorkBlockViewModel>() },
        };
            Step = 1;
            StartingDate = _constants.SetStartDate();
            EndingDate = _constants.SetEndDate();
            WorkHours = _constants.SetHours();
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
            string? day = parameter as string;
            if (day == null)
            {
                return;
            }

            var dialogViewModel = new WorkBlockCreatorViewModel();
            var checkedUsers = Users.Where(user => user.IsChecked).ToList();

            dialogViewModel.CheckedUsers = new ObservableCollection<UserViewModel>(checkedUsers);
            dialogViewModel.Day = day;
            dialogViewModel.Date = StartingDate.AddDays(_dayService.GetDayId(day) - 1);
            _dialogService.ShowDialog<WorkBlockCreatorWindow>(this, dialogViewModel);
        }

        [RelayCommand]
        private void Save()
        {
            var schedule = new Schedule
            {
                Name = ScheduleName,
                StartDate = StartingDate,
                EndDate = EndingDate,
                Users = Users.Where(user => user.IsChecked).Select(user => user.User).ToList(),
                //WorkBlocks = WorkBlocks.ToList()
            };

            _scheduleService.Add(schedule);
            DialogResult = true;
        }

        private void OnWorkBlockAdded(object sender, WorkBlock workBlock)
        {
            var workBlockViewModel = CreateWorkBlockViewModel(workBlock, Users.First(user => user.Id == workBlock.UserId).User);
            workBlockViewModel.LoadBlock();
            workBlockViewModel.SetName();
            WorkBlockViewModels[workBlock.Day.Name].Add(workBlockViewModel);
        }

    }
}
