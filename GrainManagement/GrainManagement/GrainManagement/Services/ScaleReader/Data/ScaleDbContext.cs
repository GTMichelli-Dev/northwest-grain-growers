using GrainManagement.Services.ScaleReader.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Services.ScaleReader.Data
{
    /// <summary>
    /// Local SQLite database context for persisting scale configurations.
    /// This is separate from the main GrainManagement dbContext and stores
    /// only the scale reader service configuration on the local machine.
    /// </summary>
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

            // Seed default service settings row via a separate table
            modelBuilder.Entity<ServiceSettingsEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasData(new ServiceSettingsEntity
                {
                    Id = 1,
                    ServerUrl = "http://localhost:5000",
                    SignalRHub = "/hubs/scale",
                    PollIntervalMs = 750,
                    TimeoutMs = 1000,
                    ReconnectBackoffMs = 2000,
                    MaxBackoffMs = 30000,
                    LocationId = 0,
                    LocationDescription = ""
                });
            });
        }
    }

    /// <summary>
    /// Persisted service-level settings stored in the local SQLite database.
    /// </summary>
    public class ServiceSettingsEntity
    {
        public int Id { get; set; }
        public string ServerUrl { get; set; } = "http://localhost:5000";
        public string SignalRHub { get; set; } = "/hubs/scale";
        public int PollIntervalMs { get; set; } = 750;
        public int TimeoutMs { get; set; } = 1000;
        public int ReconnectBackoffMs { get; set; } = 2000;
        public int MaxBackoffMs { get; set; } = 30000;
        public int LocationId { get; set; }
        public string LocationDescription { get; set; } = string.Empty;
    }
}
