using GrainManagement.Dtos.Warehouse;
using GrainManagement.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace GrainManagement.Services.Warehouse;

public sealed class CentralDashboardService : ICentralDashboardService
{
    private readonly dbContext _ctx;

    public CentralDashboardService(dbContext ctx) => _ctx = ctx;

    // Keyless projection used by SqlQueryRaw. EF still validates the type
    // for decimal precision; the explicit [Precision(18,2)] silences the
    // 30000 warning and matches the upstream SUM(...) widths comfortably
    // (NetLbs is at most a few million for any reasonable date range,
    // NetBu likewise).
    private sealed class ActivityFact
    {
        public string Bucket { get; set; } = "";        // "Intake" | "TransferIn" | "TransferOut"
        public int LocationId { get; set; }
        public int Loads { get; set; }
        [Precision(18, 2)] public decimal Lbs { get; set; }
        [Precision(18, 2)] public decimal Bu  { get; set; }
    }

    public async Task<IReadOnlyList<CentralDashboardRowDto>> GetAsync(
        DateOnly fromDate, DateOnly toDate, CancellationToken ct)
    {
        if (toDate < fromDate) (fromDate, toDate) = (toDate, fromDate);

        // Single round-trip pulls per-WS net lbs and converts to bushels at
        // a flat 60 lbs/bu, then unions the three buckets (intake /
        // transfer-in / transfer-out) so we can group-by-location in C#
        // alongside the locations list. The previous category-DefaultUomId
        // lookup silently fell through to 0 bushels whenever the product
        // → category → UoM chain wasn't fully wired up; the dashboard is
        // wheat-dominant so a flat 60 lbs/bu is what the user expects.
        const string sql = @"
;WITH ws_totals AS (
    SELECT
        ws.WeightSheetId,
        ws.WeightSheetType,
        ws.LocationId,
        ws.SourceLocationId,
        ws.DestinationLocationId,
        ISNULL(qty.NetLbs, 0)            AS NetLbs,
        ISNULL(qty.NetLbs, 0) / 60.0     AS NetBu
    FROM warehouse.WeightSheets ws
    OUTER APPLY (
        SELECT SUM(ISNULL(itd.NetQty, ISNULL(itd.DirectQty, 0))) AS NetLbs
        FROM Inventory.InventoryTransactionDetails itd
        WHERE itd.RefId = ws.RowUid
          AND itd.RefType = 'WeightSheet'
          AND itd.IsVoided = 0
    ) qty
    WHERE ws.CreationDate >= @from AND ws.CreationDate <= @to
)
SELECT 'Intake' AS Bucket, LocationId, COUNT(*) AS Loads,
       ISNULL(SUM(NetLbs), 0) AS Lbs, ISNULL(SUM(NetBu), 0) AS Bu
  FROM ws_totals
  WHERE WeightSheetType = 'Delivery'
  GROUP BY LocationId
UNION ALL
SELECT 'TransferIn', DestinationLocationId, COUNT(*),
       ISNULL(SUM(NetLbs), 0), ISNULL(SUM(NetBu), 0)
  FROM ws_totals
  WHERE WeightSheetType = 'Transfer' AND DestinationLocationId IS NOT NULL
  GROUP BY DestinationLocationId
UNION ALL
SELECT 'TransferOut', SourceLocationId, COUNT(*),
       ISNULL(SUM(NetLbs), 0), ISNULL(SUM(NetBu), 0)
  FROM ws_totals
  WHERE WeightSheetType = 'Transfer' AND SourceLocationId IS NOT NULL
  GROUP BY SourceLocationId;
";

        var fromParam = new SqlParameter("@from", fromDate.ToDateTime(TimeOnly.MinValue));
        var toParam   = new SqlParameter("@to",   toDate.ToDateTime(TimeOnly.MinValue));

        var facts = await _ctx.Database
            .SqlQueryRaw<ActivityFact>(sql, fromParam, toParam)
            .ToListAsync(ct);

        var locations = await _ctx.Locations.AsNoTracking()
            .Where(l => l.IsActive)
            .OrderBy(l => l.Name)
            .Select(l => new { l.LocationId, l.Name })
            .ToListAsync(ct);

        var byId = locations.ToDictionary(
            l => l.LocationId,
            l => new CentralDashboardRowDto
            {
                LocationId = l.LocationId,
                LocationName = l.Name ?? "",
            });

        foreach (var f in facts)
        {
            // Activity for a deactivated location still counts — synthesize a
            // row so it shows up rather than silently being dropped.
            if (!byId.TryGetValue(f.LocationId, out var row))
            {
                row = new CentralDashboardRowDto
                {
                    LocationId = f.LocationId,
                    LocationName = $"(inactive #{f.LocationId})",
                };
                byId[f.LocationId] = row;
            }

            switch (f.Bucket)
            {
                case "Intake":
                    row.IntakeLoads = f.Loads;
                    row.IntakeLbs   = f.Lbs;
                    row.IntakeBu    = f.Bu;
                    break;
                case "TransferIn":
                    row.TransferInLoads = f.Loads;
                    row.TransferInLbs   = f.Lbs;
                    row.TransferInBu    = f.Bu;
                    break;
                case "TransferOut":
                    row.TransferOutLoads = f.Loads;
                    row.TransferOutLbs   = f.Lbs;
                    row.TransferOutBu    = f.Bu;
                    break;
            }
        }

        return byId.Values
            .OrderBy(r => r.LocationName, StringComparer.OrdinalIgnoreCase)
            .ToList();
    }
}
