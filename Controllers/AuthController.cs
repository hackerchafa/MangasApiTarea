using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MangaApi.Controllers
{
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
        private readonly IConfiguration _config;
    public AuthController(IConfiguration config)
    {
        _config = config;
    }

    public class LoginRequest
    {
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (request.Username == "admin" && request.Password == "1234")
        {
            try
            {
                var token = GenerateJwtToken(request.Username);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error generando token: {ex.Message}");
            }
        }

        return Unauthorized("Credenciales incorrectas");
    }

    private string GenerateJwtToken(string username)
    {
        var jwtSection = _config.GetSection("Jwt");

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
            jwtSection["Key"] ?? throw new InvalidOperationException("JWT Key faltante")));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: jwtSection["Issuer"],
            audience: jwtSection["Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
}