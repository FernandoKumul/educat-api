using Domain.DTOs.Category;
using educat_api.Context;
using Microsoft.EntityFrameworkCore;

namespace educat_api.Services
{
    public class CategoryService
    {
        private readonly AppDBContext _context;
        public CategoryService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoryOutDTO>> GetAll()
        {
            return await _context.Categories
                .Select(c => new CategoryOutDTO { PkCategory = c.PkCategory , Name = c.Name})
                .ToListAsync();
        }
    }
}
