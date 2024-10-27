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
    }
}
