using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;

namespace Harmonogram.Common.Services
{
    public class ColorService : IColorService
    {
        private readonly IColorRepository _colorRepository;

        public ColorService(IColorRepository colorRepository)
        {
            _colorRepository = colorRepository;
        }

        public IEnumerable<Color> GetAll() => _colorRepository.GetAll();

        public Color GetById(int id) => _colorRepository.GetById(id);
    }
}