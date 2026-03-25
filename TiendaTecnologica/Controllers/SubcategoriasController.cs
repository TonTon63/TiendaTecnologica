using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaTecnologica.Models;

namespace TiendaTecnologica.Controllers
{
    public class SubcategoriasController : Controller
    {
        private readonly TechStoreContext _context;

        public SubcategoriasController(TechStoreContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var subcategoria = await _context.Subcategorias
                .Include(s => s.Categoria)
                .FirstOrDefaultAsync(s => s.IdSubcategoria == id && s.Estado);

            if (subcategoria == null)
            {
                return NotFound();
            }

            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Subcategoria)
                .Where(p => p.IdSubcategoria == id && p.Estado)
                .ToListAsync();

            ViewBag.Subcategoria = subcategoria;
            return View(productos);
        }
    }
}