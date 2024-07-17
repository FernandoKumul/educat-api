// UserController.cs
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Domain.DTOs;
using educat_api.Services;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }



    [HttpPost("{userId}/convert-to-instructor")]
    public async Task<IActionResult> ConvertToInstructor(int userId)
    {
        var result = await _userService.ConvertToInstructor(userId);

        if (result)
        {
            return Ok(new { message = "User successfully converted to instructor." });
        }
        return BadRequest(new { message = "Failed to convert user to instructor." });

    }
}
