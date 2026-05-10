namespace GrainManagement.Reporting
{
#nullable disable

    using System.ComponentModel;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraReports.UI;

    partial class DailyIntakeReport
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
        private DevExpress.XtraReports.UI.XRLabel capLot;
        private DevExpress.XtraReports.UI.XRLabel capCustomer;
        private DevExpress.XtraReports.UI.XRLabel capLandlord;
        private DevExpress.XtraReports.UI.XRLabel capFsa;
        private DevExpress.XtraReports.UI.XRLabel capNetLbs;
        private DevExpress.XtraReports.UI.XRLabel capNetUnits;

        private DevExpress.XtraReports.UI.XRLabel xrWs;
        private DevExpress.XtraReports.UI.XRLabel xrCommodity;
        private DevExpress.XtraReports.UI.XRLabel xrLot;
        private DevExpress.XtraReports.UI.XRLabel xrCustomer;
        private DevExpress.XtraReports.UI.XRLabel xrLandlord;
        private DevExpress.XtraReports.UI.XRLabel xrFsa;
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
            this.capLot = new DevExpress.XtraReports.UI.XRLabel();
            this.capCustomer = new DevExpress.XtraReports.UI.XRLabel();
            this.capLandlord = new DevExpress.XtraReports.UI.XRLabel();
            this.capFsa = new DevExpress.XtraReports.UI.XRLabel();
            this.capNetLbs = new DevExpress.XtraReports.UI.XRLabel();
            this.capNetUnits = new DevExpress.XtraReports.UI.XRLabel();
            this.xrWs = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCommodity = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLot = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCustomer = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLandlord = new DevExpress.XtraReports.UI.XRLabel();
            this.xrFsa = new DevExpress.XtraReports.UI.XRLabel();
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
            this.capLot,
            this.capCustomer,
            this.capLandlord,
            this.capFsa,
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
            this.xrLot,
            this.xrCustomer,
            this.xrLandlord,
            this.xrFsa,
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
            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.DailyIntakeReportDto);
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
            this.xrTitle.Text = "END OF DAY INTAKE REPORT";
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
            // capLot
            //
            this.capLot.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capLot.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capLot.LocationFloat = new DevExpress.Utils.PointFloat(220F, 0F);
            this.capLot.Name = "capLot";
            this.capLot.SizeF = new System.Drawing.SizeF(90F, 20F);
            this.capLot.StylePriority.UseBorders = false;
            this.capLot.StylePriority.UseFont = false;
            this.capLot.Text = "Lot #";
            //
            // capCustomer
            //
            this.capCustomer.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capCustomer.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capCustomer.LocationFloat = new DevExpress.Utils.PointFloat(310F, 0F);
            this.capCustomer.Name = "capCustomer";
            this.capCustomer.SizeF = new System.Drawing.SizeF(220F, 20F);
            this.capCustomer.StylePriority.UseBorders = false;
            this.capCustomer.StylePriority.UseFont = false;
            this.capCustomer.Text = "Customer";
            //
            // capLandlord
            //
            this.capLandlord.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capLandlord.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capLandlord.LocationFloat = new DevExpress.Utils.PointFloat(530F, 0F);
            this.capLandlord.Name = "capLandlord";
            this.capLandlord.SizeF = new System.Drawing.SizeF(140F, 20F);
            this.capLandlord.StylePriority.UseBorders = false;
            this.capLandlord.StylePriority.UseFont = false;
            this.capLandlord.Text = "Landlord";
            //
            // capFsa
            //
            this.capFsa.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.capFsa.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.capFsa.LocationFloat = new DevExpress.Utils.PointFloat(670F, 0F);
            this.capFsa.Name = "capFsa";
            this.capFsa.SizeF = new System.Drawing.SizeF(100F, 20F);
            this.capFsa.StylePriority.UseBorders = false;
            this.capFsa.StylePriority.UseFont = false;
            this.capFsa.Text = "Farm";
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
            // xrLot
            //
            this.xrLot.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LotNumber]")});
            this.xrLot.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrLot.LocationFloat = new DevExpress.Utils.PointFloat(220F, 0F);
            this.xrLot.Name = "xrLot";
            this.xrLot.SizeF = new System.Drawing.SizeF(90F, 20F);
            this.xrLot.StylePriority.UseFont = false;
            //
            // xrCustomer
            //
            this.xrCustomer.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Customer]")});
            this.xrCustomer.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrCustomer.LocationFloat = new DevExpress.Utils.PointFloat(310F, 0F);
            this.xrCustomer.Name = "xrCustomer";
            this.xrCustomer.SizeF = new System.Drawing.SizeF(220F, 20F);
            this.xrCustomer.StylePriority.UseFont = false;
            //
            // xrLandlord
            //
            this.xrLandlord.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Landlord]")});
            this.xrLandlord.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrLandlord.LocationFloat = new DevExpress.Utils.PointFloat(530F, 0F);
            this.xrLandlord.Name = "xrLandlord";
            this.xrLandlord.SizeF = new System.Drawing.SizeF(140F, 20F);
            this.xrLandlord.StylePriority.UseFont = false;
            //
            // xrFsa
            //
            this.xrFsa.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FsaNumber]")});
            this.xrFsa.Font = new DevExpress.Drawing.DXFont("Tahoma", 8F);
            this.xrFsa.LocationFloat = new DevExpress.Utils.PointFloat(670F, 0F);
            this.xrFsa.Name = "xrFsa";
            this.xrFsa.SizeF = new System.Drawing.SizeF(100F, 20F);
            this.xrFsa.StylePriority.UseFont = false;
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
            // DailyIntakeReport
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
