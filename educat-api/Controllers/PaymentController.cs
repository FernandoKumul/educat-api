using Domain.DTOs.CartWishList;
using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static educat_api.Services.PaymentService;

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

                var paypalData = await _service.CreateOrderAsync(userIdInt);

                return Ok(new Response<object>(true, "Orden creada exitosamente", paypalData));
            }
            catch (PayPalException ex)
            {
                return BadRequest(new Response<object>(false, ex.Message, ex.ErrorData));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string?>(false, ex.Message, ex.InnerException?.Message));
            }
        }
        
        [HttpPost("order/{id}/capture")]
        [Authorize]
        public async Task<IActionResult> CaptureOrder(string id)
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;

                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return Forbid("Token no valido");
                }

                var paypalData = await _service.CaptureOrder(id);
                await _service.CreatePayments(userIdInt, paypalData);

                return Ok(new Response<object>(true, "Orden comprada exitosamente", paypalData));
            }
            catch (PayPalException ex)
            {
                return BadRequest(new Response<object>(false, ex.Message, ex.ErrorData));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string?>(false, ex.Message, ex.InnerException?.Message));
            }
        }
    }
}
