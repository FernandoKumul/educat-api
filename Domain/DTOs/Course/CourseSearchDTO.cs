
namespace Domain.DTOs.Course
{
    public class CourseSearchDTO
    {
        public int PkCourse { get; set; }
        public string? Title { get; set; }
        public string? Difficulty { get; set; }
        public string? Cover { get; set; }
        public decimal? Price { get; set; }
        public decimal? Rating { get; set; } = 0;
        public bool? Active { get; set; }
        public string? Tags { get; set; }
        public int? FKInstructor { get; set; }
        public string? InstructorName { get; set; }
        public string? InstructorLastName { get; set; }
        public int? FkCategory { get; set; }
        public string? CategoryName { get; set; }
        public DateTime CretionDate { get; set; } = DateTime.Now;

    }
}
