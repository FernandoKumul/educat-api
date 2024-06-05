using Domain.DTOs.Auth;
using Domain.Entities;
using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;
        public AuthController(AuthService service, EmailService emailService, IConfiguration config)
        {
            _service = service;
            _emailService = emailService;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserInDTO user)
        {
            try
            {
                User newUser = await _service.Register(user);
                var token = GenerateToken(newUser); //Generar el token para verificar (cambiar)
                await _emailService.SendVerificationEmail(newUser.Email, token, newUser.Name + ' ' + newUser.LastName);

                return Ok(new Response<User>(true, "Usuario creado", newUser));
            }
            catch (Exception ex)
            {
                if(ex.Message == "El correo ya está registrado en la base de datos.")
                {
                    return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
                }

                //Cambiar por un error 500
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.PkUser.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var SecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            string token = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            return token;
        }

    }
}
