namespace GrainManagement.Services.Email;

/// <summary>
/// Minimal email send abstraction for the web app. Implementations honor
/// <see cref="EmailOptions.TestMode"/> by rewriting recipients to the test
/// inbox when test mode is enabled, so dev / staging boxes never accidentally
/// reach producers.
/// </summary>
public interface IEmailService
{
    /// <summary>
    /// Send an email with optional attachments. Returns true on success.
    /// Recipient addresses can be ';' or ',' separated; invalid addresses
    /// are silently skipped. When test mode is on, the recipient list is
    /// replaced with the configured test recipient (and the original list
    /// is appended to the body for traceability).
    /// </summary>
    Task<bool> SendAsync(
        IEnumerable<string> recipients,
        string subject,
        string body,
        IEnumerable<EmailAttachment>? attachments = null,
        CancellationToken ct = default);
}

/// <summary>In-memory attachment for <see cref="IEmailService.SendAsync"/>.</summary>
public sealed class EmailAttachment
{
    public string FileName { get; set; } = "";
    public string ContentType { get; set; } = "application/octet-stream";
    public byte[] Content { get; set; } = Array.Empty<byte>();
}
