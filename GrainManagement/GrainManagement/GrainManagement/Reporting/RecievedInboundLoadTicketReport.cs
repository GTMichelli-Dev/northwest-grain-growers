using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    public partial class RecievedInboundLoadTicketReport : XtraReport
    {
        public RecievedInboundLoadTicketReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
