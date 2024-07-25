using Domain.DTOs.CartWishList;
using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartWishService _service;

        public CartController (CartWishService service)
        {
            _service = service;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserCart()
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return Forbid("Token no valido");
                }

                var cartItems = await _service.GetCartItemsByUserId(userIdInt);

                return Ok(new Response<IEnumerable<CartItemOutDTO>>(true, "Carrito del usaurio obtenido exitosamente", cartItems));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [HttpPost("course/{courseId}")]
        [Authorize]
        public async Task<IActionResult> CreateCartItem(int courseId)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return Forbid("Token no valido");
                }

                var cartItem = await _service.CreateCartItem(userIdInt, courseId);

                return Ok(new Response<CartWishOutDTO>(true, "Artículo del item creado exitosamente", cartItem));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [HttpDelete("{cartId}")]
        [Authorize]
        public async Task<IActionResult> DeleteCartItem(int cartId)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return Forbid("Token no valido");
                }

                await _service.DeleteCartItem(userIdInt, cartId);

                return Ok(new Response<string?>(true, "Articulo del carrito borrado exitosamente"));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }
    }
}
