using Harmonogram.Common.Entities;

namespace Harmonogram.Common.Models
{
    public class SeederContext(Context context)
    {
        private readonly Context _context = context;

        public void SeedUser()
        {
            if (!_context.Users.Any())
            {
                var user = new List<User>()
                {
                    new() { IsAdmin = true, Name = "Admin", LastName = "Admin", Mail="admin@admin.com", Password = PasswordHelper.Encrypt("admin") },
                    new() { IsAdmin = false, Name = "User", LastName = "User", Mail="user@user.com", Password = PasswordHelper.Encrypt("user") }
                };

                _context.Users.AddRange(user);
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
