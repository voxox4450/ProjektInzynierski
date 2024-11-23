using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace Harmonogram.Common.Repositories
{
    public class UserRepository(Context context) : IUserRepository
    {
        private readonly Context _context = context;

        public User Login(User user)
        {
            return _context.Users.FirstOrDefault(u => u.Mail == user.Mail)!;

        }

        public void Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }


        public User CheckFirst(User user)
        {
            Reload();
            return _context.Users.FirstOrDefault(u => u.LastName == user.LastName && u.Mail == user.Mail)!;
        }

        public void Update(User user)
        {
            _context.Users.Where(u => u.Id == user.Id)
                .ExecuteUpdate(u => u
                .SetProperty(u => u.Name, user.Name)
                .SetProperty(u => u.LastName, user.LastName)
                .SetProperty(u => u.Mail, user.Mail)
                .SetProperty(u => u.PhoneNumber, user.PhoneNumber)
                .SetProperty(u => u.Password, user.Password)
                .SetProperty(u => u.AccountNumber, user.AccountNumber)
                .SetProperty(u => u.PaymentPerHour, user.PaymentPerHour)
                .SetProperty(u => u.IsArchived, user.IsArchived)
                );
        }


        public void Reload()
        {
            _context.ChangeTracker.Clear();
        }
        public User? GetById(int id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users
                .Where(u => !u.IsArchived);

        }
    }
}
