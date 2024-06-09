﻿using Domain.DTOs.Auth;
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
                var token = GenerateToken(newUser);
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
        private string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim("ID", user.PkUser.ToString()),
                new Claim("Email", user.Email)
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
