namespace MangaApi.Models
{
    // Modelo para la respuesta paginada de mangas
    public class PagedMangaResponse
    {
        // Lista de mangas en la página
        public IEnumerable<Manga> Mangas { get; set; } = new List<Manga>();
        
        // Número de la página actual
        public int PaginaActual { get; set; }
        
        // Cantidad de elementos por página
        public int TamanoPagina { get; set; }
        
        // Total de mangas encontrados
        public int TotalRegistros { get; set; }
        
        // Total de páginas disponibles
        public int TotalPaginas { get; set; }
    }
}
