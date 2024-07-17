// UserService.cs
using Domain.Entities;
using educat_api.Services;
using System.Threading.Tasks;

namespace educat_api.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> ConvertToInstructor(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            user.IsInstructor = true;
            user.Instructor = new Instructor
            {
                FkUser = user.PkUser,
                CreationDate = DateTime.Now,
                // Los demás campos se inicializan como null o valores predeterminados
            };

            await _userRepository.UpdateUserAsync(user);
            return true;
        }
    }
}
