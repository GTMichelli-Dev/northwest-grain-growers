namespace GrainManagement.As400Sync;

public sealed class As400SyncOptions
{
    public string SourceSystem { get; set; } = "AS400";
    public int BatchSize { get; set; } = 500;
    public int RunEveryMinutes { get; set; } = 60;

    // Auto-update flags. Defaults are all false so the service is idle until
    // an admin triggers a job from the website (or flips a flag in appsettings).
    public bool SyncAccounts { get; set; } = false;
    public bool SyncProducts { get; set; } = false;
    public bool SyncSplitGroups { get; set; } = false;

    // SignalR hub the service connects to so the admin page can drive it.
    public string HubUrl { get; set; } = "";
    public string ServiceId { get; set; } = "as400sync";
}
