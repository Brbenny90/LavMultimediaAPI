using LavMultimidiaAPI.Models;
using LavMultimidiaAPI.Data;

namespace LavMultimidiaAPI.Repositories
{
    public class ConteudoRepository
    {
        private readonly ApplicationDbContext _context;

        public ConteudoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Conteudo> ListarTodos() => _context.Conteudos.ToList();

        public IEnumerable<Conteudo> ListarPorCriador(int criadorId) =>
            _context.Conteudos.Where(c => c.CriadorId == criadorId).ToList();

        public void Criar(Conteudo conteudo)
        {
            _context.Conteudos.Add(conteudo);
            _context.SaveChanges();
        }
    }
}
