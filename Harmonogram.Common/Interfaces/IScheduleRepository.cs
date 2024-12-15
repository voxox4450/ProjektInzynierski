using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IScheduleRepository
    {
        void Add(Schedule schedule);

        IEnumerable<Schedule> GetAll();

        void Update(Schedule schedule);
    }
}