#nullable enable

using Microsoft.Identity.Web;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string? UPN { get; }
    IEnumerable<string> GroupIds { get; }
    bool IsAdmin { get; }
    bool IsManager { get; }
    bool IsUser { get; }
}

public sealed class CurrentUser : ICurrentUser
{
    private readonly ClaimsPrincipal _user;
    private readonly string _adminId;
    private readonly string _managerId;
    private readonly string _userId;

    private readonly ITokenAcquisition _tokenAcq;
    private readonly IHttpClientFactory _http;

    // Cache group IDs once per request
    private readonly Lazy<HashSet<string>> _groupIds;

    public CurrentUser(
        IHttpContextAccessor accessor,
        IConfiguration config,
        ITokenAcquisition tokenAcq,
        IHttpClientFactory httpFactory)
    {
        _user = accessor.HttpContext?.User ?? new ClaimsPrincipal();

        var grp = config.GetSection("GrainSecurity");
        _adminId = grp["AdminGroupId"] ?? throw new InvalidOperationException("GrainSecurity:AdminGroupId missing");
        _managerId = grp["ManagerGroupId"] ?? throw new InvalidOperationException("GrainSecurity:ManagerGroupId missing");
        _userId = grp["UserGroupId"] ?? throw new InvalidOperationException("GrainSecurity:UserGroupId missing");

        _tokenAcq = tokenAcq;
        _http = httpFactory;

        _groupIds = new Lazy<HashSet<string>>(() =>
        {
            // 1) Try token "groups" claim first (fast, no network)
            var claimGroups = _user.FindAll("groups").Select(c => c.Value).ToHashSet(StringComparer.OrdinalIgnoreCase);
            if (claimGroups.Count > 0)
                return claimGroups;

            // 2) Otherwise, call Microsoft Graph (covers nested groups & overage)
            try
            {
                var ids = GetGroupsFromGraph().GetAwaiter().GetResult();
                return ids.ToHashSet(StringComparer.OrdinalIgnoreCase);
            }
            catch
            {
                // Fail-safe: return whatever we had (likely empty)
                return claimGroups;
            }
        });
    }

    public bool IsAuthenticated => _user?.Identity?.IsAuthenticated == true;

    public string? UPN =>
        _user?.FindFirst(ClaimConstants.PreferredUserName)?.Value
        ?? _user?.FindFirst(ClaimTypes.Upn)?.Value
        ?? _user?.FindFirst("preferred_username")?.Value;

    public IEnumerable<string> GroupIds => _groupIds.Value;

    public bool IsAdmin => _groupIds.Value.Contains(_adminId);
    public bool IsManager => _groupIds.Value.Contains(_managerId);
    public bool IsUser => _groupIds.Value.Contains(_userId);

    private async Task<IEnumerable<string>> GetGroupsFromGraph()
    {
        // Requires Delegated permission: Group.Read.All (admin consent)
        var token = await _tokenAcq.GetAccessTokenForUserAsync(new[] { "Group.Read.All" }).ConfigureAwait(false);
        var client = _http.CreateClient();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Transitive includes nested membership; restrict to groups and select only id + name
        var url = "https://graph.microsoft.com/v1.0/me/transitiveMemberOf/microsoft.graph.group?$select=id,displayName";

        var ids = new List<string>();
        string? next = url;

        // Handle paging
        while (!string.IsNullOrEmpty(next))
        {
            var json = await client.GetStringAsync(next).ConfigureAwait(false);
            using var doc = JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("value", out var value))
            {
                foreach (var item in value.EnumerateArray())
                {
                    if (item.TryGetProperty("id", out var idProp))
                        ids.Add(idProp.GetString()!);
                }
            }

            next = null;
            if (doc.RootElement.TryGetProperty("@odata.nextLink", out var nextLink))
                next = nextLink.GetString();
        }

        return ids;
    }
}
