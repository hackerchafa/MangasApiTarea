using Microsoft.EntityFrameworkCore;
using MangaApi.Models;

namespace MangaApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Manga> Mangas { get; set; }
    }
}
