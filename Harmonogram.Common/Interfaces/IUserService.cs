using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IUserService
    {
        User GetById(int id);

        IEnumerable<User> GetAll();
    }
}