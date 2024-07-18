using Domain.Utilities;
using educat_api.Services;
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
        public UserController(UserService service)
        {
            _service = service;
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
    
                var result = await _service.ConvertToInstructor(userIdInt);
    
                if (result == "Usuario no encontrado")
                {
                    return NotFound(new Response<string>(false, result));
                }
    
                if (result == "El usuario ya es un instructor")
                {
                    return BadRequest(new Response<string>(false, result));
                }
    
                return Ok(new Response<string>(true, result));
            } catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }
    }
}
