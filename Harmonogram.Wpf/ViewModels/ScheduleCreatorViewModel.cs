using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class ScheduleCreatorViewModel : ObservableValidator, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IUserService _userService;
        private readonly IConstants _constants;

        public event EventHandler? OnRequestNextStep;

        public event EventHandler? OnRequestPrevioustStep;

        public ScheduleCreatorViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _userService = Ioc.Default.GetRequiredService<IUserService>();
            _constants = Ioc.Default.GetRequiredService<IConstants>();
            InitializeVariables();
            LoadUsers();
        }

        #region Fields & Properties

        public DateTime StartingDate
        {
            get => _startingDate;
            set
            {
                SetProperty(ref _startingDate, value);
                ValidateProperty(_startingDate);
            }
        }

        private DateTime _startingDate;

        public DateTime EndingDate
        {
            get => _endingDate;
            set
            {
                SetProperty(ref _endingDate, value);
                ValidateProperty(_endingDate);
            }
        }

        private DateTime _endingDate;

        public string ScheduleName
        {
            get => _scheduleName;
            set
            {
                SetProperty(ref _scheduleName, value);
                ValidateProperty(_scheduleName);
            }
        }

        private string _scheduleName;

        [ObservableProperty]
        private IEnumerable<string> _workHours;

        [ObservableProperty]
        private ObservableCollection<string> _dayDates = [];

        [ObservableProperty]
        private ObservableCollection<UserViewModel> _users = [];

        public Dictionary<string, ObservableCollection<WorkBlockViewModel>> TimeBlocksByDay { get; set; }

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
                    CreateTimeBlock("Środa", 9, 15);
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

        private void InitializeVariables()
        {
            UserControlsVisibility =
                [
                    Visibility.Visible,
                    Visibility.Hidden,
                    Visibility.Hidden,
                    Visibility.Hidden
                ];

            TimeBlocksByDay = new Dictionary<string, ObservableCollection<WorkBlockViewModel>>
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

        [ObservableProperty]
        private ObservableCollection<WorkBlockViewModel> _timeBlocks = new();

        // Metoda do dodawania bloków czasowych do kolekcji
        private void CreateTimeBlock(string day, int startHour, int endHour)
        {
            double hourHeight = 25;
            double blockHeight = (endHour - startHour) * hourHeight;

            var timeBlock = new WorkBlockViewModel
            {
                Width = 100,
                Height = blockHeight,
                Fill = Brushes.Blue,
                Opacity = 0.5,
                Top = startHour * hourHeight,
                Left = 0,
                Row = startHour + 1
            };

            // Dodaj blok do wybranego dnia
            if (TimeBlocksByDay.ContainsKey(day))
            {
                TimeBlocksByDay[day].Add(timeBlock);
            }
        }
    }
}