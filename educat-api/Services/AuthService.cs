using Domain.DTOs.Auth;
using Domain.DTOs.User;
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

        public async Task<User?> Login(LoginDTO loginData)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginData.Email && u.Password == EncryptString(loginData.Password));

                if (user != null && !user.ValidatedEmail)
                {
                    throw new Exception("Su cuenta existe, pero su correo no está verificado");
                }
                return user;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> UpdateUserVerification(int userId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.PkUser == userId) 
                    ?? throw new Exception("Usuario no encontrado");
                
                if (user.ValidatedEmail)
                {
                    return false;
                }

                user.ValidatedEmail = true;
                await _context.SaveChangesAsync();
                return true;

            } catch (Exception)
            {
                throw;
            }

        }

        public async Task<UserAuthOutDTO?> GetAuthUserById(int id)
        {
            try
            {
                return await _context.Users.Select(u => new UserAuthOutDTO
                {
                    PkUser = u.PkUser,
                    Name = u.Name,
                    LastName = u.LastName,
                    Email = u.Email,
                    AvatarUrl = u.AvatarUrl,
                    IsInstructor = u.IsInstructor,
                    CreationDate = u.CreationDate
                }).FirstOrDefaultAsync(u => u.PkUser == id);
            }
            catch (Exception e)
            {
                throw new Exception($"Error al obtener el registro: {e.Message}");
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
