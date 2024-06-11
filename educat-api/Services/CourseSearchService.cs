using Domain.DTOs.Course;
using Domain.Entities;
using educat_api.Context;
using Microsoft.EntityFrameworkCore;

namespace educat_api.Services
{
    public class CourseSearchService
    {
        private readonly AppDBContext _context;
        public CourseSearchService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<CourseSearchDTO>, int)> Search(int pageNumber, int pageSize, string query, string category)
        {
            int skip = (pageNumber - 1) * pageSize;

            var resultSearch = await _context.Courses
                .Where(c => (c.Title.Contains(query) || (c.Tags != null && c.Tags.Contains(query))) && c.Active == true)
                .Join(
                    _context.Instructors,
                    c => c.FKInstructor,
                    i => i.PkInstructor,
                    (c, i) => new { c, i }
                )
                .Join(
                    _context.Users,
                    ci => ci.i.FkUser,
                    u => u.PkUser,
                    (ci, u) => new { ci.c, ci.i, InstructorName = u.Name }
                )
                .Join(
                    _context.Categories,
                    ciu => ciu.c.FkCategory,
                    cat => cat.PkCategory,
                    (ciu, cat) => new CourseSearchDTO
                    {
                        PkCourse = ciu.c.PkCourse,
                        Title = ciu.c.Title,
                        Difficulty = ciu.c.Difficulty,
                        Price = ciu.c.Price,
                        Active = ciu.c.Active,
                        Tags = ciu.c.Tags,
                        FKInstructor = ciu.c.FKInstructor,
                        InstructorName = ciu.InstructorName,
                        FkCategory = ciu.c.FkCategory,
                        CategoryName = cat.Name
                    }
                )
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();
            var count = await _context.Courses
                .Where(c => (c.Title.Contains(query) || (c.Tags != null && c.Tags.Contains(query))) && c.Active == true)
                .CountAsync();
        
            if (category == "all" || category == null)
            {
                return (resultSearch, count);

            } else
            {
                var filteredCategories = resultSearch.Where(c => c.CategoryName == category);
                return (filteredCategories, count);
            }
        }
    }
}