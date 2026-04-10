using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    public partial class OutboundLoadTicketReport : XtraReport
    {
        public OutboundLoadTicketReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
