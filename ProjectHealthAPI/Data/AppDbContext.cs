using Microsoft.EntityFrameworkCore;
using ProjectHealthAPI.Models;

namespace ProjectHealthAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Cita> Citas { get; set; }
        public DbSet<Alquiler> Alquileres { get; set; }
        public DbSet<Pago> Pagos { get; set; }
        public DbSet<HistoriaMedica> HistoriasMedicas { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Cliente>()
                .HasIndex(p => p.Cedula)
                .IsUnique();
            
            modelBuilder.Entity<Paciente>()
                .HasIndex(p => p.Cedula)
                .IsUnique();
        }
    }
}