using Microsoft.Extensions.Options;

namespace GrainManagement.Services;

/// <summary>
/// Resolves the active UI theme on the server and stores it in HttpContext.Items.
/// Users cannot override this via querystring/cookies.
/// </summary>
public class ThemeMiddleware
{
    private readonly RequestDelegate _next;

    public ThemeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IOptions<BrandingOptions> brandingOptions)
    {
        var options = brandingOptions.Value;

        // Theme key comes ONLY from backend configuration (or later: DB/hostname resolver).
        var themeKey = (options.ThemeKey ?? "default").Trim();

        // Safety: allowlist the theme key to prevent path traversal/injection.
        if (options.AllowedThemes is { Length: > 0 })
        {
            if (!options.AllowedThemes.Contains(themeKey, StringComparer.OrdinalIgnoreCase))
            {
                themeKey = "default";
            }
        }

        context.Items["ThemeKey"] = themeKey;

        await _next(context);
    }
}
