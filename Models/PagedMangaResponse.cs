namespace MangaApi.Models
{
    public class PagedMangaResponse
    {
        public IEnumerable<Manga> Mangas { get; set; } = new List<Manga>();
        public int PaginaActual { get; set; }
        public int TamanoPagina { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
    }
}
