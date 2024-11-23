using Harmonogram.Common.Entities;
using Harmonogram.Common.Interfaces;
using Harmonogram.Common.Models;

namespace Harmonogram.Common.Services
{
    public class UserServices : IUserService
    {
        private readonly IUserRepository _userRepository;


        //Events handlers
        public event EventHandler<User>? UserUpdated;
        public event EventHandler<int>? UserArchived;

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User? Get(int id)
        {
            return _userRepository.GetById(id);
        }


        public User? Login(User user)
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
        public void Update(User user)
        {
            _userRepository.Update(user);
            Reload();
            var editedUser = Get(user.Id);
            if (editedUser != null)
            {
                UserUpdated?.Invoke(this, editedUser);
            }

        }

        public void Archive(int userId)
        {
            var user = Get(userId)!;
            user!.IsArchived = true;
            _userRepository.Update(user);

            UserArchived?.Invoke(this, userId);
        }


        public User? GetById(int id)
        {
            return _userRepository.GetById(id);
        }
        public void Reload()
        {
            _userRepository.Reload();
        }
        public IEnumerable<User> GetAll() => _userRepository.GetAll();



    }
}
