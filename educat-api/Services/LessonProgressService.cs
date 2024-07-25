using Domain.Entities;
using educat_api.Context;
using Microsoft.EntityFrameworkCore;

namespace educat_api.Services
{
    public class LessonProgressService
    {
        private readonly AppDBContext _context;

        public LessonProgressService (AppDBContext context)
        {
            _context = context;
        }

        public async Task AddProgress(int userId, int lessonId)
        {
            try
            {
                //Comprueba que el curso si fue comprado
                var hasPayment = await _context.Payments
                    .Where(p => p.FkUser == userId &&
                                p.Course != null &&
                                p.Course.Units
                                    .SelectMany(u => u.Lessons)
                                    .Any(l => l.PkLesson == lessonId))
                    .FirstOrDefaultAsync();

                if (hasPayment == null)
                {
                    throw new Exception("No tienes comprada esta lección");
                }

                var hasProgress = await _context.LessonsProgress
                    .AnyAsync(l => l.FkLesson == lessonId && l.FkPayment == hasPayment.PkPayment);

                if (hasProgress)
                {
                    throw new Exception("El progreso ya ha sido agregado");
                    //Cambiar la última lección visitada :D
                }

                LessonProgress newProgress = new LessonProgress
                {
                    FkPayment = hasPayment.PkPayment,
                    FkLesson = lessonId
                };

                await _context.LessonsProgress.AddAsync(newProgress);
                await _context.SaveChangesAsync();

            } catch (Exception)
            {
                throw;
            }

        }
    }
}
