using System.ComponentModel.DataAnnotations;
using LavMultimidiaAPI.Models.Enums;

namespace LavMultimidiaAPI.Models
{
    public class Conteudo
    {
        public int ConteudoId { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public int CriadorId { get; set; }

        public Usuario Criador { get; set; }
    }
}
