using Harmonogram.Common.Interfaces;

namespace Harmonogram.Common.Models
{
    public class Constants : IConstants
    {
        public IEnumerable<string> SetHours()
        {
            List<string> hours = new List<string>();
            for (int hour = 0; hour < 24; hour++)
            {
                hours.Add($"{hour:D2}:00");
            }
            return hours;
        }

        public DateTime SetStartDate()
        {
            DateTime dateToday = DateTime.Today;
            int daysUntilNextMonday = ((int)DayOfWeek.Monday - (int)dateToday.DayOfWeek + 7) % 7;
            if (daysUntilNextMonday == 0)
            {
                daysUntilNextMonday = 7;
            }

            return dateToday.AddDays(daysUntilNextMonday);
        }

        public DateTime SetEndDate() => SetStartDate().AddDays(6);

        public string SetBaseName(DateTime startDate) => "Schedule: " + startDate.ToString("dd-MM-yyyy");
    }
}