namespace GrainManagement.Services.Email;

/// <summary>
/// Strongly-typed view of the "Email" section in appsettings.json.
/// Bound at startup via <c>services.Configure&lt;EmailOptions&gt;(...)</c>.
/// </summary>
public sealed class EmailOptions
{
    public string SmtpHost { get; set; } = "";
    public int SmtpPort { get; set; } = 25;
    public bool EnableSsl { get; set; } = true;
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public string FromAddress { get; set; } = "";
    public string FromDisplayName { get; set; } = "";
    public int TimeoutMs { get; set; } = 60_000;

    public TestModeOptions TestMode { get; set; } = new();

    public sealed class TestModeOptions
    {
        /// <summary>
        /// When true, <see cref="IEmailService"/> overrides every recipient
        /// list with <see cref="Recipient"/>. The original recipient list is
        /// included in the body so the dev can confirm routing.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>Single address that receives every send while TestMode is on.</summary>
        public string Recipient { get; set; } = "";
    }
}
