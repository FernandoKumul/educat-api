using Domain.DTOs.Comment;
using Domain.Entities;
using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController : ControllerBase
    {
        private readonly CommentService _service;
        public CommentController(CommentService service) 
        {
            _service = service;
        }

        [HttpPost("review")]
        public async Task<IActionResult> CreateReview([FromBody] ReviewInDTO newReview)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return new ObjectResult(new Response<string>(false, "Token no válido")) { StatusCode = 403 };
                }

                if(newReview.Score > 5 || newReview.Score < 1)
                {
                    return BadRequest(new Response<string>(false, "La calificación debe ser un número entre 1 y 5"));
                }

                var reviewData = await _service.CreateReview(newReview, userIdInt);

                return Ok(new Response<CommentOutDTO>(true, "Reseña creada con exito", reviewData));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string?>(false, ex.Message, ex.InnerException?.Message));
            }
        }
    }
}
