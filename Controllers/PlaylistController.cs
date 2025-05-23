// Controllers/PlaylistsController.cs
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
    [Authorize] // exige login via JWT
    public class PlaylistsController : ControllerBase
    {
        private readonly PlaylistRepository _repository;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public PlaylistsController(PlaylistRepository repository, IStringLocalizer<SharedResource> localizer)
        {
            _repository = repository;
            _localizer = localizer;
        }

        private int ObterUsuarioId()
        {
            var claim = User.FindFirst(ClaimTypes.Name);
            return claim == null ? 0 : int.Parse(claim.Value); // ou outro identificador se usar e-mail
        }

        [HttpGet]
        public IActionResult Listar()
        {
            var usuarioId = ObterUsuarioId();
            var playlists = _repository.GetAllByUsuario(usuarioId);
            return Ok(playlists);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var usuarioId = ObterUsuarioId();
            var playlist = _repository.GetById(id, usuarioId);
            if (playlist == null)
                return NotFound(new { mensagem = _localizer["PlaylistNotFound"] });

            return Ok(playlist);
        }

        [HttpPost]
        public IActionResult Criar([FromBody] PlaylistDTO dto)
        {
            var usuarioId = ObterUsuarioId();
            var playlist = new Playlist
            {
                Nome = dto.Nome,
                UsuarioId = usuarioId
            };
            _repository.Add(playlist);
            return Ok(new { mensagem = _localizer["PlaylistCreated"] });
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, [FromBody] PlaylistDTO dto)
        {
            var usuarioId = ObterUsuarioId();
            var playlist = _repository.GetById(id, usuarioId);
            if (playlist == null)
                return NotFound(new { mensagem = _localizer["PlaylistNotFound"] });

            playlist.Nome = dto.Nome;
            _repository.Update(playlist);
            return Ok(new { mensagem = _localizer["PlaylistUpdated"] });
        }

        [HttpDelete("{id}")]
        public IActionResult Excluir(int id)
        {
            var usuarioId = ObterUsuarioId();
            var playlist = _repository.GetById(id, usuarioId);
            if (playlist == null)
                return NotFound(new { mensagem = _localizer["PlaylistNotFound"] });

            _repository.Delete(playlist);
            return Ok(new { mensagem = _localizer["PlaylistDeleted"] });
        }
    [HttpGet("porcriador/{criadorId}")]
    [Authorize]
    public IActionResult PorCriador(int criadorId)
    {
        var playlists = _repository.GetAllByUsuario(criadorId);
        return Ok(playlists);
    }



    }
}
