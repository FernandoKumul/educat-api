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

        public async Task SaveCourse(CourseSaveInDTO updatedCourse, int courseId, int userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //var idQuestionAdds = new List<int>();
                var existingCourse = await _context.Courses.FirstOrDefaultAsync(c => c.PkCourse == courseId && c.Instructor.FkUser == userId);

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

                //foreach (var updatedQuestion in updatedTest.Questions)
                //{
                //    var existingQuestion = existingTest.Questions.FirstOrDefault(q => q.Id == updatedQuestion.Id);

                //    if (existingQuestion != null)
                //    {
                //        //Actualizo pregunta
                //        idQuestionAdds.Add(existingQuestion.Id);
                //        existingQuestion.QuestionTypeId = updatedQuestion.QuestionTypeId;
                //        existingQuestion.Description = updatedQuestion.Description;
                //        existingQuestion.Image = updatedQuestion.Image;
                //        existingQuestion.Order = updatedQuestion.Order;
                //        existingQuestion.CaseSensitivity = updatedQuestion.CaseSensitivity;
                //        existingQuestion.Points = updatedQuestion.Points;
                //        existingQuestion.Duration = updatedQuestion.Duration;

                //        foreach (var updatedAnswer in updatedQuestion.Answers)
                //        {
                //            var existingAnswer = existingQuestion.Answers.FirstOrDefault(a => a.Id == updatedAnswer.Id);
                //            if (existingAnswer != null)
                //            {
                //                //Actualizar respuesta
                //                existingAnswer.Text = updatedAnswer.Text;
                //                existingAnswer.Correct = updatedAnswer.Correct;
                //            }
                //            else
                //            {
                //                // Insertar nueva respuesta
                //                var newAnswer = new QuestionAnswer
                //                {
                //                    Correct = updatedAnswer.Correct,
                //                    Text = updatedAnswer.Text,
                //                    QuestionId = existingQuestion.Id
                //                };
                //                existingQuestion.Answers.Add(newAnswer);
                //            }
                //        }

                //        List<QuestionAnswer> answerList = existingQuestion.Answers.ToList();
                //        answerList.RemoveAll(q => updatedQuestion.Answers.Exists(uq => uq.Id == q.Id));
                //        _context.QuestionAnswers.RemoveRange(answerList);
                //    }
                //    else
                //    {
                //        //Agrega nueva pregunta con respuestas
                //        var newQuestion = new Question
                //        {
                //            TestId = idTest,
                //            QuestionTypeId = updatedQuestion.QuestionTypeId,
                //            Description = updatedQuestion.Description,
                //            Image = updatedQuestion.Image,
                //            Order = updatedQuestion.Order,
                //            CaseSensitivity = updatedQuestion.CaseSensitivity,
                //            Points = updatedQuestion.Points,
                //            Duration = updatedQuestion.Duration
                //        };
                //        await _context.Questions.AddAsync(newQuestion);
                //        await _context.SaveChangesAsync();
                //        idQuestionAdds.Add(newQuestion.Id);

                //        foreach (var answer in updatedQuestion.Answers)
                //        {
                //            var newAsnwer = new QuestionAnswer
                //            {
                //                QuestionId = newQuestion.Id,
                //                Text = answer.Text,
                //                Correct = answer.Correct,
                //            };
                //            await _context.QuestionAnswers.AddAsync(newAsnwer);
                //        }
                //    }
                //}
                //await _context.SaveChangesAsync();

                ////Borra las preguntas y respuestas que ya no incluya
                //var idQuestionDelete = new List<int>();
                //foreach (var question in existingTest.Questions)
                //{
                //    if (!idQuestionAdds.Contains(question.Id)) idQuestionDelete.Add(question.Id);
                //}

                //var answersToDelete = await _context.QuestionAnswers
                //    .Where(a => idQuestionDelete.Contains(a.QuestionId))
                //    .ToListAsync();
                //_context.QuestionAnswers.RemoveRange(answersToDelete);

                //var questionsToDelete = await _context.Questions
                //    .Where(q => idQuestionDelete.Contains(q.Id))
                //    .ToListAsync();
                //_context.Questions.RemoveRange(questionsToDelete);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw new Exception($"Error guardar el curso: ${ex.Message}.", ex.InnerException);

            }

        }
    }
}
