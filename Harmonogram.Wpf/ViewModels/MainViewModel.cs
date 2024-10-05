using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Wpf.Views;
using MvvmDialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class MainViewModel(IDialogService dialogService) : ObservableObject
    {
        private readonly IDialogService _dialogService = dialogService;

        [RelayCommand]
        private void GoToSchedule()
        {
            var dialogViewModel = new ScheduleViewModel();
            _dialogService.ShowDialog<ScheduleWindow>(this, dialogViewModel);
        }

        [RelayCommand]
        private void GoToScheduleCreator()
        {
            var dialogViewModel = new ScheduleCreatorViewModel();
            _dialogService.ShowDialog<ScheduleCreatorWindow>(this, dialogViewModel);
        }

        [RelayCommand]
        private void GoToWorkTime()
        {
            var dialogViewModel = new WorkTimeViewModel();
            _dialogService.ShowDialog<WorkTimeWindow>(this, dialogViewModel);
        }
    }
}
