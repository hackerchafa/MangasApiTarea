using MangaApi.Models; // Modelos de datos
using MangaApi.Repositories; // Acceso a los repositorios
using Microsoft.AspNetCore.Authorization; // Para proteger endpoints con JWT
using Microsoft.AspNetCore.Mvc; // Funcionalidad de controladores Web API

namespace MangaApi.Controllers
{
    // Controlador para gestionar préstamos de mangas
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController : ControllerBase
    {
        private readonly IPrestamoRepository _repo; // Repositorio de préstamos

        // Inyección de dependencias del repositorio
        public PrestamosController(IPrestamoRepository repo)
        {
            _repo = repo;
        }

        // ================== ENDPOINTS =====================

        // GET: api/prestamos
        // Obtiene todos los préstamos (público)
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var prestamos = await _repo.GetPrestamosAsync();
            return Ok(prestamos);
        }

        // GET: api/prestamos/{id}
        // Obtiene un préstamo por ID (público)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var prestamo = await _repo.GetPrestamoByIdAsync(id);
            if (prestamo == null) return NotFound();
            return Ok(prestamo);
        }

        // POST: api/prestamos
        // Agrega un nuevo préstamo (protegido con JWT)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Prestamo prestamo)
        {
            await _repo.AddPrestamoAsync(prestamo);
            return CreatedAtAction(nameof(GetById), new { id = prestamo.Id }, prestamo);
        }

        // DELETE: api/prestamos/{id}
        // Elimina un préstamo (protegido con JWT)
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeletePrestamoAsync(id);
            return Ok($"Prestamo con ID {id} eliminado.");
        }

        // PUT: api/prestamos/{id}
        // Actualiza un préstamo existente (protegido con JWT)
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PrestamoUpdateDto dto)
        {
            var prestamoExistente = await _repo.GetPrestamoByIdAsync(id);
            if (prestamoExistente == null)
                return NotFound($"No se encontró el préstamo con ID {id}.");

            // Solo se actualizan los campos permitidos
            prestamoExistente.FechaPrestamo = dto.FechaPrestamo;
            prestamoExistente.FechaDevolucionEsperada = dto.FechaDevolucionEsperada;
            prestamoExistente.FechaDevolucionReal = dto.FechaDevolucionReal;

            await _repo.UpdatePrestamoAsync(prestamoExistente);
            return Ok(prestamoExistente);
        }
    }
}
