
namespace Domain.DTOs.Course
{
    public class CourseSearchDTO
    {
        public int PkCourse { get; set; }
        public string? Title { get; set; }
        public string? Difficulty { get; set; }
        public decimal? Price { get; set; }
        public int FKInstructor { get; set; }
        public string? InstructorName { get; set; }
        public int? FkCategory { get; set; }
        public string? CategoryName { get; set; }
    }
}
