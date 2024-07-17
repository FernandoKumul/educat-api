// IUserService.cs
using System.Threading.Tasks;

namespace educat_api.Services
{
    public interface IUserService
    {
        Task<bool> ConvertToInstructor(int userId);
    }
}
