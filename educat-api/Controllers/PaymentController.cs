using Domain.DTOs.CartWishList;
using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentService _service;
        public PaymentController(PaymentService service)
        {
            _service = service;
        }

        [HttpPost("order")]
        [Authorize]
        public async Task<IActionResult> CreateOrder()
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return Forbid("Token no valido");
                }

                var paypalData = await _service.CreateOrderAsync();

                return Ok(new Response<object>(true, "Carrito del usaurio obtenido exitosamente", paypalData));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string?>(false, ex.Message, ex.InnerException?.Message));
            }
        }
    }
}
