namespace GrainManagement.Services;

/// <summary>
/// Defines which application modules are enabled for this deployment.
/// Bound from the "Modules" section in appsettings.json.
/// </summary>
public class ModuleOptions
{
    public const string SectionName = "Modules";

    // ── Deployment mode ─────────────────────────────────────────────
    /// <summary>"Central" or "Remote" — controls which set of modules is typical.</summary>
    public string DeploymentMode { get; set; } = "Remote";

    // ── Remote-site modules ─────────────────────────────────────────
    public bool WarehouseIntake { get; set; } = true;
    public bool Seed { get; set; }
    public bool Scales { get; set; } = true;
    public bool GrowerDelivery { get; set; } = true;

    // ── Central-office modules ──────────────────────────────────────
    public bool DatabaseAdmin { get; set; }
    public bool WarehouseAdmin { get; set; }
    public bool SeedAdmin { get; set; }
    public bool Reporting { get; set; }

    // ── Location restriction ──────────────────────────────────────────
    /// <summary>
    /// When non-empty, only locations whose LocationId is in this list
    /// will appear in the location selector. When empty, all active
    /// locations are shown.
    /// </summary>
    public List<int> AllowedLocationIds { get; set; } = new();

    // ── Shared modules (usually on everywhere) ──────────────────────
    public bool Accounts { get; set; } = true;
    public bool Locations { get; set; } = true;
    public bool System { get; set; } = true;
    public bool Users { get; set; } = true;

    /// <summary>Returns true if the named module is enabled.</summary>
    public bool IsEnabled(string moduleName) => moduleName switch
    {
        nameof(WarehouseIntake) => WarehouseIntake,
        nameof(Seed) => Seed,
        nameof(Scales) => Scales,
        nameof(GrowerDelivery) => GrowerDelivery,
        nameof(DatabaseAdmin) => DatabaseAdmin,
        nameof(WarehouseAdmin) => WarehouseAdmin,
        nameof(SeedAdmin) => SeedAdmin,
        nameof(Reporting) => Reporting,
        nameof(Accounts) => Accounts,
        nameof(Locations) => Locations,
        nameof(System) => System,
        nameof(Users) => Users,
        _ => false
    };
}
