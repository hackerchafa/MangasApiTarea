using MangaApi.Data; // Acceso a la base de datos
using MangaApi.Models; // Modelos de datos
using Microsoft.EntityFrameworkCore; // Entity Framework Core

namespace MangaApi.Repositories
{
    // Interfaz para operaciones CRUD y búsqueda avanzada de mangas
    public interface IMangaRepository
    {
        Task AddMangaAsync(Manga manga); // Agregar manga
        Task<IEnumerable<Manga>> GetMangasAsync(); // Obtener todos los mangas

        // 🚀 Métodos para PUT, DELETE y búsqueda avanzada
        Task<Manga?> GetMangaByIdAsync(int id); // Obtener manga por ID
        Task UpdateMangaAsync(Manga manga); // Actualizar manga
        Task DeleteMangaAsync(int id); // Eliminar manga

        // 🔍 Búsqueda avanzada (id, título, autor, género)
        Task<IEnumerable<Manga>> BuscarMangasAsync(int? id, string? titulo, string? autor, string? genero); // Búsqueda avanzada
    }

    // Implementación de IMangaRepository para operaciones CRUD de mangas
    public class MangaRepository : IMangaRepository
    {
        private readonly AppDbContext _context; // Contexto de base de datos

        public MangaRepository(AppDbContext context)
        {
            _context = context;
        }

        // Agrega un nuevo manga
        public async Task AddMangaAsync(Manga manga)
        {
            _context.Mangas.Add(manga);
            await _context.SaveChangesAsync();
        }

        // Obtiene todos los mangas
        public async Task<IEnumerable<Manga>> GetMangasAsync()
        {
            return await _context.Mangas.ToListAsync();
        }

        // Obtiene un manga por ID
        public async Task<Manga?> GetMangaByIdAsync(int id)
        {
            return await _context.Mangas.FindAsync(id);
        }

        // Actualiza los datos de un manga existente
        public async Task UpdateMangaAsync(Manga manga)
        {
            var mangaExistente = await _context.Mangas.FindAsync(manga.Id);

            if (mangaExistente != null)
            {
                mangaExistente.Titulo = manga.Titulo;
                mangaExistente.Autor = manga.Autor;
                mangaExistente.Genero = manga.Genero;

                await _context.SaveChangesAsync(); // Guarda los cambios
            }
        }

        // Elimina un manga por ID
        public async Task DeleteMangaAsync(int id)
        {
            var manga = await _context.Mangas.FindAsync(id);
            if (manga != null)
            {
                _context.Mangas.Remove(manga);
                await _context.SaveChangesAsync();
            }
        }

        // Búsqueda avanzada de mangas por diferentes campos
        public async Task<IEnumerable<Manga>> BuscarMangasAsync(int? id, string? titulo, string? autor, string? genero)
        {
            var query = _context.Mangas.AsQueryable();

            if (id.HasValue)
                query = query.Where(m => m.Id == id);

            if (!string.IsNullOrEmpty(titulo))
                query = query.Where(m => m.Titulo.Contains(titulo));

            if (!string.IsNullOrEmpty(autor))
                query = query.Where(m => m.Autor.Contains(autor));

            if (!string.IsNullOrEmpty(genero))
                query = query.Where(m => m.Genero.Contains(genero));

            return await query.ToListAsync();
        }
    }
}
