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
        private readonly HttpClient _httpClient = new()
        {
            BaseAddress = new Uri("https://jsonplaceholder.typicode.com"),
        };
        public TestController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("send-email")]
        public async Task<IActionResult> SendEmail()
        {
            try
            {
                await _emailService.SendEmail("kumulherrerajosefernando@gmail.com", "djfdjf", "Jose Fernando Kumul Herrera", 6025295);
                return Ok(new Response<string>(true, "Email enviado"));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message));
            }
        }

        [HttpGet("api-http")]
        public async Task<IActionResult> GetAsync()
        {
            using HttpResponseMessage response = await _httpClient.GetAsync("todos/3");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                return Ok(data);
            }

            return StatusCode((int)response.StatusCode, response.ReasonPhrase);
        }
    }
}
