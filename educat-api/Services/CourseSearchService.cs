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

            if (category == "all" || category == null)
            {
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
                            Cover = ciu.c.Cover,
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
                    
                return (resultSearch, count);

            }
            else
            {
                var filteredCategories = await _context.Courses
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
                .Where(c => ((c.Title != null && c.Title.Contains(query)) || (c.Tags != null && c.Tags.Contains(query))) && c.Active == true && c.CategoryName == category)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

                var count = await _context.Courses
                    .Join(
                        _context.Categories,
                        c => c.FkCategory,
                        cat => cat.PkCategory,
                        (c, cat) => new { c, cat }
                    )
                    .Where(c => ((c.c.Title != null && c.c.Title.Contains(query)) || (c.c.Tags != null && c.c.Tags.Contains(query))) && c.c.Active == true && c.cat.Name == category)
                    .CountAsync();

                return (filteredCategories, count);
            }
        }
    }
}