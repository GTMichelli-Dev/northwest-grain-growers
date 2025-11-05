using Agvantage_Transfer;
using Agvantage_Transfer.AtModels;
using Agvantage_Transfer.Logging;
using Agvantage_Transfer.NwModels;
using Agvantage_Transfer.SeedModels;
using Agvantage_Transfer.Sync;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting.WindowsServices;


var builder = Host.CreateApplicationBuilder(args);
builder.Environment.ContentRootPath = AppContext.BaseDirectory;
if (OperatingSystem.IsWindows())
{
    builder.Services.AddWindowsService(options =>
    {
        options.ServiceName = "Agvantage Transfer";
    });

    // Log to Windows Event Log when running on Windows
    builder.Logging.AddEventLog();
}
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddDbContext<NW_DataContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("NW_DataContext")));
builder.Services.AddDbContext<AtDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("AtDbContext")));
builder.Services.AddDbContext<Seed_DataContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Seed_DataContext")));

builder.Services.AddDbContextFactory<AtDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("AtDbContext")));

builder.Services.AddScoped<ICarrierSyncService, CarrierSyncService>();
builder.Services.AddScoped<IProducerSyncService, ProducerSyncService>();
builder.Services.AddScoped<ICropSyncService, CropSyncService>();
builder.Services.AddScoped<ISeedSyncService, SeedSyncService>();


// Logging & app services
builder.Services.AddScoped<ITransferLogger, TransferLogger>();   
builder.Services.AddScoped<ExcelImport>();                    
builder.Services.AddScoped<AgvantageTransfer>();
builder.Services.AddHostedService<Worker>();

builder.Logging.ClearProviders();
builder.Logging.AddSimpleConsole(o => { o.SingleLine = true; o.TimestampFormat = "yyyy-MM-dd HH:mm:ss "; });

var tmp = builder.Configuration.GetSection("AppSettings").Get<AppSettings>();
var app = builder.Build();
app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("Config")
    .LogInformation("Config check — BatchFile: {BatchFile}, CompletedFilePath: {CompletedFilePath}",
        tmp?.BatchFile, tmp?.CompletedFilePath);

await app.RunAsync();
