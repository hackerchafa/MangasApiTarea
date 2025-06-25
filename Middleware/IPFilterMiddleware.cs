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
        "187.184.159.192",
        "187.155.101.200",
        "127.0.0.1",
        "::1"
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

        if (!_allowedIps.Any(ip => clientIp != null && clientIp.Contains(ip)))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync($"Acceso denegado. Tu IP: {clientIp}");
            return;
        }

        await _next(context);
    }
}
}