using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IScheduleService
    {
        void Add(Schedule schedule);

        IEnumerable<Schedule> GetAllEditable();

        void Update(Schedule schedule);
    }
}