using Microsoft.AspNetCore.Authentication;
using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

namespace GrainManagement.Auth;

public sealed class GroupClaimsTransformation : IClaimsTransformation
{
    private readonly ITokenAcquisition _tokenAcq;
    private readonly IHttpClientFactory _http;
    private readonly ILogger<GroupClaimsTransformation> _logger;

    public GroupClaimsTransformation(
        ITokenAcquisition tokenAcq,
        IHttpClientFactory http,
        ILogger<GroupClaimsTransformation> logger)
    {
        _tokenAcq = tokenAcq;
        _http = http;
        _logger = logger;
    }

    public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
        _logger.LogInformation("GCT start: user={User} hasGroupsClaim={HasGroups}",
    principal.FindFirst("preferred_username")?.Value,
    principal.HasClaim(c => c.Type == "groups"));
        if (principal?.Identity?.IsAuthenticated != true)
            return principal;

        // Already present -> do nothing
        if (principal.HasClaim(c => c.Type == "groups"))
            return principal;

        try
        {
            // Requires delegated Graph permission + admin consent (see below)
            var token = await _tokenAcq.GetAccessTokenForUserAsync(
              new[] { "Group.Read.All" },
              user: principal);

            var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var ids = new List<string>();
#nullable enable
            string? next = "https://graph.microsoft.com/v1.0/me/transitiveMemberOf/microsoft.graph.group?$select=id";
#nullable restore

            while (!string.IsNullOrEmpty(next))
            {
                var json = await client.GetStringAsync(next);
                using var doc = JsonDocument.Parse(json);

                if (doc.RootElement.TryGetProperty("value", out var value))
                {
                    foreach (var item in value.EnumerateArray())
                        if (item.TryGetProperty("id", out var idProp))
                            ids.Add(idProp.GetString()!);
                }

                next = doc.RootElement.TryGetProperty("@odata.nextLink", out var nextLink)
                    ? nextLink.GetString()
                    : null;
            }

            if (principal.Identity is ClaimsIdentity id && ids.Count > 0)
            {
                foreach (var gid in ids.Distinct(StringComparer.OrdinalIgnoreCase))
                    id.AddClaim(new Claim("groups", gid));
            }

            _logger.LogInformation("GCT: added {Count} groups for {User}",
                ids.Count, principal.FindFirst("preferred_username")?.Value);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GCT FAILED for {User}",
                principal.FindFirst("preferred_username")?.Value);
        }

        return principal;
    }
}
