using Agvantage_Transfer.AtLogModels;
using Agvantage_TransferV2;
using Agvantage_TransferV2.GmModels;
using Agvantage_TransferV2.Logging;
using Agvantage_TransferV2.Sync;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;


var builder = Host.CreateApplicationBuilder(args);
builder.Environment.ContentRootPath = AppContext.BaseDirectory;
if (OperatingSystem.IsWindows())
{
    builder.Services.AddWindowsService(options =>
    {
        options.ServiceName = "Agvantage TransferV2";
    });

    // Log to Windows Event Log when running on Windows
    builder.Logging.AddEventLog();
}
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddDbContext<GMDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("GmDbContext")));

builder.Services.AddDbContextFactory<GMDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("GmDbContext")));

builder.Services.AddDbContext<AtLogDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("AtLogDbContext")));
builder.Services.AddDbContextFactory<AtLogDbContext>(o => o.UseSqlServer(builder.Configuration.GetConnectionString("AtLogDbContext")));
//builder.Services.AddScoped<ICarrierSyncService, CarrierSyncService>();
builder.Services.AddScoped<IAccountSyncService, AccountSyncService>();
//builder.Services.AddScoped<ICropSyncService, CropSyncService>();
//builder.Services.AddScoped<ISeedSyncService, SeedSyncService>();


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
