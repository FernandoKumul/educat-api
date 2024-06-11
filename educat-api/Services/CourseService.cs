using Domain.DTOs.Course;
using Domain.DTOs.Lesson;
using Domain.DTOs.Unit;
using Domain.Entities;
using educat_api.Context;
using Microsoft.EntityFrameworkCore;

namespace educat_api.Services
{
    public class CourseService
    {
        private readonly AppDBContext _context;
        public CourseService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<CourseEditOutDTO?> GetToEdit(int courseId, int userId)
        {
            try
            {
                return await _context.Courses
                    .Where(c => c.PkCourse == courseId && c.Instructor.FkUser == userId)
                    .Select(c => new CourseEditOutDTO
                    {
                        PkCourse = c.PkCourse,
                        FkCategory = c.FkCategory,
                        FKInstructor = c.FKInstructor,
                        Title = c.Title,
                        Summary = c.Summary,
                        Language = c.Language,
                        Difficulty = c.Difficulty,
                        Price = c.Price,
                        VideoPresentation = c.VideoPresentation,
                        Cover = c.Cover,
                        Requeriments = c.Requeriments,
                        Description = c.Description,
                        LearnText = c.LearnText,
                        Tags = c.Tags,
                        Active = c.Active,
                        CretionDate = c.CretionDate,
                        UpdateDate = c.UpdateDate,
                        Units = c.Units.Select(u => new UnitEditOutDTO
                        {
                            PkUnit = u.PkUnit,
                            FkCourse = u.FkCourse,
                            Title = u.Title,
                            Order = u.Order,
                            Lessons = u.Lessons.Select(l => new LessonEditOutDTO
                            {
                                PkLesson = l.PkLesson,
                                Title = l.Title,
                                Fkunit = l.Fkunit,
                                Text = l.Text,
                                Order = l.Order,
                                TimeDuration = l.TimeDuration,
                                Type = l.Type,
                                VideoUrl = l.VideoUrl,
                                CretionDate = l.CretionDate
                            }).ToList()
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

            } catch (Exception)
            {
                throw;
            }
        }
    }
}
