using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

// Ensure EventLog source exists before host builds
#if WINDOWS
string logName = "FileMirrorMover";
string sourceName = "FileMirrorMover";

if (!EventLog.SourceExists(sourceName))
{
    EventLog.CreateEventSource(new EventSourceCreationData(sourceName, logName));
}
#endif

Host.CreateDefaultBuilder(args)
    .UseWindowsService(options => options.ServiceName = "FileMirrorMover")
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddSimpleConsole(opt =>
        {
            opt.SingleLine = true;
            opt.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
        });

#if WINDOWS
        logging.AddEventLog(settings =>
        {
            settings.SourceName = "FileMirrorMover";
            settings.LogName = "FileMirrorMover"; 
        });
#endif
    })
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<MirrorOptions>(ctx.Configuration.GetSection("MirrorOptions"));
        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();

public sealed class MirrorOptions
{
    public string SourceFolder { get; set; } = @"C:\Uploads";
    public string LocalTarget { get; set; } = @"C:\Images";
    public string NetworkTarget { get; set; } = @"\\walws001\Images";
    public string Filter { get; set; } = "*.*";
    public bool IncludeSubdirectories { get; set; } = false;
    public int MaxConcurrency { get; set; } = 2;
    public int CopyRetryCount { get; set; } = 10;
    public int CopyRetryDelayMs { get; set; } = 750;
    public int ReadyCheckAttempts { get; set; } = 20;
    public int ReadyCheckDelayMs { get; set; } = 500;
    public string? NetworkUser { get; set; }
    public string? NetworkPassword { get; set; }
}
