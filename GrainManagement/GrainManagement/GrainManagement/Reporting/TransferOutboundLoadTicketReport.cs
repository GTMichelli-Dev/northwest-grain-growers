using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    public partial class TransferOutboundLoadTicketReport : XtraReport
    {
        public TransferOutboundLoadTicketReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
