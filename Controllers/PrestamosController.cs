using MangaApi.Models;
using MangaApi.Repositories;
using Microsoft.AspNetCore.Authorization;
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

        // ✅ Solo queda este POST y está protegido
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Prestamo prestamo)
        {
            await _repo.AddPrestamoAsync(prestamo);
            return CreatedAtAction(nameof(GetById), new { id = prestamo.Id }, prestamo);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repo.DeletePrestamoAsync(id);
            return Ok($"Prestamo con ID {id} eliminado.");
        }

        // ✅ PUT usando DTO para evitar modificar Id, MangaId y Cliente
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
