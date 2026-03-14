#nullable enable

using GrainManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Services;

public interface ILocationContext
{
    int? LocationId { get; }
    string? LocationName { get; }
    string? LocationCode { get; }
    bool CanDoWarehouse { get; }
    bool CanDoSeed { get; }
    bool IsAdminOnly { get; }
    bool HasLocation { get; }
}

public sealed class LocationContext : ILocationContext
{
    private readonly IHttpContextAccessor _accessor;
    private readonly dbContext _db;
    private bool _loaded;
    private Location? _location;

    private const string CookieName = "GrainMgmt_LocationId";

    public LocationContext(IHttpContextAccessor accessor, dbContext db)
    {
        _accessor = accessor;
        _db = db;
    }

    private void EnsureLoaded()
    {
        if (_loaded) return;
        _loaded = true;

        var httpContext = _accessor.HttpContext;
        if (httpContext == null) return;

        if (!httpContext.Request.Cookies.TryGetValue(CookieName, out var raw)
            || !int.TryParse(raw, out var locId))
            return;

        _location = _db.Locations
            .AsNoTracking()
            .FirstOrDefault(l => l.LocationId == locId && l.IsActive);
    }

    public int? LocationId { get { EnsureLoaded(); return _location?.LocationId; } }
    public string? LocationName { get { EnsureLoaded(); return _location?.Name; } }
    public string? LocationCode { get { EnsureLoaded(); return _location?.Code; } }
    public bool CanDoWarehouse { get { EnsureLoaded(); return _location?.UseForWarehouse ?? false; } }
    public bool CanDoSeed { get { EnsureLoaded(); return _location?.UseForSeed ?? false; } }
    public bool IsAdminOnly { get { EnsureLoaded(); return _location != null && !_location.UseForWarehouse && !_location.UseForSeed; } }
    public bool HasLocation { get { EnsureLoaded(); return _location != null; } }

    /// <summary>
    /// Sets the selected location via cookie. Called by the API controller.
    /// </summary>
    public static void SetLocation(HttpResponse response, int locationId)
    {
        response.Cookies.Append(CookieName, locationId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax,
            MaxAge = TimeSpan.FromDays(365)
        });
    }

    /// <summary>
    /// Clears the selected location cookie.
    /// </summary>
    public static void ClearLocation(HttpResponse response)
    {
        response.Cookies.Delete(CookieName);
    }
}
