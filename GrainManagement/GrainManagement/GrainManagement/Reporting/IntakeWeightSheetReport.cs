using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    public partial class IntakeWeightSheetReport : XtraReport
    {
        public IntakeWeightSheetReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
