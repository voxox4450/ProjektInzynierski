using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;

namespace Harmonogram.Common.Services
{
    public class WorkBlockService : IWorkBlockService
    {
        private readonly IWorkBlockRepository _workBlockRepository;

        public event EventHandler<WorkBlock> WorkBlockAdded;

        public event EventHandler<WorkBlock> WorkBlockUpdated;

        public event EventHandler<WorkBlock> WorkBlockDeleted;

        public WorkBlockService(IWorkBlockRepository workBlockRepository)
        {
            _workBlockRepository = workBlockRepository;
        }
        public void Add(WorkBlock workBlock)
        {
            _workBlockRepository.Add(workBlock);
            WorkBlockAdded?.Invoke(this, workBlock);
        }
        public void Update(WorkBlock workBlock)
        {
            _workBlockRepository.Update(workBlock);
            WorkBlockUpdated?.Invoke(this, workBlock);
        }
        public void Delete(WorkBlock workBlock)
        {
            _workBlockRepository.Delete(workBlock);
            WorkBlockDeleted?.Invoke(this, workBlock);
        }
        public IEnumerable<WorkBlock> GetAll() => _workBlockRepository.GetAll();

        public IEnumerable<WorkBlock> GetByUserId(int userId)
        {
            return _workBlockRepository.GetByUserId(userId);
        }
    }
}