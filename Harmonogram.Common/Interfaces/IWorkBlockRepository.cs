using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IWorkBlockRepository
    {
        void Add(WorkBlock workBlock);

        void Update(WorkBlock workBlock);

        IEnumerable<WorkBlock> GetAll();

        void Delete(WorkBlock workBlock);
    }
}