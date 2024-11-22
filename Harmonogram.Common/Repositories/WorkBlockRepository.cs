using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public IEnumerable<WorkBlock> GetAll() => _context.WorkBlocks;
    }
}