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

        public async Task<IEnumerable<CourseSearchDTO>> Search(string query)
        {
            var resultQuery =  _context.Courses
                .Where(c => c.Title.Contains(query) && c.Active == true)
                .Select(c => new CourseSearchDTO
                {
                    PkCourse = c.PkCourse,
                    Title = c.Title,
                    Difficulty = c.Difficulty,
                    Price = c.Price,
                    FKInstructor = c.FKInstructor,
                    FkCategory = c.FkCategory
                });

            var queryInstructor =  _context.Instructors
                .Join(
                    _context.Users,
                    i => i.FkUser,
                    u => u.PkUser,
                    (i, u) => new { i, u }
                )
                .Where(iu => iu.i.PkInstructor == resultQuery.First().FKInstructor);

            var queryCategory = _context.Categories
                .Join(
                    _context.Courses,
                    c => c.PkCategory,
                    fc => fc.FkCategory,
                    (c, fc) => new { c, fc }
                )
                .Where(cc => cc.c.PkCategory == resultQuery.First().FkCategory);

            var resultSearch = await resultQuery
                .Join(
                    queryInstructor,
                    r => r.FKInstructor,
                    qi => qi.i.PkInstructor,
                    (r, qi) => new { r, qi }
                )
                .Join(
                    queryCategory,
                    rqi => rqi.r.FkCategory,
                    qc => qc.c.PkCategory,
                    (rqi, qc) => new CourseSearchDTO
                    {
                        PkCourse = rqi.r.PkCourse,
                        Title = rqi.r.Title,
                        Difficulty = rqi.r.Difficulty,
                        Price = rqi.r.Price,
                        FKInstructor = rqi.r.FKInstructor,
                        InstructorName = rqi.qi.u.Name,
                        FkCategory = rqi.r.FkCategory,
                        CategoryName = qc.c.Name
                    }
                ).ToListAsync();
                
            return resultSearch;

        }
    }
}