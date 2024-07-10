using Domain.DTOs.Auth;
using Domain.DTOs.User;
using Domain.DTOs.Google;
using Domain.Entities;
using Domain.Utilities;
using educat_api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace educat_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _service;
        private readonly EmailService _emailService;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;
        public AuthController(AuthService service, EmailService emailService, IConfiguration config)
        {
            _httpClient = new HttpClient();
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
                var token = GenerateToken(newUser, 1440);
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

        [HttpGet("verify-email/{token}")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value ?? "");
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "ID").Value;
                
                if (!Int32.TryParse(userId, out int userIdInt))
                {
                    return BadRequest("Token no valido");
                }

                var isVerify = await _service.UpdateUserVerification(userIdInt);

                if (!isVerify)
                {
                    return BadRequest(new Response<string>(false, "Tu cuenta ya está valida"));
                }

                RedirectResult redirect = new RedirectResult("http://localhost:5173/verify-email", true);

                return redirect;
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, "Token Inválido:: " + ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {
            try
            {
                var user = await _service.Login(login);

                if (user == null)
                {
                    return BadRequest(new Response<User>(false, "Correo o contraseña incorrectos"));
                }

                string token = GenerateToken(user, 120);
                return Ok(new Response<object>(true, "Inicio de sesión exitoso", new { token }));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        [HttpGet("verify-token")]
        [Authorize]
        public async Task<ActionResult<UserAuthOutDTO>> GetInfoUser()
        {
            var id = User.FindFirst("ID")?.Value;
            if (id == null)
            {
                return BadRequest(new Response<string>(false, "Usuario no autenticado"));
            }
            try
            {
                var userId = int.Parse(id);
                var user = await _service.GetAuthUserById(userId);
                if (user == null)
                {
                    return NotFound(new { message = $"No se encontró el registro con el ID: {userId}" });
                }
                return Ok(new Response<UserAuthOutDTO>(true, "Usuario autenticado", user));
            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }

        // Login/register con Google
        [HttpPost("google")]
        public async Task<IActionResult> UserWithGoogle(string accessToken){
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get,  $"https://www.googleapis.com/oauth2/v3/userinfo?access_token={accessToken}");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        
                var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var payload = await response.Content.ReadAsStringAsync();
                var googleUser = JsonConvert.DeserializeObject<UserWithGoogleDTO>(payload);

                if (googleUser is null)
                {
                    return BadRequest(new Response<string>(false, "No se pudo obtener la información del usuario"));
                }

                var user = new UserWithGoogleDTO
                {
                    Email = googleUser.Email,
                    Name = googleUser.Name,
                    LastName = googleUser.LastName,
                    AvatarUrl = googleUser.AvatarUrl,
                    ValidatedEmail = googleUser.ValidatedEmail
                };

                var userEdu = await _service.UserWithGoogle(user);
                string token = GenerateToken(userEdu, 120);
                return Ok(new Response<object>(true, "Inicio de sesión exitoso", new { token }));

            }
            catch (Exception ex)
            {
                return BadRequest(new Response<string>(false, ex.Message, ex.InnerException?.Message ?? ""));
            }
        }
        private string GenerateToken(User user, int durationMin)
        {
            var claims = new[]
            {
                new Claim("ID", user.PkUser.ToString()),
                new Claim("Email", user.Email),
                new Claim("IsInstructor", user.IsInstructor.ToString()),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("JWT:Key").Value ?? ""));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var SecurityToken = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(durationMin),
                signingCredentials: creds
            );

            string token = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
            return token;
        }

    }
}
