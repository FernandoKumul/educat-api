using Domain.DTOs.Auth;
using Domain.Entities;
using Domain.Utilities;
using educat_api.Context;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace educat_api.Services
{
    public class AuthService
    {
        private readonly AppDBContext _context;
        public AuthService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<User> Register(UserInDTO user)
        {
            try
            {
                var userExists = await _context.Users.AnyAsync(u => u.Email == user.Email);
                if (userExists)
                {
                    throw new Exception("El correo ya está registrado en la base de datos.");
                }
                var newUser = new User
                {
                    Name = user.Name,
                    Email = user.Email,
                    LastName = user.LastName,
                    Password = EncryptString(user.Password),
                };

                await _context.Users.AddAsync(newUser);
                await _context.SaveChangesAsync();
                return newUser;
            }
            catch (Exception e)
            {
                throw new Exception($"Error al registrar usuario: {e.Message}", e.InnerException);
            }
        }

        public static string EncryptString(string str)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(str));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

    }
}
