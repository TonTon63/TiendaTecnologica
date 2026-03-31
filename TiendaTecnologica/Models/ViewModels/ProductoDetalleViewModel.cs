using TiendaTecnologica.Models;

namespace TiendaTecnologica.ViewModels
{
    public class ProductoDetalleViewModel
    {
        public Producto Producto { get; set; } = null!;
        public List<Producto> ProductosRelacionados { get; set; } = new();
    }
}