namespace GrainManagement.Dtos.Warehouse;

/// <summary>
/// One row in the Bin Protein Weighted Average report — grouped by
/// (Location, Bin). The protein is a bushel-weighted average across all
/// loads in the bin where the PROTEIN transaction attribute is non-null
/// and greater than zero. Loads with missing or zero protein are
/// excluded from the weighted average but still counted in
/// <see cref="LoadCount"/> so the operator can see the sample base.
/// </summary>
public class BinProteinRowDto
{
    public int LocationId { get; set; }
    public string LocationName { get; set; } = "";
    public string Bin { get; set; } = "";

    /// <summary>Bushel-weighted average protein percentage. Zero when no
    /// load in the bin has a non-zero protein reading.</summary>
    public decimal AvgProtein { get; set; }

    /// <summary>Total bushels in the bin across every load (whether or
    /// not the load had a protein reading).</summary>
    public decimal TotalBushels { get; set; }

    /// <summary>Bushels that contributed to the weighted protein
    /// average — i.e. sum of bushels on loads where protein was > 0.</summary>
    public decimal SampledBushels { get; set; }

    /// <summary>Total loads in the bin.</summary>
    public int LoadCount { get; set; }

    /// <summary>Loads that contributed to the weighted average (protein > 0).</summary>
    public int SamplesUsed { get; set; }
}
