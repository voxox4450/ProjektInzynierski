using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;

namespace Harmonogram.Common.Repositories
{
    public class ScheduleRepository(Context context) : IScheduleRepository
    {
        private readonly Context _context = context;

        public void Add(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            _context.SaveChanges();
        }
    }
}