using Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace educat_api.Utilities
{
    public static class TokenUtility
    {
        public static string GenerateToken(User user, int durationMin, string secretKey)
        {
            try
            {
                var claims = new[]
                {
                    new Claim("ID", user.PkUser.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("IsInstructor", user.IsInstructor.ToString()),
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var SecurityToken = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(durationMin),
                    signingCredentials: creds
                );

                string token = new JwtSecurityTokenHandler().WriteToken(SecurityToken);
                return token;
            } catch (Exception)
            {
                throw;
            }
        }
    }
}
