using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IUserRepository
    {
        User Login(User user);
        void Register(User user);
        void Reload();
        User Get(int receiverId);
        User CheckFirst(User user);
        void Update(User user);
        IEnumerable<User> GetAll();
    }
}
