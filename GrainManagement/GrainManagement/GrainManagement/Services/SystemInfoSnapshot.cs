using System.Reflection;

namespace GrainManagement.Services;

/// <summary>
/// Singleton that captures assembly info once at startup.
/// Server info (from DB) is loaded on first access and cached.
/// </summary>
public sealed class SystemInfoSnapshot
{
    public string ApplicationName { get; }
    public string Version { get; }
    public string BuildDate { get; }
    public string? ServerFriendlyName { get; private set; }

    private readonly IServiceProvider _sp;
    private readonly object _lock = new();
    private bool _serverLoaded;

    public SystemInfoSnapshot(IServiceProvider sp)
    {
        _sp = sp;

        var assembly = typeof(SystemInfoSnapshot).Assembly;
        ApplicationName = assembly.GetCustomAttribute<AssemblyTitleAttribute>()?.Title ?? "GrainManagement";
        Version = assembly.GetName().Version?.ToString() ?? "0.0.0.0";
        BuildDate = File.GetLastWriteTimeUtc(assembly.Location).ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// Loads the server friendly name from the DB once, then caches it.
    /// Call this during app startup or on first request.
    /// </summary>
    public async Task EnsureServerInfoLoadedAsync(CancellationToken ct = default)
    {
        if (_serverLoaded) return;

        lock (_lock)
        {
            if (_serverLoaded) return;
        }

        try
        {
            using var scope = _sp.CreateScope();
            var provider = scope.ServiceProvider.GetRequiredService<IServerInfoProvider>();
            var server = await provider.GetAsync(ct);
            ServerFriendlyName = server.FriendlyName;
        }
        catch
        {
            ServerFriendlyName = "Unknown";
        }
        finally
        {
            _serverLoaded = true;
        }
    }

    public string ToDisplayString()
    {
        return $"{ApplicationName} · v{Version} · {BuildDate} · {ServerFriendlyName ?? "Loading..."}";
    }
}
