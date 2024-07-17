using educat_api.Context;
using Domain.Entities;
using System.Threading.Tasks;
using educat_api.Services;

public class UserService : IUserService
{
    private readonly AppDBContext _context;

    public UserService(AppDBContext context)
    {
        _context = context;
    }

    public async Task<string> ConvertToInstructor(int userIdInt)
    {
        var user = await _context.Users.FindAsync(userIdInt);
        if (user == null)
        {
            return "User not found";
        }

        if (user.IsInstructor)
        {
            return "User is already an instructor";
        }

        user.IsInstructor = true;
        var instructor = new Instructor
        {
            FkUser = userIdInt,
            User = user,
            Occupation = string.Empty,
            FacebookUser = string.Empty,
            YoutubeUser = string.Empty,
            LinkediId = string.Empty,
            TwitterUser = string.Empty,
            EmailPaypal = string.Empty,
            CreationDate = DateTime.Now
        };
        _context.Instructors.Add(instructor);
        await _context.SaveChangesAsync();

        return "User converted to instructor successfully";
    }
}
