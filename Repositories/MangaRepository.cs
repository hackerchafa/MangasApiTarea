using MangaApi.Data;
using MangaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaApi.Repositories
{
    public interface IMangaRepository
    {
        Task AddMangaAsync(Manga manga);
        Task<IEnumerable<Manga>> GetMangasAsync();

        // 🚀 Métodos para PUT, DELETE y búsqueda avanzada
        Task<Manga?> GetMangaByIdAsync(int id);
        Task UpdateMangaAsync(Manga manga);
        Task DeleteMangaAsync(int id);

        // 🔍 Búsqueda avanzada (id, título, autor, género)
        Task<IEnumerable<Manga>> BuscarMangasAsync(int? id, string? titulo, string? autor, string? genero);
    }

    public class MangaRepository : IMangaRepository
    {
        private readonly AppDbContext _context;

        public MangaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddMangaAsync(Manga manga)
        {
            _context.Mangas.Add(manga);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Manga>> GetMangasAsync()
        {
            return await _context.Mangas.ToListAsync();
        }

        public async Task<Manga?> GetMangaByIdAsync(int id)
        {
            return await _context.Mangas.FindAsync(id);
        }

        // ✅ CORREGIDO: evitar error 500 al actualizar
        public async Task UpdateMangaAsync(Manga manga)
        {
            var mangaExistente = await _context.Mangas.FindAsync(manga.Id);

            if (mangaExistente != null)
            {
                mangaExistente.Titulo = manga.Titulo;
                mangaExistente.Autor = manga.Autor;
                mangaExistente.Genero = manga.Genero;

                await _context.SaveChangesAsync(); // Sin .Update()
            }
        }

        public async Task DeleteMangaAsync(int id)
        {
            var manga = await _context.Mangas.FindAsync(id);
            if (manga != null)
            {
                _context.Mangas.Remove(manga);
                await _context.SaveChangesAsync();
            }
        }

        // 🔍 Búsqueda avanzada
        public async Task<IEnumerable<Manga>> BuscarMangasAsync(int? id, string? titulo, string? autor, string? genero)
        {
            var query = _context.Mangas.AsQueryable();

            if (id.HasValue)
                query = query.Where(m => m.Id == id);

            if (!string.IsNullOrWhiteSpace(titulo))
                query = query.Where(m => m.Titulo.ToLower().Contains(titulo.ToLower()));

            if (!string.IsNullOrWhiteSpace(autor))
                query = query.Where(m => m.Autor.ToLower().Contains(autor.ToLower()));

            if (!string.IsNullOrWhiteSpace(genero))
                query = query.Where(m => m.Genero.ToLower().Contains(genero.ToLower()));

            return await query.ToListAsync();
        }
    }
}
