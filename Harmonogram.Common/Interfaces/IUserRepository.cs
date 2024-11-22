using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int id);

        IEnumerable<User> GetAll();
    }
}