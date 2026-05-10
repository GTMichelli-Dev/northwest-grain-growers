using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.EventLog;
using Microsoft.OpenApi.Models;
using PictureUpsert.Data;
using PictureUpsert.Services;

var builder = WebApplication.CreateBuilder(args);

// Run as a Windows Service when installed; harmless when launched as console.
builder.Services.AddWindowsService(o => o.ServiceName = "GrainManagement Picture Upsert");

// SQLite — owns config + queue. Same folder as the binary so an in-place
// upgrade keeps the queue intact.
var dbPath = Path.Combine(AppContext.BaseDirectory, "pictureupsert.db");
builder.Services.AddDbContext<UpsertDbContext>(o => o.UseSqlite($"Data Source={dbPath}"));

builder.Services.AddHttpClient();
builder.Services.AddSingleton<StatusState>();
builder.Services.AddHostedService<UpsertWorker>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Picture Upsert Service API",
        Version = "v1",
        Description = "Status, settings, and queue management for the picture-upsert worker."
    });
});

if (OperatingSystem.IsWindows())
{
    builder.Logging.AddEventLog(new EventLogSettings { SourceName = "PictureUpsert" });
}

var app = builder.Build();

// Bring the DB online before the worker grabs it
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<UpsertDbContext>();
    db.Database.EnsureCreated();
    if (!db.Settings.Any())
    {
        db.Settings.Add(new PictureUpsert.Models.UpsertSettings { Id = 1 });
        db.SaveChanges();
    }
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Picture Upsert Service API v1");
    c.RoutePrefix = "swagger";
});

app.MapControllers();

var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("PictureUpsert");
var urls = builder.Configuration["Urls"] ?? "http://localhost:5310";
logger.LogInformation("============================================");
logger.LogInformation("  Picture Upsert Service");
logger.LogInformation("  Swagger: {Urls}/swagger", urls);
logger.LogInformation("  DB:      {Db}", dbPath);
logger.LogInformation("============================================");

await app.RunAsync();
