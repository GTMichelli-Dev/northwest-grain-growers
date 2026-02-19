namespace GrainManagement.Dtos.Warehouse;

public sealed class WarehouseLoadDto
{
    public int Bol { get; set; }
    public string Bin { get; set; } = "";
    public decimal Moist { get; set; }
    public decimal Protein { get; set; }
    public decimal Gross { get; set; }
    public decimal Tare { get; set; }

    // Auto-calculated (dummy service sets this to Gross - Tare)
    public decimal Net { get; set; }

    // When the load was weighed (UTC)
    public DateTimeOffset WeighedAtUtc { get; set; }

    // Cumulative net for the lot up to and including this row
    public decimal RunningNet { get; set; }

    // Marks the final "TOTAL" row (summary) in the Loads grid
    public bool IsSummary { get; set; }
}
