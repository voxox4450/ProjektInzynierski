using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;

namespace Harmonogram.Common.Repositories
{
    public class UserRepository(Context context) : IUserRepository
    {
        private readonly Context _context = context;

        public User Login(User user)
        {
            return _context.Users.FirstOrDefault(u => u.Mail == user.Mail);

        }

        public void Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }


        public User? CheckFirst(User user)
        {
            Reload();
            return _context.Users.FirstOrDefault(u => u.LastName == user.LastName && u.Mail == user.Mail);
        }

        public void Reload()
        {
            _context.ChangeTracker.Clear();
        }
    }
}
