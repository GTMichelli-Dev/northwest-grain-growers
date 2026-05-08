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
    /// <summary>
    /// PDF layout for a Transfer Weight Sheet. Layout mirrors
    /// IntakeWeightSheetReport with lot-specific fields dropped and
    /// source/destination/direction added. The visual layout in the
    /// Designer should be adjusted in the DevExpress Report Designer to
    /// rebind the lot-named cells to Variety / SourceLocation /
    /// DestinationLocation / Direction; missing bindings render blank.
    /// </summary>
    public partial class TransferWeightSheetReport : XtraReport
    {
        public TransferWeightSheetReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
            this.BeforePrint += ApplyVoidWatermarkIfEmpty;
        }

        private void ApplyVoidWatermarkIfEmpty(object sender, CancelEventArgs e)
        {
            var dto = (this.DataSource as IEnumerable)?
                .OfType<TransferWeightSheetDto>()
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
