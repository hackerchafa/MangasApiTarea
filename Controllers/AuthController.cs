using MangaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MangaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                // Verificamos que se envíe un nombre de usuario válido (no vacío)
                var username = request?.Username;
                if (string.IsNullOrWhiteSpace(username))
                {
                    return BadRequest(new { error = "Debe proporcionar un nombre de usuario." });
                }

                // Generamos el token
                var token = GenerateJwtToken(username);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                // Retorna información útil en caso de error 500
                return StatusCode(500, new
                {
                    error = "Ocurrió un error al generar el token",
                    detalle = ex.Message
                });
            }
        }

        private string GenerateJwtToken(string username)
        {
            // Lee la clave desde la configuración
            var jwtKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key no configurada");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Creamos los claims
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
            };

            // Creamos el token
            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
