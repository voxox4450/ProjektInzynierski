namespace Harmonogram.Wpf.Models
{
    class Const
    {
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
