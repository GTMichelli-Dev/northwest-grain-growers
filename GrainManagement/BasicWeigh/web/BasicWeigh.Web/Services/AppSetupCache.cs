using BasicWeigh.Web.Data;
using BasicWeigh.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace BasicWeigh.Web.Services;

/// <summary>
/// In-memory cache for AppSetup. Reads from DB once and caches until Invalidate() is called.
/// Thread-safe. Register as Singleton.
/// </summary>
public class AppSetupCache
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly object _lock = new();
    private AppSetup? _cached;

    public AppSetupCache(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    /// <summary>
    /// Get the cached AppSetup. Reads from DB on first call or after Invalidate().
    /// Returns a detached copy so callers can't accidentally modify the cache.
    /// </summary>
    public AppSetup Get()
    {
        lock (_lock)
        {
            if (_cached != null) return _cached;
        }

        // Read from DB outside the lock to avoid holding it during I/O
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
        var setup = db.AppSetup.AsNoTracking().First();

        lock (_lock)
        {
            _cached = setup;
            return _cached;
        }
    }

    /// <summary>
    /// Call after saving AppSetup changes to the database.
    /// Next call to Get() will re-read from DB.
    /// </summary>
    public void Invalidate()
    {
        lock (_lock) { _cached = null; }
    }
}
