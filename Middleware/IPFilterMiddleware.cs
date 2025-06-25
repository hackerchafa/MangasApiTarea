using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Threading.Tasks;

namespace MangaApi.Middleware
{
public class IPFilterMiddleware
{
        private readonly RequestDelegate _next;
            // Lista actualizada con tu nueva IP
    private readonly string[] _allowedIps = new[]
    {
        "187.184.159.192", // IP anterior
        "187.155.101.200", // ✅ IP nueva permitida
        "127.0.0.1",       // Localhost IPv4
        "::1"              // Localhost IPv6
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
            clientIp = context.Connection.RemoteIpAddress?.ToString();
        }

        if (!_allowedIps.Any(ip => clientIp?.Contains(ip) == true))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync($"Acceso denegado. Tu IP: {clientIp}");
            return;
        }

        await _next(context);
    }
}
}

