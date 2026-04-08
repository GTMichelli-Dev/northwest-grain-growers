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
