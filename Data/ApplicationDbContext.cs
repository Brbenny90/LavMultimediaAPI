// Data/ApplicationDbContext.cs
using Microsoft.EntityFrameworkCore;
using LavMultimidiaAPI.Models;

namespace LavMultimidiaAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        
        public DbSet<Conteudo> Conteudos { get; set; }

        public DbSet<Playlist> Playlists { get; set; }

    }
}
