using Domain.DTOs.Course;
using Domain.DTOs.Lesson;
using Domain.DTOs.Unit;
using Domain.DTOs.User;
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

        public async Task SaveCourse(CourseSaveInDTO updatedCourse, int courseId, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var idUnitsUpdateAndAdd = new List<int>();
                var existingCourse = await _context.Courses
                    .Include(c => c.Units)
                        .ThenInclude(u => u.Lessons)
                    .FirstOrDefaultAsync(c => c.PkCourse == courseId && c.Instructor.FkUser == userId);

                if (existingCourse is null)
                {
                    await transaction.RollbackAsync();
                    throw new Exception("Curso no encontrado");
                }

                existingCourse.FkCategory = updatedCourse.FkCategory;
                existingCourse.Title = updatedCourse.Title;
                existingCourse.Summary = updatedCourse.Summary;
                existingCourse.Language = updatedCourse.Language;
                existingCourse.Difficulty = updatedCourse.Difficulty;
                existingCourse.Price = updatedCourse.Price;
                existingCourse.VideoPresentation = updatedCourse.VideoPresentation;
                existingCourse.Cover = updatedCourse.Cover;
                existingCourse.Requeriments = updatedCourse.Requeriments;
                existingCourse.Description = updatedCourse.Description;
                existingCourse.LearnText = updatedCourse.LearnText;
                existingCourse.Tags = updatedCourse.Tags;
                existingCourse.UpdateDate = DateTime.Now;

                foreach(var updatedUnit in  updatedCourse.Units)
                {
                    var idLessonsUpdateAndAdd = new List<int>();
                    var existingUnit = existingCourse.Units.FirstOrDefault(q => q.PkUnit == updatedUnit.PkUnit);

                    if (existingUnit != null)
                    {
                        //Actualizo unidad existente
                        existingUnit.Title = updatedUnit.Title;
                        existingUnit.Order = updatedUnit.Order;
                        idUnitsUpdateAndAdd.Add(existingUnit.PkUnit);

                        foreach(var updateLesson in updatedUnit.Lessons)
                        {
                            var existingLesson = existingUnit.Lessons.FirstOrDefault(l => l.PkLesson == updateLesson.PkLesson);

                            if(existingLesson != null)
                            {
                                //Actualizar lección
                                existingLesson.Text = updateLesson.Text;
                                existingLesson.Title = updateLesson.Title;
                                existingLesson.VideoUrl = updateLesson.VideoUrl;
                                existingLesson.Type = updateLesson.Type;
                                existingLesson.Order = updateLesson.Order;
                                existingLesson.TimeDuration = updateLesson.TimeDuration;
                                idLessonsUpdateAndAdd.Add(existingLesson.PkLesson);

                            } else
                            {
                                //Agregar nueva lección
                                Lesson newLesson = new Lesson
                                {
                                    Fkunit = existingUnit.PkUnit,
                                    Type = updateLesson.Type,
                                    Title = updateLesson.Title,
                                    VideoUrl = updateLesson.VideoUrl,
                                    Text = updateLesson.Text,
                                    TimeDuration = updateLesson.TimeDuration,
                                    Order = updateLesson.Order,
                                    CretionDate = DateTime.Now
                                };

                                await _context.Lessons.AddAsync(newLesson);
                                idLessonsUpdateAndAdd.Add(newLesson.PkLesson);

                            }
                        }

                        //Borrar lecciones de la unidad
                        var lessonToDelete = new List<Lesson>();
                        foreach (var lesson in existingUnit.Lessons)
                        {
                            if (!idLessonsUpdateAndAdd.Contains(lesson.PkLesson)) lessonToDelete.Add(lesson);
                        }

                        _context.Lessons.RemoveRange(lessonToDelete);
                    }
                    else
                    {
                        //Agrego nueva unidad
                        var newUnit = new Unit
                        {
                            FkCourse = courseId,
                            Title = updatedUnit.Title,
                            Order = updatedUnit.Order,
                        };
                        await _context.Units.AddAsync(newUnit);
                        await _context.SaveChangesAsync();
                        idUnitsUpdateAndAdd.Add(newUnit.PkUnit);
                        
                        //Agregar todas sus lecciones
                        foreach(var lesson in updatedUnit.Lessons)
                        {
                            Lesson newLesson = new Lesson()
                            {
                                Fkunit = newUnit.PkUnit,
                                Type = lesson.Type,
                                Title = lesson.Title,
                                VideoUrl = lesson.VideoUrl,
                                Text = lesson.Text,
                                TimeDuration = lesson.TimeDuration,
                                Order = lesson.Order,
                                CretionDate = DateTime.Now
                            };

                            await _context.Lessons.AddAsync(newLesson);
                        }
                    }
                    
                }

                //Borrar unidades y lecciones
                var UnitsToDelete = new List<Unit>();
                foreach (var unit in existingCourse.Units)
                {
                    if (!idUnitsUpdateAndAdd.Contains(unit.PkUnit)) UnitsToDelete.Add(unit);
                }

                _context.Units.RemoveRange(UnitsToDelete);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error guardar el curso: ${ex.Message}.", ex.InnerException);

            }

        }

        public async Task<CoursePublicOutDTO?> GetInfoPublic(int courseId)
        {
            try
            {
                var course = await _context.Courses
                    .Where(c => c.PkCourse == courseId && c.Active == true)
                    .Select(c => new CoursePublicOutDTO
                    {
                        PkCourse = c.PkCourse,
                        FKInstructor = c.FKInstructor,
                        FkCategory = c.FkCategory,
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
                        Instructor = new UserMinOutDTO
                        {
                            PkUser = c.Instructor.FkUser,
                            AvatarUrl = c.Instructor.User.AvatarUrl,
                            LastName = c.Instructor.User.LastName,
                            Name = c.Instructor.User.Name
                        },
                        Units = c.Units.Select(u => new UnitProgramOutDTO
                        {
                            PkUnit = u.PkUnit,
                            FkCourse = u.FkCourse,
                            Title = u.Title,
                            Order = u.Order,
                            Lessons = u.Lessons.Select(l => new LessonProgramOutDTO
                            {
                                PkLesson = l.PkLesson,
                                Title = l.Title,
                                Fkunit = l.Fkunit,
                                Order = l.Order,
                                TimeDuration = l.TimeDuration,
                                Type = l.Type,
                                CretionDate = l.CretionDate
                            }).ToList()
                        }).ToList()

                    })
                    .FirstOrDefaultAsync();

                if (course is null) return null;

                //Traer las calificaciones

                int students = await _context.Payments.CountAsync(c => c.FkCourse == courseId);
                course.NumberStudents = students;

                return course;
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
