using Domain.DTOs.Auth;
using Domain.DTOs.Course;
using Domain.Entities;
using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseService _courseService;

        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [Authorize]
        [HttpGet("to-edit/{id}")]
        public async Task<IActionResult> GetToEdit(int id)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;
                var isInstructor = User.FindFirst("IsInstructor")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return Forbid("Token no valido");
                }

                if (!Boolean.TryParse(isInstructor, out bool isInstructorBool))
                {
                    return Forbid("Token no valido");
                }

                if (!isInstructorBool)
                {
                    return new ObjectResult(new Response<string>(false, "No eres un instructor")) { StatusCode = 403 };
                }

                var course = await _courseService.GetToEdit(id, userIdInt);

                if (course == null)
                {
                    return NotFound(new Response<string>(false, "Curso no encontrado"));
                }

                return Ok(new Response<CourseEditOutDTO>(true, "Curso obtenido", course));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [Authorize]
        [HttpPut("save-draft/{id}")]
        public async Task<ActionResult<Response<string>>> Update(int id, [FromBody] CourseSaveInDTO updateCourse)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;
                var isInstructor = User.FindFirst("IsInstructor")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return new ObjectResult(new Response<string>(false, "Token no válido")) { StatusCode = 403 };

                }

                if (!Boolean.TryParse(isInstructor, out bool isInstructorBool))
                {
                    return new ObjectResult(new Response<string>(false, "Token no válido")) { StatusCode = 403 };
                }

                if (!isInstructorBool)
                {
                    return new ObjectResult(new Response<string>(false, "No eres un instructor")) { StatusCode = 403 };
                }

                await _courseService.SaveCourse(updateCourse, id, userIdInt);

                return Ok(new Response<string>(true, "Curso guardado de manera exitosa"));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D
            }
        }

        [HttpGet("public/{id}")]
        public async Task<IActionResult> GetInfoPublic(int id)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;
                var course = await _courseService.GetInfoPublic(id);
                if (course is null)
                {
                    return NotFound(new Response<string>(false, "Curso no encontrado"));
                }

                if (userId is not null)
                {
                    var hasCourse = await _courseService.HasPurchasedCourse(id, int.Parse(userId));
                    course.Purchased = hasCourse;
                }

                return Ok(new Response<CoursePublicOutDTO>(true, "Curso encontrado exitosamente", course));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetCourses()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var courses = await _courseService.GetCoursesByUserId(userId);
            return Ok(courses);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] CourseTitleDTO courseTitleDTO)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var courseId = await _courseService.CreateCourse(courseTitleDTO, userId);
            return CreatedAtAction(nameof(GetToEdit), new { id = courseId }, courseId);
        }

        [Authorize]
        [HttpGet("{courseId}")]
        public async Task<IActionResult> GetCourseById(int courseId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var course = await _courseService.GetToEdit(courseId, userId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [Authorize]
        [HttpPut("{courseId}")]
        public async Task<IActionResult> SaveCourse(int courseId, [FromBody] CourseSaveInDTO updatedCourse)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _courseService.SaveCourse(updatedCourse, courseId, userId);
            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var course = await _courseService.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            if (course.Instructor.FkUser != userId)
            {
                return Forbid();
            }

            var result = await _courseService.DeleteCourseAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return Ok(courses);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCourse([FromBody] Course course)
        {
            var createdCourse = await _courseService.CreateCourseAsync(course);
            return CreatedAtAction(nameof(GetCourseById), new { id = createdCourse.PkCourse }, createdCourse);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] Course course)
        {
            if (id != course.PkCourse)
            {
                return BadRequest();
            }

            var updatedCourse = await _courseService.UpdateCourseAsync(course);
            if (updatedCourse == null)
            {
                return NotFound();
            }

            return Ok(updatedCourse);
        }
    }
}
