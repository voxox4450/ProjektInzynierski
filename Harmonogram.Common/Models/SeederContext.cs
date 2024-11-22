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

        public void SeedUsers()
        {
            if (!_context.Users.Any())
            {
                var users = new List<User>()
                {
                    new User { IsAdmin = true, Name = "Admin", LastName = "Admin", Mail="admin@admin.com", Password = PasswordHelper.Encrypt("admin") },
                    new User { IsAdmin = false, Name = "User", LastName = "User", Mail="user@user.com", Password = PasswordHelper.Encrypt("user") }
                };

                _context.Users.AddRange(users);
                _context.SaveChanges();
            }
        }

        public void SeedDays()
        {
            if (!_context.Days.Any())
            {
                var days = new List<Day>()
                {
                    new Day {Name = "Poniedziałek"},
                    new Day {Name = "Wtorek"},
                    new Day {Name = "Środa"},
                    new Day {Name = "Czwartek"},
                    new Day {Name = "Piątek"},
                    new Day {Name = "Sobota"},
                    new Day {Name = "Niedziela"}
                };

                _context.Days.AddRange(days);
                _context.SaveChanges();
            }
        }
    }
}