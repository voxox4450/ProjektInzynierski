using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IWorkBlockService
    {
        event EventHandler<WorkBlock>? WorkBlockAdded;

        void Add(WorkBlock workBlock);

        IEnumerable<WorkBlock> GetAll();
    }
}