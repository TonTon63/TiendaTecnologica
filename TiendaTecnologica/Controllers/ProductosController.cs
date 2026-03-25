using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaTecnologica.Models;

namespace TiendaTecnologica.Controllers
{
    public class ProductosController : Controller
    {
        private readonly TechStoreContext _context;

        public ProductosController(TechStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Subcategoria)
                .FirstOrDefaultAsync(p => p.IdProducto == id && p.Estado);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }
    }
}