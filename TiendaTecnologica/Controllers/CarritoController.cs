using Microsoft.AspNetCore.Mvc;

namespace TiendaTecnologica.Controllers
{
    public class CarritoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}