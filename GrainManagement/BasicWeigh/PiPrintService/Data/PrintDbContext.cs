using Microsoft.EntityFrameworkCore;

namespace WebPrintService.Data;

public class PrintDbContext : DbContext
{
    public PrintDbContext(DbContextOptions<PrintDbContext> options) : base(options) { }

    public DbSet<ServiceSettings> Settings => Set<ServiceSettings>();
}

public class ServiceSettings
{
    public int Id { get; set; }
    public string ServiceId { get; set; } = "default";
    public string ServerUrl { get; set; } = "http://localhost:5110";
    public string SignalRHub { get; set; } = "/scaleHub";
}
