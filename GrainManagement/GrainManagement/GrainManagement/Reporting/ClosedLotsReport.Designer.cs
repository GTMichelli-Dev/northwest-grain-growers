namespace GrainManagement.Reporting
{
#nullable disable

    using System.ComponentModel;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraReports.UI;

    partial class ClosedLotsReport
    {
        private IContainer components = null;

        private DevExpress.XtraReports.UI.TopMarginBand topMarginBand1;
        private DevExpress.XtraReports.UI.ReportHeaderBand reportHeaderBand1;
        private DevExpress.XtraReports.UI.PageHeaderBand pageHeaderBand1;
        private DevExpress.XtraReports.UI.DetailBand detailBand1;
        private DevExpress.XtraReports.UI.ReportFooterBand reportFooterBand1;
        private DevExpress.XtraReports.UI.BottomMarginBand bottomMarginBand1;

        private DevExpress.DataAccess.ObjectBinding.ObjectDataSource objectDataSource1;

        private DevExpress.XtraReports.UI.XRLabel xrTitle;
        private DevExpress.XtraReports.UI.XRLabel xrLocation;
        private DevExpress.XtraReports.UI.XRLabel xrDateRange;

        private DevExpress.XtraReports.UI.XRLabel capLot;
        private DevExpress.XtraReports.UI.XRLabel capClosed;
        private DevExpress.XtraReports.UI.XRLabel capCrop;
        private DevExpress.XtraReports.UI.XRLabel capProducer;

        private DevExpress.XtraReports.UI.XRLabel xrLot;
        private DevExpress.XtraReports.UI.XRLabel xrClosed;
        private DevExpress.XtraReports.UI.XRLabel xrCrop;
        private DevExpress.XtraReports.UI.XRLabel xrProducer;

        private DevExpress.XtraReports.UI.XRLabel xrTotalLabel;
        private DevExpress.XtraReports.UI.XRLabel xrTotalCount;

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
            this.reportHeaderBand1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.pageHeaderBand1 = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.reportFooterBand1 = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.xrTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLocation = new DevExpress.XtraReports.UI.XRLabel();
            this.xrDateRange = new DevExpress.XtraReports.UI.XRLabel();
            this.capLot = new DevExpress.XtraReports.UI.XRLabel();
            this.capClosed = new DevExpress.XtraReports.UI.XRLabel();
            this.capCrop = new DevExpress.XtraReports.UI.XRLabel();
            this.capProducer = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLot = new DevExpress.XtraReports.UI.XRLabel();
            this.xrClosed = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCrop = new DevExpress.XtraReports.UI.XRLabel();
            this.xrProducer = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTotalLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTotalCount = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            //
            // topMarginBand1
            //
            this.topMarginBand1.HeightF = 40F;
            this.topMarginBand1.Name = "topMarginBand1";
            //
            // reportHeaderBand1
            //
            this.reportHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTitle,
            this.xrLocation,
            this.xrDateRange});
            this.reportHeaderBand1.HeightF = 78F;
            this.reportHeaderBand1.Name = "reportHeaderBand1";
            //
            // pageHeaderBand1
            //
            this.pageHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.capLot,
            this.capClosed,
            this.capCrop,
            this.capProducer});
            this.pageHeaderBand1.HeightF = 22F;
            this.pageHeaderBand1.Name = "pageHeaderBand1";
            //
            // detailBand1
            //
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLot,
            this.xrClosed,
            this.xrCrop,
            this.xrProducer});
            this.detailBand1.HeightF = 22F;
            this.detailBand1.Name = "detailBand1";
            //
            // reportFooterBand1
            //
            this.reportFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTotalLabel,
            this.xrTotalCount});
            this.reportFooterBand1.HeightF = 32F;
            this.reportFooterBand1.Name = "reportFooterBand1";
            //
            // bottomMarginBand1
            //
            this.bottomMarginBand1.HeightF = 40F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            //
            // objectDataSource1
            //
            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.ClosedLotsReportDto);
            this.objectDataSource1.Name = "objectDataSource1";
            //
            // xrTitle
            //
            this.xrTitle.Font = new DevExpress.Drawing.DXFont("Tahoma", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTitle.Name = "xrTitle";
            this.xrTitle.SizeF = new System.Drawing.SizeF(720F, 26F);
            this.xrTitle.StylePriority.UseFont = false;
            this.xrTitle.StylePriority.UseTextAlignment = false;
            this.xrTitle.Text = "Closed Lots";
            this.xrTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // xrLocation
            //
            this.xrLocation.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LocationHeader]")});
            this.xrLocation.Font = new DevExpress.Drawing.DXFont("Tahoma", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLocation.LocationFloat = new DevExpress.Utils.PointFloat(0F, 28F);
            this.xrLocation.Name = "xrLocation";
            this.xrLocation.SizeF = new System.Drawing.SizeF(720F, 22F);
            this.xrLocation.StylePriority.UseFont = false;
            this.xrLocation.StylePriority.UseTextAlignment = false;
            this.xrLocation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // xrDateRange
            //
            this.xrDateRange.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DateRangeHeader]")});
            this.xrDateRange.LocationFloat = new DevExpress.Utils.PointFloat(0F, 52F);
            this.xrDateRange.Name = "xrDateRange";
            this.xrDateRange.SizeF = new System.Drawing.SizeF(720F, 20F);
            this.xrDateRange.StylePriority.UseTextAlignment = false;
            this.xrDateRange.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // capLot
            //
            this.capLot.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capLot.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capLot.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.capLot.Name = "capLot";
            this.capLot.SizeF = new System.Drawing.SizeF(140F, 20F);
            this.capLot.StylePriority.UseBorders = false;
            this.capLot.StylePriority.UseFont = false;
            this.capLot.Text = "Lot";
            //
            // capClosed
            //
            this.capClosed.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capClosed.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capClosed.LocationFloat = new DevExpress.Utils.PointFloat(140F, 0F);
            this.capClosed.Name = "capClosed";
            this.capClosed.SizeF = new System.Drawing.SizeF(120F, 20F);
            this.capClosed.StylePriority.UseBorders = false;
            this.capClosed.StylePriority.UseFont = false;
            this.capClosed.Text = "Closed";
            //
            // capCrop
            //
            this.capCrop.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capCrop.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capCrop.LocationFloat = new DevExpress.Utils.PointFloat(260F, 0F);
            this.capCrop.Name = "capCrop";
            this.capCrop.SizeF = new System.Drawing.SizeF(220F, 20F);
            this.capCrop.StylePriority.UseBorders = false;
            this.capCrop.StylePriority.UseFont = false;
            this.capCrop.Text = "Crop";
            //
            // capProducer
            //
            this.capProducer.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capProducer.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capProducer.LocationFloat = new DevExpress.Utils.PointFloat(480F, 0F);
            this.capProducer.Name = "capProducer";
            this.capProducer.SizeF = new System.Drawing.SizeF(240F, 20F);
            this.capProducer.StylePriority.UseBorders = false;
            this.capProducer.StylePriority.UseFont = false;
            this.capProducer.Text = "Producer";
            //
            // xrLot
            //
            this.xrLot.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LotNumber]")});
            this.xrLot.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrLot.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLot.Name = "xrLot";
            this.xrLot.SizeF = new System.Drawing.SizeF(140F, 20F);
            this.xrLot.StylePriority.UseFont = false;
            //
            // xrClosed
            //
            this.xrClosed.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CloseDate]")});
            this.xrClosed.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrClosed.LocationFloat = new DevExpress.Utils.PointFloat(140F, 0F);
            this.xrClosed.Name = "xrClosed";
            this.xrClosed.SizeF = new System.Drawing.SizeF(120F, 20F);
            this.xrClosed.StylePriority.UseFont = false;
            //
            // xrCrop
            //
            this.xrCrop.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Crop]")});
            this.xrCrop.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrCrop.LocationFloat = new DevExpress.Utils.PointFloat(260F, 0F);
            this.xrCrop.Name = "xrCrop";
            this.xrCrop.SizeF = new System.Drawing.SizeF(220F, 20F);
            this.xrCrop.StylePriority.UseFont = false;
            //
            // xrProducer
            //
            this.xrProducer.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Producer]")});
            this.xrProducer.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrProducer.LocationFloat = new DevExpress.Utils.PointFloat(480F, 0F);
            this.xrProducer.Name = "xrProducer";
            this.xrProducer.SizeF = new System.Drawing.SizeF(240F, 20F);
            this.xrProducer.StylePriority.UseFont = false;
            //
            // xrTotalLabel
            //
            this.xrTotalLabel.Font = new DevExpress.Drawing.DXFont("Tahoma", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTotalLabel.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4F);
            this.xrTotalLabel.Name = "xrTotalLabel";
            this.xrTotalLabel.SizeF = new System.Drawing.SizeF(180F, 20F);
            this.xrTotalLabel.StylePriority.UseFont = false;
            this.xrTotalLabel.Text = "Total # Closed Lots:";
            //
            // xrTotalCount
            //
            this.xrTotalCount.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TotalCount]")});
            this.xrTotalCount.Font = new DevExpress.Drawing.DXFont("Tahoma", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTotalCount.LocationFloat = new DevExpress.Utils.PointFloat(180F, 4F);
            this.xrTotalCount.Name = "xrTotalCount";
            this.xrTotalCount.SizeF = new System.Drawing.SizeF(80F, 20F);
            this.xrTotalCount.StylePriority.UseFont = false;
            //
            // ClosedLotsReport
            //
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.reportHeaderBand1,
            this.pageHeaderBand1,
            this.detailBand1,
            this.reportFooterBand1,
            this.bottomMarginBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataMember = "Rows";
            this.DataSource = this.objectDataSource1;
            this.Margins = new System.Drawing.Printing.Margins(40, 40, 40, 40);
            this.PageHeight = 1100;
            this.PageWidth = 850;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Letter;
            this.Version = "25.2";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
        }
    }
}
