using LavMultimidiaAPI.Data;
using LavMultimidiaAPI.DTOs;
using LavMultimidiaAPI.Models;
using LavMultimidiaAPI.Models.Enums;
using LavMultimidiaAPI.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;



namespace LavMultimidiaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuarioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public UsuarioController(ApplicationDbContext context, IConfiguration configuration, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _configuration = configuration;
            _localizer = localizer;
        }

        // ========= CADASTRO =========
        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadastrar([FromBody] CadastroDTO dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                var msg = _localizer["EmailAlreadyExists"];
                return Conflict(new { mensagem = msg });
            }

            var usuario = new Usuario
            {
                Email = dto.Email,
                SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
                TipoUsuario = dto.TipoUsuario
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var msgSuccess = _localizer["RegistrationSuccess"];
            return Ok(new { mensagem = msgSuccess });
        }

        // ========= LOGIN =========
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (usuario == null || BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
            {               var erro = _localizer["InvalidLogin"];
                return Unauthorized(new { mensagem = erro });
            }

            var token = GerarToken(usuario);
            var sucesso = _localizer["LoginSuccess"];

            return Ok(new
            {
                mensagem = sucesso,
                token = token
            });
        }

        // ========= HASH DE SENHA =========
        private static string CalcularHash(string senha)
        {
            using var sha = System.Security.Cryptography.SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        // ========= GERAÇÃO DE TOKEN =========
        private string GerarToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim(ClaimTypes.Role, usuario.TipoUsuario.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
