namespace GrainManagement.Services;

public class BrandingOptions
{
    public string ThemeKey { get; set; } = "default";

    /// <summary>
    /// Comma-separated allowlist of theme keys (prevents path injection and mistakes).
    /// </summary>
    public string[] AllowedThemes { get; set; } = ["default"];
}
