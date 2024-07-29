using Azure.Core;
using Domain.DTOs.User;
using Domain.Entities;
using educat_api.Context;
using Microsoft.EntityFrameworkCore;

namespace educat_api.Services
{
    public class UserService
    {
        private readonly AppDBContext _context;

        public UserService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<(string, Instructor?)> ConvertToInstructor(int userIdInt)
        {
            try
            {
                var user = await _context.Users.FindAsync(userIdInt);
                if (user == null)
                {
                    return ("Usuario no encontrado", null);
                }

                if (user.IsInstructor)
                {
                    return ("El usuario ya es un instructor", null);
                }

                user.IsInstructor = true;
                var instructor = new Instructor
                {
                    FkUser = userIdInt,
                    Occupation = "",
                    FacebookUser = "",
                    YoutubeUser = "",
                    LinkediId = "",
                    TwitterUser = "",
                    EmailPaypal = "",
                };
                _context.Instructors.Add(instructor);
                await _context.SaveChangesAsync();

                return ("Usuario convertido a instructor exitosamente", instructor);
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<User> EditUser(int userIdint, UserEditInDTO updateData)
        {
            try
            {
                var user = await _context.Users
                    .SingleOrDefaultAsync(i => i.PkUser == userIdint) ?? throw new Exception("No se encontró el usuario.");
                user.Name = updateData.Name;
                user.LastName = updateData.LastName;
                user.AvatarUrl = updateData.AvatarUrl;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return user;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
