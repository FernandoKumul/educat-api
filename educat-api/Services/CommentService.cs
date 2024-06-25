using Domain.DTOs.Comment;
using Domain.Entities;
using educat_api.Context;
using Microsoft.EntityFrameworkCore;

namespace educat_api.Services
{
    public class CommentService
    {
        private readonly AppDBContext _context;
        public CommentService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<CommentOutDTO> CreateReview(ReviewInDTO review, int userId)
        {
            try
            {
                bool hasCourseByUser = await _context.Payments
                    .AnyAsync(p => p.FkCourse == review.courseId && p.FkUser == userId);

                if(!hasCourseByUser) 
                {
                    throw new Exception("No tienes este curso comprado");
                }

                bool hasComment = await _context.Comments
                    .AnyAsync(c => c.FkUser == userId && c.FkCourse == review.courseId);

                if(hasComment)
                {
                    throw new Exception("No puedes agregar más de un comentario por curso");
                }
                var newReview = new Comment
                {
                    FkUser = userId,
                    FkCourse = review.courseId,
                    Text = review.Text,
                    Score = review.Score
                };
                
                await _context.Comments.AddAsync(newReview);
                await _context.SaveChangesAsync();

                var returnReview = new CommentOutDTO
                {
                    PkComment = newReview.PkComment,
                    FkUser = newReview.FkUser,
                    FkCourse = newReview.FkCourse,
                    FkLesson = newReview.FkLesson,
                    Text = newReview.Text,
                    Score = newReview.Score,
                    CretionDate = newReview.CretionDate
                };

                return returnReview;
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
