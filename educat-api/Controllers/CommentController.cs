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

                return Ok(new Response<CommentOutDTO>(true, "Reseña creada con éxito", reviewData));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string?>(false, ex.Message, ex.InnerException?.Message));
            }
        }

        [HttpGet("review")]
        public async Task<IActionResult> GetReviews([FromQuery(Name = "course")] int courseId)
        {
            try
            {
                var reviews = await _service.GetReviews(courseId);

                return Ok(new Response<IEnumerable<CommentUserOutDTO>>(true, "Reseñas conseguidas con éxito", reviews));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string?>(false, ex.Message, ex.InnerException?.Message));
            }
        }

        [HttpPut("review/{reviewId}")]
        public async Task<IActionResult> UpdateReview([FromBody] ReviewEditInDTO reviewIn, int reviewId)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return new ObjectResult(new Response<string>(false, "Token no válido")) { StatusCode = 403 };
                }

                if (reviewIn.Score > 5 || reviewIn.Score < 1)
                {
                    return BadRequest(new Response<string>(false, "La calificación debe ser un número entre 1 y 5"));
                }

                var reviewUpdate = await _service.UpdateReview(reviewIn, userIdInt, reviewId);

                if(reviewUpdate is null)
                {
                    return NotFound(new Response<string?>(false, "No se encontro la reseña para actualizar"));
                }

                return Ok(new Response<CommentOutDTO>(true, "Reseña editada con éxito", reviewUpdate));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string?>(false, ex.Message, ex.InnerException?.Message));
            }
        }

        [HttpDelete("review/{reviewId}")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return new ObjectResult(new Response<string>(false, "Token no válido")) { StatusCode = 403 };
                }

                var result = await _service.DeleteComment(reviewId, userIdInt);

                if (!result)
                {
                    return NotFound(new Response<string?>(false, "No se encontro la reseña a borrar"));
                }

                return Ok(new Response<string?>(true, "Reseña borrada con éxito"));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string?>(false, ex.Message, ex.InnerException?.Message));
            }
        }
    }
}
