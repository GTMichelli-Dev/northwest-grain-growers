namespace GrainManagement.As400Sync;

public sealed class As400SyncOptions
{
    public string SourceSystem { get; set; } = "AS400";
    public int BatchSize { get; set; } = 500;
    public int RunEveryMinutes { get; set; } = 60;
}

