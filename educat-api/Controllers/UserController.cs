using educat_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("convert-to-instructor")]
    [Authorize]
    public async Task<IActionResult> ConvertToInstructor()
    {
        var userIdInt = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        var result = await _userService.ConvertToInstructor(userIdInt);
        if (result == "User not found")
        {
            return NotFound(result);
        }
        if (result == "User is already an instructor")
        {
            return BadRequest(result);
        }

        return Ok(result);
    }
}
