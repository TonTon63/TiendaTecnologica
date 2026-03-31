using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using TiendaTecnologica.Models;

namespace TiendaTecnologica.Controllers
{
    public class HomeController : Controller
    {
        private readonly TechStoreContext _context;
        private readonly EmailSettings _emailSettings;

        public HomeController(
            TechStoreContext context,
            IOptions<EmailSettings> emailSettings)
        {
            _context = context;
            _emailSettings = emailSettings.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SobreNosotros()
        {
            return View();
        }

        public IActionResult EnviosDevoluciones()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Contactenos()
        {
            return View(new Contacto());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnviarContacto(Contacto model)
        {
            if (!ModelState.IsValid)
            {
                return View("Contactenos", model);
            }

            try
            {
                _context.Contactos.Add(model);
                await _context.SaveChangesAsync();

                if (string.IsNullOrWhiteSpace(_emailSettings.FromEmail))
                {
                    ModelState.AddModelError("", "FromEmail está vacío.");
                    return View("Contactenos", model);
                }

                if (string.IsNullOrWhiteSpace(_emailSettings.ToEmail))
                {
                    ModelState.AddModelError("", "ToEmail está vacío.");
                    return View("Contactenos", model);
                }

                if (string.IsNullOrWhiteSpace(_emailSettings.Username))
                {
                    ModelState.AddModelError("", "Username está vacío.");
                    return View("Contactenos", model);
                }

                if (string.IsNullOrWhiteSpace(model.Correo))
                {
                    ModelState.AddModelError("", "El correo del formulario está vacío.");
                    return View("Contactenos", model);
                }

                var asuntoCorreo = $"Nuevo mensaje de contacto - {model.Asunto}";
                var cuerpoCorreo = $@"
            <h2>Nuevo mensaje desde TechStore CR</h2>
            <p><strong>Nombre:</strong> {model.Nombre}</p>
            <p><strong>Correo:</strong> {model.Correo}</p>
            <p><strong>Teléfono:</strong> {model.Telefono}</p>
            <p><strong>Asunto:</strong> {model.Asunto}</p>
            <hr />
            <p><strong>Mensaje:</strong></p>
            <p>{model.Mensaje}</p>
            <hr />
            <p><strong>Fecha:</strong> {model.Fecha:dd/MM/yyyy HH:mm}</p>
        ";

                using var mensaje = new MailMessage();
                mensaje.From = new MailAddress(_emailSettings.FromEmail, _emailSettings.FromName);
                mensaje.To.Add(_emailSettings.ToEmail);
                mensaje.ReplyToList.Add(new MailAddress(model.Correo, model.Nombre));
                mensaje.Subject = asuntoCorreo;
                mensaje.Body = cuerpoCorreo;
                mensaje.IsBodyHtml = true;

                using var smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.Port)
                {
                    Credentials = new NetworkCredential(
                        _emailSettings.Username,
                        _emailSettings.Password),
                    EnableSsl = true
                };

                await smtp.SendMailAsync(mensaje);

                TempData["MensajeExito"] = "Tu mensaje fue enviado correctamente.";
                return RedirectToAction("Contactenos");
            }
            catch (Exception ex)
            {
                var detalle = ex.InnerException != null
                    ? $"{ex.Message} | Inner: {ex.InnerException.Message}"
                    : ex.Message;

                ModelState.AddModelError("", $"Ocurrió un error al enviar el mensaje: {detalle}");
                return View("Contactenos", model);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Suscribirse(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                TempData["ErrorSuscripcion"] = "Ingresa un correo válido.";
                return Redirect("/Home/Index#suscripcion");
            }

            var existe = _context.Suscriptores.Any(s => s.Correo == correo);

            if (existe)
            {
                TempData["ErrorSuscripcion"] = "Este correo ya está suscrito.";
                return Redirect("/Home/Index#suscripcion");
            }

            var suscriptor = new Suscriptor
            {
                Correo = correo
            };

            _context.Suscriptores.Add(suscriptor);
            await _context.SaveChangesAsync();

            TempData["MensajeSuscripcion"] = "¡Te suscribiste correctamente!";
            return Redirect("/Home/Index#suscripcion");
        }

        public IActionResult Garantias()
        {
            return View();
        }

        public IActionResult TerminosCondiciones()
        {
            return View();
        }
    }
}