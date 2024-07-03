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

                if (!hasCourseByUser)
                {
                    throw new Exception("No tienes este curso comprado");
                }

                bool hasComment = await _context.Comments
                    .AnyAsync(c => c.FkUser == userId && c.FkCourse == review.courseId);

                if (hasComment)
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

                if (comment is null)
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

                if (reviewFind is null)
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

        public async Task<object> GetReviews(int courseId, int pageNumber, int pageSize)
        {
            try
            {
                int skip = (pageNumber - 1) * pageSize;

                //Verificar si el curso está activo
                var reviews = await _context.Comments
                                .Where(c => c.FkCourse == courseId)
                                .GroupJoin(
                                    _context.Likes,
                                    c => c.PkComment,
                                    l => l.PkLike,
                                    (c, likes) => new { Comment = c, LikesCount = likes.Count()}
                                )
                                .OrderByDescending(x => x.LikesCount)
                                .OrderByDescending(x => x.Comment.PkComment)
                                .Skip(skip)
                                .Take(pageSize)
                                .Select(c => new CommentUserOutDTO
                                {
                                    PkComment = c.Comment.PkComment,
                                    FkCourse = c.Comment.FkCourse,
                                    FkLesson = c.Comment.FkLesson,
                                    Score = c.Comment.Score,
                                    Text = c.Comment.Text,
                                    User = new UserMinOutDTO
                                    {
                                        PkUser = c.Comment.User.PkUser,
                                        AvatarUrl = c.Comment.User.AvatarUrl,
                                        Name = c.Comment.User.Name,
                                        LastName = c.Comment.User.LastName,
                                    }
                                })
                                .ToListAsync();

                var count = await _context.Comments
                               .CountAsync(c => c.FkCourse == courseId);

                var rating = await GetAvgReviewsByCourse(courseId);

                return new { result = reviews, count, rating };
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetAvgReviewsByCourse(int courseId)
        {
            try
            {
                var average = await _context.Comments
                    .Where(c => c.FkCourse == courseId)
                    .AverageAsync(c => c.Score) ?? 0m;
                Console.WriteLine(average);

                return average;
            } catch (Exception)
            {
                throw;
            }
        }

        public async Task<CommentUserOutDTO?> GetCourseReviewByUser(int userId, int courseId)
        {
            try
            {
                var review = await _context.Comments
                    .Where(c => c.FkCourse == courseId && c.FkUser == userId)
                    .GroupJoin(
                        _context.Likes,
                        c => c.PkComment,
                        l => l.PkLike,
                        (c, likes) => new { Comment = c, LikesCount = likes.Count() }
                    )
                    .Select(c => new CommentUserOutDTO
                    {
                        PkComment = c.Comment.PkComment,
                        FkCourse = c.Comment.FkCourse,
                        FkLesson = c.Comment.FkLesson,
                        Score = c.Comment.Score,
                        Text = c.Comment.Text,
                        User = new UserMinOutDTO
                        {
                            PkUser = c.Comment.User.PkUser,
                            AvatarUrl = c.Comment.User.AvatarUrl,
                            Name = c.Comment.User.Name,
                            LastName = c.Comment.User.LastName,
                        }
                    })
                    .FirstOrDefaultAsync();

                return review;
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
