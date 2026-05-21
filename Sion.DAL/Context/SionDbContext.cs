using Microsoft.EntityFrameworkCore;
using Sion.DAL.Entities;

namespace Sion.DAL.Context
{
    public class SionDbContext : DbContext
    {
        public SionDbContext(DbContextOptions<SionDbContext> options) : base(options) { }

        public DbSet<ConfiguracionSitio> ConfiguracionesSitio { get; set; }
        public DbSet<SeccionHome> SeccionesHome { get; set; }
        public DbSet<ImagenGaleria> ImagenesGaleria { get; set; }
        public DbSet<Donacion> Donaciones { get; set; }
        public DbSet<LogAuditoria> LogsAuditoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ── ConfiguracionSitio ────────────────────────────────────────────────
            modelBuilder.Entity<ConfiguracionSitio>(e =>
            {
                e.Property(c => c.Clave)
                    .HasMaxLength(100)
                    .IsRequired();

                e.Property(c => c.Valor)
                    .HasMaxLength(1000)
                    .IsRequired();

                e.Property(c => c.Descripcion)
                    .HasMaxLength(500);

                // Clave es única: no puede haber dos claves iguales en el key-value store
                e.HasIndex(c => c.Clave)
                    .IsUnique()
                    .HasDatabaseName("IX_ConfiguracionesSitio_Clave");
            });

            // ── SeccionHome ───────────────────────────────────────────────────────
            modelBuilder.Entity<SeccionHome>(e =>
            {
                e.Property(s => s.Titulo)
                    .HasMaxLength(150)
                    .IsRequired();

                // Descripcion es nvarchar(MAX): puede contener texto largo
                e.Property(s => s.Descripcion)
                    .IsRequired();

                e.Property(s => s.RutaImagen)
                    .HasMaxLength(500);

                e.Property(s => s.Estilo)
                    .HasMaxLength(50)
                    .IsRequired();

                // Índice en Orden: el Razor hace ORDER BY Orden en cada request del Home
                e.HasIndex(s => s.Orden)
                    .HasDatabaseName("IX_SeccionesHome_Orden");
            });

            // ── ImagenGaleria ─────────────────────────────────────────────────────
            modelBuilder.Entity<ImagenGaleria>(e =>
            {
                e.Property(i => i.Titulo)
                    .HasMaxLength(200)
                    .IsRequired();

                e.Property(i => i.RutaOriginal)
                    .HasMaxLength(500)
                    .IsRequired();

                e.Property(i => i.RutaThumbnail)
                    .HasMaxLength(500)
                    .IsRequired();

                e.Property(i => i.RutaWebP)
                    .HasMaxLength(500)
                    .IsRequired();

                // Índice en EstaActiva: la galería pública filtra WHERE EstaActiva = 1
                e.HasIndex(i => i.EstaActiva)
                    .HasDatabaseName("IX_ImagenesGaleria_EstaActiva");
            });

            // ── Donacion ──────────────────────────────────────────────────────────
            modelBuilder.Entity<Donacion>(e =>
            {
                e.Property(d => d.Monto)
                    .HasColumnType("decimal(18,2)");

                e.Property(d => d.TransaccionPaypalId)
                    .HasMaxLength(100)
                    .IsRequired();

                e.Property(d => d.Moneda)
                    .HasMaxLength(10)
                    .IsRequired();

                e.Property(d => d.NombreDonante)
                    .HasMaxLength(200);

                e.Property(d => d.EmailDonante)
                    .HasMaxLength(254); // RFC 5321: longitud máxima de una dirección email

                e.Property(d => d.Estado)
                    .HasMaxLength(50)
                    .IsRequired();

                // Unique en TransaccionPaypalId: PayPal no puede registrar la misma transacción dos veces
                e.HasIndex(d => d.TransaccionPaypalId)
                    .IsUnique()
                    .HasDatabaseName("IX_Donaciones_TransaccionPaypalId");

                // Índice en FechaRegistro: el dashboard filtra donaciones por rango de fechas
                e.HasIndex(d => d.FechaRegistro)
                    .HasDatabaseName("IX_Donaciones_FechaRegistro");
            });

            // ── LogAuditoria ──────────────────────────────────────────────────────
            modelBuilder.Entity<LogAuditoria>(e =>
            {
                e.Property(l => l.Accion)
                    .HasMaxLength(100)
                    .IsRequired();

                e.Property(l => l.EntidadAfectada)
                    .HasMaxLength(100)
                    .IsRequired();

                e.Property(l => l.UsuarioEmail)
                    .HasMaxLength(254)
                    .IsRequired();

                // Detalle es nvarchar(MAX): puede contener JSON con datos del cambio
                e.Property(l => l.Detalle)
                    .IsRequired();

                // Índice en FechaHora: los logs se consultan siempre ordenados por fecha
                e.HasIndex(l => l.FechaHora)
                    .HasDatabaseName("IX_LogsAuditoria_FechaHora");
            });
        }
    }
}
