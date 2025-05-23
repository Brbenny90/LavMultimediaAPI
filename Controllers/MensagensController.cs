using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using LavMultimidiaAPI.Resources;

namespace LavMultimidiaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MensagensController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public MensagensController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet("boasvindas")]
        public IActionResult GetBoasVindas()
        {
            var mensagem = _localizer["WelcomeMessage"];
            return Ok(new { mensagem });
        }

        [HttpGet("erro-login")]
        public IActionResult GetErroLogin()
        {
            var mensagem = _localizer["InvalidLogin"];
            return BadRequest(new { mensagem });
        }
    }
}
