using System.ComponentModel.DataAnnotations;

namespace PictureUpsert.Models;

/// <summary>Single-row settings table — keeps the picture-upsert config alongside the queue.</summary>
public sealed class UpsertSettings
{
    [Key]
    public int Id { get; set; }

    /// <summary>Local folder this service watches. Web app writes ticket images here.</summary>
    [Required, StringLength(500)]
    public string LocalFolder { get; set; } = "C:\\Images";

    /// <summary>Base URL of the remote server that ingests the images (Central HQ).</summary>
    [Required, StringLength(500)]
    public string RemoteBaseUrl { get; set; } = "http://localhost:51791";

    /// <summary>POST endpoint on the remote that receives a multipart upload. Filename carries the load number + direction.</summary>
    [StringLength(500)]
    public string RemoteUploadPath { get; set; } = "/api/picture-upsert/ingest";

    /// <summary>Optional bearer token sent in the Authorization header on each upload.</summary>
    [StringLength(500)]
    public string? AuthToken { get; set; }

    /// <summary>Delete the local file after a successful upload? Default false (the web still needs it for /ticket-images/).</summary>
    public bool DeleteAfterUpload { get; set; } = false;

    /// <summary>Delay between retry attempts when the remote is unreachable.</summary>
    public int RetryDelaySeconds { get; set; } = 30;

    /// <summary>Globs to watch — semicolon-separated.</summary>
    [StringLength(200)]
    public string Filter { get; set; } = "*.jpg;*.jpeg;*.png";
}

/// <summary>Per-file row tracking upload progress; survives restarts.</summary>
public sealed class UpsertQueueItem
{
    [Key]
    public long Id { get; set; }

    [Required, StringLength(500)]
    public string FilePath { get; set; } = "";

    [StringLength(200)]
    public string FileName { get; set; } = "";

    public long FileSize { get; set; }

    /// <summary>Pending | Sent | Failed.</summary>
    [Required, StringLength(20)]
    public string Status { get; set; } = "Pending";

    public int AttemptCount { get; set; }

    [StringLength(500)]
    public string? LastError { get; set; }

    public DateTime EnqueuedUtc { get; set; } = DateTime.UtcNow;

    public DateTime? LastAttemptUtc { get; set; }

    public DateTime? SentUtc { get; set; }
}
