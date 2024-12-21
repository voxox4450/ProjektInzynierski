using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int id);
        User Login(User user);
        void Register(User user);
        void Reload();
        User CheckFirst(User user);
        void Update(User user);
        IEnumerable<User> GetAll();
    }
}
