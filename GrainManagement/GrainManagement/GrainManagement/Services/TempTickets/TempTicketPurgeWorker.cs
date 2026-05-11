using GrainManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Services.TempTickets;

/// <summary>
/// Sweeps orphaned temp tickets nightly. Anything older than
/// <see cref="MaxAge"/> that hasn't been consumed by a lot gets
/// deleted, along with its image file. Keeps the table from
/// accumulating button-press junk over time.
/// </summary>
public sealed class TempTicketPurgeWorker : BackgroundService
{
    private static readonly TimeSpan MaxAge = TimeSpan.FromHours(36);
    private static readonly TimeSpan SweepInterval = TimeSpan.FromHours(1);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IConfiguration _config;
    private readonly ILogger<TempTicketPurgeWorker> _log;

    public TempTicketPurgeWorker(
        IServiceScopeFactory scopeFactory,
        IConfiguration config,
        ILogger<TempTicketPurgeWorker> log)
    {
        _scopeFactory = scopeFactory;
        _config = config;
        _log = log;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Delay the first sweep a minute past startup so the warm-up
        // path stays uncluttered.
        try { await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); }
        catch (OperationCanceledException) { return; }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await SweepAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Temp ticket purge sweep failed.");
            }

            try { await Task.Delay(SweepInterval, stoppingToken); }
            catch (OperationCanceledException) { return; }
        }
    }

    private async Task SweepAsync(CancellationToken ct)
    {
        var cutoff = DateTime.UtcNow - MaxAge;
        var imagesRoot = _config["TicketImages:PhysicalPath"];

        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<dbContext>();

        var stale = await db.Set<TempWeightTicket>()
            .Where(t => t.CreatedAt < cutoff && t.ConsumedByLotId == null)
            .ToListAsync(ct);

        if (stale.Count == 0) return;

        foreach (var t in stale)
        {
            if (!string.IsNullOrWhiteSpace(t.ImagePath) && !string.IsNullOrWhiteSpace(imagesRoot))
            {
                var full = Path.Combine(imagesRoot, t.ImagePath);
                try { if (File.Exists(full)) File.Delete(full); }
                catch (Exception ex)
                {
                    _log.LogWarning(ex, "Temp ticket {Id}: failed to delete image {Path}.", t.TempTicketId, full);
                }
            }
        }

        db.Set<TempWeightTicket>().RemoveRange(stale);
        await db.SaveChangesAsync(ct);

        _log.LogInformation("Temp ticket purge: removed {Count} rows older than {Cutoff:u}.",
            stale.Count, cutoff);
    }
}
