using Domain.DTOs.Instructor;
using Domain.Entities;
using educat_api.Context;
using Microsoft.EntityFrameworkCore;

namespace educat_api.Services
{
    public class InstructorService
    {
        private readonly AppDBContext _context;
        public InstructorService(AppDBContext context)
        {
            _context = context;
        }
        public async Task<InstructorInfoDTO> GetInstructorById(int id)
        {
            try
            {
                var instructor = await _context.Instructors
                    .Include(i => i.User)
                    .SingleOrDefaultAsync(i => i.User.PkUser == id);
                if (instructor == null)
                {
                    throw new Exception("No se encontró el instructor");
                }
                var instructorData = new InstructorInfoDTO
                {
                    PkInstructor = instructor.PkInstructor,
                    FkUser = instructor.FkUser,
                    Occupation = instructor.Occupation,
                    FacebookUser = instructor.FacebookUser,
                    YoutubeUser = instructor.YoutubeUser,
                    LinkediId = instructor.LinkediId,
                    TwitterUser = instructor.TwitterUser,
                    EmailPaypal = instructor.EmailPaypal,
                    CreationDate = instructor.CreationDate,
                    Name = instructor.User.Name,
                    LastName = instructor.User.LastName,
                    Email = instructor.User.Email,
                    AvatarUrl = instructor.User.AvatarUrl,
                    Description = instructor.User.Description,
                    ValidatedEmail = instructor.User.ValidatedEmail
                };
                return instructorData;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}