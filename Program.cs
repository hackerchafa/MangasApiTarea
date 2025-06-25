using MangaApi.Data;
using MangaApi.Middleware;
using MangaApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ======================= JWT CONFIG =======================
var jwtSettings = builder.Configuration.GetSection("Jwt");
builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
options.TokenValidationParameters = new TokenValidationParameters
{
ValidateIssuer = true,
ValidateAudience = true,
ValidateLifetime = true,
ValidateIssuerSigningKey = true,
ValidIssuer = jwtSettings["Issuer"],
ValidAudience = jwtSettings["Audience"],
IssuerSigningKey = new SymmetricSecurityKey(
Encoding.UTF8.GetBytes(jwtSettings["Key"]
?? throw new InvalidOperationException("JWT Key no configurada"))
)
};
});

builder.Services.AddAuthorization();

// ======================= DB CONTEXT =======================
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!)
);

// ======================= DEPENDENCIAS ======================
builder.Services.AddScoped<IMangaRepository, MangaRepository>();
builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();

builder.Services.AddControllers();

// ======================= SWAGGER CONFIG ====================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
c.SwaggerDoc("v1", new OpenApiInfo { Title = "Manga API", Version = "v1" });
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

// ======================= CORS =============================
builder.Services.AddCors(options =>
{
options.AddPolicy("FrontendPolicy", policy =>
{
policy.WithOrigins("http://localhost:3000")
.AllowAnyHeader()
.AllowAnyMethod();
});
});

// ======================= BUILD APP ========================
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Orden correcto de middlewares
app.UseCors("FrontendPolicy");

// 🚫 IP Filter personalizado
app.UseMiddleware<IPFilterMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();