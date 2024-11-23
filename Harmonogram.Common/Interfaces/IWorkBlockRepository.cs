using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IWorkBlockRepository
    {
        void Add(WorkBlock workBlock);

        IEnumerable<WorkBlock> GetAll();
    }
}