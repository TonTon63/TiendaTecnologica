using Microsoft.AspNetCore.Mvc.Rendering;

namespace TiendaTecnologica.ViewModels
{
    public class SeleccionarProductoViewModel
    {
        public int? IdCategoria { get; set; }
        public int? IdSubcategoria { get; set; }
        public int? IdProducto { get; set; }

        public List<SelectListItem> Categorias { get; set; } = new();
    }
}
