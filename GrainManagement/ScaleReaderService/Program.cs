using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ScaleReaderService.Data;
using ScaleReaderService.Models;
using ScaleReaderService.Services;

var version = Assembly.GetExecutingAssembly()
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
    ?? Assembly.GetExecutingAssembly().GetName().Version?.ToString()
    ?? "unknown";

Console.WriteLine($"ScaleReaderService v{version}");
Console.WriteLine(new string('-', 40));

var builder = Host.CreateApplicationBuilder(args);

// .NET 8+ defaults to StopHost — any unhandled exception in a hosted
// service brings the whole service down. For an always-on field service
// we want the opposite: log and keep running so the worker can self-heal.
builder.Services.Configure<HostOptions>(o =>
{
    o.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});

// Belt-and-suspenders: catch any unobserved task exception and log it
// instead of letting the runtime escalate to process-exit. We add Tasks
// with `_ = Task.Run(...)` in a few places, and even the most defensive
// inner try/catch can't cover a logger-itself-throwing edge case.
TaskScheduler.UnobservedTaskException += (s, e) =>
{
    Console.Error.WriteLine($"[ScaleReaderService] Unobserved task exception suppressed: {e.Exception}");
    e.SetObserved();
};

// Logging
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

// Options
builder.Services.Configure<ServiceSettings>(builder.Configuration.GetSection("Service"));

// SQLite database for scale configs
builder.Services.AddDbContext<ScaleDbContext>(opt =>
    opt.UseSqlite($"Data Source={Path.Combine(AppContext.BaseDirectory, "scalereaderservice.db")}"));

// Services
builder.Services.AddSingleton<SmaClient>();
builder.Services.AddSingleton<RestartSignal>();
builder.Services.AddHostedService<ScaleWorker>();

await builder.Build().RunAsync();
