using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TiendaTecnologica.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; }

        public string? Descripcion { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        public int Stock { get; set; }

        [StringLength(100)]
        public string? Marca { get; set; }

        [StringLength(255)]
        public string? ImagenUrl { get; set; }

        public int IdCategoria { get; set; }

        public int? IdSubcategoria { get; set; }

        public bool Estado { get; set; }

        [ForeignKey("IdCategoria")]
        public Categoria? Categoria { get; set; }

        [ForeignKey("IdSubcategoria")]
        public Subcategoria? Subcategoria { get; set; }
    }
}