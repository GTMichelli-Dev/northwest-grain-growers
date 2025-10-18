using Agvantage_Transfer.AtModels;
using Agvantage_Transfer.Logging;
using Agvantage_Transfer.NwModels;
using Agvantage_Transfer.SeedModels;
using Agvantage_Transfer.Sync;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Agvantage_Transfer;

public sealed class AgvantageTransfer(
    AtDbContext dbContext, 
    ITransferLogger log,

    ExcelImport excel,
    NW_DataContext nwDb,
    ICarrierSyncService carrierSync,
     IProducerSyncService producerSync,
     ICropSyncService cropSync,
     ISeedSyncService seedSync,
    IOptions<AppSettings> options)
{
    private readonly ISeedSyncService _seedSync = seedSync;
    private readonly ICropSyncService _cropSync = cropSync;
    private readonly IProducerSyncService _producerSync = producerSync;
    private readonly AppSettings _cfg = options.Value;
    private readonly AtDbContext _db = dbContext;
    private readonly ITransferLogger _log = log;
    private readonly NW_DataContext _nw = nwDb;
    private readonly ICarrierSyncService _carrierSync = carrierSync;

    public bool TransferStarted { get; private set; }
    public enum ProcessState { Idle, Running, TimedOut }
    public ProcessState State { get; private set; } = ProcessState.Idle;

    private Process? _process;

    public async Task StartTransferAsync(
        string batchFile, string completedFilePath,
        TimeSpan timeout, TimeSpan updateInterval, CancellationToken ct)
    {
        if (TransferStarted)
        {
            await _log.WarnAsync("Cannot start transfer because it is already started","System");
            return;
        }

        TransferStarted = true;
        //try { await ProcessAgvantageDataAsync(completedFilePath); }
        try { await StartProcessAsync(batchFile, completedFilePath, timeout, updateInterval, ct); }
        finally { TransferStarted = false; State = ProcessState.Idle; }
    }

    private async Task StartProcessAsync(
        string batchFile, string completedFilePath,
        TimeSpan timeout, TimeSpan updateInterval, CancellationToken ct)
    {
        // Example: keep your log table trim logic if desired
        var allLogs = await _db.AgvantageTransferLogs.ToListAsync();
        _db.AgvantageTransferLogs.RemoveRange(allLogs);
        await _db.SaveChangesAsync();

        await _log.InfoAsync("Starting Transfer", "System");

        if (State == ProcessState.Running) return;

        await _log.InfoAsync("Deleting Existing Excel Files", "System");
        if (!Directory.Exists(completedFilePath))
            Directory.CreateDirectory(completedFilePath);
        else
            foreach (var f in Directory.EnumerateFiles(completedFilePath))
                try { File.Delete(f); } catch { }

        if (string.IsNullOrWhiteSpace(batchFile))
            throw new ArgumentException("BatchFile path is empty.");

        var workDir = Path.GetDirectoryName(batchFile) ?? Environment.CurrentDirectory;
        var batchExists = File.Exists(batchFile);
        var workDirExists = Directory.Exists(workDir);

        await _log.InfoAsync("Preflight check...", "System");
        await _log.InfoAsync($"BatchFile: {batchFile}", "System");
        await _log.InfoAsync($"WorkingDir: {workDir}", "System");
        await _log.InfoAsync($"Exists(BatchFile)={batchExists}", "System");
        await _log.InfoAsync($"Exists(WorkingDir)={workDirExists}", "System");

        if (!batchExists) throw new FileNotFoundException($"Batch file not found: {batchFile}", "System");
        if (!workDirExists) throw new DirectoryNotFoundException($"Working directory not found: {workDir}");

        _process = new()
        {
            StartInfo = new()
            {
                FileName = "cmd.exe",
                Arguments = $"/c \"{batchFile}\"",
                WorkingDirectory = workDir,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            },
            EnableRaisingEvents = true
        };

        _process.OutputDataReceived += async (_, e) => { if (!string.IsNullOrEmpty(e.Data)) await _log.InfoAsync(e.Data!, "System"); };
        _process.ErrorDataReceived += async (_, e) => { if (!string.IsNullOrEmpty(e.Data)) await _log.WarnAsync(e.Data!, "System"); };

        await _log.InfoAsync("Logging on to Agvantage Server", "System");
        _process.Start();
        State = ProcessState.Running;
        _process.BeginOutputReadLine();
        _process.BeginErrorReadLine();

        using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
        var exitedTask = WaitForExitAsync(_process, cts.Token);
        var timeoutTask = Task.Delay(timeout, cts.Token);

        var completed = await Task.WhenAny(exitedTask, timeoutTask);
        if (completed == timeoutTask)
        {
            State = ProcessState.TimedOut;
            TryKillProcess();
            await _log.WarnAsync("Agvantage Connection Timed Out", "System");
            return;
        }

        await exitedTask;
        if (State != ProcessState.TimedOut)
            await ProcessAgvantageDataAsync(completedFilePath);

        await _log.InfoAsync("Transfer Complete", "System");
        await _log.InfoAsync($"Next Update {DateTime.Now.Add(updateInterval)}", "System");
        State = ProcessState.Idle;
    }

    private static Task WaitForExitAsync(Process p, CancellationToken ct)
    {
        var tcs = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);
        void Handler(object? s, EventArgs e) { p.Exited -= Handler; tcs.TrySetResult(null); }
        p.Exited += Handler;
        if (p.HasExited) { p.Exited -= Handler; tcs.TrySetResult(null); }
        if (ct.CanBeCanceled) ct.Register(() => { p.Exited -= Handler; tcs.TrySetCanceled(ct); });
        return tcs.Task;
    }

    private void TryKillProcess() { try { _process?.Kill(entireProcessTree: true); } catch { } }

    public async Task ProcessAgvantageDataAsync(string xlsxFilePath)
    {
        xlsxFilePath = (xlsxFilePath ?? "").Trim();
        if (!xlsxFilePath.EndsWith('\\')) xlsxFilePath += "\\";
        await _log.InfoAsync("Updating Carriers", "Nw_Data - Carriers");
        var carriers = await excel.LoadCarriersAsync($"{_cfg.CompletedFilePath}\\Carriers.xlsx");
        await _log.InfoAsync($"Worker loaded {carriers.Count} carriers", "Nw_Data - Carriers");
        await _carrierSync.UpsertAsync(carriers);
        await _log.InfoAsync("Updating Producers", "NW_Data - Producers");
        var producers = await excel.LoadProducersAsync($"{_cfg.CompletedFilePath}\\Customers.xlsx");
        await _log.InfoAsync($"Worker loaded {producers.Count} producers", "NW_Data - Producers");
        await _producerSync.UpsertAsync(producers);

        await _log.InfoAsync("Updating Crops", "NW_Data - Crops");
        var crops = await excel.LoadCropsAsync($"{_cfg.CompletedFilePath}\\Crops.xlsx");
        await _log.InfoAsync($"Worker loaded {crops.Count} Crops" ,"NW_Data - Crops");
        await _cropSync.UpsertAsync(crops);

        await _log.InfoAsync("Updating Seed Master", "Seed - Items");
        var seedItems = await excel.LoadItemsAsync($"{_cfg.CompletedFilePath}\\SeedItemMasterFile.xlsx");
        await _log.InfoAsync($"Worker loaded {seedItems.Count} Seed Master Items", "Seed - Items");
        await _seedSync.UpsertSeedItemsAsync(seedItems);

        await _log.InfoAsync("Updating Seed Item Location Pricing", "Seed - ItemLocation");
        var seedItemLocations = await excel.LoadItemLocationsAsync($"{_cfg.CompletedFilePath}\\SeedItemLocationPrice.xlsx");
        await _log.InfoAsync($"Worker loaded {seedItemLocations.Count} Seed - ItemLocations", "Seed - ItemLocation");
        await _seedSync.UpsertSeedItemLocationAsync(seedItemLocations);


        await _log.InfoAsync("Updating Seed Deptartments"," Seed - SeedDeptartments");
        var seedDepartments = await excel.LoadSeedDepartmentsAsync($"{_cfg.CompletedFilePath}\\SeedDept.xlsx");

        await _log.InfoAsync($"Worker loaded {seedDepartments.Count} SeedDeptartments"," Seed - SeedDeptartments");
        await _seedSync.UpsertSeedDepartmentsAsync(seedDepartments);



    }


  
    /// <summary>
    /// Logs details from a DbUpdateException, including entity states that caused the error.
    /// </summary>
    private async Task LogDbUpdateExceptionAsync(string message, DbUpdateException ex)
    {
        try
        {
            var entries = ex.Entries?.Select(e =>
            {
                // Build a safe dictionary of current values
                var values = new Dictionary<string, string>();

                if (e.CurrentValues != null)
                {
                    foreach (var p in e.CurrentValues.Properties)
                    {
                        var v = e.CurrentValues[p];
                        values[p.Name] = v?.ToString() ?? "<null>";
                    }
                }

                return new
                {
                    Entity = e.Entity?.GetType().Name ?? "<unknown>",
                    State = e.State.ToString(),
                    Values = values
                };
            }).ToList();

            await _log.ErrorAsync($"{message}: {ex.Message}","System");

            if (entries != null && entries.Count > 0)
            {
                foreach (var entry in entries)
                {
                    var kvPairs = (entry.Values ?? new Dictionary<string, string>())
                        .Select(kv => kv.Key + "=" + kv.Value); // safe: never null source now

                    await _log.ErrorAsync(
                        $"Entity={entry.Entity}, State={entry.State}, Values={string.Join(", ", kvPairs)}","System");
                }
            }
        }
        catch
        {
            // best-effort logging; avoid throwing from logger
            await _log.ErrorAsync($"{message} (additional entry logging failed): {ex.Message}", "System");
        }
    }


}
