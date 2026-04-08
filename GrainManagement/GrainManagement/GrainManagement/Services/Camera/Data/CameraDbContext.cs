using GrainManagement.Services.Camera.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Services.Camera.Data;

/// <summary>
/// Local SQLite database context for camera service configuration.
/// Stores camera configs and service settings independently of the main GrainManagement dbContext.
/// </summary>
public class CameraDbContext : DbContext
{
    public CameraDbContext(DbContextOptions<CameraDbContext> options) : base(options)
    {
    }

    public DbSet<CameraConfigEntity> CameraConfigs { get; set; } = null!;
    public DbSet<ServiceSettings> ServiceSettings { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CameraConfigEntity>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ScaleId);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Brand).HasMaxLength(50);
            entity.Property(e => e.IpAddress).HasMaxLength(200);
            entity.Property(e => e.Username).HasMaxLength(100);
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.RtspAddress).HasMaxLength(500);
            entity.Property(e => e.CameraLocation).HasMaxLength(200);
        });

        modelBuilder.Entity<ServiceSettings>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ServerUrl).HasMaxLength(500);
            entity.Property(e => e.SignalRHub).HasMaxLength(200);
            entity.Property(e => e.DefaultImageOutputPath).HasMaxLength(500);
            entity.Property(e => e.ImageUploadEndpoint).HasMaxLength(500);
        });

        // Seed default settings row
        modelBuilder.Entity<ServiceSettings>().HasData(new ServiceSettings
        {
            Id = 1,
            ServerUrl = "http://localhost:5000",
            SignalRHub = "/hubs/scale",
            IsEnabled = true
        });
    }
}
