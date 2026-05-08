using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    /// <summary>
    /// End Of Day Intake summary. Bound to <see cref="GrainManagement.Dtos.Warehouse.DailyIntakeReportDto"/>
    /// — header fields live on the parent DTO; the detail band iterates over
    /// <c>Rows</c>. Open in the DevExpress XtraReports designer to refine the
    /// layout (group bands, totals, formatting).
    /// </summary>
    public partial class DailyIntakeReport : XtraReport
    {
        public DailyIntakeReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
