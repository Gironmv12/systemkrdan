using CrudUsuariosClientesApi.Models;
using Microsoft.EntityFrameworkCore;


namespace CrudUsuariosClientesApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; } = null!;
        public DbSet<Cliente> Clientes { get; set; } = null!;
    }
}
