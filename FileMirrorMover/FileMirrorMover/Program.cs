using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
        // For EventLog (optional):
        // logging.AddEventLog(settings => settings.SourceName = "FileMirrorMover");
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
    public string Filter { get; set; } = "*.*";         // e.g., "*.png;*.jpg"
    public bool IncludeSubdirectories { get; set; } = false;
    public int MaxConcurrency { get; set; } = 2;
    public int CopyRetryCount { get; set; } = 10;
    public int CopyRetryDelayMs { get; set; } = 750;
    public int ReadyCheckAttempts { get; set; } = 20;
    public int ReadyCheckDelayMs { get; set; } = 500;
    public string? NetworkUser { get; set; }            // e.g. "DOMAIN\\svc_files"
    public string? NetworkPassword { get; set; }
}
