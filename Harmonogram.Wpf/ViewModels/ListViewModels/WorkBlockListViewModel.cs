using CommunityToolkit.Mvvm.ComponentModel;
using Harmonogram.Common.Entities;

namespace Harmonogram.Wpf.ViewModels.ListViewModels
{
    public partial class WorkBlockListViewModel(WorkBlock workBlock) : ObservableValidator
    {
        public int Id { get; set; } = workBlock.Id;
        public int UserId { get; set; } = workBlock.UserId;
        public int? ScheduleId { get; set; } = workBlock.ScheduleId;
        public int DayId { get; set; } = workBlock.DayId;

        [ObservableProperty]
        private int _startHour = workBlock.StartHour;

        [ObservableProperty]
        private int _endHour = workBlock.EndHour;

        [ObservableProperty]
        private DateTime _date = workBlock.Date;

        public bool ToWork;

        public bool IsToday
        {
            get
            {
                var today = DateTime.Today;
                int currentDayId = today.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)today.DayOfWeek;


                bool isDayMatch = DayId == currentDayId;


                bool isDateMatch = _date.Date == today;

                return isDayMatch && isDateMatch;
            }
        }

        public TimeSpan StartTime => TimeSpan.FromHours(StartHour);
        public TimeSpan EndTime => TimeSpan.FromHours(EndHour);

        public string StartHourFormatted => StartTime.ToString(@"hh\:mm");
        public string EndHourFormatted => EndTime.ToString(@"hh\:mm");


    }
}
