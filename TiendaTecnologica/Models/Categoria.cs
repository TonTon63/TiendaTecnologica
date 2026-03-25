using System.ComponentModel.DataAnnotations;

namespace TiendaTecnologica.Models
{
    public class Categoria
    {
        [Key]
        public int IdCategoria { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string? Descripcion { get; set; }

        public bool Estado { get; set; }
    }
}

