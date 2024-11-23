using Harmonogram.Common.Interfaces;

namespace Harmonogram.Common.Services
{
    public class DayService : IDayService
    {
        private readonly IDayRepository _dayRepository;

        public DayService(IDayRepository dayRepository)
        {
            _dayRepository = dayRepository;
        }

        public int GetDayId(string dayName) => _dayRepository.GetDayId(dayName);
    }
}