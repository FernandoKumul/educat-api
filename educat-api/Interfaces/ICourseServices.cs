using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.DTOs.Course;
using Domain.Entities;

namespace educat_api.Services
{
    public interface ICourseService
    {
        Task<CourseEditOutDTO> GetToEdit(int courseId, int userId);
        Task SaveCourse(CourseSaveInDTO updatedCourse, int courseId, int userId);
        Task<CoursePublicOutDTO> GetInfoPublic(int courseId);
        Task<bool> HasPurchasedCourse(int courseId, int userId);
        Task<IEnumerable<Course>> GetCoursesByUserId(int userId);
        Task<int> CreateCourse(CourseTitleDTO courseTitleDTO, int userId);
        Task<Course> GetCourseByIdAsync(int courseId);
        Task<bool> DeleteCourseAsync(int courseId);
        Task<IEnumerable<Course>> GetAllCoursesAsync();
        Task<Course> CreateCourseAsync(Course course);
        Task<Course> UpdateCourseAsync(Course course);
    }
}
