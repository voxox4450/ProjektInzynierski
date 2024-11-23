using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;

namespace Harmonogram.Common.Services
{
    public class WorkBlockService : IWorkBlockService
    {
        private readonly IWorkBlockRepository _workBlockRepository;

        public event EventHandler<WorkBlock> WorkBlockAdded;

        public WorkBlockService(IWorkBlockRepository workBlockRepository)
        {
            _workBlockRepository = workBlockRepository;
        }

        public void Add(WorkBlock workBlock)
        {
            _workBlockRepository.Add(workBlock);
            WorkBlockAdded?.Invoke(this, workBlock);
        }

        public IEnumerable<WorkBlock> GetAll() => _workBlockRepository.GetAll();
    }
}