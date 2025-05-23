using LavMultimidiaAPI.DTOs;
using LavMultimidiaAPI.Models;
using LavMultimidiaAPI.Repositories;
using LavMultimidiaAPI.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Security.Claims;

namespace LavMultimidiaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConteudosController : ControllerBase
    {
        private readonly ConteudoRepository _repo;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ConteudosController(ConteudoRepository repo, IStringLocalizer<SharedResource> localizer)
        {
            _repo = repo;
            _localizer = localizer;
        }

        private int ObterUsuarioId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

        private string ObterTipoUsuario() =>
            User.FindFirst(ClaimTypes.Role)?.Value ?? "";

        // 👤 Acesso livre para Comum e Criador
        [HttpGet]
        [Authorize]
        public IActionResult Listar()
        {
            var conteudos = _repo.ListarTodos();
            return Ok(conteudos);
        }

        // Buscar conteúdo por ID
        [HttpGet("{id}")]
        [Authorize]
        public IActionResult ObterPorId(int id)
        {
            var conteudo = _repo.ListarTodos().FirstOrDefault(c => c.ConteudoId == id);
            if (conteudo == null)
                return NotFound(new { mensagem = _localizer["ConteudoNaoEncontrado"] });

            return Ok(conteudo);
        }

        //Buscar conteúdo por criador
        [HttpGet("porcriador/{criadorId}")]
        [Authorize]
        public IActionResult PorCriador(int criadorId)
        {
            var lista = _repo.ListarPorCriador(criadorId);
            return Ok(lista);
        }

        // 👤 Criador pode ver só seus conteúdos
        [HttpGet("meus")]
        [Authorize(Roles = "Criador")]
        public IActionResult MeusConteudos()
        {
            var id = ObterUsuarioId();
            return Ok(_repo.ListarPorCriador(id));
        }

        // ➕ Somente Criador pode criar
        [HttpPost]
        [Authorize(Roles = "Criador")]
        public IActionResult Criar([FromBody] ConteudoDTO dto)
        {
            var conteudo = new Conteudo
            {
                Titulo = dto.Titulo,
                CriadorId = ObterUsuarioId()
            };

            _repo.Criar(conteudo);
            return Ok(new { mensagem = _localizer["ConteudoCriado"] });
        }
    }
}
