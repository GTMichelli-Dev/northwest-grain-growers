using DevExpress.XtraReports.Web.Extensions;
using DevExpress.XtraReports.UI;
using GrainManagement.Reporting;
using Microsoft.Extensions.Options;

namespace GrainManagement.Services;

/// <summary>
/// Provides reports to the DevExpress Web Document Viewer.
/// Reports are gated by the active module configuration.
/// </summary>
public sealed class ReportStorageService : ReportStorageWebExtension
{
    private readonly ModuleOptions _modules;

    // Registry of all known reports: key = display name, value = factory + module gate
    private static readonly Dictionary<string, ReportDefinition> AllReports = new()
    {
        ["Load Ticket"] = new("Warehouse", () => new LoadTicketReport()),
        ["Test Ticket"] = new("Master", () => new TestTicketReport())
    };

    public ReportStorageService(IOptions<ModuleOptions> modules)
    {
        _modules = modules.Value;
    }

    public override bool CanSetData(string url) => false;
    public override bool IsValidUrl(string url) => GetEnabledReports().ContainsKey(url);

    public override byte[] GetData(string url)
    {
        var reports = GetEnabledReports();
        if (!reports.TryGetValue(url, out var def))
            throw new InvalidOperationException($"Report '{url}' not found or not enabled.");

        var report = def.Factory();
        using var ms = new MemoryStream();
        report.SaveLayoutToXml(ms);
        return ms.ToArray();
    }

    public override Dictionary<string, string> GetUrls()
    {
        return GetEnabledReports()
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Key);
    }

    public override void SetData(XtraReport report, string url) { }
    public override string SetNewData(XtraReport report, string defaultUrl) => defaultUrl;

    private Dictionary<string, ReportDefinition> GetEnabledReports()
    {
        return AllReports
            .Where(kvp => IsModuleEnabled(kvp.Value.Category))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    private bool IsModuleEnabled(string category) => category switch
    {
        "Warehouse" => _modules.WarehouseIntake || _modules.WarehouseAdmin,
        "Seed" => _modules.Seed || _modules.SeedAdmin,
        "Master" => true,
        _ => true
    };

    private sealed record ReportDefinition(string Category, Func<XtraReport> Factory);
}
