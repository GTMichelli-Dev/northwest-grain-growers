using System.Collections.Concurrent;
using System.Net.Http.Headers;
using Microsoft.EntityFrameworkCore;
using PictureUpsert.Data;
using PictureUpsert.Models;

namespace PictureUpsert.Services;

/// <summary>
/// Watches a local folder for new ticket images and pushes each one to a
/// remote ingestion API. On remote outage the queue stays in SQLite and
/// resumes when the remote comes back — never drops a file. Modeled on
/// FileMirrorMover's bursty-watcher pattern, but the destination is an
/// HTTP endpoint (Central HQ) instead of a UNC share.
/// </summary>
public sealed class UpsertWorker : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IHttpClientFactory _httpFactory;
    private readonly ILogger<UpsertWorker> _logger;
    private readonly StatusState _status;

    private readonly ConcurrentQueue<string> _enqueue = new();
    private FileSystemWatcher? _watcher;

    public UpsertWorker(
        IServiceScopeFactory scopeFactory,
        IHttpClientFactory httpFactory,
        ILogger<UpsertWorker> logger,
        StatusState status)
    {
        _scopeFactory = scopeFactory;
        _httpFactory  = httpFactory;
        _logger       = logger;
        _status       = status;
    }

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await EnsureDbAsync(ct);

        var settings = await LoadSettingsAsync();
        Directory.CreateDirectory(settings.LocalFolder);
        StartWatcher(settings);

        // Pick up any pre-existing files we haven't enqueued yet (recovers
        // from a stop+restart where files landed while we were down).
        foreach (var pattern in ExpandFilters(settings.Filter))
        {
            foreach (var path in Directory.EnumerateFiles(settings.LocalFolder, pattern, SearchOption.TopDirectoryOnly))
                _enqueue.Enqueue(path);
        }

        // Reconciliation loop — handles both new arrivals AND retrying failed/pending items.
        while (!ct.IsCancellationRequested)
        {
            try
            {
                // Drain the in-memory queue into the DB so it survives crashes.
                await PersistEnqueuedAsync();

                // Process Pending + Failed (the latter on retry) one at a time.
                settings = await LoadSettingsAsync();
                var batch = await NextBatchAsync(20);
                if (batch.Count == 0)
                {
                    await UpdateStatusCountsAsync();
                    await Task.Delay(TimeSpan.FromSeconds(2), ct);
                    continue;
                }

                foreach (var item in batch)
                {
                    if (ct.IsCancellationRequested) break;
                    await TryUploadAsync(item, settings, ct);
                }
            }
            catch (OperationCanceledException) { break; }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled error in upsert loop; backing off.");
                try { await Task.Delay(TimeSpan.FromSeconds(5), ct); } catch { break; }
            }
        }

        _watcher?.Dispose();
    }

    private void StartWatcher(UpsertSettings s)
    {
        _watcher = new FileSystemWatcher(s.LocalFolder)
        {
            IncludeSubdirectories = false,
            EnableRaisingEvents = true,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size
        };
        _watcher.Created += (_, e) => _enqueue.Enqueue(e.FullPath);
        _watcher.Renamed += (_, e) => _enqueue.Enqueue(e.FullPath);
        _watcher.Error   += (_, e) => _logger.LogError(e.GetException(), "Watcher error.");
        _logger.LogInformation("Watching {Folder} (filters: {Filter}).", s.LocalFolder, s.Filter);
    }

    private async Task PersistEnqueuedAsync()
    {
        if (_enqueue.IsEmpty) return;
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UpsertDbContext>();

        while (_enqueue.TryDequeue(out var path))
        {
            if (!File.Exists(path)) continue;
            // Wait briefly for the file to be unlocked
            if (!await WaitUntilReadyAsync(path)) { _enqueue.Enqueue(path); break; }

            var existing = await db.Queue.FirstOrDefaultAsync(q => q.FilePath == path);
            if (existing is null)
            {
                var info = new FileInfo(path);
                db.Queue.Add(new UpsertQueueItem
                {
                    FilePath = path,
                    FileName = info.Name,
                    FileSize = info.Length,
                    Status = "Pending"
                });
            }
            else if (existing.Status == "Failed")
            {
                existing.Status = "Pending"; // reset for retry
                existing.LastError = null;
            }
        }
        await db.SaveChangesAsync();
    }

    private async Task<List<UpsertQueueItem>> NextBatchAsync(int n)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UpsertDbContext>();
        return await db.Queue
            .Where(q => q.Status == "Pending" || q.Status == "Failed")
            .OrderBy(q => q.EnqueuedUtc)
            .Take(n)
            .ToListAsync();
    }

    private async Task TryUploadAsync(UpsertQueueItem item, UpsertSettings settings, CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UpsertDbContext>();
        var tracked = await db.Queue.FirstAsync(q => q.Id == item.Id);
        tracked.AttemptCount++;
        tracked.LastAttemptUtc = DateTime.UtcNow;

        if (!File.Exists(tracked.FilePath))
        {
            tracked.Status = "Sent"; // file vanished — drop from queue
            tracked.LastError = "File no longer exists";
            await db.SaveChangesAsync();
            return;
        }

        try
        {
            await using var fs = new FileStream(tracked.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            using var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(fs);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(GuessMime(tracked.FileName));
            content.Add(fileContent, "file", tracked.FileName);

            var client = _httpFactory.CreateClient("Remote");
            client.BaseAddress = new Uri(settings.RemoteBaseUrl.TrimEnd('/'));
            client.Timeout = TimeSpan.FromSeconds(30);

            using var req = new HttpRequestMessage(HttpMethod.Post, settings.RemoteUploadPath)
            {
                Content = content
            };
            if (!string.IsNullOrWhiteSpace(settings.AuthToken))
                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", settings.AuthToken);

            using var resp = await client.SendAsync(req, ct);
            if (resp.IsSuccessStatusCode)
            {
                tracked.Status = "Sent";
                tracked.SentUtc = DateTime.UtcNow;
                tracked.LastError = null;
                await db.SaveChangesAsync();

                if (settings.DeleteAfterUpload)
                {
                    try { File.Delete(tracked.FilePath); }
                    catch (Exception ex) { _logger.LogWarning(ex, "Failed to delete {File} after upload.", tracked.FilePath); }
                }

                var (pending, failed) = await CountsAsync();
                _status.RecordSuccess(pending, failed);
                _logger.LogInformation("Uploaded {File} ({Bytes} bytes).", tracked.FileName, tracked.FileSize);
            }
            else
            {
                tracked.Status = "Failed";
                tracked.LastError = $"HTTP {(int)resp.StatusCode} {resp.ReasonPhrase}";
                await db.SaveChangesAsync();
                var (pending, failed) = await CountsAsync();
                _status.RecordFailure(tracked.LastError ?? "unknown", pending, failed);
                _logger.LogWarning("Upload failed for {File}: {Err}. Will retry after {Delay}s.",
                    tracked.FileName, tracked.LastError, settings.RetryDelaySeconds);
                try { await Task.Delay(TimeSpan.FromSeconds(settings.RetryDelaySeconds), ct); } catch { }
            }
        }
        catch (Exception ex)
        {
            tracked.Status = "Failed";
            tracked.LastError = ex.Message;
            await db.SaveChangesAsync();
            var (pending, failed) = await CountsAsync();
            _status.RecordFailure(ex.Message, pending, failed);
            _logger.LogWarning(ex, "Upload threw for {File}. Will retry after {Delay}s.",
                tracked.FileName, settings.RetryDelaySeconds);
            try { await Task.Delay(TimeSpan.FromSeconds(settings.RetryDelaySeconds), ct); } catch { }
        }
    }

    private async Task<UpsertSettings> LoadSettingsAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UpsertDbContext>();
        return await db.Settings.OrderBy(s => s.Id).FirstAsync();
    }

    private async Task EnsureDbAsync(CancellationToken ct)
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UpsertDbContext>();
        await db.Database.EnsureCreatedAsync(ct);
        // The HasData seed only runs on first create — make sure the row
        // exists even if EnsureCreated thought the DB was fine.
        if (!await db.Settings.AnyAsync(ct))
        {
            db.Settings.Add(new UpsertSettings { Id = 1 });
            await db.SaveChangesAsync(ct);
        }
    }

    private async Task<(int pending, int failed)> CountsAsync()
    {
        using var scope = _scopeFactory.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<UpsertDbContext>();
        var pending = await db.Queue.CountAsync(q => q.Status == "Pending");
        var failed  = await db.Queue.CountAsync(q => q.Status == "Failed");
        return (pending, failed);
    }

    private async Task UpdateStatusCountsAsync()
    {
        var (p, f) = await CountsAsync();
        _status.RecordCounts(p, f);
    }

    private static async Task<bool> WaitUntilReadyAsync(string path)
    {
        for (int i = 0; i < 10; i++)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.None);
                return true;
            }
            catch (IOException) { await Task.Delay(200); }
            catch (UnauthorizedAccessException) { await Task.Delay(200); }
        }
        return false;
    }

    private static IEnumerable<string> ExpandFilters(string filter)
    {
        if (string.IsNullOrWhiteSpace(filter)) yield return "*.*";
        else
        {
            foreach (var p in filter.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                yield return p;
        }
    }

    private static string GuessMime(string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        return ext switch
        {
            ".png" => "image/png",
            ".gif" => "image/gif",
            _      => "image/jpeg"
        };
    }
}
