namespace GrainManagement.Services;

/// <summary>
/// Defines which application modules are enabled for this deployment.
/// Bound from the "Modules" section in appsettings.json.
///
/// Deployment modes:
///   "Central"   — HQ admin server (admin, accounts, locations, users, reporting)
///   "Remote"    — Field location server (warehouse intake, scales, grower delivery, reporting)
///   "Reporting" — Reporting-only site (reporting only, no admin or intake)
///
/// Seed is available on Remote but gated per-location via UseForSeed in the database.
/// </summary>
public class ModuleOptions
{
    public const string SectionName = "Modules";

    // ── Deployment mode ─────────────────────────────────────────────
    public string DeploymentMode { get; set; } = "Remote";

    // ── Remote-only modules ─────────────────────────────────────────
    public bool WarehouseIntake => IsRemote;
    public bool Scales         => IsRemote;
    public bool GrowerDelivery => IsRemote;

    // ── Per-location module ─────────────────────────────────────────
    // Available on Remote; UI further gates by location.UseForSeed.
    public bool Seed => IsRemote;

    // ── Central-only modules ────────────────────────────────────────
    public bool DatabaseAdmin  => IsCentral;
    public bool WarehouseAdmin => IsCentral;
    public bool SeedAdmin      => IsCentral;
    public bool Accounts       => IsCentral;
    public bool Locations      => IsCentral;
    public bool Users          => IsCentral;

    // ── Remote-only system module ───────────────────────────────────
    // Local system settings for field location servers.
    public bool System => IsRemote;

    // ── Shared modules ──────────────────────────────────────────────
    // Reporting is available on Central, Remote, and Reporting modes.
    public bool Reporting => IsCentral || IsRemote || IsReporting;

    // ── Location restriction ────────────────────────────────────────
    /// <summary>
    /// When non-empty, only locations whose LocationId is in this list
    /// will appear in the location selector. When empty, all active
    /// locations are shown.
    /// </summary>
    public List<int> AllowedLocationIds { get; set; } = new();

    // ── Helpers ─────────────────────────────────────────────────────
    public bool IsCentral   => string.Equals(DeploymentMode, "Central", StringComparison.OrdinalIgnoreCase);
    public bool IsRemote    => string.Equals(DeploymentMode, "Remote", StringComparison.OrdinalIgnoreCase);
    public bool IsReporting => string.Equals(DeploymentMode, "Reporting", StringComparison.OrdinalIgnoreCase);

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
