using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    public partial class TransferInboundLoadTicketReport : XtraReport
    {
        public TransferInboundLoadTicketReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
