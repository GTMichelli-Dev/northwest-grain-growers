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
