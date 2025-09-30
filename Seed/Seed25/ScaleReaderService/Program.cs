using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System; // for OperatingSystem
using ScaleReaderService.Models;
using ScaleReaderService.Services;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

// Logging: console + Windows Event Log (Windows only)
builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(o =>
{
    o.TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff ";
    o.UseUtcTimestamp = false;
});

if (OperatingSystem.IsWindows())
{
    builder.Logging.AddEventLog(settings =>
    {
        settings.SourceName = "ScaleReaderService";
        settings.LogName = "Application";
    });
}

// === Options binding ===
// NOTE: bind lists, not arrays
builder.Services.Configure<PollingOptions>(builder.Configuration.GetSection("Polling"));
builder.Services.Configure<SmaOptions>(builder.Configuration.GetSection("Sma"));
builder.Services.Configure<List<EndpointOptions>>(builder.Configuration.GetSection("Endpoints"));
builder.Services.Configure<List<ScaleOptions>>(builder.Configuration.GetSection("Scales"));

// HttpClient factory
builder.Services.AddHttpClient("endpoints");

// Services
builder.Services.AddSingleton<SmaClient>();
builder.Services.AddSingleton<ScalePoller>();

// Hosted service that runs the poller
builder.Services.AddHostedService(provider =>
{
    var poller = provider.GetRequiredService<ScalePoller>();
    var log = provider.GetRequiredService<ILogger<BackgroundServiceImpl>>();
    return new BackgroundServiceImpl(poller, log);
});

await builder.Build().RunAsync();

// Wrapper background service to drive the poller loop
public class BackgroundServiceImpl : BackgroundService
{
    private readonly ScalePoller _poller;
    private readonly ILogger<BackgroundServiceImpl> _log;

    public BackgroundServiceImpl(ScalePoller poller, ILogger<BackgroundServiceImpl> log)
    {
        _poller = poller;
        _log = log;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _log.LogInformation("ScaleReaderService starting.");
        await _poller.RunAsync(stoppingToken);
        _log.LogInformation("ScaleReaderService stopping.");
    }
}
