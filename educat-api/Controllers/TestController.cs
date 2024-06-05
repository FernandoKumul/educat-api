using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly EmailService _emailService;
        public TestController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("send-email")]
        public async Task<IActionResult> SendEmail()
        {
            try
            {
                await _emailService.SendEmail("kumulherrerajosefernando@gmail.com", "djfdjf", "Jose Fernando Kumul Herrera");
                return Ok(new Response<string>(true, "Email enviado"));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message));
            }
        }
    }
}
