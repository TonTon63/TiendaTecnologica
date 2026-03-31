using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaTecnologica.Models;
using TiendaTecnologica.ViewModels;

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

            var productosRelacionados = new List<Producto>();

            if (producto.IdSubcategoria.HasValue)
            {
                productosRelacionados = await _context.Productos
                    .Include(p => p.Categoria)
                    .Include(p => p.Subcategoria)
                    .Where(p => p.Estado
                                && p.IdProducto != producto.IdProducto
                                && p.IdSubcategoria == producto.IdSubcategoria)
                    .OrderByDescending(p => p.IdProducto)
                    .Take(4)
                    .ToListAsync();
            }

            if (productosRelacionados.Count < 4)
            {
                var idsExistentes = productosRelacionados
                    .Select(p => p.IdProducto)
                    .ToList();

                idsExistentes.Add(producto.IdProducto);

                var faltantes = 4 - productosRelacionados.Count;

                var adicionalesCategoria = await _context.Productos
                    .Include(p => p.Categoria)
                    .Include(p => p.Subcategoria)
                    .Where(p => p.Estado
                                && p.IdCategoria == producto.IdCategoria
                                && !idsExistentes.Contains(p.IdProducto))
                    .OrderByDescending(p => p.IdProducto)
                    .Take(faltantes)
                    .ToListAsync();

                productosRelacionados.AddRange(adicionalesCategoria);
            }

            var viewModel = new ProductoDetalleViewModel
            {
                Producto = producto,
                ProductosRelacionados = productosRelacionados
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Buscar(string q)
        {
            q = q?.Trim() ?? string.Empty;

            var productos = new List<Producto>();

            if (!string.IsNullOrWhiteSpace(q))
            {
                productos = await _context.Productos
                    .Include(p => p.Categoria)
                    .Include(p => p.Subcategoria)
                    .Where(p => p.Estado &&
                           (
                               p.Nombre.Contains(q) ||
                               (p.Marca != null && p.Marca.Contains(q))
                           ))
                    .OrderBy(p => p.Nombre)
                    .ToListAsync();
            }

            ViewBag.Busqueda = q;
            ViewBag.TotalResultados = productos.Count;

            return View(productos);
        }
        [HttpGet]
        public async Task<IActionResult> Sugerencias(string term)
        {
            term = term?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
            {
                return Json(new List<object>());
            }

            var sugerencias = await _context.Productos
                .Where(p => p.Estado &&
                       (p.Nombre.Contains(term) ||
                       (p.Marca != null && p.Marca.Contains(term))))
                .OrderBy(p => p.Nombre)
                .Select(p => new
                {
                    id = p.IdProducto,
                    nombre = p.Nombre,
                    marca = p.Marca ?? "Marca no especificada",
                    imagenUrl = p.ImagenUrl,
                    precio = p.Precio
                })
                .Take(6)
                .ToListAsync();

            return Json(sugerencias);
        }
    }
}