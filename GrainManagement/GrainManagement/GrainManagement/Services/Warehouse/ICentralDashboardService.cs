using GrainManagement.Dtos.Warehouse;

namespace GrainManagement.Services.Warehouse;

public interface ICentralDashboardService
{
    /// <summary>
    /// Returns one row per active location with intake / transfer-in /
    /// transfer-out totals for the given creation-date range (inclusive).
    /// Locations with no activity are still returned so the UI can show or
    /// hide them via the "Show only active" toggle.
    /// </summary>
    Task<IReadOnlyList<CentralDashboardRowDto>> GetAsync(
        DateOnly fromDate, DateOnly toDate, CancellationToken ct);
}
