using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IColorRepository
    {
        IEnumerable<Color> GetAll();

        Color GetById(int id);
    }
}