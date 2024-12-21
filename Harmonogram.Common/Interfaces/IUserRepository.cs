using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IUserRepository
    {
        User GetById(int id);
        User Login(User user);
        void Register(User user);
        User CheckFirst(User user);
        IEnumerable<User> GetAll();
        void Update(User user);
        void Reload();
        
        
    }
}
