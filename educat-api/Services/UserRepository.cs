// Infrastructure/Repositories/UserRepository.cs
using System.Threading.Tasks;
using Domain.Entities;
using educat_api.Context;
using educat_api.Services;
using Google;
using Microsoft.EntityFrameworkCore;

namespace educat_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _context;

        public UserRepository(AppDBContext context)
        {
            _context = context;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .Include(u => u.Instructor)
                .FirstOrDefaultAsync(u => u.PkUser == userId);
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
