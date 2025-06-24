using System.ComponentModel.DataAnnotations.Schema; // Para mapear propiedades a columnas de la base de datos

namespace MangaApi.Models
{
    // Modelo de datos para la entidad Manga
    [Table("mangas")]
    public class Manga
    {
        [Column("id")]
        public int Id { get; set; } // Identificador único del manga

        [Column("titulo")]
        public string Titulo { get; set; } = ""; // Título del manga

        [Column("autor")]
        public string Autor { get; set; } = ""; // Autor del manga

        [Column("genero")]
        public string Genero { get; set; } = ""; // Género del manga
    }
}


