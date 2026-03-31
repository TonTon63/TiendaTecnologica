using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TiendaTecnologica.Models;
using TiendaTecnologica.ViewModels;


namespace TiendaTecnologica.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministradorController : Controller
    {
        private readonly TechStoreContext _context;

        public AdministradorController(TechStoreContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        // =========================
        // AGREGAR PRODUCTO
        // =========================
        [HttpGet]
        public IActionResult AgregarProducto()
        {
            var model = new ProductoAdminViewModel
            {
                Categorias = _context.Categorias
                    .Where(c => c.Estado)
                    .Select(c => new SelectListItem
                    {
                        Value = c.IdCategoria.ToString(),
                        Text = c.Nombre
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AgregarProducto(ProductoAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categorias = _context.Categorias
                    .Where(c => c.Estado)
                    .Select(c => new SelectListItem
                    {
                        Value = c.IdCategoria.ToString(),
                        Text = c.Nombre
                    })
                    .ToList();

                return View(model);
            }

            string? rutaImagen = null;

            if (model.ImagenArchivo != null && model.ImagenArchivo.Length > 0)
            {
                var carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "productos");

                if (!Directory.Exists(carpetaDestino))
                {
                    Directory.CreateDirectory(carpetaDestino);
                }

                var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(model.ImagenArchivo.FileName)}";
                var rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await model.ImagenArchivo.CopyToAsync(stream);
                }

                rutaImagen = $"/images/productos/{nombreArchivo}";
            }

            var producto = new Producto
            {
                Nombre = model.Nombre,
                Descripcion = model.Descripcion,
                Precio = model.Precio,
                Marca = model.Marca,
                IdCategoria = model.IdCategoria,
                IdSubcategoria = model.IdSubcategoria,
                Estado = model.Estado,
                ImagenUrl = rutaImagen
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerSubcategorias(int idCategoria)
        {
            var subcategorias = await _context.Subcategorias
                .Where(s => s.Estado && s.IdCategoria == idCategoria)
                .Select(s => new
                {
                    idSubcategoria = s.IdSubcategoria,
                    nombre = s.Nombre
                })
                .ToListAsync();

            return Json(subcategorias);
        }
        // =========================
        // EDITAR PRODUCTO
        // =========================
        [HttpGet]
        public async Task<IActionResult> EditarProducto(int? id)
        {
            if (id == null)
                return NotFound();

            var producto = await _context.Productos
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto == null)
                return NotFound();

            var model = new ProductoAdminViewModel
            {
                IdProducto = producto.IdProducto,
                Nombre = producto.Nombre,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                Marca = producto.Marca,
                IdCategoria = producto.IdCategoria,
                IdSubcategoria = producto.IdSubcategoria,
                Estado = producto.Estado,
                ImagenUrlActual = producto.ImagenUrl,
                Categorias = _context.Categorias
                    .Where(c => c.Estado)
                    .Select(c => new SelectListItem
                    {
                        Value = c.IdCategoria.ToString(),
                        Text = c.Nombre
                    }).ToList(),

                Subcategorias = _context.Subcategorias
                    .Where(s => s.Estado && s.IdCategoria == producto.IdCategoria)
                    .Select(s => new SelectListItem
                    {
                        Value = s.IdSubcategoria.ToString(),
                        Text = s.Nombre
                    }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditarProducto(ProductoAdminViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Categorias = _context.Categorias
                    .Where(c => c.Estado)
                    .Select(c => new SelectListItem
                    {
                        Value = c.IdCategoria.ToString(),
                        Text = c.Nombre
                    }).ToList();

                model.Subcategorias = _context.Subcategorias
                    .Where(s => s.Estado && s.IdCategoria == model.IdCategoria)
                    .Select(s => new SelectListItem
                    {
                        Value = s.IdSubcategoria.ToString(),
                        Text = s.Nombre
                    }).ToList();

                return View(model);
            }

            var producto = await _context.Productos.FindAsync(model.IdProducto);

            if (producto == null)
                return NotFound();

            producto.Nombre = model.Nombre;
            producto.Descripcion = model.Descripcion;
            producto.Precio = model.Precio;
            producto.Marca = model.Marca;
            producto.IdCategoria = model.IdCategoria;
            producto.IdSubcategoria = model.IdSubcategoria;
            producto.Estado = model.Estado;

            if (model.ImagenArchivo != null && model.ImagenArchivo.Length > 0)
            {
                var carpetaDestino = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "productos");

                if (!Directory.Exists(carpetaDestino))
                {
                    Directory.CreateDirectory(carpetaDestino);
                }

                var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(model.ImagenArchivo.FileName)}";
                var rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await model.ImagenArchivo.CopyToAsync(stream);
                }

                producto.ImagenUrl = $"/images/productos/{nombreArchivo}";
            }

            _context.Update(producto);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult SeleccionarProductoEditar()
        {
            var model = new SeleccionarProductoViewModel
            {
                Categorias = _context.Categorias
                    .Where(c => c.Estado)
                    .Select(c => new SelectListItem
                    {
                        Value = c.IdCategoria.ToString(),
                        Text = c.Nombre
                    })
                    .ToList()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> ObtenerProductos(int idCategoria, int? idSubcategoria)
        {
            var query = _context.Productos
                .Where(p => p.IdCategoria == idCategoria);

            if (idSubcategoria.HasValue)
            {
                query = query.Where(p => p.IdSubcategoria == idSubcategoria.Value);
            }

            var productos = await query
                .Select(p => new
                {
                    idProducto = p.IdProducto,
                    nombre = p.Nombre
                })
                .ToListAsync();

            return Json(productos);
        }

        // =========================
        // ELIMINAR / OCULTAR PRODUCTO
        // =========================

        [HttpGet]
        public async Task<IActionResult> EliminarProducto(int? id)
        {
            if (id == null)
                return NotFound();

            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Subcategoria)
                .FirstOrDefaultAsync(p => p.IdProducto == id);

            if (producto == null)
                return NotFound();

            return View(producto);
        }

        [HttpPost, ActionName("EliminarProducto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarProductoConfirmado(int id)
        {
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
                return NotFound();

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();

            // MENSAJE DE CONFIRMACIÓN
            TempData["Success"] = "El producto fue eliminado correctamente.";

            return RedirectToAction(nameof(ListaProductos)); // o Index
        }

        [HttpGet]
        public IActionResult SeleccionarProductoEliminar()
        {
            var model = new SeleccionarProductoViewModel
            {
                Categorias = _context.Categorias
                    .Where(c => c.Estado)
                    .Select(c => new SelectListItem
                    {
                        Value = c.IdCategoria.ToString(),
                        Text = c.Nombre
                    })
                    .ToList()
            };

            return View(model);
        }

        private void CargarCombos()
        {
            ViewBag.Categorias = new SelectList(_context.Categorias.Where(c => c.Estado).ToList(), "IdCategoria", "Nombre");
            ViewBag.Subcategorias = new SelectList(_context.Subcategorias.Where(s => s.Estado).ToList(), "IdSubcategoria", "Nombre");
        }

        [HttpGet]
        public async Task<IActionResult> ListaProductos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Subcategoria)
                .OrderByDescending(p => p.IdProducto)
                .ToListAsync();

            return View(productos);
        }

    }
}