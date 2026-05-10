using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using DevExpress.Drawing;
using DevExpress.XtraPrinting.Drawing;
using DevExpress.XtraReports.UI;
using GrainManagement.Dtos.Warehouse;

namespace GrainManagement.Reporting
{
    public partial class IntakeWeightSheetReport : XtraReport
    {
        public IntakeWeightSheetReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
            this.BeforePrint += ApplyVoidWatermarkIfEmpty;

            // Hyperlink the BOL + InWeight + OutWeight cells to their server-aware
            // image URLs. The DTO populates InImageUrl/OutImageUrl/BolImageUrl
            // with /api/ticket-image/{loadNumber}?direction=... when a weight or
            // BOL is recorded; empty string when not. PDF viewers render cells
            // with a non-empty NavigateUrl as clickable links.
            detBOL.ExpressionBindings.Add(new ExpressionBinding(
                "BeforePrint", "NavigateUrl", "[BolImageUrl]"));
            detInWt.ExpressionBindings.Add(new ExpressionBinding(
                "BeforePrint", "NavigateUrl", "[InImageUrl]"));
            detOutWt.ExpressionBindings.Add(new ExpressionBinding(
                "BeforePrint", "NavigateUrl", "[OutImageUrl]"));

            // Visual cue — blue + underline when a URL exists, plain when not.
            // Done in BeforePrint of the detail band so we can read the current
            // row's URLs and toggle per-cell.
            detailBand1.BeforePrint += StyleImageLinkCells;
        }

        private void StyleImageLinkCells(object sender, CancelEventArgs e)
        {
            var inUrl  = GetCurrentColumnValue("InImageUrl")  as string ?? "";
            var outUrl = GetCurrentColumnValue("OutImageUrl") as string ?? "";
            var bolUrl = GetCurrentColumnValue("BolImageUrl") as string ?? "";

            ApplyLinkStyle(detInWt,  !string.IsNullOrEmpty(inUrl));
            ApplyLinkStyle(detOutWt, !string.IsNullOrEmpty(outUrl));
            ApplyLinkStyle(detBOL,   !string.IsNullOrEmpty(bolUrl));
        }

        private static void ApplyLinkStyle(XRTableCell cell, bool isLink)
        {
            if (isLink)
            {
                cell.ForeColor = Color.Blue;
                cell.Font = new DXFont(cell.Font.Name, cell.Font.Size, DXFontStyle.Underline);
            }
            else
            {
                cell.ForeColor = Color.Black;
                cell.Font = new DXFont(cell.Font.Name, cell.Font.Size, DXFontStyle.Regular);
            }
        }

        // VOID watermark is applied at print time rather than in the designer
        // because the watermark is per-document and we need the actual data
        // to know whether the WS has any loads.
        private void ApplyVoidWatermarkIfEmpty(object sender, CancelEventArgs e)
        {
            var dto = (this.DataSource as IEnumerable)?
                .OfType<IntakeWeightSheetDto>()
                .FirstOrDefault();
            if (dto?.Loads is { Count: > 0 }) return;

            this.Watermark.Text = "VOID";
            this.Watermark.Font = new DXFont("Arial", 72f, DXFontStyle.Bold);
            this.Watermark.TextDirection = DirectionMode.ForwardDiagonal;
            this.Watermark.ForeColor = Color.Red;
            this.Watermark.TextTransparency = 200;
            this.Watermark.ShowBehind = false;
        }
    }
}
