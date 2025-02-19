﻿using Harmonogram.Common.Entities;
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

        public void Delete(WorkBlock workBlock)
        {
            _context.WorkBlocks.Remove(workBlock);
            _context.SaveChanges();
        }

        public void Update(WorkBlock workBlock)
        {
            _context.WorkBlocks.Update(workBlock);
            _context.SaveChanges();
        }

        public IEnumerable<WorkBlock> GetAll() => _context.WorkBlocks
                                        .Include(wb => wb.User)
                                        .Include(wb => wb.Day)
                                        .Include(wb => wb.Schedule).ToList();

        public IEnumerable<WorkBlock> GetByUserId(int userId)
        {
            return _context.WorkBlocks
                           .Include(wb => wb.User)
                           .Include(wb => wb.Day)
                           .Include(wb => wb.Schedule)
                           .Where(wb => wb.UserId == userId)
                           .ToList();
        }
    }
}