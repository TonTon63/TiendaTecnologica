using System;
using System.ComponentModel.DataAnnotations;

namespace TiendaTecnologica.Models
{
    public class Suscriptor
    {
        [Key]
        public int IdSuscriptor { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}