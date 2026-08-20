using BasicWeigh.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicWeigh.Web.Data;

public class ScaleDbContext : DbContext
{
    public ScaleDbContext(DbContextOptions<ScaleDbContext> options) : base(options) { }

    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Carrier> Carriers => Set<Carrier>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<Destination> Destinations => Set<Destination>();
    public DbSet<Truck> Trucks => Set<Truck>();
    public DbSet<Commodity> Commodities => Set<Commodity>();
    public DbSet<AppSetup> AppSetup => Set<AppSetup>();
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<CameraConfig> CameraConfigs => Set<CameraConfig>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(e =>
        {
            e.ToTable("Transactions");
            e.HasIndex(t => t.DateIn);
            e.HasIndex(t => t.Customer);
            e.HasIndex(t => t.Carrier);
        });

        modelBuilder.Entity<Customer>(e =>
        {
            e.ToTable("Customers");
            e.HasIndex(c => c.CustomerName).IsUnique();
            e.Property(c => c.UseAtKiosk).HasDefaultValue(true);
        });
        modelBuilder.Entity<Carrier>(e =>
        {
            e.ToTable("Carriers");
            e.HasIndex(c => c.CarrierName).IsUnique();
            e.Property(c => c.UseAtKiosk).HasDefaultValue(true);
        });
        modelBuilder.Entity<Location>(e =>
        {
            e.ToTable("Locations");
            e.HasIndex(l => l.LocationName).IsUnique();
            e.Property(l => l.UseAtKiosk).HasDefaultValue(true);
        });
        modelBuilder.Entity<Destination>(e =>
        {
            e.ToTable("Destinations");
            e.HasIndex(d => d.DestinationName).IsUnique();
            e.Property(d => d.UseAtKiosk).HasDefaultValue(true);
        });
        modelBuilder.Entity<Truck>(e =>
        {
            e.ToTable("Trucks");
            e.HasIndex(t => new { t.TruckId, t.CarrierName }).IsUnique();
            e.Property(t => t.UseAtKiosk).HasDefaultValue(true);
        });
        modelBuilder.Entity<Commodity>(e =>
        {
            e.ToTable("Commodities");
            e.HasIndex(c => c.CommodityName).IsUnique();
            e.Property(c => c.UseAtKiosk).HasDefaultValue(true);
        });
        modelBuilder.Entity<AppSetup>(e =>
        {
            e.ToTable("AppSetup");
            e.Property(s => s.PromptKioskCommodity).HasDefaultValue(true);
            e.Property(s => s.PromptKioskCustomer).HasDefaultValue(true);
            e.Property(s => s.PromptKioskCarrier).HasDefaultValue(true);
            e.Property(s => s.PromptKioskLocation).HasDefaultValue(true);
            e.Property(s => s.PromptKioskTruckId).HasDefaultValue(true);
            e.Property(s => s.PromptKioskDestinationOnOutbound).HasDefaultValue(true);
        });

        modelBuilder.Entity<AppUser>(e =>
        {
            e.ToTable("Users");
            e.HasIndex(u => u.Username).IsUnique();
        });

        modelBuilder.Entity<CameraConfig>(e =>
        {
            e.ToTable("CameraConfigs");
            e.HasIndex(c => c.CameraId).IsUnique();
        });

        modelBuilder.Entity<AppSetup>().HasData(new AppSetup
        {
            Id = 1,
            TicketNumber = 1,
            Header1 = "Basic Weigh",
            Header2 = "",
            Header3 = "",
            Header4 = "",
            TicketsPerPage = 1,
            DemoMode = false,
            KioskCount = 0,
            Icon = null,
            IconContentType = null,
            Theme = "default"
        });
    }
}
