namespace Seed25.Report
{
    partial class Invoice
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Invoice));
            DevExpress.XtraPrinting.Drawing.Watermark watermark1 = new DevExpress.XtraPrinting.Drawing.Watermark();
            DevExpress.XtraPrinting.Drawing.Watermark watermark2 = new DevExpress.XtraPrinting.Drawing.Watermark();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLblTreatedSeed = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.VerticalDetail = new DevExpress.XtraReports.UI.VerticalDetailBand();
            this.xrWeightSubReport = new DevExpress.XtraReports.UI.XRSubreport();
            this.VerticalHeader = new DevExpress.XtraReports.UI.VerticalHeaderBand();
            this.xrVarietiesSubReport = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrMiscSubReport = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrTreatmentSubReport = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrAnalysisSubreport = new DevExpress.XtraReports.UI.XRSubreport();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrLblTreatedSeed,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1,
            this.xrPictureBox1});
            this.TopMargin.Dpi = 254F;
            this.TopMargin.HeightF = 381.198F;
            this.TopMargin.Name = "TopMargin";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Dpi = 254F;
            this.xrLabel8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CustomerName]")});
            this.xrLabel8.Font = new DevExpress.Drawing.DXFont("Arial", 18F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 255.3229F);
            this.xrLabel8.Multiline = true;
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(2080F, 70F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "xrLabel8";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLblTreatedSeed
            // 
            this.xrLblTreatedSeed.Dpi = 254F;
            this.xrLblTreatedSeed.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "[TreatedSeed]\n")});
            this.xrLblTreatedSeed.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F, ((DevExpress.Drawing.DXFontStyle)((DevExpress.Drawing.DXFontStyle.Bold | DevExpress.Drawing.DXFontStyle.Italic))));
            this.xrLblTreatedSeed.LocationFloat = new DevExpress.Utils.PointFloat(0F, 325.3229F);
            this.xrLblTreatedSeed.Multiline = true;
            this.xrLblTreatedSeed.Name = "xrLblTreatedSeed";
            this.xrLblTreatedSeed.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLblTreatedSeed.SizeF = new System.Drawing.SizeF(2080F, 40.00003F);
            this.xrLblTreatedSeed.StylePriority.UseFont = false;
            this.xrLblTreatedSeed.StylePriority.UseTextAlignment = false;
            this.xrLblTreatedSeed.Text = "TREATED SEED - NOT RETURNABLE";
            this.xrLblTreatedSeed.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Dpi = 254F;
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(1696.563F, 5.050402F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(341.3125F, 38.47046F);
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Ticket";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel5
            // 
            this.xrLabel5.Dpi = 254F;
            this.xrLabel5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Ticket]")});
            this.xrLabel5.Font = new DevExpress.Drawing.DXFont("Arial", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(1696.563F, 43.52084F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(341.3125F, 66.35751F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "xrLabel5";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Dpi = 254F;
            this.xrLabel4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Phone]")});
            this.xrLabel4.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(818.1458F, 188.3833F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(568.8542F, 53.12833F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "xrLabel4";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 254F;
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Address2]")});
            this.xrLabel3.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(818.1458F, 129.9633F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(568.8542F, 53.12833F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "xrLabel3";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 254F;
            this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Address1]")});
            this.xrLabel2.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(818.1458F, 71.54331F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(568.8542F, 53.12833F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "xrLabel2";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 254F;
            this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Location]")});
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(818.1458F, 13.12331F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 5, 0, 0, 254F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(568.8542F, 53.12833F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "xrLabel1";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Dpi = 254F;
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(510.6458F, 255.3229F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 254F;
            this.BottomMargin.HeightF = 284.9687F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(global::Seed25.DTO.InvoiceDTO);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // VerticalDetail
            // 
            this.VerticalDetail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrWeightSubReport});
            this.VerticalDetail.Dpi = 254F;
            this.VerticalDetail.HeightF = 394.0312F;
            this.VerticalDetail.Name = "VerticalDetail";
            this.VerticalDetail.WidthF = 666.75F;
            // 
            // xrWeightSubReport
            // 
            this.xrWeightSubReport.Dpi = 254F;
            this.xrWeightSubReport.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrWeightSubReport.Name = "xrWeightSubReport";
            this.xrWeightSubReport.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            // 
            // VerticalHeader
            // 
            this.VerticalHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrAnalysisSubreport,
            this.xrVarietiesSubReport,
            this.xrMiscSubReport,
            this.xrTreatmentSubReport});
            this.VerticalHeader.Dpi = 254F;
            this.VerticalHeader.HeightF = 394.0312F;
            this.VerticalHeader.Name = "VerticalHeader";
            this.VerticalHeader.WidthF = 1413.458F;
            // 
            // xrVarietiesSubReport
            // 
            this.xrVarietiesSubReport.Dpi = 254F;
            this.xrVarietiesSubReport.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrVarietiesSubReport.Name = "xrVarietiesSubReport";
            this.xrVarietiesSubReport.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            // 
            // xrMiscSubReport
            // 
            this.xrMiscSubReport.Dpi = 254F;
            this.xrMiscSubReport.LocationFloat = new DevExpress.Utils.PointFloat(0F, 146.7379F);
            this.xrMiscSubReport.Name = "xrMiscSubReport";
            this.xrMiscSubReport.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            // 
            // xrTreatmentSubReport
            // 
            this.xrTreatmentSubReport.Dpi = 254F;
            this.xrTreatmentSubReport.LocationFloat = new DevExpress.Utils.PointFloat(0F, 75.30039F);
            this.xrTreatmentSubReport.Name = "xrTreatmentSubReport";
            this.xrTreatmentSubReport.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            // 
            // xrAnalysisSubreport
            // 
            this.xrAnalysisSubreport.Dpi = 254F;
            this.xrAnalysisSubreport.LocationFloat = new DevExpress.Utils.PointFloat(0F, 219.3003F);
            this.xrAnalysisSubreport.Name = "xrAnalysisSubreport";
            this.xrAnalysisSubreport.SizeF = new System.Drawing.SizeF(254F, 58.42F);
            // 
            // Invoice
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.TopMargin,
            this.BottomMargin,
            this.VerticalDetail,
            this.VerticalHeader});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.Dpi = 254F;
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9.75F);
            this.Margins = new DevExpress.Drawing.DXMargins(10F, 10F, 381.198F, 284.9687F);
            this.PageHeight = 2970;
            this.PageWidth = 2100;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.A4;
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.TenthsOfAMillimeter;
            this.SnapGridSize = 25F;
            this.Version = "24.2";
            watermark1.Font = new DevExpress.Drawing.DXFont("Verdana", 72F, DevExpress.Drawing.DXFontStyle.Bold);
            watermark1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(118)))), ((int)(((byte)(146)))), ((int)(((byte)(60)))));
            watermark1.Id = "Coaxium";
            watermark1.Text = "COAXIUM";
            watermark1.TextPosition = DevExpress.XtraPrinting.Drawing.WatermarkPosition.InFront;
            watermark1.TextTransparency = 165;
            watermark2.Font = new DevExpress.Drawing.DXFont("Verdana", 72F, DevExpress.Drawing.DXFontStyle.Bold);
            watermark2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            watermark2.Id = "Clearfield";
            watermark2.Text = "CLEARFIELD";
            watermark2.TextPosition = DevExpress.XtraPrinting.Drawing.WatermarkPosition.InFront;
            watermark2.TextTransparency = 165;
            this.Watermarks.AddRange(new DevExpress.XtraPrinting.Drawing.Watermark[] {
            watermark1,
            watermark2});
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRPictureBox xrPictureBox1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel5;
        private DevExpress.XtraReports.UI.XRLabel xrLabel4;
        private DevExpress.XtraReports.UI.XRLabel xrLabel3;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;
        private DevExpress.XtraReports.UI.VerticalDetailBand VerticalDetail;
        private DevExpress.XtraReports.UI.VerticalHeaderBand VerticalHeader;
        private DevExpress.XtraReports.UI.XRSubreport xrWeightSubReport;
        private DevExpress.XtraReports.UI.XRSubreport xrMiscSubReport;
        private DevExpress.XtraReports.UI.XRSubreport xrTreatmentSubReport;
        private DevExpress.XtraReports.UI.XRLabel xrLblTreatedSeed;
        private DevExpress.XtraReports.UI.XRLabel xrLabel6;
        private DevExpress.XtraReports.UI.XRLabel xrLabel8;
        private DevExpress.XtraReports.UI.XRSubreport xrVarietiesSubReport;
        private DevExpress.XtraReports.UI.XRSubreport xrAnalysisSubreport;
    }
}
