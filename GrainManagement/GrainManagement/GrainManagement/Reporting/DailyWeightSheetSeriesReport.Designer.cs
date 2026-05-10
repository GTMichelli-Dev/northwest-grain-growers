namespace GrainManagement.Reporting
{
#nullable disable

    using System.ComponentModel;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraReports.UI;

    partial class DailyWeightSheetSeriesReport
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
        private DevExpress.XtraReports.UI.XRLabel xrFor;
        private DevExpress.XtraReports.UI.XRLabel xrLocation;
        private DevExpress.XtraReports.UI.XRLabel xrReportDate;

        private DevExpress.XtraReports.UI.XRLabel capWs;
        private DevExpress.XtraReports.UI.XRLabel capType;
        private DevExpress.XtraReports.UI.XRLabel capCrop;
        private DevExpress.XtraReports.UI.XRLabel capLoads;
        private DevExpress.XtraReports.UI.XRLabel capNet;

        private DevExpress.XtraReports.UI.XRLabel xrWs;
        private DevExpress.XtraReports.UI.XRLabel xrType;
        private DevExpress.XtraReports.UI.XRLabel xrCrop;
        private DevExpress.XtraReports.UI.XRLabel xrLoads;
        private DevExpress.XtraReports.UI.XRLabel xrNet;

        private DevExpress.XtraReports.UI.XRLabel xrTotalLoadsLabel;
        private DevExpress.XtraReports.UI.XRLabel xrTotalLoads;
        private DevExpress.XtraReports.UI.XRLabel xrTotalNetLabel;
        private DevExpress.XtraReports.UI.XRLabel xrTotalNet;
        private DevExpress.XtraReports.UI.XRLabel xrPageInfo;

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
            this.xrFor = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLocation = new DevExpress.XtraReports.UI.XRLabel();
            this.xrReportDate = new DevExpress.XtraReports.UI.XRLabel();
            this.capWs = new DevExpress.XtraReports.UI.XRLabel();
            this.capType = new DevExpress.XtraReports.UI.XRLabel();
            this.capCrop = new DevExpress.XtraReports.UI.XRLabel();
            this.capLoads = new DevExpress.XtraReports.UI.XRLabel();
            this.capNet = new DevExpress.XtraReports.UI.XRLabel();
            this.xrWs = new DevExpress.XtraReports.UI.XRLabel();
            this.xrType = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCrop = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLoads = new DevExpress.XtraReports.UI.XRLabel();
            this.xrNet = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTotalLoadsLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTotalLoads = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTotalNetLabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTotalNet = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo = new DevExpress.XtraReports.UI.XRLabel();
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
            this.xrFor,
            this.xrLocation,
            this.xrReportDate});
            this.reportHeaderBand1.HeightF = 100F;
            this.reportHeaderBand1.Name = "reportHeaderBand1";
            //
            // pageHeaderBand1
            //
            this.pageHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.capWs,
            this.capType,
            this.capCrop,
            this.capLoads,
            this.capNet});
            this.pageHeaderBand1.HeightF = 22F;
            this.pageHeaderBand1.Name = "pageHeaderBand1";
            //
            // detailBand1
            //
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrWs,
            this.xrType,
            this.xrCrop,
            this.xrLoads,
            this.xrNet});
            this.detailBand1.HeightF = 22F;
            this.detailBand1.Name = "detailBand1";
            //
            // reportFooterBand1
            //
            this.reportFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTotalLoadsLabel,
            this.xrTotalLoads,
            this.xrTotalNetLabel,
            this.xrTotalNet,
            this.xrPageInfo});
            this.reportFooterBand1.HeightF = 56F;
            this.reportFooterBand1.Name = "reportFooterBand1";
            //
            // bottomMarginBand1
            //
            this.bottomMarginBand1.HeightF = 40F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            //
            // objectDataSource1
            //
            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.DailyWeightSheetSeriesReportDto);
            this.objectDataSource1.Name = "objectDataSource1";
            //
            // xrTitle
            //
            this.xrTitle.Font = new DevExpress.Drawing.DXFont("Tahoma", 16F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTitle.Name = "xrTitle";
            this.xrTitle.SizeF = new System.Drawing.SizeF(720F, 30F);
            this.xrTitle.StylePriority.UseFont = false;
            this.xrTitle.StylePriority.UseTextAlignment = false;
            this.xrTitle.Text = "Daily Weight Sheet Report";
            this.xrTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // xrFor
            //
            this.xrFor.Font = new DevExpress.Drawing.DXFont("Tahoma", 10F, DevExpress.Drawing.DXFontStyle.Italic);
            this.xrFor.LocationFloat = new DevExpress.Utils.PointFloat(0F, 32F);
            this.xrFor.Name = "xrFor";
            this.xrFor.SizeF = new System.Drawing.SizeF(720F, 18F);
            this.xrFor.StylePriority.UseFont = false;
            this.xrFor.StylePriority.UseTextAlignment = false;
            this.xrFor.Text = "For";
            this.xrFor.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // xrLocation
            //
            this.xrLocation.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LocationName] + \' - \' + [LocationId]")});
            this.xrLocation.Font = new DevExpress.Drawing.DXFont("Tahoma", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLocation.LocationFloat = new DevExpress.Utils.PointFloat(0F, 52F);
            this.xrLocation.Name = "xrLocation";
            this.xrLocation.SizeF = new System.Drawing.SizeF(720F, 24F);
            this.xrLocation.StylePriority.UseFont = false;
            this.xrLocation.StylePriority.UseTextAlignment = false;
            this.xrLocation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // xrReportDate
            //
            this.xrReportDate.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ReportDate]")});
            this.xrReportDate.Font = new DevExpress.Drawing.DXFont("Tahoma", 10F);
            this.xrReportDate.LocationFloat = new DevExpress.Utils.PointFloat(0F, 78F);
            this.xrReportDate.Name = "xrReportDate";
            this.xrReportDate.SizeF = new System.Drawing.SizeF(720F, 18F);
            this.xrReportDate.StylePriority.UseFont = false;
            this.xrReportDate.StylePriority.UseTextAlignment = false;
            this.xrReportDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // capWs
            //
            this.capWs.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capWs.Font = new DevExpress.Drawing.DXFont("Tahoma", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capWs.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.capWs.Name = "capWs";
            this.capWs.SizeF = new System.Drawing.SizeF(120F, 20F);
            this.capWs.StylePriority.UseBorders = false;
            this.capWs.StylePriority.UseFont = false;
            this.capWs.Text = "Weight Sheet #";
            //
            // capType
            //
            this.capType.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capType.Font = new DevExpress.Drawing.DXFont("Tahoma", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capType.LocationFloat = new DevExpress.Utils.PointFloat(120F, 0F);
            this.capType.Name = "capType";
            this.capType.SizeF = new System.Drawing.SizeF(110F, 20F);
            this.capType.StylePriority.UseBorders = false;
            this.capType.StylePriority.UseFont = false;
            this.capType.Text = "Type";
            //
            // capCrop
            //
            this.capCrop.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capCrop.Font = new DevExpress.Drawing.DXFont("Tahoma", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capCrop.LocationFloat = new DevExpress.Utils.PointFloat(230F, 0F);
            this.capCrop.Name = "capCrop";
            this.capCrop.SizeF = new System.Drawing.SizeF(220F, 20F);
            this.capCrop.StylePriority.UseBorders = false;
            this.capCrop.StylePriority.UseFont = false;
            this.capCrop.Text = "Crop";
            //
            // capLoads
            //
            this.capLoads.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capLoads.Font = new DevExpress.Drawing.DXFont("Tahoma", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capLoads.LocationFloat = new DevExpress.Utils.PointFloat(450F, 0F);
            this.capLoads.Name = "capLoads";
            this.capLoads.SizeF = new System.Drawing.SizeF(90F, 20F);
            this.capLoads.StylePriority.UseBorders = false;
            this.capLoads.StylePriority.UseFont = false;
            this.capLoads.StylePriority.UseTextAlignment = false;
            this.capLoads.Text = "# Loads";
            this.capLoads.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // capNet
            //
            this.capNet.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capNet.Font = new DevExpress.Drawing.DXFont("Tahoma", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capNet.LocationFloat = new DevExpress.Utils.PointFloat(540F, 0F);
            this.capNet.Name = "capNet";
            this.capNet.SizeF = new System.Drawing.SizeF(180F, 20F);
            this.capNet.StylePriority.UseBorders = false;
            this.capNet.StylePriority.UseFont = false;
            this.capNet.StylePriority.UseTextAlignment = false;
            this.capNet.Text = "Net Total";
            this.capNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // xrWs
            //
            this.xrWs.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WeightSheetId]")});
            this.xrWs.Font = new DevExpress.Drawing.DXFont("Tahoma", 9F);
            this.xrWs.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrWs.Name = "xrWs";
            this.xrWs.SizeF = new System.Drawing.SizeF(120F, 20F);
            this.xrWs.StylePriority.UseFont = false;
            //
            // xrType
            //
            this.xrType.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Type]")});
            this.xrType.Font = new DevExpress.Drawing.DXFont("Tahoma", 9F);
            this.xrType.LocationFloat = new DevExpress.Utils.PointFloat(120F, 0F);
            this.xrType.Name = "xrType";
            this.xrType.SizeF = new System.Drawing.SizeF(110F, 20F);
            this.xrType.StylePriority.UseFont = false;
            //
            // xrCrop
            //
            this.xrCrop.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Crop]")});
            this.xrCrop.Font = new DevExpress.Drawing.DXFont("Tahoma", 9F);
            this.xrCrop.LocationFloat = new DevExpress.Utils.PointFloat(230F, 0F);
            this.xrCrop.Name = "xrCrop";
            this.xrCrop.SizeF = new System.Drawing.SizeF(220F, 20F);
            this.xrCrop.StylePriority.UseFont = false;
            //
            // xrLoads
            //
            this.xrLoads.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LoadCount]")});
            this.xrLoads.Font = new DevExpress.Drawing.DXFont("Tahoma", 9F);
            this.xrLoads.LocationFloat = new DevExpress.Utils.PointFloat(450F, 0F);
            this.xrLoads.Name = "xrLoads";
            this.xrLoads.SizeF = new System.Drawing.SizeF(90F, 20F);
            this.xrLoads.StylePriority.UseFont = false;
            this.xrLoads.StylePriority.UseTextAlignment = false;
            this.xrLoads.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // xrNet
            //
            this.xrNet.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NetDisplay]")});
            this.xrNet.Font = new DevExpress.Drawing.DXFont("Tahoma", 9F);
            this.xrNet.LocationFloat = new DevExpress.Utils.PointFloat(540F, 0F);
            this.xrNet.Name = "xrNet";
            this.xrNet.SizeF = new System.Drawing.SizeF(180F, 20F);
            this.xrNet.StylePriority.UseFont = false;
            this.xrNet.StylePriority.UseTextAlignment = false;
            this.xrNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // xrTotalLoadsLabel
            //
            this.xrTotalLoadsLabel.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTotalLoadsLabel.Font = new DevExpress.Drawing.DXFont("Tahoma", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTotalLoadsLabel.LocationFloat = new DevExpress.Utils.PointFloat(230F, 6F);
            this.xrTotalLoadsLabel.Name = "xrTotalLoadsLabel";
            this.xrTotalLoadsLabel.SizeF = new System.Drawing.SizeF(220F, 20F);
            this.xrTotalLoadsLabel.StylePriority.UseBorders = false;
            this.xrTotalLoadsLabel.StylePriority.UseFont = false;
            this.xrTotalLoadsLabel.StylePriority.UseTextAlignment = false;
            this.xrTotalLoadsLabel.Text = "Total Loads:";
            this.xrTotalLoadsLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // xrTotalLoads
            //
            this.xrTotalLoads.Borders = DevExpress.XtraPrinting.BorderSide.Top;
            this.xrTotalLoads.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TotalLoads]")});
            this.xrTotalLoads.Font = new DevExpress.Drawing.DXFont("Tahoma", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTotalLoads.LocationFloat = new DevExpress.Utils.PointFloat(450F, 6F);
            this.xrTotalLoads.Name = "xrTotalLoads";
            this.xrTotalLoads.SizeF = new System.Drawing.SizeF(90F, 20F);
            this.xrTotalLoads.StylePriority.UseBorders = false;
            this.xrTotalLoads.StylePriority.UseFont = false;
            this.xrTotalLoads.StylePriority.UseTextAlignment = false;
            this.xrTotalLoads.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // xrTotalNetLabel
            //
            this.xrTotalNetLabel.Font = new DevExpress.Drawing.DXFont("Tahoma", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTotalNetLabel.LocationFloat = new DevExpress.Utils.PointFloat(230F, 28F);
            this.xrTotalNetLabel.Name = "xrTotalNetLabel";
            this.xrTotalNetLabel.SizeF = new System.Drawing.SizeF(220F, 20F);
            this.xrTotalNetLabel.StylePriority.UseFont = false;
            this.xrTotalNetLabel.StylePriority.UseTextAlignment = false;
            this.xrTotalNetLabel.Text = "Total Net:";
            this.xrTotalNetLabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // xrTotalNet
            //
            this.xrTotalNet.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TotalNetDisplay]")});
            this.xrTotalNet.Font = new DevExpress.Drawing.DXFont("Tahoma", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTotalNet.LocationFloat = new DevExpress.Utils.PointFloat(450F, 28F);
            this.xrTotalNet.Name = "xrTotalNet";
            this.xrTotalNet.SizeF = new System.Drawing.SizeF(270F, 20F);
            this.xrTotalNet.StylePriority.UseFont = false;
            this.xrTotalNet.StylePriority.UseTextAlignment = false;
            this.xrTotalNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // xrPageInfo
            //
            this.xrPageInfo.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Page \' + [PageNumber] + \' of \' + [TotalPageCount]")});
            this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(550F, 50F);
            this.xrPageInfo.Name = "xrPageInfo";
            this.xrPageInfo.SizeF = new System.Drawing.SizeF(170F, 20F);
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            this.xrPageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // DailyWeightSheetSeriesReport
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
