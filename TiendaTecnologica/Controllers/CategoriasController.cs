using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaTecnologica.Models;
using TiendaTecnologica.ViewModels;

namespace TiendaTecnologica.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly TechStoreContext _context;

        public CategoriasController(TechStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var categoria = await _context.Categorias
                .FirstOrDefaultAsync(c => c.IdCategoria == id && c.Estado);

            if (categoria == null)
            {
                return NotFound();
            }

            var subcategorias = await _context.Subcategorias
                .Where(s => s.IdCategoria == id && s.Estado)
                .ToListAsync();

            var productos = await _context.Productos
                .Include(p => p.Subcategoria)
                .Where(p => p.IdCategoria == id && p.Estado)
                .ToListAsync();

            var vm = new CategoriaDetalleViewModel
            {
                Categoria = categoria,
                Subcategorias = subcategorias,
                Productos = productos
            };

            return View(vm);
        }
    }
}