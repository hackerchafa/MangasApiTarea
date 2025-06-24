using MangaApi.Models; // Modelos de datos
using MangaApi.Repositories; // Acceso a los repositorios
using Microsoft.AspNetCore.Authorization; // Para proteger endpoints con JWT
using Microsoft.AspNetCore.Mvc; // Funcionalidad de controladores Web API

namespace MangaApi.Controllers
{
    // Define la ruta base para este controlador: api/manga
    [Route("api/[controller]")]
    [ApiController] // Indica que es un controlador de API
    public class MangaController : ControllerBase
    {
        private readonly IMangaRepository _repo; // Repositorio de mangas

        // Inyección de dependencias del repositorio
        public MangaController(IMangaRepository repo)
        {
            _repo = repo;
        }

        // ================== ENDPOINTS =====================

        // GET: api/manga
        // Obtiene todos los mangas (público)
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var mangas = await _repo.GetMangasAsync();
            return Ok(mangas);
        }

        // POST: api/manga
        // Agrega un nuevo manga (protegido con JWT)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Manga manga)
        {
            await _repo.AddMangaAsync(manga);
            return CreatedAtAction(nameof(Get), new { id = manga.Id }, manga);
        }

        // PUT: api/manga/{id}
        // Actualiza un manga existente (protegido con JWT)
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

        // DELETE: api/manga/{id}
        // Elimina un manga (protegido con JWT)
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var manga = await _repo.GetMangaByIdAsync(id);
            if (manga == null)
                return NotFound("No se encontró el manga.");

            await _repo.DeleteMangaAsync(id);
            return Ok($"Manga con ID {id} eliminado.");
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
