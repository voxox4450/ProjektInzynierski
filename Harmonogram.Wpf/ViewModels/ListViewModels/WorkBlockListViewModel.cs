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

        [ObservableProperty]
        private string _salary = (workBlock.User.PaymentPerHour * (workBlock.EndHour - workBlock.StartHour)).ToString("F2");

        [ObservableProperty]
        private int _hourAmount = workBlock.EndHour - workBlock.StartHour;

        [ObservableProperty]
        private string _hourRange = string.Concat(workBlock.StartHour, " - ", workBlock.EndHour);

        [ObservableProperty]
        private string _fullName = string.Concat(workBlock.User.Name, " ", workBlock.User.LastName);

        [ObservableProperty]
        private string _day = $"{workBlock.Date:dd.MM.yyyy}";

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
