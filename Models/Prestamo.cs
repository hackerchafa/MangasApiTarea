using System.ComponentModel.DataAnnotations.Schema;

namespace MangaApi.Models
{
    [Table("prestamos")]
    public class Prestamo
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("mangaid")]
        public int MangaId { get; set; }

        [Column("cliente")]
        public string Cliente { get; set; } = "";

        [Column("fechaprestamo")]
        public DateTime FechaPrestamo { get; set; }

        [Column("fechaDevolucionEsperada")]
        public DateTime FechaDevolucionEsperada { get; set; }

        [Column("fechaDevolucionReal")]
        public DateTime? FechaDevolucionReal { get; set; } // null si no se ha devuelto
    }
}
