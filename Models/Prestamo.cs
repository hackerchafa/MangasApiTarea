using System.ComponentModel.DataAnnotations.Schema; // Para mapear propiedades a columnas de la base de datos

namespace MangaApi.Models
{
    // Modelo de datos para la entidad Prestamo
    [Table("prestamos")]
    public class Prestamo
    {
        [Column("id")]
        public int Id { get; set; } // Identificador único del préstamo

        [Column("mangaid")]
        public int MangaId { get; set; } // ID del manga prestado

        [Column("cliente")]
        public string Cliente { get; set; } = ""; // Nombre del cliente

        [Column("fechaprestamo")]
        public DateTime FechaPrestamo { get; set; } // Fecha en que se realizó el préstamo

        [Column("fechaDevolucionEsperada")]
        public DateTime FechaDevolucionEsperada { get; set; } // Fecha esperada de devolución

        [Column("fechaDevolucionReal")]
        public DateTime? FechaDevolucionReal { get; set; } // Fecha real de devolución (puede ser null)
    }
}
