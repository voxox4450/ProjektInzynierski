using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IWorkBlockService
    {
        event EventHandler<WorkBlock>? WorkBlockAdded;

        event EventHandler<WorkBlock>? WorkBlockUpdated;

        event EventHandler<WorkBlock>? WorkBlockDeleted;

        void Add(WorkBlock workBlock);
        void Update(WorkBlock workBlock);
        void Delete(WorkBlock workBlock);
        IEnumerable<WorkBlock> GetAll();
        IEnumerable<WorkBlock> GetByUserId(int userId);
    }
}