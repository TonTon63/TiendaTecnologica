using System;
using System.ComponentModel.DataAnnotations;

namespace TiendaTecnologica.Models
{
    public class Contacto
    {
        [Key]
        public int IdContacto { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
        public string Correo { get; set; }

        public string Telefono { get; set; }

        [Required(ErrorMessage = "Selecciona un asunto.")]
        public string Asunto { get; set; }

        [Required(ErrorMessage = "El mensaje es obligatorio.")]
        public string Mensaje { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}