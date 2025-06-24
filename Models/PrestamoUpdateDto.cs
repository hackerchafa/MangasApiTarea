namespace MangaApi.Models
{
    // DTO para actualizar solo ciertos campos de un préstamo
    public class PrestamoUpdateDto
    {
        public DateTime FechaPrestamo { get; set; } // Nueva fecha de préstamo
        public DateTime FechaDevolucionEsperada { get; set; } // Nueva fecha esperada de devolución
        public DateTime? FechaDevolucionReal { get; set; } // Nueva fecha real de devolución (opcional)
    }
}
