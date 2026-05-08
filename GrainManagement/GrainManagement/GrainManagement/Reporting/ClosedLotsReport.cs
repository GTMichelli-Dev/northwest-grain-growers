using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    /// <summary>
    /// Closed Lots summary. Bound to <see cref="GrainManagement.Dtos.Warehouse.ClosedLotsReportDto"/>.
    /// Used by both the End Of Day flow (date range collapses to a single
    /// day) and the on-demand closed-lots-by-date-range view.
    /// </summary>
    public partial class ClosedLotsReport : XtraReport
    {
        public ClosedLotsReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
