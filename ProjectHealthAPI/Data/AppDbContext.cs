using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Models;

namespace ProjectHealthAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Cliente> Clientes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cliente>()
                .HasIndex(p => p.Cedula)
                .IsUnique();
        }
    }
}