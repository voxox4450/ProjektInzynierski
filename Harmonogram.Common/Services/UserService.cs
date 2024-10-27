using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;

namespace Harmonogram.Common.Services
{
    public class UserServices : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User Login(User user)
        {
            var loggedInUser = _userRepository.Login(user);
            if (loggedInUser != null && PasswordHelper.Decrypt(user.Password) == PasswordHelper.Decrypt(loggedInUser.Password))
            {
                return loggedInUser;
            }
            else
            {
                return null;
            }
        }

        public void Register(User user)
        {
            _userRepository.Register(user);

        }

        public User CheckFirst(User user)
        {
            return _userRepository.CheckFirst(user);
        }
    }
}
