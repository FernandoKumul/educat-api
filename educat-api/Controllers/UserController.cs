using Domain.Utilities;
using educat_api.Services;
using educat_api.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _service;
        private readonly IConfiguration _config;
        public UserController(UserService service, IConfiguration configuration)
        {
            _service = service;
            _config = configuration;
        }

        [HttpPost("convert-to-instructor")]
        [Authorize]
        public async Task<IActionResult> ConvertToInstructor()
        {
            try
            {
                var userId = User.FindFirst("ID")?.Value;
    
                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return Forbid("Token no valido");
                }
    
                var (message, instructor) = await _service.ConvertToInstructor(userIdInt);
    
                if (message == "Usuario no encontrado")
                {
                    return NotFound(new Response<string>(false, message));
                }
    
                if (message == "El usuario ya es un instructor")
                {
                    return BadRequest(new Response<string>(false, message));
                }

                if (instructor is null)
                {
                    return NotFound(new Response<string>(false, "Usuario no encontrado"));
                }

                var token = TokenUtility.GenerateToken(instructor.User, 180, _config.GetSection("JWT:Key").Value ?? "");
    
                return Ok(new Response<object>(true, message, new {token}));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }
    }
}
