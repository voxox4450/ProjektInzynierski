using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IColorService
    {
        IEnumerable<Color> GetAll();

        Color GetById(int id);
    }
}