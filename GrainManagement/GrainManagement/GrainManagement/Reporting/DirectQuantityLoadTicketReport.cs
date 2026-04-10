using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    public partial class DirectQuantityLoadTicketReport : XtraReport
    {
        public DirectQuantityLoadTicketReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
