using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace MangaApi.Middleware
{
    public class IPFilterMiddleware
    {
        private readonly RequestDelegate _next;

        // Lista de IPs permitidas (tu IP pública y localhost)
        private readonly string[] _allowedIps = new[]
        {
            "187.184.159.192", // ✅ Tu IP pública
            "127.0.0.1",       // localhost IPv4
            "::1"              // localhost IPv6
        };

        public IPFilterMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            string? clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (string.IsNullOrEmpty(clientIp))
            {
                // Si no viene del proxy, usa la IP directa
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
