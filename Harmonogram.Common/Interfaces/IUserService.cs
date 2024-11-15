using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IUserService
    {
        User Login(User user);

        void Register(User user);

        User CheckFirst(User user);
        IEnumerable<User> GetAll();
        void Update(User user);

        //event handlers
        event EventHandler<User> UserUpdated;
    }
}
