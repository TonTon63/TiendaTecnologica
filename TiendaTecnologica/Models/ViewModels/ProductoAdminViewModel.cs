using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TiendaTecnologica.ViewModels
{
    public class ProductoAdminViewModel
    {
        public int IdProducto { get; set; }

        [Required]
        [StringLength(150)]
        public string Nombre { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        [Required]
        public decimal Precio { get; set; }

        [Required]

        public string? Marca { get; set; }

        [Required]
        public int IdCategoria { get; set; }

        public int? IdSubcategoria { get; set; }

        public bool Estado { get; set; } = true;

        public IFormFile? ImagenArchivo { get; set; }

        public string? ImagenUrlActual { get; set; }

        public List<SelectListItem> Categorias { get; set; } = new();

        public List<SelectListItem> Subcategorias { get; set; } = new();
    }
}