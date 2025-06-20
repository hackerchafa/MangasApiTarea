using MangaApi.Models;

namespace MangaApi.Repositories
{
    public interface IPrestamoRepository
    {
        Task<IEnumerable<Prestamo>> GetPrestamosAsync();
        Task<Prestamo?> GetPrestamoByIdAsync(int id);
        Task AddPrestamoAsync(Prestamo prestamo);
        Task DeletePrestamoAsync(int id);
        
        // ✅ Método para actualizar préstamo
        Task UpdatePrestamoAsync(Prestamo prestamo);
    }
}


