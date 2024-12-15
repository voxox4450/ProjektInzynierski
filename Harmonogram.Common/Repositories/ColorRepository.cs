using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;

namespace Harmonogram.Common.Repositories
{
    public class ColorRepository(Context context) : IColorRepository
    {
        private readonly Context _context = context;

        public IEnumerable<Color> GetAll() => _context.Colors.ToList();

        public Color GetById(int id) => _context.Colors.FirstOrDefault(c => c.Id == id);
    }
}