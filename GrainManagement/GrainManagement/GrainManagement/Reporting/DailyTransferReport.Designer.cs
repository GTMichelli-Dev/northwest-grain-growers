namespace GrainManagement.Reporting
{
#nullable disable

    using System.ComponentModel;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraReports.UI;

    partial class DailyTransferReport
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
        private DevExpress.XtraReports.UI.XRLabel xrCreationDate;

        private DevExpress.XtraReports.UI.XRLabel capWs;
        private DevExpress.XtraReports.UI.XRLabel capCommodity;
        private DevExpress.XtraReports.UI.XRLabel capSource;
        private DevExpress.XtraReports.UI.XRLabel capVariety;
        private DevExpress.XtraReports.UI.XRLabel capComment;
        private DevExpress.XtraReports.UI.XRLabel capNetLbs;
        private DevExpress.XtraReports.UI.XRLabel capNetUnits;

        private DevExpress.XtraReports.UI.XRLabel xrWs;
        private DevExpress.XtraReports.UI.XRLabel xrCommodity;
        private DevExpress.XtraReports.UI.XRLabel xrSource;
        private DevExpress.XtraReports.UI.XRLabel xrVariety;
        private DevExpress.XtraReports.UI.XRLabel xrComment;
        private DevExpress.XtraReports.UI.XRLabel xrNetLbs;
        private DevExpress.XtraReports.UI.XRLabel xrNetUnits;

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
            this.xrLocation = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCreationDate = new DevExpress.XtraReports.UI.XRLabel();
            this.capWs = new DevExpress.XtraReports.UI.XRLabel();
            this.capCommodity = new DevExpress.XtraReports.UI.XRLabel();
            this.capSource = new DevExpress.XtraReports.UI.XRLabel();
            this.capVariety = new DevExpress.XtraReports.UI.XRLabel();
            this.capComment = new DevExpress.XtraReports.UI.XRLabel();
            this.capNetLbs = new DevExpress.XtraReports.UI.XRLabel();
            this.capNetUnits = new DevExpress.XtraReports.UI.XRLabel();
            this.xrWs = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCommodity = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSource = new DevExpress.XtraReports.UI.XRLabel();
            this.xrVariety = new DevExpress.XtraReports.UI.XRLabel();
            this.xrComment = new DevExpress.XtraReports.UI.XRLabel();
            this.xrNetLbs = new DevExpress.XtraReports.UI.XRLabel();
            this.xrNetUnits = new DevExpress.XtraReports.UI.XRLabel();
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
            this.xrLocation,
            this.xrCreationDate});
            this.reportHeaderBand1.HeightF = 78F;
            this.reportHeaderBand1.Name = "reportHeaderBand1";
            //
            // pageHeaderBand1
            //
            this.pageHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.capWs,
            this.capCommodity,
            this.capSource,
            this.capVariety,
            this.capComment,
            this.capNetLbs,
            this.capNetUnits});
            this.pageHeaderBand1.HeightF = 22F;
            this.pageHeaderBand1.Name = "pageHeaderBand1";
            //
            // detailBand1
            //
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrWs,
            this.xrCommodity,
            this.xrSource,
            this.xrVariety,
            this.xrComment,
            this.xrNetLbs,
            this.xrNetUnits});
            this.detailBand1.HeightF = 22F;
            this.detailBand1.Name = "detailBand1";
            //
            // reportFooterBand1
            //
            this.reportFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo});
            this.reportFooterBand1.HeightF = 22F;
            this.reportFooterBand1.Name = "reportFooterBand1";
            //
            // bottomMarginBand1
            //
            this.bottomMarginBand1.HeightF = 40F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            //
            // objectDataSource1
            //
            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.DailyTransferReportDto);
            this.objectDataSource1.Name = "objectDataSource1";
            //
            // xrTitle
            //
            this.xrTitle.Font = new DevExpress.Drawing.DXFont("Tahoma", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTitle.Name = "xrTitle";
            this.xrTitle.SizeF = new System.Drawing.SizeF(1000F, 26F);
            this.xrTitle.StylePriority.UseFont = false;
            this.xrTitle.StylePriority.UseTextAlignment = false;
            this.xrTitle.Text = "END OF DAY TRANSFER REPORT";
            this.xrTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // xrLocation
            //
            this.xrLocation.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LocationName] + \' / \' + [LocationId]")});
            this.xrLocation.Font = new DevExpress.Drawing.DXFont("Tahoma", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLocation.LocationFloat = new DevExpress.Utils.PointFloat(0F, 28F);
            this.xrLocation.Name = "xrLocation";
            this.xrLocation.SizeF = new System.Drawing.SizeF(1000F, 22F);
            this.xrLocation.StylePriority.UseFont = false;
            this.xrLocation.StylePriority.UseTextAlignment = false;
            this.xrLocation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // xrCreationDate
            //
            this.xrCreationDate.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CreationDate]")});
            this.xrCreationDate.LocationFloat = new DevExpress.Utils.PointFloat(0F, 52F);
            this.xrCreationDate.Name = "xrCreationDate";
            this.xrCreationDate.SizeF = new System.Drawing.SizeF(1000F, 20F);
            this.xrCreationDate.StylePriority.UseTextAlignment = false;
            this.xrCreationDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            //
            // capWs
            //
            this.capWs.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capWs.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capWs.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.capWs.Name = "capWs";
            this.capWs.SizeF = new System.Drawing.SizeF(100F, 20F);
            this.capWs.StylePriority.UseBorders = false;
            this.capWs.StylePriority.UseFont = false;
            this.capWs.Text = "WS #";
            //
            // capCommodity
            //
            this.capCommodity.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capCommodity.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capCommodity.LocationFloat = new DevExpress.Utils.PointFloat(100F, 0F);
            this.capCommodity.Name = "capCommodity";
            this.capCommodity.SizeF = new System.Drawing.SizeF(120F, 20F);
            this.capCommodity.StylePriority.UseBorders = false;
            this.capCommodity.StylePriority.UseFont = false;
            this.capCommodity.Text = "Commod";
            //
            // capSource
            //
            this.capSource.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capSource.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capSource.LocationFloat = new DevExpress.Utils.PointFloat(220F, 0F);
            this.capSource.Name = "capSource";
            this.capSource.SizeF = new System.Drawing.SizeF(180F, 20F);
            this.capSource.StylePriority.UseBorders = false;
            this.capSource.StylePriority.UseFont = false;
            this.capSource.Text = "Source";
            //
            // capVariety
            //
            this.capVariety.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capVariety.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capVariety.LocationFloat = new DevExpress.Utils.PointFloat(400F, 0F);
            this.capVariety.Name = "capVariety";
            this.capVariety.SizeF = new System.Drawing.SizeF(150F, 20F);
            this.capVariety.StylePriority.UseBorders = false;
            this.capVariety.StylePriority.UseFont = false;
            this.capVariety.Text = "Variety";
            //
            // capComment
            //
            this.capComment.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capComment.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capComment.LocationFloat = new DevExpress.Utils.PointFloat(550F, 0F);
            this.capComment.Name = "capComment";
            this.capComment.SizeF = new System.Drawing.SizeF(220F, 20F);
            this.capComment.StylePriority.UseBorders = false;
            this.capComment.StylePriority.UseFont = false;
            this.capComment.Text = "Comment";
            //
            // capNetLbs
            //
            this.capNetLbs.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capNetLbs.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capNetLbs.LocationFloat = new DevExpress.Utils.PointFloat(770F, 0F);
            this.capNetLbs.Name = "capNetLbs";
            this.capNetLbs.SizeF = new System.Drawing.SizeF(100F, 20F);
            this.capNetLbs.StylePriority.UseBorders = false;
            this.capNetLbs.StylePriority.UseFont = false;
            this.capNetLbs.StylePriority.UseTextAlignment = false;
            this.capNetLbs.Text = "Net Lbs";
            this.capNetLbs.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // capNetUnits
            //
            this.capNetUnits.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capNetUnits.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capNetUnits.LocationFloat = new DevExpress.Utils.PointFloat(870F, 0F);
            this.capNetUnits.Name = "capNetUnits";
            this.capNetUnits.SizeF = new System.Drawing.SizeF(100F, 20F);
            this.capNetUnits.StylePriority.UseBorders = false;
            this.capNetUnits.StylePriority.UseFont = false;
            this.capNetUnits.StylePriority.UseTextAlignment = false;
            this.capNetUnits.Text = "Net Units";
            this.capNetUnits.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // xrWs
            //
            this.xrWs.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WeightSheetId]")});
            this.xrWs.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrWs.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrWs.Name = "xrWs";
            this.xrWs.SizeF = new System.Drawing.SizeF(100F, 20F);
            this.xrWs.StylePriority.UseFont = false;
            //
            // xrCommodity
            //
            this.xrCommodity.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Commodity]")});
            this.xrCommodity.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrCommodity.LocationFloat = new DevExpress.Utils.PointFloat(100F, 0F);
            this.xrCommodity.Name = "xrCommodity";
            this.xrCommodity.SizeF = new System.Drawing.SizeF(120F, 20F);
            this.xrCommodity.StylePriority.UseFont = false;
            //
            // xrSource
            //
            this.xrSource.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Source]")});
            this.xrSource.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrSource.LocationFloat = new DevExpress.Utils.PointFloat(220F, 0F);
            this.xrSource.Name = "xrSource";
            this.xrSource.SizeF = new System.Drawing.SizeF(180F, 20F);
            this.xrSource.StylePriority.UseFont = false;
            //
            // xrVariety
            //
            this.xrVariety.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Variety]")});
            this.xrVariety.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrVariety.LocationFloat = new DevExpress.Utils.PointFloat(400F, 0F);
            this.xrVariety.Name = "xrVariety";
            this.xrVariety.SizeF = new System.Drawing.SizeF(150F, 20F);
            this.xrVariety.StylePriority.UseFont = false;
            //
            // xrComment
            //
            this.xrComment.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Comment]")});
            this.xrComment.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrComment.LocationFloat = new DevExpress.Utils.PointFloat(550F, 0F);
            this.xrComment.Name = "xrComment";
            this.xrComment.SizeF = new System.Drawing.SizeF(220F, 20F);
            this.xrComment.StylePriority.UseFont = false;
            //
            // xrNetLbs
            //
            this.xrNetLbs.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "FormatString(\'{0:n0} lbs\', [Net])")});
            this.xrNetLbs.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrNetLbs.LocationFloat = new DevExpress.Utils.PointFloat(770F, 0F);
            this.xrNetLbs.Name = "xrNetLbs";
            this.xrNetLbs.SizeF = new System.Drawing.SizeF(100F, 20F);
            this.xrNetLbs.StylePriority.UseFont = false;
            this.xrNetLbs.StylePriority.UseTextAlignment = false;
            this.xrNetLbs.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // xrNetUnits
            //
            this.xrNetUnits.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "FormatString(\'{0:n2} \', [Units]) + [Uom]")});
            this.xrNetUnits.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrNetUnits.LocationFloat = new DevExpress.Utils.PointFloat(870F, 0F);
            this.xrNetUnits.Name = "xrNetUnits";
            this.xrNetUnits.SizeF = new System.Drawing.SizeF(100F, 20F);
            this.xrNetUnits.StylePriority.UseFont = false;
            this.xrNetUnits.StylePriority.UseTextAlignment = false;
            this.xrNetUnits.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // xrPageInfo
            //
            this.xrPageInfo.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Page \' + [PageNumber] + \' of \' + [TotalPageCount]")});
            this.xrPageInfo.LocationFloat = new DevExpress.Utils.PointFloat(800F, 0F);
            this.xrPageInfo.Name = "xrPageInfo";
            this.xrPageInfo.SizeF = new System.Drawing.SizeF(170F, 20F);
            this.xrPageInfo.StylePriority.UseTextAlignment = false;
            this.xrPageInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            //
            // DailyTransferReport
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
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(40, 40, 40, 40);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Letter;
            this.Version = "25.2";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
        }
    }
}
