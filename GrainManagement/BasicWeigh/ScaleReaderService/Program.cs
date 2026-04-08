using ScaleReaderService;
using ScaleReaderService.Data;
using ScaleReaderService.Models;
using ScaleReaderService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.OpenApi.Models;

var version = typeof(ScaleWorker).Assembly.GetName().Version?.ToString(3) ?? "1.0.0";

var builder = WebApplication.CreateBuilder(args);

// Suppress noisy EF Core command/query warnings
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Query", LogLevel.Error);

// Windows Service support
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "BasicWeigh Scale Reader Service";
});

// SQLite database for scale configs and service settings
var dbPath = Path.Combine(AppContext.BaseDirectory, "scalereaderservice.db");
builder.Services.AddDbContext<ScaleDbContext>(options =>
    options.UseSqlite($"Data Source={dbPath}"));

// Load scale brand definitions from local first, then remote
builder.Services.AddSingleton(sp =>
{
    var logger = sp.GetRequiredService<ILoggerFactory>().CreateLogger("ScaleBrands");
    var localPath = Path.Combine(AppContext.BaseDirectory, "scale-models.json");

    // Load local first (fast), then try remote to update
    var config = builder.Configuration.GetSection("Scale");
    var brandsUrl = config["BrandsUrl"] ?? "";
    var brandsToken = config["BrandsToken"] ?? "";

    var brands = ScaleBrandDefinition.LoadBrandsAsync(brandsUrl, localPath, brandsToken, logger)
        .GetAwaiter().GetResult();
    return brands;
});

// HTTP client for calling web API
builder.Services.AddHttpClient("BasicWeighApi", client =>
{
    var serverUrl = builder.Configuration["Scale:ServerUrl"] ?? "http://localhost:5110";
    client.BaseAddress = new Uri(serverUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
});

// SMA client (scale protocol handler)
builder.Services.AddSingleton<SmaClient>();

// Restart signal (triggered when settings change via API)
builder.Services.AddSingleton<RestartSignal>();

// Announce signal (triggered when scales change via API)
builder.Services.AddSingleton<AnnounceSignal>();

// In-memory weight store (latest reading per scale)
builder.Services.AddSingleton<ScaleWeightStore>();

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Scale Reader Service API",
        Version = "v1",
        Description = "Health, status, configuration, and scale management endpoints for the Scale Reader Service."
    });
});

// Background worker
builder.Services.AddHostedService<ScaleWorker>();

// Event Log on Windows
if (OperatingSystem.IsWindows())
{
    builder.Logging.AddEventLog(new EventLogSettings
    {
        SourceName = "ScaleReaderService"
    });
}

var app = builder.Build();

// Auto-create/migrate database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ScaleDbContext>();
    db.Database.EnsureCreated();

    // Seed from appsettings.json if no scales exist yet
    if (!db.Scales.Any())
    {
        var scalesConfig = builder.Configuration.GetSection("Scales").Get<List<ScaleOptions>>();
        if (scalesConfig != null)
        {
            foreach (var s in scalesConfig)
            {
                db.Scales.Add(new ScaleConfigEntity
                {
                    ScaleId = $"scale-{s.Id}",
                    DisplayName = s.Description,
                    ScaleBrand = "Generic SMA",
                    IpAddress = s.IpAddress,
                    Port = s.Port,
                    Active = true
                });
            }
            db.SaveChanges();
        }
    }

    // Update settings from appsettings if ServerUrl changed
    var settings = db.Settings.OrderBy(s => s.Id).FirstOrDefault();
    if (settings != null)
    {
        var configUrl = builder.Configuration["Scale:ServerUrl"];
        if (!string.IsNullOrWhiteSpace(configUrl) && settings.ServerUrl == "http://localhost:5110")
        {
            settings.ServerUrl = configUrl;
            db.SaveChanges();
        }
    }
}

// Swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Scale Reader Service API v1");
});

app.MapControllers();

// Startup banner
var urls = builder.Configuration["Urls"] ?? "http://localhost:5220";
var logger2 = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("ScaleReaderService");
logger2.LogInformation("============================================");
logger2.LogInformation("  Scale Reader Service v{Version}", version);
logger2.LogInformation("  Swagger: {Urls}/swagger", urls);
logger2.LogInformation("============================================");

await app.RunAsync();
