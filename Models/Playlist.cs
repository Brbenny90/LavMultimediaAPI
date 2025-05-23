using System.ComponentModel.DataAnnotations;
using LavMultimidiaAPI.Models;

namespace LavMultimidiaAPI.Models
{
    public class Playlist
    {
        public int PlaylistId { get; set; }

        [Required]
        public string Nome { get; set; }

        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
