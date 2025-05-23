// Models/Usuario.cs
using System.ComponentModel.DataAnnotations;
using LavMultimidiaAPI.Models.Enums;

namespace LavMultimidiaAPI.Models
{
    public class Usuario
    {
        public int UsuarioId { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string SenhaHash { get; set; }

   
        [Required]
        public TipoUsuario TipoUsuario { get; set; }
    }
}
