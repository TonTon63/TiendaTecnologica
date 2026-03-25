using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaTecnologica.Models;

namespace TiendaTecnologica.ViewComponents
{
    public class CategoriasMenuViewComponent : ViewComponent
    {
        private readonly TechStoreContext _context;

        public CategoriasMenuViewComponent(TechStoreContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categorias = await _context.Categorias
                .Where(c => c.Estado)
                .OrderBy(c => c.Nombre)
                .ToListAsync();

            return View(categorias);
        }
    }
}