namespace MangaApi.Models
{
    // Modelo para la solicitud de login
    public class LoginRequest
    {
        public required string Username { get; set; } // Nombre de usuario
        public required string Password { get; set; } // Contraseña
    }
}
