using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Interfaces
{
    public interface IUserService
    {
        User? GetById(int id); // Preferred version
        User Login(User user);
        void Register(User user);
        void Reload();
        User CheckFirst(User user);
        void Update(User user);
        IEnumerable<User> GetAll();
        void Archive(int userId);
        

        //event handlers
        event EventHandler<User>? UserUpdated;

        event EventHandler<int>? UserArchived;
    }
}
