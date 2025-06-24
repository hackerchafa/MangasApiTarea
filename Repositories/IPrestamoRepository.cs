using MangaApi.Models; // Modelo de préstamo

namespace MangaApi.Repositories
{
    // Interfaz para operaciones CRUD de préstamos
    public interface IPrestamoRepository
    {
        /// <summary>
        /// Obtener todos los préstamos.
        /// </summary>
        Task<IEnumerable<Prestamo>> GetPrestamosAsync(); 

        /// <summary>
        /// Obtener préstamo por ID.
        /// </summary>
        Task<Prestamo?> GetPrestamoByIdAsync(int id); 

        /// <summary>
        /// Agregar préstamo.
        /// </summary>
        Task AddPrestamoAsync(Prestamo prestamo); 

        /// <summary>
        /// Eliminar préstamo.
        /// </summary>
        Task DeletePrestamoAsync(int id); 
        
        /// <summary>
        /// Actualizar préstamo.
        /// </summary>
        Task UpdatePrestamoAsync(Prestamo prestamo); 
    }
}


