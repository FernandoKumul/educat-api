using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs.Auth
{
    public class LoginDTO
    {
        [MaxLength(50, ErrorMessage = "El correo no puede tener más de 50 caracteres.")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
        public string Email { get; set; } = null!;
        [MaxLength(50, ErrorMessage = "La contraseña no puede tener más de 50 caracteres.")]
        public string Password { get; set; } = null!;
    }
}
