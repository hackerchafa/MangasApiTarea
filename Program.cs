// ======================= IMPORTACIONES =========================
// Espacios de nombres para funcionalidades clave de la WebAPI
using MangaApi.Data; // Acceso a la base de datos
using MangaApi.Repositories; // Interfaces y clases repositorio
using Microsoft.AspNetCore.Authentication.JwtBearer; // JWT Bearer Auth
using Microsoft.EntityFrameworkCore; // Soporte para Entity Framework Core
using Microsoft.IdentityModel.Tokens; // Validación de JWT
using Microsoft.OpenApi.Models; // Documentación Swagger
using System.Text; // Codificación de claves JWT
using MangaApi.Middleware; // Middleware personalizado para IP

// ======================= CREAR BUILDER =========================
var builder = WebApplication.CreateBuilder(args);

// ======================= CONFIG JWT ===========================
// Sección "Jwt" del appsettings.json
var jwtSettings = builder.Configuration.GetSection("Jwt");

// Configura el esquema de autenticación como JWT Bearer
builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
// Parámetros para validar los tokens JWT
options.TokenValidationParameters = new TokenValidationParameters
{
ValidateIssuer = true, // Valida el emisor
ValidateAudience = true, // Valida el público
ValidateLifetime = true, // Valida que no haya expirado
ValidateIssuerSigningKey = true, // Valida la firma
ValidIssuer = jwtSettings["Issuer"], // Emisor válido
ValidAudience = jwtSettings["Audience"], // Audiencia válida
IssuerSigningKey = new SymmetricSecurityKey(
Encoding.UTF8.GetBytes(jwtSettings["Key"]
?? throw new InvalidOperationException("JWT Key no configurada"))
)
};
});

// ======================= AUTORIZACIÓN =========================
// Habilita sistema de autorización por roles o políticas
builder.Services.AddAuthorization();

// ======================= BASE DE DATOS ========================
// Configura EF Core para usar MySQL con la cadena de conexión
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!)
);

// ======================= REPOSITORIOS =========================
// Inyección de dependencias para acceso a datos
builder.Services.AddScoped<IMangaRepository, MangaRepository>();
builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();

// ======================= CONTROLADORES ========================
// Habilita el uso de controladores (API REST)
builder.Services.AddControllers();

// ======================= SWAGGER ==============================
// Documentación interactiva de la API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
c.SwaggerDoc("v1", new OpenApiInfo { Title = "Manga API", Version = "v1" });
// Configuración para usar JWT en Swagger
c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
{
    In = ParameterLocation.Header,
    Description = "Ingresa el token como: Bearer {token}",
    Name = "Authorization",
    Type = SecuritySchemeType.ApiKey,
    Scheme = "Bearer"
});

c.AddSecurityRequirement(new OpenApiSecurityRequirement
{
    {
        new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
            }
        },
        Array.Empty<string>()
    }
});
});

// ======================= CORS ================================
// Permitir solicitudes desde el frontend (React en localhost:3000)
builder.Services.AddCors(options =>
{
options.AddPolicy("FrontendPolicy", policy =>
{
policy.WithOrigins("http://localhost:3000") // URL de tu frontend
.AllowAnyHeader() // Permite cualquier encabezado
.AllowAnyMethod(); // Permite GET, POST, etc.
// Si usarás cookies o JWT desde el frontend, agrega: .AllowCredentials()
});
});

// ======================= CONSTRUIR APP ========================
var app = builder.Build();

// ======================= MIDDLEWARES =========================

// Habilita Swagger para probar la API en el navegador
app.UseSwagger();
app.UseSwaggerUI(c =>
{
c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manga API v1");
});

// Redirige automáticamente HTTP a HTTPS
app.UseHttpsRedirection();

// === 🔥 IMPORTANTE: CORS DEBE IR ANTES DE AUTH ===
// Permite peticiones del frontend (según la política definida)
app.UseCors("FrontendPolicy");

// ================== MIDDLEWARE DE FILTRO IP ==================
// 🔐 Solo permite el acceso desde la IP pública autorizada (tú y nadie más)
app.UseMiddleware<IPFilterMiddleware>();

// Habilita autenticación y autorización JWT
app.UseAuthentication(); // Verifica el token JWT (si se requiere)
app.UseAuthorization(); // Aplica reglas de acceso a endpoints

// Mapea los controladores con sus rutas definidas
app.MapControllers();

// Inicia la ejecución de la API
app.Run();