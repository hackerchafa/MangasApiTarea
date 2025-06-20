using MangaApi.Data;
using MangaApi.Models;
using Microsoft.EntityFrameworkCore;

namespace MangaApi.Repositories
{
    public class PrestamoRepository : IPrestamoRepository
    {
        private readonly AppDbContext _context;

        public PrestamoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Prestamo>> GetPrestamosAsync()
        {
            return await _context.Prestamos.ToListAsync();
        }

        public async Task<Prestamo?> GetPrestamoByIdAsync(int id)
        {
            return await _context.Prestamos.FindAsync(id);
        }

        public async Task AddPrestamoAsync(Prestamo prestamo)
        {
            _context.Prestamos.Add(prestamo);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePrestamoAsync(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
                await _context.SaveChangesAsync();
            }
        }

        // ✅ Método para actualizar préstamo
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
