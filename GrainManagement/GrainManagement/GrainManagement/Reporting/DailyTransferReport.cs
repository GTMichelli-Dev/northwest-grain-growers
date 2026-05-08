using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    /// <summary>
    /// End Of Day Transfer summary. Bound to <see cref="GrainManagement.Dtos.Warehouse.DailyTransferReportDto"/>.
    /// Open in the DevExpress XtraReports designer to refine layout, add
    /// Commodity grouping, and totals.
    /// </summary>
    public partial class DailyTransferReport : XtraReport
    {
        public DailyTransferReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
