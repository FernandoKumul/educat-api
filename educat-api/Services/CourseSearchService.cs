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
                        (ci, u) => new { ci.c, ci.i, InstructorName = u.Name, InstructorLastName = u.LastName }
                    )
                    .Join(
                        _context.Categories,
                        ciu => ciu.c.FkCategory,
                        cat => cat.PkCategory,
                        (ciu, cat) => new { ciu.c, ciu.InstructorName, CategoryName = cat.Name, ciu.InstructorLastName }
                    )
                    .GroupJoin(
                        _context.Comments,
                        course => course.c.PkCourse,
                        comment => comment.FkCourse,
                        (course, comments) => new { course, comments }
                    )
                    .Select(x => new CourseSearchDTO
                    {
                        PkCourse = x.course.c.PkCourse,
                        Title = x.course.c.Title,
                        Difficulty = x.course.c.Difficulty,
                        Cover = x.course.c.Cover,
                        Price = x.course.c.Price,
                        Active = x.course.c.Active,
                        Tags = x.course.c.Tags,
                        FKInstructor = x.course.c.FKInstructor,
                        InstructorName = x.course.InstructorName,
                        FkCategory = x.course.c.FkCategory,
                        CategoryName = x.course.CategoryName,
                        InstructorLastName = x.course.InstructorLastName,
                        Rating = x.comments.Any() ? x.comments.Average(c => c.Score) : 0,
                        CretionDate = x.course.c.CretionDate
                    })
                    .OrderByDescending(c => c.CretionDate)
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
                    (ci, u) => new { ci.c, ci.i, InstructorName = u.Name, InstructorLastName = u.LastName }
                )
                .Join(
                    _context.Categories,
                    ciu => ciu.c.FkCategory,
                    cat => cat.PkCategory,
                    (ciu, cat) => new { ciu.c, ciu.InstructorName, CategoryName = cat.Name, ciu.InstructorLastName }
                )
                .GroupJoin(
                    _context.Comments,
                    course => course.c.PkCourse,
                    comment => comment.FkCourse,
                    (course, comments) => new { course, comments }
                )
                .Select(x => new CourseSearchDTO
                {
                    PkCourse = x.course.c.PkCourse,
                    Title = x.course.c.Title,
                    Difficulty = x.course.c.Difficulty,
                    Cover = x.course.c.Cover,
                    Price = x.course.c.Price,
                    Active = x.course.c.Active,
                    Tags = x.course.c.Tags,
                    FKInstructor = x.course.c.FKInstructor,
                    InstructorName = x.course.InstructorName,
                    FkCategory = x.course.c.FkCategory,
                    CategoryName = x.course.CategoryName,
                    InstructorLastName = x.course.InstructorLastName,
                    Rating = x.comments.Any() ? x.comments.Average(c => c.Score) : 0,
                    CretionDate = x.course.c.CretionDate
                })
                .Where(c => ((c.Title != null && c.Title.Contains(query)) || (c.Tags != null && c.Tags.Contains(query))) && c.Active == true && c.CategoryName == category)
                .OrderByDescending(c => c.CretionDate)
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