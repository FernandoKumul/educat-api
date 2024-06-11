using Domain.DTOs.Auth;
using Domain.DTOs.Course;
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
        private readonly CourseService _service;

        public CourseController(CourseService service)
        {
            _service = service;
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

                if(!isInstructorBool)
                {
                    return new ObjectResult(new Response<string>(false, "No eres un instructor")) { StatusCode = 403 };
                }

                var course = await _service.GetToEdit(id, userIdInt);

                if(course == null)
                {
                    return NotFound(new Response<string>(false, "Curso no encontrado"));
                }

                return Ok(new Response<CourseEditOutDTO>(true, "Curso obtenido", course));
            }
            catch (Exception ex)
            {

                //Cambiar por un error 500
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

    }
}
