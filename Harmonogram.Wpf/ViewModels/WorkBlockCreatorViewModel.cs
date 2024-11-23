using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using MvvmDialogs;
using System.Collections.ObjectModel;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class WorkBlockCreatorViewModel : ObservableValidator, IModalDialogViewModel
    {
        private readonly IDialogService _dialogService;
        private readonly IDayService _dayService;
        private readonly IWorkBlockService _workBlockService;

        public WorkBlockCreatorViewModel()
        {
            _dialogService = Ioc.Default.GetRequiredService<IDialogService>();
            _dayService = Ioc.Default.GetRequiredService<IDayService>();
            _workBlockService = Ioc.Default.GetRequiredService<IWorkBlockService>();
        }

        #region Fields & Properties

        [ObservableProperty]
        private string _workStart = string.Empty;

        [ObservableProperty]
        private string _workEnd = string.Empty;

        [ObservableProperty]
        private DateTime _date;

        [ObservableProperty]
        private string _day;

        [ObservableProperty]
        private ObservableCollection<UserViewModel> _checkedUsers = [];

        [ObservableProperty]
        private UserViewModel _selectedUser;

        [ObservableProperty]
        private bool? _dialogResult;

        #endregion Fields & Properties

        [RelayCommand]
        private void Save()
        {
            var workBlock = new WorkBlock
            {
                StartHour = int.Parse(_workStart),
                EndHour = int.Parse(_workEnd),
                DayId = _dayService.GetDayId(Day),
                UserId = _selectedUser.Id
            };

            _workBlockService.Add(workBlock);

            DialogResult = true;
        }
    }
}