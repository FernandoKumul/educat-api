using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/lesson-progress")]
    public class LessonProgressController : ControllerBase
    {
        private readonly LessonProgressService _service;
        public LessonProgressController(LessonProgressService service)
        {
            _service = service;
        }

        [HttpPost("{lessonId}")]
        [Authorize]
        public async Task<IActionResult> AddProgress(int lessonId)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return new ObjectResult(new Response<string>(false, "Token no válido")) { StatusCode = 403 };

                }

                await _service.AddProgress(userIdInt, lessonId);

                return Ok(new Response<bool>(true, "Progreso agregado", true));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? "")); //Cambiar por un 500 luego :D

            }
        }

    }
}
