using Domain.DTOs.Comment;
using Domain.DTOs.User;
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
        public async Task<bool> DeleteComment(int commentId, int userId)
        {
            try
            {
                var comment = await _context.Comments
                    .FirstOrDefaultAsync(c => c.PkComment == commentId && c.FkUser == userId);

                if(comment is null)
                {
                    return false;
                }
                
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();

                return true;
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommentOutDTO?> UpdateReview(ReviewEditInDTO reviewData, int userId, int reviewId)
        {
            try
            {
                var reviewFind = await _context.Comments.FirstOrDefaultAsync(c => c.PkComment == reviewId && c.FkUser == userId);

                if(reviewFind is null)
                {
                    return null;
                }

                reviewFind.Text = reviewData.Text;
                reviewFind.Score = reviewData.Score;

                await _context.SaveChangesAsync();

                var returnReview = new CommentOutDTO
                {
                    PkComment = reviewFind.PkComment,
                    FkUser = reviewFind.FkUser,
                    FkCourse = reviewFind.FkCourse,
                    FkLesson = reviewFind.FkLesson,
                    Text = reviewFind.Text,
                    Score = reviewFind.Score,
                    CretionDate = reviewFind.CretionDate
                };

                return returnReview;
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<CommentUserOutDTO>> GetReviews(int courseId)
        {
            try
            {
                var reviews = await _context.Comments
                                .Select(c => new CommentUserOutDTO
                                {
                                    PkComment = c.PkComment,
                                    FkCourse = c.FkCourse,
                                    FkLesson = c.FkLesson,
                                    Score = c.Score,
                                    Text = c.Text,
                                    User = new UserMinOutDTO
                                    {
                                        PkUser = c.User.PkUser,
                                        AvatarUrl = c.User.AvatarUrl,
                                        Name = c.User.Name,
                                        LastName = c.User.LastName,
                                    }
                                })
                                .Where(c => c.FkCourse == courseId)
                                .ToListAsync();
                return reviews;
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
