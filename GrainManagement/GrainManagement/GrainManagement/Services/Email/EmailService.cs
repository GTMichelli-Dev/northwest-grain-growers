using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using Microsoft.Extensions.Options;

namespace GrainManagement.Services.Email;

/// <summary>
/// SMTP-backed implementation. Uses System.Net.Mail (matches the legacy
/// Windows app's send shape so the smtp2go account "just works"). All
/// network I/O happens on a worker thread via <see cref="Task.Run"/> because
/// SmtpClient.SendAsync has a fragile contract on .NET 8 — we'd rather pay
/// the thread-pool tax than chase deadlocks.
/// </summary>
public sealed class EmailService : IEmailService
{
    private readonly EmailOptions _opts;
    private readonly ILogger<EmailService> _log;

    public EmailService(IOptions<EmailOptions> opts, ILogger<EmailService> log)
    {
        _opts = opts.Value;
        _log = log;
    }

    public Task<bool> SendAsync(
        IEnumerable<string> recipients,
        string subject,
        string body,
        IEnumerable<EmailAttachment>? attachments = null,
        CancellationToken ct = default)
    {
        var originalList = (recipients ?? Array.Empty<string>())
            .SelectMany(r => (r ?? "").Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries))
            .Select(r => r.Trim())
            .Where(IsValidEmail)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        if (originalList.Count == 0)
        {
            _log.LogWarning("EmailService.SendAsync: no valid recipients (subject {Subject})", subject);
            return Task.FromResult(false);
        }

        // Test-mode diversion. When enabled, all sends go to a single
        // configured inbox and the original recipient list is appended to
        // the body so the dev / QA tester can verify routing was correct.
        var effectiveRecipients = originalList;
        var effectiveBody = body ?? "";
        if (_opts.TestMode.Enabled)
        {
            if (!IsValidEmail(_opts.TestMode.Recipient))
            {
                _log.LogError(
                    "EmailService.SendAsync: TestMode is enabled but Recipient '{Recipient}' is not a valid email. Aborting send.",
                    _opts.TestMode.Recipient);
                return Task.FromResult(false);
            }

            effectiveRecipients = new List<string> { _opts.TestMode.Recipient };
            effectiveBody += Environment.NewLine + Environment.NewLine
                + "--- TEST MODE ---" + Environment.NewLine
                + "Original recipients: " + string.Join("; ", originalList);
        }

        var attachList = (attachments ?? Array.Empty<EmailAttachment>()).ToList();
        var subjectSafe = subject ?? "";

        return Task.Run(() => SendInternal(effectiveRecipients, subjectSafe, effectiveBody, attachList), ct);
    }

    private bool SendInternal(List<string> recipients, string subject, string body, List<EmailAttachment> attachments)
    {
        try
        {
            using var smtp = new SmtpClient
            {
                Host = _opts.SmtpHost,
                Port = _opts.SmtpPort,
                EnableSsl = _opts.EnableSsl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_opts.Username, _opts.Password),
                Timeout = _opts.TimeoutMs,
            };

            using var message = new MailMessage
            {
                From = string.IsNullOrWhiteSpace(_opts.FromDisplayName)
                    ? new MailAddress(_opts.FromAddress)
                    : new MailAddress(_opts.FromAddress, _opts.FromDisplayName),
                Subject = subject,
                Body = body,
            };
            foreach (var to in recipients) message.To.Add(to);

            foreach (var a in attachments)
            {
                // SmtpClient takes ownership of the stream lifetime; using a
                // MemoryStream over the buffer is the cheapest option.
                var stream = new MemoryStream(a.Content, writable: false);
                var attachment = new Attachment(stream, a.FileName, a.ContentType);
                message.Attachments.Add(attachment);
            }

            smtp.Send(message);
            _log.LogInformation(
                "EmailService.SendAsync: sent '{Subject}' to {Count} recipient(s){TestMode}",
                subject, recipients.Count,
                _opts.TestMode.Enabled ? " (test mode)" : "");
            return true;
        }
        catch (Exception ex)
        {
            _log.LogError(ex,
                "EmailService.SendAsync failed for subject '{Subject}' recipients={Recipients}",
                subject, string.Join("; ", recipients));
            return false;
        }
    }

    private static bool IsValidEmail(string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) return false;
        try
        {
            var addr = new MailAddress(value);
            return string.Equals(addr.Address, value, StringComparison.OrdinalIgnoreCase);
        }
        catch { return false; }
    }
}
