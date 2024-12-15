using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<Schedule> GetAll()
        {
            return _context.Schedules
                           .Include(schedule => schedule.WorkBlocks)
                           .Include(schedule => schedule.Users)
                           .ToList();
        }

        public void Update(Schedule schedule)
        {
            _context.Schedules.Update(schedule);
            _context.SaveChanges();
        }
    }
}