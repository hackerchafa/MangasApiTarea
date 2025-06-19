using System.ComponentModel.DataAnnotations.Schema;

namespace MangaApi.Models
{
    [Table("mangas")]
    public class Manga
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("titulo")]
        public string Titulo { get; set; } = "";

        [Column("autor")]
        public string Autor { get; set; } = "";

        [Column("genero")]
        public string Genero { get; set; } = "";
    }
}


