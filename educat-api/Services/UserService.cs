using Domain.Entities;
using educat_api.Context;

namespace educat_api.Services
{
    public class UserService
    {
        private readonly AppDBContext _context;

        public UserService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<string> ConvertToInstructor(int userIdInt)
        {
            try
            {
                var user = await _context.Users.FindAsync(userIdInt);
                if (user == null)
                {
                    return "Usuario no encontrado";
                }

                if (user.IsInstructor)
                {
                    return "El usuario ya es un instructor";
                }

                user.IsInstructor = true;
                var instructor = new Instructor
                {
                    FkUser = userIdInt,
                    User = user,
                    Occupation = "",
                    FacebookUser = "",
                    YoutubeUser = "",
                    LinkediId = "",
                    TwitterUser = "",
                    EmailPaypal = "",
                };
                _context.Instructors.Add(instructor);
                await _context.SaveChangesAsync();

                return "Usuario convertido a instructor exitosamente";

            } catch (Exception)
            {
                throw;
            }
        }
    }
}
