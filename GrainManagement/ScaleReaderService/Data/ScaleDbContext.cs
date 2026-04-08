using Microsoft.EntityFrameworkCore;
using ScaleReaderService.Models;

namespace ScaleReaderService.Data
{
    public class ScaleDbContext : DbContext
    {
        public DbSet<ScaleConfigEntity> ScaleConfigs { get; set; } = null!;

        public ScaleDbContext(DbContextOptions<ScaleDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ScaleConfigEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Description).HasMaxLength(100);
                entity.Property(e => e.IpAddress).HasMaxLength(50);
                entity.Property(e => e.Brand).HasMaxLength(50);
                entity.Property(e => e.RequestCommand).HasMaxLength(50);
                entity.Property(e => e.Encoding).HasMaxLength(10);
            });
        }
    }
}
