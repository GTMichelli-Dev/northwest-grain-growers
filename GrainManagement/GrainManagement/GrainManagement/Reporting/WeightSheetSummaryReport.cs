using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    public partial class WeightSheetSummaryReport : XtraReport
    {
        public WeightSheetSummaryReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
