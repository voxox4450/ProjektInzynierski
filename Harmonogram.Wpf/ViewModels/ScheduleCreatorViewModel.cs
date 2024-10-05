using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class ScheduleCreatorViewModel : ObservableObject, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;

        public event EventHandler? OnRequestNextStep;

        public event EventHandler? OnRequestPrevioustStep;

        public ScheduleCreatorViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            InitializeVariables();
        }

        #region Fields & Properties

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
            }
            OnRequestNextStep?.Invoke(this, new EventArgs());
        }

        [RelayCommand]
        private void PreviousStep()
        {
            if (Step > 1)
            {
                Step--;
            }
            OnRequestPrevioustStep?.Invoke(this, new EventArgs());
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
        }

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
    }
}