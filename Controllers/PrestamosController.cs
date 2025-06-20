using MangaApi.Models;
using MangaApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MangaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrestamosController : ControllerBase
    {
        private readonly IPrestamoRepository _repo;

        public PrestamosController(IPrestamoRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var prestamos = await _repo.GetPrestamosAsync();
            return Ok(prestamos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var prestamo = await _repo.GetPrestamoByIdAsync(id);
            if (prestamo == null) return NotFound();
            return Ok(prestamo);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Prestamo prestamo)
        {
            await _repo.AddPrestamoAsync(prestamo);
            return CreatedAtAction(nameof(GetById), new { id = prestamo.Id }, prestamo);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeletePrestamoAsync(id);
            return Ok($"Prestamo con ID {id} eliminado.");
        }

        // ✅ Método PUT para actualizar préstamo
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Prestamo prestamo)
        {
            if (id != prestamo.Id)
                return BadRequest("El ID en la URL no coincide con el del objeto.");

            await _repo.UpdatePrestamoAsync(prestamo);
            return Ok(prestamo);
        }
    }
}

