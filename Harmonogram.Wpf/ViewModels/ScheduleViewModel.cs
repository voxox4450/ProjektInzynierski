using CommunityToolkit.Mvvm.ComponentModel;
using Harmonogram.Common.Entities;
using System.Collections.ObjectModel;

namespace Harmonogram.Wpf.ViewModels
{
    public partial class ScheduleViewModel(Schedule schedule) : ObservableObject
    {
        public int Id { get; set; } = schedule.Id;

        [ObservableProperty]
        private Schedule _schedule = schedule;

        [ObservableProperty]
        private string _name = schedule.Name;

        [ObservableProperty]
        private DateTime _startDate = schedule.StartDate;

        [ObservableProperty]
        private DateTime _endDate = schedule.EndDate;

        public DateTime? ModifiedOn { get; set; } = schedule.ModifiedOn;

        [ObservableProperty]
        private bool _isChecked = false;

        [ObservableProperty]
        private ObservableCollection<UserViewModel> _users = new ObservableCollection<UserViewModel>(schedule
            .Users
            .Select(u => new UserViewModel(u)));

        [ObservableProperty]
        private ObservableCollection<WorkBlockViewModel> _workBlocks = new ObservableCollection<WorkBlockViewModel>(schedule
            .WorkBlocks
            .Select(wb => new WorkBlockViewModel(wb, schedule
                .Users
                .First(u => u.Id == wb.UserId))));
    }
}