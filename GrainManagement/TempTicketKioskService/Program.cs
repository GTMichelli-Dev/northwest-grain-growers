using System.Reflection;
using Microsoft.OpenApi.Models;
using TempTicketKioskService.Models;
using TempTicketKioskService.Services;

var version = Assembly.GetExecutingAssembly()
    .GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion
    ?? Assembly.GetExecutingAssembly().GetName().Version?.ToString()
    ?? "unknown";

var builder = WebApplication.CreateBuilder(args);

// Keep the service alive even when a hosted-service ExecuteAsync escapes
// with an unhandled exception. Field hardware can spit garbage at us and
// we'd rather log + keep running than fall over.
builder.Services.Configure<HostOptions>(o =>
{
    o.BackgroundServiceExceptionBehavior = BackgroundServiceExceptionBehavior.Ignore;
});
TaskScheduler.UnobservedTaskException += (s, e) =>
{
    Console.Error.WriteLine($"[TempTicketKioskService] Unobserved task exception suppressed: {e.Exception}");
    e.SetObserved();
};

// Logging — console banner + EventLog on Windows so installs without a
// console get something searchable.
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
        settings.SourceName = "TempTicketKioskService";
        settings.LogName    = "Application";
    });
}

// Bind the Kiosk options section. IOptionsMonitor so a future appsettings
// reload (e.g. via /api/config PUT in a later phase) takes effect without
// restarting the service.
builder.Services.Configure<KioskOptions>(builder.Configuration.GetSection("Kiosk"));

// HTTP + Swagger surface
builder.Services.AddHttpClient();
builder.Services.AddSingleton<KioskHttpClient>();
builder.Services.AddSingleton<GpioMonitor>();
builder.Services.AddHostedService(sp => sp.GetRequiredService<GpioMonitor>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title       = "Temp Ticket Kiosk Service",
        Version     = "v1",
        Description = "Local API for the kiosk Pi: status, test press, current config.",
    });
});

var app = builder.Build();

// Startup banner
var listenUrls = builder.Configuration["Urls"]
    ?? string.Join(',', builder.WebHost.GetSetting("urls")?.Split(';', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>());
Console.WriteLine();
Console.WriteLine(new string('─', 70));
Console.WriteLine($" TempTicketKioskService v{version}");
Console.WriteLine($" Environment : {app.Environment.EnvironmentName}");
Console.WriteLine($" GPIO        : {(OperatingSystem.IsLinux() ? "enabled" : "DISABLED (non-Linux host)")}");
Console.WriteLine(new string('─', 70));
Console.WriteLine();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Temp Ticket Kiosk v1");
    c.DocumentTitle = "Temp Ticket Kiosk Service";
});

app.MapControllers();

await app.RunAsync();
