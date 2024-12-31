using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Microsoft.IdentityModel.Tokens;
using MvvmDialogs;
using System.Collections.ObjectModel;
using System.Windows;
using HC = HandyControl.Controls;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class WorkBlockEditorViewModel : ObservableValidator, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IWorkBlockService _workBlockService;
        private readonly IColorService _colorService;

        public event EventHandler? OnRequestNextStep;

        public event EventHandler? OnRequestPrevioustStep;

        public WorkBlockEditorViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _workBlockService = Ioc.Default.GetRequiredService<IWorkBlockService>();
            _colorService = Ioc.Default.GetRequiredService<IColorService>();
            _colors = new ObservableCollection<Color>(_colorService.GetAll());

            _workBlockService.WorkBlockDeleted += OnWorkBlockDeleted!;
            InitializeVariables();
            ValidateAllProperties();
        }

        #region Fields & Properties

        [ObservableProperty]
        private DateTime _date;

        [ObservableProperty]
        private string _day = string.Empty;

        [ObservableProperty]
        private ObservableCollection<Color> _colors;

        [ObservableProperty]
        private Color _selectedColor;

        [ObservableProperty]
        private ObservableCollection<UserViewModel> _checkedUsers = [];

        [ObservableProperty]
        private UserViewModel _selectedUser;

        [ObservableProperty]
        private ObservableCollection<WorkBlockViewModel> _workBlocks = [];

        public WorkBlockViewModel SelectedWorkBlock
        {
            get => _selectedWorkBlock;
            set
            {
                SetProperty(ref _selectedWorkBlock, value);
                ValidateProperty(_selectedWorkBlock);
                NextStepCommand.NotifyCanExecuteChanged();
                PreviousStepCommand.NotifyCanExecuteChanged();
            }
        }

        private WorkBlockViewModel _selectedWorkBlock;

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

        #endregion Fields & Properties

        private bool CanNextStepBeExecuted()
        {
            return Step switch
            {
                1 => !WorkBlocks.IsNullOrEmpty() && SelectedWorkBlock != null,
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
                    Visibility.Hidden
                ];
            Step = 1;
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
            if (Step == 2)
            {
                SelectedUser = CheckedUsers.FirstOrDefault(cu => cu.Id == SelectedWorkBlock.User.Id)!;
                SelectedColor = _colorService.GetById(SelectedWorkBlock.ColorId);
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

        [RelayCommand]
        private void Save()
        {
            if (!SelectedWorkBlock.Name.IsNullOrEmpty() &&
                SelectedWorkBlock.StartHour >= 0 &&
                SelectedWorkBlock.StartHour <= 24 &&
                SelectedWorkBlock.EndHour >= 0 &&
                SelectedWorkBlock.EndHour <= 24 &&
                SelectedWorkBlock.EndHour > SelectedWorkBlock.StartHour)
            {
                SelectedWorkBlock.WorkBlock.ColorId = SelectedColor.Id;
                SelectedWorkBlock.WorkBlock.StartHour = SelectedWorkBlock.StartHour;
                SelectedWorkBlock.WorkBlock.EndHour = SelectedWorkBlock.EndHour;
                SelectedWorkBlock.WorkBlock.Date = Date;
                SelectedWorkBlock.WorkBlock.UserId = SelectedUser.Id;
                SelectedWorkBlock.WorkBlock.User = SelectedUser.User;

                _workBlockService.Update(SelectedWorkBlock.WorkBlock);

                DialogResult = true;
            }
            else
            {
                HC.MessageBox.Show("Wszystkie pola muszą być uzupełnione!\n" +
                    "Godziny rozpoczęcia i zakończneia muszą mieścić się w zakresie <0,23> (int).\n" +
                    "Godzina rozpoczęcia nie może być większa od godziny zakończenia.",
                    "Ostrzeżenie",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Delete()
        {
            _workBlockService.Delete(SelectedWorkBlock.WorkBlock);
            DialogResult = true;
        }

        private void OnWorkBlockDeleted(object sender, WorkBlock workBlock)
        {
            WorkBlocks.Remove(WorkBlocks.FirstOrDefault(wb => wb.WorkBlock.Id == workBlock.Id));
        }
    }
}