using Domain.DTOs.Course;
using Domain.Entities;
using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikeController : ControllerBase
    {
        private readonly LikeService _service;
        public LikeController(LikeService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("review/toggle")]
        public async Task<IActionResult> ToggleLike([FromQuery(Name = "comment")] int commentId)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return Forbid("Token no valido");
                }

                var isLike = await _service.ToggleLike(userIdInt, commentId);

                return Ok(new Response<bool>(true, "Like modificado exitosamente", isLike));

            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

    }
}
