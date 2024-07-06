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
                    .SingleOrDefaultAsync(i => i.User.PkUser == id) ?? throw new Exception("No se encontró el instructor");
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
                    Name = instructor.User.Name,
                    LastName = instructor.User.LastName,
                    Email = instructor.User.Email,
                    AvatarUrl = instructor.User.AvatarUrl,
                    Description = instructor.User.Description,
                };
                return instructorData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<InstructorInfoDTO> updateInstructor(InstructorInfoDTO request)
        {
            try
            {
                var instructor = await _context.Instructors
                    .Include(i => i.User)
                    .SingleOrDefaultAsync(i => i.PkInstructor == request.PkInstructor) ?? throw new Exception("No se encontró el instructor");
                instructor.Occupation = request.Occupation;
                instructor.FacebookUser = request.FacebookUser;
                instructor.YoutubeUser = request.YoutubeUser;
                instructor.LinkediId = request.LinkediId;
                instructor.TwitterUser = request.TwitterUser;
                instructor.EmailPaypal = request.EmailPaypal;
                instructor.User.Name = request.Name;
                instructor.User.LastName = request.LastName;
                instructor.User.Email = request.Email;
                instructor.User.AvatarUrl = request.AvatarUrl;
                instructor.User.Description = request.Description;
                _context.Instructors.Update(instructor);
                await _context.SaveChangesAsync();
                return request;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}