using educat_api.Services;
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

        [HttpGet]
        public async Task<IActionResult> GenerateToken()
        {
            var token = await _service.GenerateAccessTokenAsync();
            return Ok(new { accessToken = token });
        }
    }
}
