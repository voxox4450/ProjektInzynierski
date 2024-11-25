using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IWorkBlockService
    {
        event EventHandler<WorkBlock>? WorkBlockAdded;

        void Add(WorkBlock workBlock);

        IEnumerable<WorkBlock> GetAll();
        IEnumerable<WorkBlock> GetByUserId(int userId);
    }
}