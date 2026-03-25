using TiendaTecnologica.Models;

namespace TiendaTecnologica.ViewModels
{
    public class CategoriaDetalleViewModel
    {
        public Categoria? Categoria { get; set; }
        public List<Subcategoria> Subcategorias { get; set; } = new();
        public List<Producto> Productos { get; set; } = new();
    }
}