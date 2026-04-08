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

    /// <summary>
    /// Comma-separated list of server base URLs (e.g. "http://server1:5000,http://server2:5000").
    /// Each URL gets its own SignalR connection.
    /// </summary>
    public string ServerUrl { get; set; } = "http://localhost:5000";

    public string SignalRHub { get; set; } = "/hubs/print";

    /// <summary>
    /// Parsed list of individual server URLs.
    /// </summary>
    public List<string> ServerUrls => ServerUrl
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Where(u => !string.IsNullOrWhiteSpace(u))
        .ToList();

    /// <summary>
    /// Resolved list of full hub URLs (each ServerUrl + SignalRHub).
    /// </summary>
    public List<string> HubUrls => ServerUrls
        .Select(u => u.TrimEnd('/') + SignalRHub)
        .ToList();
}
