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

  

 

    public CurrentUser(
        IHttpContextAccessor accessor,
        IConfiguration config
       )
    {
        _user = accessor.HttpContext?.User ?? new ClaimsPrincipal();

        var grp = config.GetSection("GrainSecurity");
        _adminId = grp["AdminGroupId"] ?? throw new InvalidOperationException("GrainSecurity:AdminGroupId missing");
        _managerId = grp["ManagerGroupId"] ?? throw new InvalidOperationException("GrainSecurity:ManagerGroupId missing");
        _userId = grp["UserGroupId"] ?? throw new InvalidOperationException("GrainSecurity:UserGroupId missing");

   

    }

    public bool IsAuthenticated => _user?.Identity?.IsAuthenticated == true;

    public string? UPN =>
        _user?.FindFirst(ClaimConstants.PreferredUserName)?.Value
        ?? _user?.FindFirst(ClaimTypes.Upn)?.Value
        ?? _user?.FindFirst("preferred_username")?.Value;

    public IEnumerable<string> GroupIds =>
       _user.FindAll("groups").Select(c => c.Value);

    // TEMP: grant all roles when not authenticated (local testing) — re-enable before deploy!
    public bool IsAdmin => !IsAuthenticated || GroupIds.Contains(_adminId, StringComparer.OrdinalIgnoreCase);
    public bool IsManager => !IsAuthenticated || GroupIds.Contains(_managerId, StringComparer.OrdinalIgnoreCase);
    public bool IsUser => !IsAuthenticated || GroupIds.Contains(_userId, StringComparer.OrdinalIgnoreCase);

   
}
