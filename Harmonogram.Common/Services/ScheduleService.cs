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

        public IEnumerable<Schedule> GetAllEditable()
        {
            var today = DateTime.Today;
            return _scheduleRepository.GetAll().Where(s => s.StartDate > today);
        }

        public void Update(Schedule schedule)
        {
            _scheduleRepository.Update(schedule);
        }
    }
}