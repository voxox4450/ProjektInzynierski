using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IUserRepository
    {
        User Login(User user);
        void Register(User user);

        User CheckFirst(User user);
    }
}
