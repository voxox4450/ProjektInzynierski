using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harmonogram.Common.Repositories
{
    public class UserRepository(Context context) : IUserRepository
    {
        private readonly Context _context = context;

        public IEnumerable<User> GetAll() => _context.Users;
    }
}