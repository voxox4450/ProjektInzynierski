using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Harmonogram.Common.Repositories
{
    public class WorkBlockRepository(Context context) : IWorkBlockRepository
    {
        private readonly Context _context = context;

        public void Add(WorkBlock workBlock)
        {
            _context.WorkBlocks.Add(workBlock);
            _context.SaveChanges();
        }

        public void Update(WorkBlock workBlock)
        {
            _context.WorkBlocks.Update(workBlock);
            _context.SaveChanges();
        }

        public IEnumerable<WorkBlock> GetAll() => _context.WorkBlocks;

        public void Delete(WorkBlock workBlock)
        {
            _context.WorkBlocks.Remove(workBlock);
            _context.SaveChanges();
        }
    }
}