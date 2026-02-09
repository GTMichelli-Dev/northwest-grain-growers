using GrainManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace GrainManagement.Services;

public sealed class ServerInfoProvider : IServerInfoProvider
{
    private const string CacheKey = "ServerInfo";
    private readonly IMemoryCache _cache;
    private readonly dbContext _ctx;
    private readonly ILogger<ServerInfoProvider> _logger;

    public ServerInfoProvider(
        IMemoryCache cache,
        dbContext ctx,
        ILogger<ServerInfoProvider> logger)
    {
        _cache = cache;
        _ctx = ctx;
        _logger = logger;
    }

    public Task<Server> GetAsync(CancellationToken ct = default)
        => _cache.GetOrCreateAsync(CacheKey, async entry =>
        {
            // Adjust TTL as desired (or remove for "until recycle")
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);

            var serverName = await _ctx.Database
                .SqlQueryRaw<string>(
                    "SELECT CAST(@@SERVERNAME AS nvarchar(128)) AS Value")
                .FirstOrDefaultAsync(ct);

            if (string.IsNullOrWhiteSpace(serverName))
                throw new InvalidOperationException("Unable to read @@SERVERNAME.");

            serverName = serverName.Trim();

            var server = await _ctx.Servers
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.ServerName == serverName, ct);

            if (server is null)
                throw new InvalidOperationException(
                    $"No server row found for ServerName '{serverName}'.");

            _logger.LogInformation("ServerInfo loaded and cached: {ServerName}", server.ServerName);
            return server;
        })!;
}
