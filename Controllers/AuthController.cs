using MangaApi.Models; // Modelo para login
using Microsoft.AspNetCore.Mvc; // Funcionalidad de controladores Web API
using Microsoft.IdentityModel.Tokens; // Para la generación y validación de tokens
using System.IdentityModel.Tokens.Jwt; // Para trabajar con JWT
using System.Security.Claims; // Para claims de usuario
using System.Text; // Para codificación

namespace MangaApi.Controllers
{
    // Controlador para autenticación y generación de tokens JWT
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config; // Acceso a la configuración de la app

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        // POST: api/auth/login
        // Endpoint para autenticación de usuario y generación de token JWT
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // Usuario y contraseña fijos para autenticación
            const string fixedUser = "admin";
            const string fixedPass = "1234";

            if (request?.Username != fixedUser || request?.Password != fixedPass)
            {
                return Unauthorized(new { error = "Usuario o contraseña incorrectos." });
            }

            var token = GenerateJwtToken(fixedUser);
            return Ok(new { token });
        }

        // Genera un token JWT válido para el usuario autenticado
        private string GenerateJwtToken(string username)
        {
            // Lee la clave y parámetros desde la configuración
            var jwtKey = _config["Jwt:Key"] ?? throw new InvalidOperationException("JWT Key no configurada");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims: información del usuario dentro del token
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
            };

            // Crea el token JWT con los parámetros configurados
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
