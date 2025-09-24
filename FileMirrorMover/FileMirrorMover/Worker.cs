using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public sealed class Worker : BackgroundService
{
    private readonly ILogger<Worker> _log;
    private readonly MirrorOptions _opt;
    private readonly BlockingCollection<string> _queue = new(new ConcurrentQueue<string>());
    private FileSystemWatcher? _watcher;
    private readonly List<Task> _workers = new();

    public Worker(ILogger<Worker> log, IOptions<MirrorOptions> opt)
    {
        _log = log;
        _opt = opt.Value;
      
    }









    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await EnsureDirectoryExistsAsync(_opt.SourceFolder, stoppingToken);
        await EnsureDirectoryExistsAsync(_opt.LocalTarget, stoppingToken);
        await EnsureDirectoryExistsAsync(_opt.NetworkTarget, stoppingToken);

        _log.LogInformation("Starting watcher on {src}", _opt.SourceFolder);

        _watcher = new FileSystemWatcher(_opt.SourceFolder)
        {
            IncludeSubdirectories = _opt.IncludeSubdirectories,
            EnableRaisingEvents = true,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size
        };

        foreach (var pattern in ExpandFilters(_opt.Filter))
        {
            // One watcher can only take one filter; create a child watcher per pattern
            var w = new FileSystemWatcher(_opt.SourceFolder, pattern)
            {
                IncludeSubdirectories = _opt.IncludeSubdirectories,
                EnableRaisingEvents = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size
            };
            w.Created += OnEvent;
            w.Changed += OnEvent;
            w.Renamed += OnRenamed;
            w.Error += (_, e) => _log.LogError(e.GetException(), "FileSystemWatcher error");
            // hold a reference so GC doesn’t collect
            _childWatchers.Add(w);
        }

        // Start workers
        for (int i = 0; i < Math.Max(1, _opt.MaxConcurrency); i++)
        {
            _workers.Add(Task.Run(() => WorkerLoop(stoppingToken), stoppingToken));
        }

        // Optional: Pre-connect to share with credentials
        if (!string.IsNullOrWhiteSpace(_opt.NetworkUser) && !string.IsNullOrWhiteSpace(_opt.NetworkPassword))
        {
            TryConnectShare(_opt.NetworkTarget, _opt.NetworkUser!, _opt.NetworkPassword!);
        }

        await Task.CompletedTask;
    }


    private async Task EnsureDirectoryExistsAsync(string path, CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    // quick sanity: try creating to ensure permission (optional)
                    // Directory.CreateDirectory(path);
                    _log.LogInformation("Directory ready: {path}", path);
                    return;
                }

                Directory.CreateDirectory(path);
                _log.LogInformation("Created directory: {path}", path);
                return;
            }
            catch (Exception ex)
            {
                // Log and retry in 1s until canceled
                _log.LogWarning(ex, "Failed to ensure directory {path}. Retrying in 1s...", path);
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(1), ct);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        _log.LogError("Stopped trying to create {path} due to cancellation.", path);
    }

    private readonly List<FileSystemWatcher> _childWatchers = new();

    private static IEnumerable<string> ExpandFilters(string filter)
    {
        if (string.IsNullOrWhiteSpace(filter)) yield return "*.*";
        else
        {
            foreach (var part in filter.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                yield return part;
        }
    }

    private void OnEvent(object sender, FileSystemEventArgs e)
    {
        if (Directory.Exists(e.FullPath)) return;
        EnqueueUnique(e.FullPath);
    }

    private void OnRenamed(object sender, RenamedEventArgs e)
    {
        if (Directory.Exists(e.FullPath)) return;
        EnqueueUnique(e.FullPath);
    }

    private readonly ConcurrentDictionary<string, byte> _seen = new(StringComparer.OrdinalIgnoreCase);

    private void EnqueueUnique(string path)
    {
        // Deduplicate bursts
        if (_seen.TryAdd(path, 1))
        {
            _queue.Add(path);
        }
    }

    private async Task WorkerLoop(CancellationToken ct)
    {
        foreach (var path in _queue.GetConsumingEnumerable(ct))
        {
            try
            {
                // Wait until file is stable and unlocked
                if (!await WaitUntilReady(path, ct))
                {
                    _log.LogWarning("File not ready after retries: {file}", path);
                    _seen.TryRemove(path, out _);
                    continue;
                }

                // Compute destinations
                var fileName = Path.GetFileName(path);
                var localDest = Path.Combine(_opt.LocalTarget, fileName);
                var netDest = Path.Combine(_opt.NetworkTarget, fileName);

                // Ensure dirs
                Directory.CreateDirectory(_opt.LocalTarget);
                Directory.CreateDirectory(_opt.NetworkTarget);

                // Copy with retry
                bool localOk = await CopyWithRetry(path, localDest, ct);
                bool netOk = await CopyWithRetry(path, netDest, ct);

                if (localOk && netOk)
                {
                    TryDelete(path);
                    _log.LogInformation("Moved: {file} -> {l} & {n}", path, localDest, netDest);
                }
                else
                {
                    _log.LogError("Failed to move {file}. LocalOk={localOk}, NetOk={netOk}", path, localOk, netOk);
                    // keep source file; will re-enqueue on next change OR you can choose to requeue explicitly:
                    // _queue.Add(path);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Unhandled during move for {file}", path);
            }
            finally
            {
                _seen.TryRemove(path, out _);
            }
        }
    }

    private async Task<bool> WaitUntilReady(string path, CancellationToken ct)
    {
        for (int i = 0; i < _opt.ReadyCheckAttempts; i++)
        {
            if (ct.IsCancellationRequested) return false;
            if (File.Exists(path))
            {
                try
                {
                    using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                    // If we can open exclusive, the file is ready
                    return true;
                }
                catch (IOException)
                {
                    // still locked/writing
                }
                catch (UnauthorizedAccessException)
                {
                    // transient
                }
            }
            await Task.Delay(_opt.ReadyCheckDelayMs, ct);
        }
        return false;
    }

    private async Task<bool> CopyWithRetry(string src, string dest, CancellationToken ct)
    {
        for (int i = 0; i < _opt.CopyRetryCount; i++)
        {
            try
            {
                // Copy to a temp, then atomic replace to prevent partial reads downstream
                string tmp = dest + ".tmp_" + Guid.NewGuid().ToString("N");
                using (var inStream = new FileStream(src, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (var outStream = new FileStream(tmp, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await inStream.CopyToAsync(outStream, ct);
                }

                // Ensure destination directory still exists
                Directory.CreateDirectory(Path.GetDirectoryName(dest)!);

                // If dest exists, replace; else move
                if (File.Exists(dest))
                {
                    File.Replace(tmp, dest, null);
                }
                else
                {
                    File.Move(tmp, dest);
                }
                return true;
            }
            catch (OperationCanceledException) { throw; }
            catch (Exception ex)
            {
                _log.LogWarning("Copy attempt {try}/{max} failed {src} -> {dest}: {msg}",
                    i + 1, _opt.CopyRetryCount, src, dest, ex.Message);
                await Task.Delay(_opt.CopyRetryDelayMs, ct);
            }
        }
        return false;
    }

    private void TryDelete(string path)
    {
        try
        {
            File.Delete(path);
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Failed to delete source {file}", path);
        }
    }

    // ---- Optional: connect to UNC using credentials ----
    private void TryConnectShare(string uncPath, string user, string password)
    {
        try
        {
            var netResource = new NETRESOURCE
            {
                dwType = 1, // RESOURCETYPE_DISK
                lpRemoteName = uncPath
            };

            var result = WNetAddConnection2(ref netResource, password, user, 0);
            if (result != 0 && result != 1219 /*already connected*/)
            {
                _log.LogWarning("WNetAddConnection2 failed {code} for {unc}", result, uncPath);
            }
            else
            {
                _log.LogInformation("Connected to {unc}", uncPath);
            }
        }
        catch (Exception ex)
        {
            _log.LogWarning(ex, "Could not pre-connect to {unc}", uncPath);
        }
    }

    [DllImport("mpr.dll", CharSet = CharSet.Unicode)]
    private static extern int WNetAddConnection2(ref NETRESOURCE netResource, string? password, string? username, int flags);

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct NETRESOURCE
    {
        public int dwScope;
        public int dwType;
        public int dwDisplayType;
        public int dwUsage;
        public string? lpLocalName;
        public string? lpRemoteName;
        public string? lpComment;
        public string? lpProvider;
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        foreach (var w in _childWatchers) w.EnableRaisingEvents = false;
        _queue.CompleteAdding();
        await Task.WhenAll(_workers.ToArray());
        foreach (var w in _childWatchers) w.Dispose();
        _watcher?.Dispose();
        await base.StopAsync(cancellationToken);
    }
}
