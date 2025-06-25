using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace MangaApi.Middleware
{
    public class IPFilterMiddleware
    {
        private readonly RequestDelegate _next;
        // Lista de IPs permitidas
        private readonly string[] _allowedIps = new[]
        {
        "187.155.101.200", // Tu IP pública
        "127.0.0.1",       // localhost IPv4
        "::1"              // localhost IPv6
    };

        public IPFilterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            string? clientIp = null;

            if (!string.IsNullOrEmpty(forwardedFor))
            {
                // Tomar solo la primera IP (la del cliente real)
                clientIp = forwardedFor.Split(',').First().Trim();
            }
            else
            {
                // Si no viene del proxy, usar la IP directa
                clientIp = context.Connection.RemoteIpAddress?.ToString();
            }

            if (!_allowedIps.Contains(clientIp))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsync($"Acceso denegado. Tu IP: {clientIp}");
                return;
            }

            await _next(context);
        }
    }
}