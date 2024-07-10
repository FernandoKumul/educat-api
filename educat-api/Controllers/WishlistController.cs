using Domain.DTOs.CartWishList;
using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WishlistController : ControllerBase
    {
        private readonly CartWishService _service;
        public WishlistController(CartWishService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserWishlist()
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return Forbid("Token no valido");
                }

                var wishlistItems = await _service.GetWishListByUserId(userIdInt);

                return Ok(new Response<IEnumerable<CartItemOutDTO>>(true, "Lista de deseos del usuario obtenida exitosamente", wishlistItems));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [HttpPost("course/{courseId}")]
        [Authorize]
        public async Task<IActionResult> CreateWishlistItem(int courseId)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return Forbid("Token no valido");
                }

                var wishlistItem = await _service.CreateWishListItem(userIdInt, courseId);

                return Ok(new Response<CartWishOutDTO>(true, "Artículo de la lista de deseos creado exitosamente", wishlistItem));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [HttpDelete("course/{courseId}")]
        [Authorize]
        public async Task<IActionResult> DeleteWishlistItem(int courseId)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return Forbid("Token no valido");
                }

                await _service.DeleteWishListItem(userIdInt, courseId);

                return Ok(new Response<CartWishOutDTO>(true, "Artículo de la lista de deseos eliminado exitosamente"));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }
    }
}