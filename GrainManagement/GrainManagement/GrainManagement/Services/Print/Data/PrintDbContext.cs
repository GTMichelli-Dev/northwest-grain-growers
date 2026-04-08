#nullable enable
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Services.Print.Data
{
    /// <summary>
    /// Lightweight EF Core context for persisting print service settings locally (SQLite).
    /// The database file is stored alongside the application executable.
    /// </summary>
    public class PrintDbContext : DbContext
    {
        public DbSet<ServiceSettings> Settings { get; set; } = null!;

        private readonly string _dbPath;

        public PrintDbContext()
        {
            _dbPath = Path.Combine(AppContext.BaseDirectory, "printservice.db");
        }

        public PrintDbContext(string dbPath)
        {
            _dbPath = dbPath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={_dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ServiceSettings>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasData(new ServiceSettings
                {
                    Id = 1,
                    ServerUrl = "http://localhost:5000",
                    SignalRHub = "/hubs/print",
                    DeviceId = Environment.MachineName,
                    DefaultPrinter = "Kiosk",
                    IsEnabled = true
                });
            });
        }

        /// <summary>
        /// Ensures the database and tables exist, then returns the current settings row.
        /// Creates a default row if none exists.
        /// </summary>
        public ServiceSettings GetSettings()
        {
            Database.EnsureCreated();

            var settings = Settings.FirstOrDefault(s => s.Id == 1);
            if (settings == null)
            {
                settings = new ServiceSettings
                {
                    Id = 1,
                    ServerUrl = "http://localhost:5000",
                    SignalRHub = "/hubs/print",
                    DeviceId = Environment.MachineName,
                    DefaultPrinter = "Kiosk",
                    IsEnabled = true
                };
                Settings.Add(settings);
                SaveChanges();
            }

            return settings;
        }

        /// <summary>
        /// Updates the settings and saves to the database.
        /// </summary>
        public void UpdateSettings(ServiceSettings settings)
        {
            Database.EnsureCreated();

            var existing = Settings.FirstOrDefault(s => s.Id == 1);
            if (existing != null)
            {
                existing.ServerUrl = settings.ServerUrl;
                existing.SignalRHub = settings.SignalRHub;
                existing.DeviceId = settings.DeviceId;
                existing.DefaultPrinter = settings.DefaultPrinter;
                existing.IsEnabled = settings.IsEnabled;
            }
            else
            {
                settings.Id = 1;
                Settings.Add(settings);
            }

            SaveChanges();
        }
    }

    /// <summary>
    /// Persisted settings for the print service.
    /// Stored in a local SQLite database.
    /// </summary>
    public class ServiceSettings
    {
        public int Id { get; set; }

        /// <summary>
        /// Base URL of the GrainManagement server (e.g., "http://localhost:5000").
        /// </summary>
        public string ServerUrl { get; set; } = "http://localhost:5000";

        /// <summary>
        /// SignalR hub path to connect to (e.g., "/hubs/print").
        /// The full hub URL is ServerUrl + SignalRHub.
        /// </summary>
        public string SignalRHub { get; set; } = "/hubs/print";

        /// <summary>
        /// Unique device identifier for this print station.
        /// Used to register with the PrintHub so the server can target print commands.
        /// </summary>
        public string? DeviceId { get; set; }

        /// <summary>
        /// CUPS printer name to send print jobs to.
        /// </summary>
        public string? DefaultPrinter { get; set; }

        /// <summary>
        /// Whether the print service is enabled.
        /// When false, the worker will not connect to the hub.
        /// </summary>
        public bool IsEnabled { get; set; } = true;
    }
}
