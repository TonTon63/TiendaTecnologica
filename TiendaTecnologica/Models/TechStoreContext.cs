using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TiendaTecnologica.Models
{
    public class TechStoreContext : IdentityDbContext<ApplicationUser>
    {
        public TechStoreContext(DbContextOptions<TechStoreContext> options)
            : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Subcategoria> Subcategorias { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Contacto> Contactos { get; set; }
        public DbSet<Suscriptor> Suscriptores { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}