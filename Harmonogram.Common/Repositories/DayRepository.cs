using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;

namespace Harmonogram.Common.Repositories
{
    public class DayRepository(Context context) : IDayRepository
    {
        private readonly Context _context = context;

        public int GetDayId(string dayName)
        {
            var day = _context.Days.FirstOrDefault(d => d.Name == dayName);
            return day.Id;
        }
    }
}