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
            DevExpress.XtraPrinting.BarCode.QRCodeGenerator qrCodeGenerator3 = new DevExpress.XtraPrinting.BarCode.QRCodeGenerator();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestTicketReport));
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
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.HeightF = 40F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.HeightF = 20F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
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
            this.lblFooter});
            this.detailBand1.HeightF = 426.4583F;
            this.detailBand1.Name = "detailBand1";
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new DevExpress.Drawing.DXFont("Arial", 22F, DevExpress.Drawing.DXFontStyle.Bold);
            this.lblTitle.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 10.00001F);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.SizeF = new System.Drawing.SizeF(242.9999F, 35F);
            this.lblTitle.StylePriority.UseFont = false;
            this.lblTitle.Text = "TEST PAGE";
            this.lblTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.Font = new DevExpress.Drawing.DXFont("Arial", 12F);
            this.lblSubtitle.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 50F);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.SizeF = new System.Drawing.SizeF(242.9999F, 22F);
            this.lblSubtitle.StylePriority.UseFont = false;
            this.lblSubtitle.Text = "Web Print Service";
            this.lblSubtitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // line1
            // 
            this.line1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 78.00001F);
            this.line1.Name = "line1";
            this.line1.SizeF = new System.Drawing.SizeF(242.9998F, 2F);
            // 
            // lblMessage
            // 
            this.lblMessage.Font = new DevExpress.Drawing.DXFont("Arial", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.lblMessage.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 89.99999F);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.SizeF = new System.Drawing.SizeF(242.9999F, 39.99999F);
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
            this.valServiceId.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.valServiceId.LocationFloat = new DevExpress.Utils.PointFloat(115F, 130F);
            this.valServiceId.Name = "valServiceId";
            this.valServiceId.SizeF = new System.Drawing.SizeF(137.9999F, 20F);
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
            this.valPrinter.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.valPrinter.LocationFloat = new DevExpress.Utils.PointFloat(115F, 155F);
            this.valPrinter.Name = "valPrinter";
            this.valPrinter.SizeF = new System.Drawing.SizeF(137.9999F, 20F);
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
            this.valDate.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.valDate.LocationFloat = new DevExpress.Utils.PointFloat(115F, 180F);
            this.valDate.Name = "valDate";
            this.valDate.SizeF = new System.Drawing.SizeF(137.9999F, 20F);
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
            this.valPlatform.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.valPlatform.LocationFloat = new DevExpress.Utils.PointFloat(115F, 205F);
            this.valPlatform.Name = "valPlatform";
            this.valPlatform.SizeF = new System.Drawing.SizeF(137.9999F, 20F);
            this.valPlatform.StylePriority.UseFont = false;
            this.valPlatform.Text = "Windows";
            // 
            // line2
            // 
            this.line2.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 235F);
            this.line2.Name = "line2";
            this.line2.SizeF = new System.Drawing.SizeF(242.9999F, 2F);
            // 
            // lblFooter
            // 
            this.lblFooter.Font = new DevExpress.Drawing.DXFont("Arial", 8F, DevExpress.Drawing.DXFontStyle.Italic);
            this.lblFooter.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 245F);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.SizeF = new System.Drawing.SizeF(242.9999F, 30.62498F);
            this.lblFooter.StylePriority.UseFont = false;
            this.lblFooter.Text = "Grain Management - Web Print Service Test Page";
            this.lblFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrBarCode1});
            this.ReportHeader.HeightF = 128.75F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 10.00001F);
            this.xrBarCode1.Module = 4F;
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10F, 10F, 0F, 0F, 100F);
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(243F, 118.75F);
            this.xrBarCode1.StylePriority.UseTextAlignment = false;
            this.xrBarCode1.Symbology = qrCodeGenerator3;
            this.xrBarCode1.Text = "604063000001";
            this.xrBarCode1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(30.2083F, 275.625F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(184.875F, 150.8333F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // TestTicketReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1,
            this.ReportHeader});
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(10F, 10F, 40F, 20F);
            this.PageHeightF = 600F;
            this.PageWidthF = 283F;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            this.RollPaper = true;
            this.Version = "25.2";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        private ReportHeaderBand ReportHeader;
        private XRBarCode xrBarCode1;
        private XRPictureBox xrPictureBox1;
    }
}
