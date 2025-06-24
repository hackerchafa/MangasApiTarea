using Microsoft.EntityFrameworkCore; // Entity Framework Core
using MangaApi.Models; // Modelos de datos

namespace MangaApi.Data
{
    // Contexto de base de datos para la aplicación
    public class AppDbContext : DbContext
    {
        // Constructor que recibe las opciones de configuración
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Tabla de mangas
        public DbSet<Manga> Mangas { get; set; }
        // Tabla de préstamos
        public DbSet<Prestamo> Prestamos { get; set; } // NUEVA TABLA AGREGADA
    }
}
