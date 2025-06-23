using MangaApi.Models;
using MangaApi.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MangaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MangaController : ControllerBase
    {
        private readonly IMangaRepository _repo;

        public MangaController(IMangaRepository repo)
        {
            _repo = repo;
        }

        // ✅ Público
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var mangas = await _repo.GetMangasAsync();
            return Ok(mangas);
        }

        // ✅ Agregar un manga nuevo (protegido)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Manga manga)
        {
            await _repo.AddMangaAsync(manga);
            return CreatedAtAction(nameof(Get), new { id = manga.Id }, manga);
        }

        // ✅ Actualizar manga (protegido)
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Manga manga)
        {
            if (id != manga.Id)
                return BadRequest("El ID en la URL no coincide con el del objeto.");

            var existingManga = await _repo.GetMangaByIdAsync(id);
            if (existingManga == null)
                return NotFound("No se encontró el manga.");

            await _repo.UpdateMangaAsync(manga);
            return Ok(manga);
        }

        // ✅ Eliminar manga (protegido)
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var manga = await _repo.GetMangaByIdAsync(id);
            if (manga == null)
                return NotFound("No se encontró el manga.");

            await _repo.DeleteMangaAsync(id);
            return Ok($"Manga con ID {id} eliminado correctamente.");
        }

        // ✅ Buscar mangas (público)
        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar(
            [FromQuery] int? id,
            [FromQuery] string? titulo,
            [FromQuery] string? autor,
            [FromQuery] string? genero)
        {
            var resultados = await _repo.BuscarMangasAsync(id, titulo, autor, genero);
            return Ok(resultados);
        }

        // ✅ Obtener mangas con paginación (público)
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var todos = await _repo.GetMangasAsync();

            var totalRegistros = todos.Count();
            var totalPaginas = (int)Math.Ceiling(totalRegistros / (double)pageSize);

            var mangasPaginados = todos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var respuesta = new PagedMangaResponse
            {
                Mangas = mangasPaginados,
                PaginaActual = pageNumber,
                TamanoPagina = pageSize,
                TotalRegistros = totalRegistros,
                TotalPaginas = totalPaginas
            };

            return Ok(respuesta);
        }
    }
}
