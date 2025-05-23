using LavMultimidiaAPI.Models.Enums;

namespace LavMultimidiaAPI.DTOs
{
    public class CadastroDTO
    {
        public string Email { get; set; }
        public string Senha { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
    }
}
