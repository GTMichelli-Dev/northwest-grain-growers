namespace GrainManagement.Reporting
{
#nullable disable

    using System.ComponentModel;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraReports.UI;

    partial class TestTicketReport
    {
        private IContainer components = null;

        private TopMarginBand topMarginBand1;
        private BottomMarginBand bottomMarginBand1;
        private DetailBand detailBand1;

        private XRLabel lblTitle;
        private XRLabel lblSubtitle;
        private XRLine line1;
        private XRLabel lblMessage;
        private XRLabel capServiceId;
        private XRLabel valServiceId;
        private XRLabel capPrinter;
        private XRLabel valPrinter;
        private XRLabel capDate;
        private XRLabel valDate;
        private XRLabel capPlatform;
        private XRLabel valPlatform;
        private XRLine line2;
        private XRLabel lblFooter;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.lblTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.lblSubtitle = new DevExpress.XtraReports.UI.XRLabel();
            this.line1 = new DevExpress.XtraReports.UI.XRLine();
            this.lblMessage = new DevExpress.XtraReports.UI.XRLabel();
            this.capServiceId = new DevExpress.XtraReports.UI.XRLabel();
            this.valServiceId = new DevExpress.XtraReports.UI.XRLabel();
            this.capPrinter = new DevExpress.XtraReports.UI.XRLabel();
            this.valPrinter = new DevExpress.XtraReports.UI.XRLabel();
            this.capDate = new DevExpress.XtraReports.UI.XRLabel();
            this.valDate = new DevExpress.XtraReports.UI.XRLabel();
            this.capPlatform = new DevExpress.XtraReports.UI.XRLabel();
            this.valPlatform = new DevExpress.XtraReports.UI.XRLabel();
            this.line2 = new DevExpress.XtraReports.UI.XRLine();
            this.lblFooter = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            //
            // topMarginBand1
            //
            this.topMarginBand1.HeightF = 40F;
            this.topMarginBand1.Name = "topMarginBand1";
            //
            // bottomMarginBand1
            //
            this.bottomMarginBand1.HeightF = 40F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            //
            // lblTitle
            //
            this.lblTitle.Font = new DevExpress.Drawing.DXFont("Arial", 22F, DevExpress.Drawing.DXFontStyle.Bold);
            this.lblTitle.LocationFloat = new DevExpress.Utils.PointFloat(10F, 10F);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.SizeF = new System.Drawing.SizeF(254F, 35F);
            this.lblTitle.StylePriority.UseFont = false;
            this.lblTitle.Text = "TEST PAGE";
            this.lblTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // lblSubtitle
            //
            this.lblSubtitle.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Regular);
            this.lblSubtitle.LocationFloat = new DevExpress.Utils.PointFloat(10F, 50F);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.SizeF = new System.Drawing.SizeF(254F, 22F);
            this.lblSubtitle.StylePriority.UseFont = false;
            this.lblSubtitle.Text = "Web Print Service";
            this.lblSubtitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // line1
            //
            this.line1.LocationFloat = new DevExpress.Utils.PointFloat(10F, 78F);
            this.line1.Name = "line1";
            this.line1.SizeF = new System.Drawing.SizeF(254F, 2F);
            //
            // lblMessage
            //
            this.lblMessage.Font = new DevExpress.Drawing.DXFont("Arial", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.lblMessage.LocationFloat = new DevExpress.Utils.PointFloat(10F, 90F);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.SizeF = new System.Drawing.SizeF(254F, 22F);
            this.lblMessage.StylePriority.UseFont = false;
            this.lblMessage.Text = "If you can read this, printing is working.";
            this.lblMessage.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // capServiceId
            //
            this.capServiceId.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capServiceId.LocationFloat = new DevExpress.Utils.PointFloat(10F, 130F);
            this.capServiceId.Name = "capServiceId";
            this.capServiceId.SizeF = new System.Drawing.SizeF(100F, 20F);
            this.capServiceId.StylePriority.UseFont = false;
            this.capServiceId.Text = "Service ID:";
            //
            // valServiceId
            //
            this.valServiceId.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Regular);
            this.valServiceId.LocationFloat = new DevExpress.Utils.PointFloat(115F, 130F);
            this.valServiceId.Name = "valServiceId";
            this.valServiceId.SizeF = new System.Drawing.SizeF(149F, 20F);
            this.valServiceId.StylePriority.UseFont = false;
            this.valServiceId.Text = "default";
            //
            // capPrinter
            //
            this.capPrinter.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capPrinter.LocationFloat = new DevExpress.Utils.PointFloat(10F, 155F);
            this.capPrinter.Name = "capPrinter";
            this.capPrinter.SizeF = new System.Drawing.SizeF(100F, 20F);
            this.capPrinter.StylePriority.UseFont = false;
            this.capPrinter.Text = "Printer:";
            //
            // valPrinter
            //
            this.valPrinter.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Regular);
            this.valPrinter.LocationFloat = new DevExpress.Utils.PointFloat(115F, 155F);
            this.valPrinter.Name = "valPrinter";
            this.valPrinter.SizeF = new System.Drawing.SizeF(149F, 20F);
            this.valPrinter.StylePriority.UseFont = false;
            this.valPrinter.Text = "Test Printer";
            //
            // capDate
            //
            this.capDate.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capDate.LocationFloat = new DevExpress.Utils.PointFloat(10F, 180F);
            this.capDate.Name = "capDate";
            this.capDate.SizeF = new System.Drawing.SizeF(100F, 20F);
            this.capDate.StylePriority.UseFont = false;
            this.capDate.Text = "Date/Time:";
            //
            // valDate
            //
            this.valDate.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Regular);
            this.valDate.LocationFloat = new DevExpress.Utils.PointFloat(115F, 180F);
            this.valDate.Name = "valDate";
            this.valDate.SizeF = new System.Drawing.SizeF(149F, 20F);
            this.valDate.StylePriority.UseFont = false;
            this.valDate.Text = "2026-01-01 12:00:00";
            //
            // capPlatform
            //
            this.capPlatform.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capPlatform.LocationFloat = new DevExpress.Utils.PointFloat(10F, 205F);
            this.capPlatform.Name = "capPlatform";
            this.capPlatform.SizeF = new System.Drawing.SizeF(100F, 20F);
            this.capPlatform.StylePriority.UseFont = false;
            this.capPlatform.Text = "Platform:";
            //
            // valPlatform
            //
            this.valPlatform.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Regular);
            this.valPlatform.LocationFloat = new DevExpress.Utils.PointFloat(115F, 205F);
            this.valPlatform.Name = "valPlatform";
            this.valPlatform.SizeF = new System.Drawing.SizeF(149F, 20F);
            this.valPlatform.StylePriority.UseFont = false;
            this.valPlatform.Text = "Windows";
            //
            // line2
            //
            this.line2.LocationFloat = new DevExpress.Utils.PointFloat(10F, 235F);
            this.line2.Name = "line2";
            this.line2.SizeF = new System.Drawing.SizeF(254F, 2F);
            //
            // lblFooter
            //
            this.lblFooter.Font = new DevExpress.Drawing.DXFont("Arial", 8F, DevExpress.Drawing.DXFontStyle.Italic);
            this.lblFooter.LocationFloat = new DevExpress.Utils.PointFloat(10F, 245F);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.SizeF = new System.Drawing.SizeF(254F, 18F);
            this.lblFooter.StylePriority.UseFont = false;
            this.lblFooter.Text = "Grain Management - Web Print Service Test Page";
            this.lblFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // detailBand1
            //
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
                this.lblTitle,
                this.lblSubtitle,
                this.line1,
                this.lblMessage,
                this.capServiceId,
                this.valServiceId,
                this.capPrinter,
                this.valPrinter,
                this.capDate,
                this.valDate,
                this.capPlatform,
                this.valPlatform,
                this.line2,
                this.lblFooter
            });
            this.detailBand1.HeightF = 275F;
            this.detailBand1.Name = "detailBand1";
            //
            // TestTicketReport
            //
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
                this.topMarginBand1,
                this.detailBand1,
                this.bottomMarginBand1
            });
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(10F, 10F, 40F, 40F);
            this.PageWidth = 283;
            this.PageHeight = 400;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            this.Version = "24.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
        }
    }
}
