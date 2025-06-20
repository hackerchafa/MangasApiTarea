namespace MangaApi.Models
{
    public class PrestamoUpdateDto
    {
        public DateTime FechaPrestamo { get; set; }
        public DateTime FechaDevolucionEsperada { get; set; }
        public DateTime? FechaDevolucionReal { get; set; }
    }
}
