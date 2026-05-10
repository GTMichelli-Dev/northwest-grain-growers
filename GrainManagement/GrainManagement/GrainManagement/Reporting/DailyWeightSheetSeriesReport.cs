using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    /// <summary>
    /// Daily Weight Sheet Series report — one row per WS at a location on a
    /// given day. Bound to <see cref="GrainManagement.Dtos.Warehouse.DailyWeightSheetSeriesReportDto"/>.
    /// Header band repeats the location/day; detail iterates over Rows;
    /// report footer shows total loads and total net.
    /// </summary>
    public partial class DailyWeightSheetSeriesReport : XtraReport
    {
        public DailyWeightSheetSeriesReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
