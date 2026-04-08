using Microsoft.EntityFrameworkCore;
using WebPrintService;
using WebPrintService.Data;
using WebPrintService.Services;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "Web Print Service", Version = "v1" });
});
builder.Services.AddControllers();

// SQLite
builder.Services.AddDbContext<PrintDbContext>(opt =>
    opt.UseSqlite("Data Source=webprintservice.db"));

// Services — register the right print client based on OS
if (OperatingSystem.IsWindows())
{
    builder.Services.AddSingleton<IPrintClient, WindowsPrintClient>();
}
else
{
    builder.Services.AddSingleton<IPrintClient, CupsClient>();
}
builder.Services.AddSingleton<RestartSignal>();
builder.Services.AddHostedService<PrintWorker>();
builder.Services.AddHttpClient();

// Logging — suppress EF command logs
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Query", LogLevel.Warning);

var app = builder.Build();

// Auto-migrate and seed
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PrintDbContext>();
    db.Database.EnsureCreated();

    // Seed default settings if empty
    if (!db.Settings.Any())
    {
        // Build comma-separated URL list from config array (or fall back to single URL)
        var urls = builder.Configuration.GetSection("Print:ServerUrls").Get<string[]>();
        var serverUrl = urls != null && urls.Length > 0
            ? string.Join(",", urls)
            : builder.Configuration["Print:ServerUrl"] ?? "http://localhost:5000";

        db.Settings.Add(new ServiceSettings
        {
            ServiceId = "default",
            ServerUrl = serverUrl,
            SignalRHub = builder.Configuration["Print:SignalRHub"] ?? "/hubs/print"
        });
        db.SaveChanges();
    }
}

// Banner
var port = builder.Configuration["Print:Port"] ?? "5230";
app.Urls.Add($"http://*:{port}");

var version = typeof(Program).Assembly.GetName().Version?.ToString(3) ?? "0.0.0";
var startLog = app.Services.GetRequiredService<ILogger<Program>>();
var printSystem = OperatingSystem.IsWindows() ? "Windows Print" : "CUPS (Linux/macOS)";

Console.WriteLine();
Console.WriteLine("  =============================================");
Console.WriteLine($"   Web Print Service  v{version}");
Console.WriteLine($"   Print System:  {printSystem}");
Console.WriteLine($"   Listening on:  http://localhost:{port}");
Console.WriteLine($"   Swagger:       http://localhost:{port}/swagger");
Console.WriteLine("  =============================================");
Console.WriteLine();

startLog.LogInformation("Web Print Service v{Version} started. PrintSystem={PrintSystem} Port={Port}",
    version, printSystem, port);

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
