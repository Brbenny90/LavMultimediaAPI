using LavMultimidiaAPI.Models;
using LavMultimidiaAPI.Data;

namespace LavMultimidiaAPI.Repositories
{
    public class PlaylistRepository
    {
        private readonly ApplicationDbContext _context;

        public PlaylistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Playlist> GetAllByUsuario(int usuarioId)
        {
            return _context.Playlists.Where(p => p.UsuarioId == usuarioId).ToList();
        }

        public Playlist GetById(int id, int usuarioId)
        {
            return _context.Playlists.FirstOrDefault(p => p.PlaylistId == id && p.UsuarioId == usuarioId);
        }

        public void Add(Playlist playlist)
        {
            _context.Playlists.Add(playlist);
            _context.SaveChanges();
        }

        public void Update(Playlist playlist)
        {
            _context.Playlists.Update(playlist);
            _context.SaveChanges();
        }

        public void Delete(Playlist playlist)
        {
            _context.Playlists.Remove(playlist);
            _context.SaveChanges();
        }
    }
}
