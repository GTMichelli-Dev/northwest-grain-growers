using CameraService;
using CameraService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.OpenApi.Models;

var version = typeof(CameraWorker).Assembly.GetName().Version?.ToString(3) ?? "1.0.0";

var builder = WebApplication.CreateBuilder(args);

// Suppress noisy EF Core command/query warnings
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Query", LogLevel.Error);

// Windows Service support
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "BasicWeigh Camera Service";
});

// SQLite database for camera configs and service settings
var dbPath = Path.Combine(AppContext.BaseDirectory, "cameraservice.db");
builder.Services.AddDbContext<CameraDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Configuration (appsettings.json used as initial seed only)
builder.Services.Configure<CameraOptions>(
    builder.Configuration.GetSection("Camera"));

// Load camera brand definitions from remote URL or local fallback
builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<CameraOptions>>().Value;
    var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("CameraBrands");
    var localPath = Path.Combine(AppContext.BaseDirectory, "camera-snapshot.json");
    var brands = CameraOptions.LoadBrandsAsync(options.BrandsUrl, localPath, options.BrandsToken, logger).GetAwaiter().GetResult();
    options.SetBrands(brands);
    return brands;
});

// HTTP client for calling web API and camera
builder.Services.AddHttpClient("BasicWeighApi", client =>
{
    var serverUrl = builder.Configuration["Camera:ServerUrl"] ?? "http://localhost:5110";
    client.BaseAddress = new Uri(serverUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});
builder.Services.AddHttpClient(); // Default client for IP camera

// Restart signal (triggered when settings change via API)
builder.Services.AddSingleton<CameraService.Services.RestartSignal>();

// Announce signal (triggered when cameras change via API — lighter than restart)
builder.Services.AddSingleton<CameraService.Services.AnnounceSignal>();

// Shared capture service (used by both worker and API)
builder.Services.AddScoped<CameraService.Services.CameraCaptureService>();

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Camera Capture Service API",
        Version = "v1",
        Description = "Health, status, configuration, and test capture endpoints for the Camera Capture Service."
    });
});

// Background worker
builder.Services.AddHostedService<CameraWorker>();

// Event Log on Windows
if (OperatingSystem.IsWindows())
{
    builder.Logging.AddEventLog(new EventLogSettings
    {
        SourceName = "CameraService"
    });
}

var app = builder.Build();

// Auto-create/migrate database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CameraDbContext>();
    db.Database.EnsureCreated();

    // Seed from appsettings.json if no cameras exist yet
    if (!db.Cameras.Any())
    {
        var opts = scope.ServiceProvider.GetRequiredService<Microsoft.Extensions.Options.IOptions<CameraOptions>>().Value;
        if (!string.IsNullOrWhiteSpace(opts.CameraIp) || !string.IsNullOrWhiteSpace(opts.UsbDeviceName))
        {
            db.Cameras.Add(new CameraService.Models.CameraConfigEntity
            {
                CameraId = "default",
                DisplayName = "Default Camera",
                CameraBrand = opts.CameraBrand ?? "Custom",
                CameraIp = opts.CameraIp,
                CameraUser = opts.CameraUser,
                CameraPassword = opts.CameraPassword,
                UsbDeviceName = opts.UsbDeviceName,
                CameraUrl = opts.CameraUrl,
                UsbCommand = opts.UsbCommand,
                TimeoutSeconds = opts.TimeoutSeconds,
                Active = true,
                IsDefault = true
            });
            db.SaveChanges();
        }
    }

    // Seed settings from appsettings if not in DB yet
    var settings = db.Settings.OrderBy(s => s.Id).FirstOrDefault();
    if (settings != null)
    {
        var configUrl = builder.Configuration["Camera:ServerUrl"];
        if (!string.IsNullOrWhiteSpace(configUrl) && settings.ServerUrl == "http://localhost:5110")
        {
            settings.ServerUrl = configUrl;
            db.SaveChanges();
        }
    }
}

// Swagger — always enabled
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Camera Capture Service API v1");
});

app.MapControllers();

// Startup banner
var urls = builder.Configuration["Urls"] ?? "http://localhost:5210";
var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("CameraService");
logger.LogInformation("============================================");
logger.LogInformation("  Camera Capture Service v{Version}", version);
logger.LogInformation("  Swagger: {Urls}/swagger", urls);
logger.LogInformation("============================================");

await app.RunAsync();
