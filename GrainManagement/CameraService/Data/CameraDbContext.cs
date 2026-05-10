using CameraService.Models;
using Microsoft.EntityFrameworkCore;

namespace CameraService.Data;

public class CameraDbContext : DbContext
{
    public CameraDbContext(DbContextOptions<CameraDbContext> options) : base(options) { }

    public DbSet<CameraConfigEntity> Cameras => Set<CameraConfigEntity>();
    public DbSet<ServiceSettings> Settings => Set<ServiceSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CameraConfigEntity>(e =>
        {
            e.ToTable("Cameras");
            e.HasIndex(c => c.CameraId).IsUnique();
        });

        modelBuilder.Entity<ServiceSettings>(e =>
        {
            e.ToTable("Settings");
            e.HasData(new ServiceSettings
            {
                Id = 1,
                ServiceId = "default",
                ServerUrl = "http://localhost:51791",
                SignalRHub = "/hubs/camera",
                StreamBaseUrl = "",
                BrandsUrl = "",
                BrandsToken = ""
            });
        });
    }
}
