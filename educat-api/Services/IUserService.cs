using System.Threading.Tasks;

public interface IUserService
{
    Task<string> ConvertToInstructor(int userIdInt);
}
