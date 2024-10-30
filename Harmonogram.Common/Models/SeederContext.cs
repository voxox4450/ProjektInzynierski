using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonogram.Common.Models
{
    public class SeederContext(Context context) : ISeederContext
    {
        private readonly Context _context = context;

        public void Seed()
        {
            if (!_context.Users.Any())
            {
                var user = new List<User>()
                {
                    new User { IsAdmin = true, Name = "Admin", LastName = "Admin", Mail="admin@admin.com", Password = PasswordHelper.Encrypt("admin") },
                    new User { IsAdmin = false, Name = "User", LastName = "User", Mail="user@user.com", Password = PasswordHelper.Encrypt("user") }
                };

                _context.Users.AddRange(user);
                _context.SaveChanges();
            }
        }
    }
}