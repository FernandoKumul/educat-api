using Domain.Entities;
using educat_api.Context;
using Microsoft.EntityFrameworkCore;

namespace educat_api.Services
{
    public class LikeService
    {
        private readonly AppDBContext _context;

        public LikeService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<bool> ToggleLike(int userId, int commentId)
        {
            try
            {
                var findReview = await _context.Likes
                    .FirstOrDefaultAsync(l => l.FkUser ==  userId && l.FkComment == commentId);
                if(findReview is null)
                {
                    var newLike = new Like
                    {
                        FkComment = commentId,
                        FkUser = userId,
                        IsLike = true
                    };
                    await _context.Likes.AddAsync(newLike);
                    await _context.SaveChangesAsync();
                    return true;
                } else
                {
                    _context.Likes.Remove(findReview);
                    await _context.SaveChangesAsync();
                    return false;
                }
                
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
