using DevExpress.XtraReports.UI;
using DevExpress.Drawing.Printing;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace GrainManagement.Reporting
{
    public partial class LoadTicketReport : DevExpress.XtraReports.UI.XtraReport
    {
        public LoadTicketReport()
        {
            InitializeComponent();
            //RollPaper = true;
            //PaperKind = DXPaperKind.Custom;
            //// CRITICAL for thermal printers
            //ReportUnit = ReportUnit.HundredthsOfAnInch;
          

            //// 80mm paper ≈ 3.15 inches
            //PageWidth = 315;

            //// Make page height LARGE so it never clips
            //PageHeight = 2000;

            //// Generous margins (thermal printers NEED this)
            //Margins = new DevExpress.Drawing.DXMargins(
            //    left: 10,
            //    right: 10,
            //    top: 30,
            //    bottom: 40
            //);

            this.CanShrink = true;
            this.CanGrow = true;
        }
    }
}
