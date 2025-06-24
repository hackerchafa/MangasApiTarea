using MangaApi.Models;
using MangaApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Protege todos los endpoints de este controlador
    public class PrestamosController : ControllerBase
    {
        private readonly IPrestamoRepository _repo;

        public PrestamosController(IPrestamoRepository repo)
        {
            _repo = repo;
        }

        // GET: api/prestamos
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var prestamos = await _repo.GetPrestamosAsync();
            return Ok(prestamos);
        }

        // GET: api/prestamos/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var prestamo = await _repo.GetPrestamoByIdAsync(id);
            if (prestamo == null) return NotFound();
            return Ok(prestamo);
        }

        // POST: api/prestamos
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Prestamo prestamo)
        {
            await _repo.AddPrestamoAsync(prestamo);
            return CreatedAtAction(nameof(GetById), new { id = prestamo.Id }, prestamo);
        }

        // PUT: api/prestamos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] PrestamoUpdateDto dto)
        {
            var prestamoExistente = await _repo.GetPrestamoByIdAsync(id);
            if (prestamoExistente == null)
                return NotFound($"No se encontró el préstamo con ID {id}.");

            prestamoExistente.FechaPrestamo = dto.FechaPrestamo;
            prestamoExistente.FechaDevolucionEsperada = dto.FechaDevolucionEsperada;
            prestamoExistente.FechaDevolucionReal = dto.FechaDevolucionReal;

            await _repo.UpdatePrestamoAsync(prestamoExistente);
            return Ok(prestamoExistente);
        }

        // DELETE: api/prestamos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeletePrestamoAsync(id);
            return Ok($"Prestamo con ID {id} eliminado.");
        }
    }
}
