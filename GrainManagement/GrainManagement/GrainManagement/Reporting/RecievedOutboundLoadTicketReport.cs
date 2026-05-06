using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    public partial class RecievedOutboundLoadTicketReport : XtraReport
    {
        public RecievedOutboundLoadTicketReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
