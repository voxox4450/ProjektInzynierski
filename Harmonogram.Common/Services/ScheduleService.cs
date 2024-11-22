using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;

namespace Harmonogram.Common.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(IScheduleRepository scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public void Add(Schedule schedule)
        {
            _scheduleRepository.Add(schedule);
        }
    }
}