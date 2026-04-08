using System.Drawing;
using DevExpress.Drawing;
using DevExpress.Drawing.Printing;
using DevExpress.XtraReports.UI;
using DevExpress.XtraPrinting.BarCode;

namespace BasicWeigh.Web.Reports;

public class KioskTicketReport : XtraReport
{
    public KioskTicketReport()
    {
        // 3" receipt paper
        PaperKind = DXPaperKind.Custom;
        PageWidth = 300;
        PageHeight = 500;
        Margins = new DXMargins(10, 10, 10, 10);
        Font = new DXFont("Courier New", 9f);
        RollPaper = true;

        // Parameters
        AddParameter("Ticket", typeof(string));
        AddParameter("DateIn", typeof(string));
        AddParameter("Customer", typeof(string));
        AddParameter("Carrier", typeof(string));
        AddParameter("TruckId", typeof(string));
        AddParameter("Commodity", typeof(string));
        AddParameter("Location", typeof(string));
        AddParameter("InWeight", typeof(string));
        AddParameter("Header1", typeof(string));
        AddParameter("Header2", typeof(string));
        AddParameter("Header3", typeof(string));
        AddParameter("Header4", typeof(string));

        var detail = new DetailBand { HeightF = 0, Name = "Detail" };
        Bands.Add(detail);

        var reportHeader = new ReportHeaderBand { HeightF = 0, Name = "ReportHeader" };
        Bands.Add(reportHeader);

        float y = 0;
        float pageContentWidth = 280; // 300 - 10 left margin - 10 right margin

        // Barcode at the very top, horizontally centered
        float barcodeWidth = 240;
        float barcodeX = (pageContentWidth - barcodeWidth) / 2f;
        var barcode = new XRBarCode
        {
            Name = "barcodeTicket",
            LocationF = new PointF(barcodeX, y),
            SizeF = new SizeF(barcodeWidth, 60),
            Symbology = new Code128Generator(),
            ShowText = true,
            TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomCenter,
            Font = new DXFont("Courier New", 10f, DXFontStyle.Bold),
            Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0)
        };
        barcode.ExpressionBindings.Add(
            new ExpressionBinding("BeforePrint", "Text", "[Parameters.Ticket]"));
        reportHeader.Controls.Add(barcode);
        y += 65;

        // Separator
        reportHeader.Controls.Add(CreateLine(y));
        y += 5;

        // Header lines (centered, bold)
        y = AddCenteredLabel(reportHeader, "Header1", y, 12f, true);
        y = AddCenteredLabel(reportHeader, "Header2", y, 10f, true);
        y = AddCenteredLabel(reportHeader, "Header3", y, 9f, false);
        y = AddCenteredLabel(reportHeader, "Header4", y, 9f, false);

        // Separator
        y += 3;
        reportHeader.Controls.Add(CreateLine(y));
        y += 5;

        // Title
        var titleLabel = new XRLabel
        {
            Name = "lblTitle",
            Text = "INBOUND KIOSK TICKET",
            LocationF = new PointF(0, y),
            SizeF = new SizeF(280, 20),
            Font = new DXFont("Courier New", 11f, DXFontStyle.Bold),
            TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        };
        reportHeader.Controls.Add(titleLabel);
        y += 22;

        // Separator
        reportHeader.Controls.Add(CreateLine(y));
        y += 5;

        // Detail rows
        y = AddLabelRow(reportHeader, "Ticket #:", "Ticket", y);
        y = AddLabelRow(reportHeader, "Date In:", "DateIn", y);
        y = AddLabelRow(reportHeader, "Customer:", "Customer", y);
        y = AddLabelRow(reportHeader, "Carrier:", "Carrier", y);
        y = AddLabelRow(reportHeader, "Truck ID:", "TruckId", y);
        y = AddLabelRow(reportHeader, "Commodity:", "Commodity", y);
        y = AddLabelRow(reportHeader, "Location:", "Location", y);

        // Weight separator
        y += 3;
        reportHeader.Controls.Add(CreateLine(y));
        y += 5;

        // In Weight (larger font)
        y = AddLabelRow(reportHeader, "In Weight:", "InWeight", y, 11f, true);

        y += 10;
        reportHeader.HeightF = y;
    }

    private void AddParameter(string name, Type type)
    {
        var param = new DevExpress.XtraReports.Parameters.Parameter
        {
            Name = name,
            Type = type,
            Visible = false
        };
        Parameters.Add(param);
    }

    private float AddCenteredLabel(Band band, string paramName, float y, float fontSize = 9f, bool bold = false)
    {
        var label = new XRLabel
        {
            Name = "lbl" + paramName,
            LocationF = new PointF(0, y),
            SizeF = new SizeF(280, 18),
            Font = new DXFont("Courier New", fontSize, bold ? DXFontStyle.Bold : DXFontStyle.Regular),
            TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
        };
        label.ExpressionBindings.Add(
            new ExpressionBinding("BeforePrint", "Text", $"[Parameters.{paramName}]"));
        band.Controls.Add(label);
        return y + 18;
    }

    private float AddLabelRow(Band band, string caption, string paramName, float y, float fontSize = 9f, bool bold = false)
    {
        var captionLabel = new XRLabel
        {
            Name = "cap" + paramName,
            Text = caption,
            LocationF = new PointF(0, y),
            SizeF = new SizeF(110, 16),
            Font = new DXFont("Courier New", fontSize, bold ? DXFontStyle.Bold : DXFontStyle.Regular)
        };
        band.Controls.Add(captionLabel);

        var valueLabel = new XRLabel
        {
            Name = "val" + paramName,
            LocationF = new PointF(110, y),
            SizeF = new SizeF(170, 16),
            Font = new DXFont("Courier New", fontSize, bold ? DXFontStyle.Bold : DXFontStyle.Regular)
        };
        valueLabel.ExpressionBindings.Add(
            new ExpressionBinding("BeforePrint", "Text", $"[Parameters.{paramName}]"));
        band.Controls.Add(valueLabel);

        return y + 16;
    }

    private XRLine CreateLine(float y)
    {
        return new XRLine
        {
            Name = "line" + y.ToString("F0"),
            LocationF = new PointF(0, y),
            SizeF = new SizeF(280, 2),
            LineStyle = DXDashStyle.Dash
        };
    }
}
