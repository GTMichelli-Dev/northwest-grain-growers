using DevExpress.XtraReports.UI;

namespace GrainManagement.Reporting
{
    /// <summary>
    /// PDF layout for a Transfer Weight Sheet. Layout mirrors
    /// IntakeWeightSheetReport with lot-specific fields dropped and
    /// source/destination/direction added. The visual layout in the
    /// Designer should be adjusted in the DevExpress Report Designer to
    /// rebind the lot-named cells to Variety / SourceLocation /
    /// DestinationLocation / Direction; missing bindings render blank.
    /// </summary>
    public partial class TransferWeightSheetReport : XtraReport
    {
        public TransferWeightSheetReport()
        {
            InitializeComponent();
            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
