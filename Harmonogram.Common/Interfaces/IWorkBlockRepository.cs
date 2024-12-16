using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IWorkBlockRepository
    {
        void Add(WorkBlock workBlock);
        void Update(WorkBlock workBlock);
        void Delete(WorkBlock workBlock);
        IEnumerable<WorkBlock> GetAll();
        IEnumerable<WorkBlock> GetByUserId(int userId);
    }
}