using MangaApi.Data;
using MangaApi.Middleware;
using MangaApi.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =================== JWT CONFIG ==========================
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
?? throw new InvalidOperationException("JWT Key no configurada")))
};
});

builder.Services.AddAuthorization();

// ================ DB CONNECTION ===========================
builder.Services.AddDbContext<AppDbContext>(options =>
options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")!)
);

// ================ DEPENDENCY INJECTION ====================
builder.Services.AddScoped<IMangaRepository, MangaRepository>();
builder.Services.AddScoped<IPrestamoRepository, PrestamoRepository>();

// ================ CONTROLLERS & SWAGGER ===================
builder.Services.AddControllers();

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

// ================ CORS ======================================
builder.Services.AddCors(options =>
{
options.AddPolicy("FrontendPolicy", policy =>
{
policy.WithOrigins("http://localhost:3000") // o el dominio real
.AllowAnyHeader()
.AllowAnyMethod();
});
});

var app = builder.Build();

// ================ DESARROLLO ================================
if (app.Environment.IsDevelopment())
{
app.UseDeveloperExceptionPage(); // muestra errores 500 detallados
}

// ================ MIDDLEWARES ==============================
app.UseMiddleware<IPFilterMiddleware>(); // IP Filtering personalizado
app.UseSwagger();
app.UseSwaggerUI(c =>
{
c.SwaggerEndpoint("/swagger/v1/swagger.json", "Manga API v1");
});

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();