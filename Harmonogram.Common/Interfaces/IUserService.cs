using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IUserService
    {
        User? Get(int userid);
        User Login(User user);

        void Register(User user);

        User CheckFirst(User user);
        IEnumerable<User> GetAll();
        void Update(User user);
        void Archive(int userId);
        void Reload();

        //event handlers
        event EventHandler<User>? UserUpdated;

        event EventHandler<int>? UserArchived;
    }
}
