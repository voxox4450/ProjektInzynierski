namespace Harmonogram.Common.Models
{
    public class Constants
    {
        public IEnumerable<string> SetHours()
        {
            List<string> hours = new List<string>();
            for (int hour = 0; hour < 25; hour++)
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

        public DateTime SetStartOfWeek()
        {
            DateTime dateToday = DateTime.Today;
            int daysToMonday = (int)dateToday.DayOfWeek - (int)DayOfWeek.Monday;

            if (daysToMonday < 0)
            {
                daysToMonday += 7;
            }

            return dateToday.AddDays(-daysToMonday);
        }

        public DateTime SetEndOfWeek()
        {
            DateTime startOfWeek = SetStartOfWeek();
            return startOfWeek.AddDays(6);
        }

        public DateTime SetStartOfMonth()
        {
            DateTime dateToday = DateTime.Today;
            return new DateTime(dateToday.Year, dateToday.Month, 1);
        }

        public DateTime SetEndOfMonth()
        {
            DateTime dateToday = DateTime.Today;
            int daysInMonth = DateTime.DaysInMonth(dateToday.Year, dateToday.Month);
            return new DateTime(dateToday.Year, dateToday.Month, daysInMonth);
        }
    }
}