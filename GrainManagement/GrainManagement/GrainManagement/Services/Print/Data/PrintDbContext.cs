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
        public DbSet<PrinterAssignment> PrinterAssignments { get; set; } = null!;

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

            modelBuilder.Entity<PrinterAssignment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Role).IsUnique();
            });
        }

        /// <summary>
        /// Ensures the PrinterAssignments table exists in an already-created database.
        /// EnsureCreated() is a no-op when the DB already exists, so we must
        /// create new tables manually for existing databases.
        /// </summary>
        private void EnsureSchemaUpToDate()
        {
            Database.EnsureCreated();

            // Check if PrinterAssignments table exists; create if missing
            try
            {
                Database.ExecuteSqlRaw(@"
                    CREATE TABLE IF NOT EXISTS PrinterAssignments (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Role TEXT NOT NULL DEFAULT '',
                        ServiceId TEXT NOT NULL DEFAULT '',
                        PrinterId TEXT NOT NULL DEFAULT ''
                    )");

                // Ensure unique index on Role (ignore if already exists)
                Database.ExecuteSqlRaw(@"
                    CREATE UNIQUE INDEX IF NOT EXISTS IX_PrinterAssignments_Role
                    ON PrinterAssignments (Role)");
            }
            catch
            {
                // Table/index already exists or other non-critical error
            }
        }

        /// <summary>
        /// Ensures the database and tables exist, then returns the current settings row.
        /// Creates a default row if none exists.
        /// </summary>
        public ServiceSettings GetSettings()
        {
            EnsureSchemaUpToDate();

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
            EnsureSchemaUpToDate();

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

        /// <summary>
        /// Gets the printer assignment for the given role (e.g. "Inbound", "Outbound").
        /// Returns null if no assignment exists.
        /// </summary>
        public PrinterAssignment? GetAssignment(string role)
        {
            EnsureSchemaUpToDate();
            return PrinterAssignments.FirstOrDefault(a =>
                a.Role == role);
        }

        /// <summary>
        /// Gets all printer assignments.
        /// </summary>
        public List<PrinterAssignment> GetAllAssignments()
        {
            EnsureSchemaUpToDate();
            return PrinterAssignments.ToList();
        }

        /// <summary>
        /// Saves a printer assignment for the given role.
        /// Upserts: updates if the role already exists, inserts otherwise.
        /// </summary>
        public void SaveAssignment(string role, string serviceId, string printerId)
        {
            EnsureSchemaUpToDate();

            var existing = PrinterAssignments.FirstOrDefault(a => a.Role == role);
            if (existing != null)
            {
                existing.ServiceId = serviceId;
                existing.PrinterId = printerId;
            }
            else
            {
                PrinterAssignments.Add(new PrinterAssignment
                {
                    Role = role,
                    ServiceId = serviceId,
                    PrinterId = printerId
                });
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

    /// <summary>
    /// Maps a print role (e.g. "Inbound", "Outbound") to a specific
    /// printer on a specific WebPrintService instance.
    /// Format matches BasicWeigh's "serviceId:printerId" pattern.
    /// </summary>
    public class PrinterAssignment
    {
        public int Id { get; set; }

        /// <summary>
        /// Role name, e.g. "Inbound" or "Outbound".
        /// </summary>
        public string Role { get; set; } = "";

        /// <summary>
        /// The WebPrintService instance ID (e.g. "default", "office").
        /// </summary>
        public string ServiceId { get; set; } = "";

        /// <summary>
        /// The printer ID on that service (e.g. "HP_LaserJet_Pro").
        /// </summary>
        public string PrinterId { get; set; } = "";
    }
}
