using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    public partial class InboundLoadTicketReport : XtraReport
    {
        public InboundLoadTicketReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
