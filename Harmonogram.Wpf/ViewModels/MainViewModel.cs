using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Wpf.Views;
using MvvmDialogs;

namespace Harmonogram.Wpf.ViewModels

{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        public MainViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
        }

        [RelayCommand]
        private void OpenWorkHoursPanel()
        {
            var dialogViewModel = new WorkHoursViewModel();
            _dialogService.ShowDialog<WorkHoursWindow>(this, dialogViewModel);
        }

        [RelayCommand]
        private void OpenSchedulePanel()
        {
            var dialogViewModel = new SchedulePanelViewModel();
            _dialogService.ShowDialog<SchedulePanelWindow>(this, dialogViewModel);
        }
        [RelayCommand]
        private void OpenCreateSchedule()
        {
            var dialogViewModel = new CreateScheduleViewModel();
            _dialogService.ShowDialog<CreateScheduleWindow>(this, dialogViewModel);
        }
        [RelayCommand]
        private void OpenCreateUser()
        {
            var dialogViewModel = new CreateUserViewModel();
            _dialogService.ShowDialog<CreateUserWindow>(this, dialogViewModel);
        }

    }

}
