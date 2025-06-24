using MangaApi.Data; // Acceso a la base de datos
using MangaApi.Models; // Modelos de datos
using Microsoft.EntityFrameworkCore; // Entity Framework Core

namespace MangaApi.Repositories
{
    // Implementación de IPrestamoRepository para operaciones CRUD de préstamos
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly AppDbContext _context; // Contexto de base de datos

        public PrestamoRepository(AppDbContext context)
        {
            _context = context;
        }

        // Obtiene todos los préstamos
        public async Task<IEnumerable<Prestamo>> GetPrestamosAsync()
        {
            return await _context.Prestamos.ToListAsync();
        }

        // Obtiene un préstamo por ID
        public async Task<Prestamo?> GetPrestamoByIdAsync(int id)
        {
            return await _context.Prestamos.FindAsync(id);
        }

        // Agrega un nuevo préstamo
        public async Task AddPrestamoAsync(Prestamo prestamo)
        {
            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();
        }

        // Elimina un préstamo por ID
        public async Task DeletePrestamoAsync(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
                await _context.SaveChangesAsync();
            }
        }

        // Actualiza los datos de un préstamo existente
        public async Task UpdatePrestamoAsync(Prestamo prestamo)
        {
            var prestamoExistente = await _context.Prestamos.FindAsync(prestamo.Id);
            if (prestamoExistente != null)
            {
                prestamoExistente.MangaId = prestamo.MangaId;
                prestamoExistente.Cliente = prestamo.Cliente;
                prestamoExistente.FechaPrestamo = prestamo.FechaPrestamo;
                prestamoExistente.FechaDevolucionEsperada = prestamo.FechaDevolucionEsperada;
                prestamoExistente.FechaDevolucionReal = prestamo.FechaDevolucionReal;

                await _context.SaveChangesAsync();
            }
        }
    }
}
