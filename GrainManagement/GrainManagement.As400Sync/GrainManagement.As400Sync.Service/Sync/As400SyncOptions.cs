public sealed class As400SyncOptions
{
    public string SourceSystem { get; set; } = "AS400";
    public int BatchSize { get; set; } = 500;
    public int RunEveryMinutes { get; set; } = 60;

    public bool SyncAccounts { get; set; } = true;
    public bool SyncProducts { get; set; } = false;

    public bool SyncSplitGroups { get; set; } = false;


}
