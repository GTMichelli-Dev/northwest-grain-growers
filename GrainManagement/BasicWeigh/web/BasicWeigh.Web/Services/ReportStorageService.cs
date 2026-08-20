using DevExpress.XtraReports.Web.Extensions;
using DevExpress.XtraReports.UI;
using BasicWeigh.Web.Reports;

namespace BasicWeigh.Web.Services;

public class ReportStorageService : ReportStorageWebExtension
{
    private readonly string _reportsDir;

    public ReportStorageService()
    {
        _reportsDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
        if (!Directory.Exists(_reportsDir))
            Directory.CreateDirectory(_reportsDir);
    }

    public override bool CanSetData(string url) => true;

    public override bool IsValidUrl(string url) => true;

    public override byte[] GetData(string url)
    {
        var path = GetPath(url);
        if (File.Exists(path))
            return File.ReadAllBytes(path);

        // If no saved file, generate from the coded report
        if (url == "TicketReport")
        {
            using var stream = new MemoryStream();
            var report = new TicketReport();
            report.SaveLayoutToXml(stream);
            return stream.ToArray();
        }

        if (url == "KioskTicketReport")
        {
            using var stream = new MemoryStream();
            var report = new KioskTicketReport();
            report.SaveLayoutToXml(stream);
            return stream.ToArray();
        }

        throw new FileNotFoundException($"Report '{url}' not found.");
    }

    public override void SetData(XtraReport report, string url)
    {
        var path = GetPath(url);
        report.SaveLayoutToXml(path);
    }

    public override string SetNewData(XtraReport report, string defaultUrl)
    {
        var url = string.IsNullOrEmpty(defaultUrl) ? "NewReport" : defaultUrl;
        SetData(report, url);
        return url;
    }

    public override Dictionary<string, string> GetUrls()
    {
        var urls = new Dictionary<string, string>();

        // Always show built-in reports
        urls["TicketReport"] = "Ticket Report";
        urls["KioskTicketReport"] = "Kiosk Inbound Ticket";

        // Add any other .repx files in the folder
        foreach (var file in Directory.GetFiles(_reportsDir, "*.repx"))
        {
            var name = Path.GetFileNameWithoutExtension(file);
            if (!urls.ContainsKey(name))
                urls[name] = name;
        }

        return urls;
    }

    private string GetPath(string url)
    {
        var safeName = Path.GetFileNameWithoutExtension(url);
        return Path.Combine(_reportsDir, safeName + ".repx");
    }
}
