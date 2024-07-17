// Domain/Repositories/IUserRepository.cs
using System.Threading.Tasks;
using Domain.Entities;

namespace educat_api.Services
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task UpdateUserAsync(User user);
    }
}
