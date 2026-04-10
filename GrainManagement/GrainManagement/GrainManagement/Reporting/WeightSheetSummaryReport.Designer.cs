namespace GrainManagement.Reporting
{
#nullable disable

    using System;
    using System.ComponentModel;
    using DevExpress.DataAccess.ObjectBinding;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraReports.UI;

    partial class WeightSheetSummaryReport
    {
        private IContainer components = null;

        private TopMarginBand topMarginBand1;
        private BottomMarginBand bottomMarginBand1;
        private ReportHeaderBand reportHeaderBand1;
        private DetailBand detailBand1;
        private ReportFooterBand reportFooterBand1;
        private ObjectDataSource objectDataSource1;

        // Header controls
        private XRLabel xrReportTitle;
private XRLabel xrWsId;
        private XRLine xrHeaderLine1;
private XRLabel xrLotId;
        private XRLabel capAccount;    private XRLabel xrAccount;
        private XRLabel capCrop;       private XRLabel xrCropName;
        private XRLabel capSplitId;    private XRLabel xrSplitId;
        private XRLabel capSplit;      private XRLabel xrSplitName;
        private XRLabel capBol;        private XRLabel xrBolType;
        private XRLabel capHauler;     private XRLabel xrHauler;
        private XRLabel capMiles;      private XRLabel xrMiles;
        private XRLabel capRate;       private XRLabel xrRate;
        private XRLine xrHeaderLine2;
        private XRLabel xrTotalLoads;
        private XRLabel xrTotalNet;

        // Table header
        private XRTable xrTableHeader;
        private XRTableRow xrTableHeaderRow;
        private XRTableCell hdrLoadId;
        private XRTableCell hdrDate;
        private XRTableCell hdrTimeIn;
        private XRTableCell hdrTimeOut;
        private XRTableCell hdrBin;
        private XRTableCell hdrInWt;
        private XRTableCell hdrOutWt;
        private XRTableCell hdrNet;
        private XRTableCell hdrProtein;

        // Detail table
        private XRTable xrTableDetail;
        private XRTableRow xrTableDetailRow;
        private XRTableCell detLoadId;
        private XRTableCell detDate;
        private XRTableCell detTimeIn;
        private XRTableCell detTimeOut;
        private XRTableCell detBin;
        private XRTableCell detInWt;
        private XRTableCell detOutWt;
        private XRTableCell detNet;
        private XRTableCell detProtein;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeightSheetSummaryReport));
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reportHeaderBand1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrReportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrWsId = new DevExpress.XtraReports.UI.XRLabel();
            this.xrHeaderLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLotId = new DevExpress.XtraReports.UI.XRLabel();
            this.capAccount = new DevExpress.XtraReports.UI.XRLabel();
            this.xrAccount = new DevExpress.XtraReports.UI.XRLabel();
            this.capCrop = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCropName = new DevExpress.XtraReports.UI.XRLabel();
            this.capSplitId = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSplitId = new DevExpress.XtraReports.UI.XRLabel();
            this.capSplit = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSplitName = new DevExpress.XtraReports.UI.XRLabel();
            this.capBol = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBolType = new DevExpress.XtraReports.UI.XRLabel();
            this.capHauler = new DevExpress.XtraReports.UI.XRLabel();
            this.xrHauler = new DevExpress.XtraReports.UI.XRLabel();
            this.capMiles = new DevExpress.XtraReports.UI.XRLabel();
            this.xrMiles = new DevExpress.XtraReports.UI.XRLabel();
            this.capRate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrRate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrHeaderLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrTableHeader = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableHeaderRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.hdrLoadId = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrDate = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrTimeIn = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrTimeOut = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrBin = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrInWt = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrOutWt = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrNet = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrProtein = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTotalLoads = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTotalNet = new DevExpress.XtraReports.UI.XRLabel();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTableDetail = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableDetailRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.detLoadId = new DevExpress.XtraReports.UI.XRTableCell();
            this.detDate = new DevExpress.XtraReports.UI.XRTableCell();
            this.detTimeIn = new DevExpress.XtraReports.UI.XRTableCell();
            this.detTimeOut = new DevExpress.XtraReports.UI.XRTableCell();
            this.detBin = new DevExpress.XtraReports.UI.XRTableCell();
            this.detInWt = new DevExpress.XtraReports.UI.XRTableCell();
            this.detOutWt = new DevExpress.XtraReports.UI.XRTableCell();
            this.detNet = new DevExpress.XtraReports.UI.XRTableCell();
            this.detProtein = new DevExpress.XtraReports.UI.XRTableCell();
            this.reportFooterBand1 = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xrTableHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.HeightF = 22.29166F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.HeightF = 40F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // reportHeaderBand1
            // 
            this.reportHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrPictureBox1,
            this.xrReportTitle,
            this.xrWsId,
            this.xrHeaderLine1,
            this.xrLotId,
            this.capAccount,
            this.xrAccount,
            this.capCrop,
            this.xrCropName,
            this.capSplitId,
            this.xrSplitId,
            this.capSplit,
            this.xrSplitName,
            this.capBol,
            this.xrBolType,
            this.capHauler,
            this.xrHauler,
            this.capMiles,
            this.xrMiles,
            this.capRate,
            this.xrRate,
            this.xrHeaderLine2,
            this.xrTableHeader});
            this.reportHeaderBand1.HeightF = 220.2084F;
            this.reportHeaderBand1.Name = "reportHeaderBand1";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(191.1458F, 42.25F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(367.7084F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Location:[Location] - [LocationId]";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(170.8333F, 82.91667F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrReportTitle
            // 
            this.xrReportTitle.Font = new DevExpress.Drawing.DXFont("Arial", 16F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(264.8958F, 0F);
            this.xrReportTitle.Name = "xrReportTitle";
            this.xrReportTitle.SizeF = new System.Drawing.SizeF(220.2083F, 28F);
            this.xrReportTitle.StylePriority.UseFont = false;
            this.xrReportTitle.StylePriority.UseTextAlignment = false;
            this.xrReportTitle.Text = "Intake Weight Sheet";
            this.xrReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrWsId
            // 
            this.xrWsId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WeightSheetId]")});
            this.xrWsId.Font = new DevExpress.Drawing.DXFont("Arial", 16F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrWsId.LocationFloat = new DevExpress.Utils.PointFloat(490.0813F, 0F);
            this.xrWsId.Name = "xrWsId";
            this.xrWsId.SizeF = new System.Drawing.SizeF(259.9187F, 28F);
            this.xrWsId.StylePriority.UseFont = false;
            this.xrWsId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrHeaderLine1
            // 
            this.xrHeaderLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 82.91669F);
            this.xrHeaderLine1.Name = "xrHeaderLine1";
            this.xrHeaderLine1.SizeF = new System.Drawing.SizeF(750F, 2F);
            // 
            // xrLotId
            // 
            this.xrLotId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Lot#: [LotId]")});
            this.xrLotId.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLotId.LocationFloat = new DevExpress.Utils.PointFloat(235F, 84.91669F);
            this.xrLotId.Name = "xrLotId";
            this.xrLotId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLotId.SizeF = new System.Drawing.SizeF(280F, 18F);
            this.xrLotId.StylePriority.UseFont = false;
            this.xrLotId.StylePriority.UseTextAlignment = false;
            this.xrLotId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // capAccount
            // 
            this.capAccount.Font = new DevExpress.Drawing.DXFont("Arial", 8F);
            this.capAccount.LocationFloat = new DevExpress.Utils.PointFloat(390F, 110.9167F);
            this.capAccount.Name = "capAccount";
            this.capAccount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capAccount.SizeF = new System.Drawing.SizeF(50F, 17.99999F);
            this.capAccount.StylePriority.UseFont = false;
            this.capAccount.Text = "Account:";
            this.capAccount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrAccount
            // 
            this.xrAccount.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif([PrimaryAccountId] <> '', [PrimaryAccountName] + ' (' + [PrimaryAccountId] + ')', [PrimaryAccountName])")});
            this.xrAccount.Font = new DevExpress.Drawing.DXFont("Arial", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrAccount.LocationFloat = new DevExpress.Utils.PointFloat(440F, 110.9167F);
            this.xrAccount.Name = "xrAccount";
            this.xrAccount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrAccount.SizeF = new System.Drawing.SizeF(300F, 18.00001F);
            this.xrAccount.StylePriority.UseFont = false;
            this.xrAccount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // capCrop
            // 
            this.capCrop.Font = new DevExpress.Drawing.DXFont("Arial", 8F);
            this.capCrop.LocationFloat = new DevExpress.Utils.PointFloat(0F, 110.9167F);
            this.capCrop.Name = "capCrop";
            this.capCrop.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capCrop.SizeF = new System.Drawing.SizeF(47.91667F, 18F);
            this.capCrop.StylePriority.UseFont = false;
            this.capCrop.Text = "Crop:";
            this.capCrop.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrCropName
            // 
            this.xrCropName.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CropName]")});
            this.xrCropName.Font = new DevExpress.Drawing.DXFont("Arial", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrCropName.LocationFloat = new DevExpress.Utils.PointFloat(47.91667F, 110.9167F);
            this.xrCropName.Name = "xrCropName";
            this.xrCropName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrCropName.SizeF = new System.Drawing.SizeF(327.0835F, 18.00001F);
            this.xrCropName.StylePriority.UseFont = false;
            this.xrCropName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // capSplitId
            // 
            this.capSplitId.Font = new DevExpress.Drawing.DXFont("Arial", 8F);
            this.capSplitId.LocationFloat = new DevExpress.Utils.PointFloat(0F, 132.9167F);
            this.capSplitId.Name = "capSplitId";
            this.capSplitId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capSplitId.SizeF = new System.Drawing.SizeF(47.91667F, 18F);
            this.capSplitId.StylePriority.UseFont = false;
            this.capSplitId.Text = "Split :";
            this.capSplitId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrSplitId
            // 
            this.xrSplitId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SplitGroupId]")});
            this.xrSplitId.Font = new DevExpress.Drawing.DXFont("Arial", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrSplitId.LocationFloat = new DevExpress.Utils.PointFloat(47.91667F, 132.9167F);
            this.xrSplitId.Name = "xrSplitId";
            this.xrSplitId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrSplitId.SizeF = new System.Drawing.SizeF(104.9187F, 18F);
            this.xrSplitId.StylePriority.UseFont = false;
            this.xrSplitId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // capSplit
            // 
            this.capSplit.Font = new DevExpress.Drawing.DXFont("Arial", 8F);
            this.capSplit.LocationFloat = new DevExpress.Utils.PointFloat(0F, 150.9167F);
            this.capSplit.Name = "capSplit";
            this.capSplit.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capSplit.SizeF = new System.Drawing.SizeF(100F, 18F);
            this.capSplit.StylePriority.UseFont = false;
            this.capSplit.Text = "Split:";
            this.capSplit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrSplitName
            // 
            this.xrSplitName.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SplitName]")});
            this.xrSplitName.Font = new DevExpress.Drawing.DXFont("Arial", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrSplitName.LocationFloat = new DevExpress.Utils.PointFloat(152.8353F, 132.9167F);
            this.xrSplitName.Name = "xrSplitName";
            this.xrSplitName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrSplitName.SizeF = new System.Drawing.SizeF(587.1647F, 18F);
            this.xrSplitName.StylePriority.UseFont = false;
            this.xrSplitName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // capBol
            // 
            this.capBol.Font = new DevExpress.Drawing.DXFont("Arial", 8F);
            this.capBol.LocationFloat = new DevExpress.Utils.PointFloat(390F, 150.9167F);
            this.capBol.Name = "capBol";
            this.capBol.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capBol.SizeF = new System.Drawing.SizeF(100F, 18F);
            this.capBol.StylePriority.UseFont = false;
            this.capBol.Text = "BOL Type:";
            this.capBol.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrBolType
            // 
            this.xrBolType.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RateType]")});
            this.xrBolType.Font = new DevExpress.Drawing.DXFont("Arial", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrBolType.LocationFloat = new DevExpress.Utils.PointFloat(490F, 150.9167F);
            this.xrBolType.Name = "xrBolType";
            this.xrBolType.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrBolType.SizeF = new System.Drawing.SizeF(260F, 18F);
            this.xrBolType.StylePriority.UseFont = false;
            this.xrBolType.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // capHauler
            // 
            this.capHauler.CanShrink = true;
            this.capHauler.Font = new DevExpress.Drawing.DXFont("Arial", 8F);
            this.capHauler.LocationFloat = new DevExpress.Utils.PointFloat(0F, 170.9167F);
            this.capHauler.Name = "capHauler";
            this.capHauler.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capHauler.SizeF = new System.Drawing.SizeF(100F, 18F);
            this.capHauler.StylePriority.UseFont = false;
            this.capHauler.Text = "Hauler:";
            this.capHauler.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrHauler
            // 
            this.xrHauler.CanShrink = true;
            this.xrHauler.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[HaulerName]")});
            this.xrHauler.Font = new DevExpress.Drawing.DXFont("Arial", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrHauler.LocationFloat = new DevExpress.Utils.PointFloat(100F, 170.9167F);
            this.xrHauler.Name = "xrHauler";
            this.xrHauler.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrHauler.SizeF = new System.Drawing.SizeF(280F, 18F);
            this.xrHauler.StylePriority.UseFont = false;
            this.xrHauler.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // capMiles
            // 
            this.capMiles.CanShrink = true;
            this.capMiles.Font = new DevExpress.Drawing.DXFont("Arial", 8F);
            this.capMiles.LocationFloat = new DevExpress.Utils.PointFloat(390F, 170.9167F);
            this.capMiles.Name = "capMiles";
            this.capMiles.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capMiles.SizeF = new System.Drawing.SizeF(50F, 18F);
            this.capMiles.StylePriority.UseFont = false;
            this.capMiles.Text = "Miles:";
            this.capMiles.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrMiles
            // 
            this.xrMiles.CanShrink = true;
            this.xrMiles.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Miles]")});
            this.xrMiles.Font = new DevExpress.Drawing.DXFont("Arial", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrMiles.LocationFloat = new DevExpress.Utils.PointFloat(440F, 170.9167F);
            this.xrMiles.Name = "xrMiles";
            this.xrMiles.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrMiles.SizeF = new System.Drawing.SizeF(60F, 18F);
            this.xrMiles.StylePriority.UseFont = false;
            this.xrMiles.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // capRate
            // 
            this.capRate.CanShrink = true;
            this.capRate.Font = new DevExpress.Drawing.DXFont("Arial", 8F);
            this.capRate.LocationFloat = new DevExpress.Utils.PointFloat(510F, 170.9167F);
            this.capRate.Name = "capRate";
            this.capRate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capRate.SizeF = new System.Drawing.SizeF(40F, 18F);
            this.capRate.StylePriority.UseFont = false;
            this.capRate.Text = "Rate:";
            this.capRate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrRate
            // 
            this.xrRate.CanShrink = true;
            this.xrRate.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Rate]")});
            this.xrRate.Font = new DevExpress.Drawing.DXFont("Arial", 8F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrRate.LocationFloat = new DevExpress.Utils.PointFloat(550F, 170.9167F);
            this.xrRate.Name = "xrRate";
            this.xrRate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrRate.SizeF = new System.Drawing.SizeF(80F, 18F);
            this.xrRate.StylePriority.UseFont = false;
            this.xrRate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrRate.TextFormatString = "${0:F2}";
            // 
            // xrHeaderLine2
            // 
            this.xrHeaderLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 196.9167F);
            this.xrHeaderLine2.Name = "xrHeaderLine2";
            this.xrHeaderLine2.SizeF = new System.Drawing.SizeF(750F, 2F);
            // 
            // xrTableHeader
            // 
            this.xrTableHeader.LocationFloat = new DevExpress.Utils.PointFloat(60F, 198.9167F);
            this.xrTableHeader.Name = "xrTableHeader";
            this.xrTableHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableHeaderRow});
            this.xrTableHeader.SizeF = new System.Drawing.SizeF(575F, 20F);
            // 
            // xrTableHeaderRow
            // 
            this.xrTableHeaderRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.hdrLoadId,
            this.hdrDate,
            this.hdrTimeIn,
            this.hdrTimeOut,
            this.hdrBin,
            this.hdrInWt,
            this.hdrOutWt,
            this.hdrNet,
            this.hdrProtein});
            this.xrTableHeaderRow.Name = "xrTableHeaderRow";
            this.xrTableHeaderRow.Weight = 1D;
            // 
            // hdrLoadId
            // 
            this.hdrLoadId.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrLoadId.BorderWidth = 1F;
            this.hdrLoadId.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrLoadId.Name = "hdrLoadId";
            this.hdrLoadId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrLoadId.Text = "Load ID";
            this.hdrLoadId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.hdrLoadId.Weight = 95D;
            // 
            // hdrDate
            // 
            this.hdrDate.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrDate.BorderWidth = 1F;
            this.hdrDate.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrDate.Name = "hdrDate";
            this.hdrDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrDate.Text = "Date";
            this.hdrDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.hdrDate.Weight = 60D;
            // 
            // hdrTimeIn
            // 
            this.hdrTimeIn.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrTimeIn.BorderWidth = 1F;
            this.hdrTimeIn.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrTimeIn.Name = "hdrTimeIn";
            this.hdrTimeIn.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrTimeIn.Text = "Time In";
            this.hdrTimeIn.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.hdrTimeIn.Weight = 100D;
            // 
            // hdrTimeOut
            // 
            this.hdrTimeOut.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrTimeOut.BorderWidth = 1F;
            this.hdrTimeOut.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrTimeOut.Name = "hdrTimeOut";
            this.hdrTimeOut.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrTimeOut.Text = "Time Out";
            this.hdrTimeOut.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.hdrTimeOut.Weight = 100D;
            // 
            // hdrBin
            // 
            this.hdrBin.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrBin.BorderWidth = 1F;
            this.hdrBin.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrBin.Name = "hdrBin";
            this.hdrBin.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrBin.Text = "Bin";
            this.hdrBin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.hdrBin.Weight = 50D;
            // 
            // hdrInWt
            // 
            this.hdrInWt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrInWt.BorderWidth = 1F;
            this.hdrInWt.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrInWt.Name = "hdrInWt";
            this.hdrInWt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrInWt.Text = "In Wt";
            this.hdrInWt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.hdrInWt.Weight = 55D;
            // 
            // hdrOutWt
            // 
            this.hdrOutWt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrOutWt.BorderWidth = 1F;
            this.hdrOutWt.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrOutWt.Name = "hdrOutWt";
            this.hdrOutWt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrOutWt.Text = "Out Wt";
            this.hdrOutWt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.hdrOutWt.Weight = 55D;
            // 
            // hdrNet
            // 
            this.hdrNet.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrNet.BorderWidth = 1F;
            this.hdrNet.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrNet.Name = "hdrNet";
            this.hdrNet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrNet.Text = "Net";
            this.hdrNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.hdrNet.Weight = 55D;
            // 
            // hdrProtein
            // 
            this.hdrProtein.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrProtein.BorderWidth = 1F;
            this.hdrProtein.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrProtein.Name = "hdrProtein";
            this.hdrProtein.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrProtein.Text = "Protein";
            this.hdrProtein.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.hdrProtein.Weight = 45D;
            // 
            // xrTotalLoads
            // 
            this.xrTotalLoads.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Loads: \' + [TotalLoads]")});
            this.xrTotalLoads.Font = new DevExpress.Drawing.DXFont("Arial", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTotalLoads.LocationFloat = new DevExpress.Utils.PointFloat(188.3334F, 9.999974F);
            this.xrTotalLoads.Name = "xrTotalLoads";
            this.xrTotalLoads.SizeF = new System.Drawing.SizeF(186.6667F, 20F);
            this.xrTotalLoads.StylePriority.UseFont = false;
            // 
            // xrTotalNet
            // 
            this.xrTotalNet.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Net: \' + [TotalNetWeight]")});
            this.xrTotalNet.Font = new DevExpress.Drawing.DXFont("Arial", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTotalNet.LocationFloat = new DevExpress.Utils.PointFloat(455F, 9.999974F);
            this.xrTotalNet.Name = "xrTotalNet";
            this.xrTotalNet.SizeF = new System.Drawing.SizeF(200F, 20F);
            this.xrTotalNet.StylePriority.UseFont = false;
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTableDetail});
            this.detailBand1.HeightF = 18F;
            this.detailBand1.Name = "detailBand1";
            // 
            // xrTableDetail
            // 
            this.xrTableDetail.LocationFloat = new DevExpress.Utils.PointFloat(60F, 0F);
            this.xrTableDetail.Name = "xrTableDetail";
            this.xrTableDetail.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableDetailRow});
            this.xrTableDetail.SizeF = new System.Drawing.SizeF(575F, 18F);
            // 
            // xrTableDetailRow
            // 
            this.xrTableDetailRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.detLoadId,
            this.detDate,
            this.detTimeIn,
            this.detTimeOut,
            this.detBin,
            this.detInWt,
            this.detOutWt,
            this.detNet,
            this.detProtein});
            this.xrTableDetailRow.Name = "xrTableDetailRow";
            this.xrTableDetailRow.Weight = 1D;
            // 
            // detLoadId
            // 
            this.detLoadId.BorderColor = System.Drawing.Color.LightGray;
            this.detLoadId.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detLoadId.BorderWidth = 0.5F;
            this.detLoadId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TransactionId]")});
            this.detLoadId.Font = new DevExpress.Drawing.DXFont("Arial", 7F);
            this.detLoadId.Name = "detLoadId";
            this.detLoadId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detLoadId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.detLoadId.Weight = 95D;
            // 
            // detDate
            // 
            this.detDate.BorderColor = System.Drawing.Color.LightGray;
            this.detDate.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detDate.BorderWidth = 0.5F;
            this.detDate.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CreationDate]")});
            this.detDate.Font = new DevExpress.Drawing.DXFont("Arial", 7F);
            this.detDate.Name = "detDate";
            this.detDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.detDate.Weight = 60D;
            // 
            // detTimeIn
            // 
            this.detTimeIn.BorderColor = System.Drawing.Color.LightGray;
            this.detTimeIn.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detTimeIn.BorderWidth = 0.5F;
            this.detTimeIn.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TimeIn]")});
            this.detTimeIn.Font = new DevExpress.Drawing.DXFont("Arial", 7F);
            this.detTimeIn.Name = "detTimeIn";
            this.detTimeIn.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detTimeIn.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.detTimeIn.Weight = 100D;
            // 
            // detTimeOut
            // 
            this.detTimeOut.BorderColor = System.Drawing.Color.LightGray;
            this.detTimeOut.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detTimeOut.BorderWidth = 0.5F;
            this.detTimeOut.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TimeOut]")});
            this.detTimeOut.Font = new DevExpress.Drawing.DXFont("Arial", 7F);
            this.detTimeOut.Name = "detTimeOut";
            this.detTimeOut.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detTimeOut.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.detTimeOut.Weight = 100D;
            // 
            // detBin
            // 
            this.detBin.BorderColor = System.Drawing.Color.LightGray;
            this.detBin.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detBin.BorderWidth = 0.5F;
            this.detBin.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Bin]")});
            this.detBin.Font = new DevExpress.Drawing.DXFont("Arial", 7F);
            this.detBin.Name = "detBin";
            this.detBin.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detBin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.detBin.Weight = 50D;
            // 
            // detInWt
            // 
            this.detInWt.BorderColor = System.Drawing.Color.LightGray;
            this.detInWt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detInWt.BorderWidth = 0.5F;
            this.detInWt.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[InWeight]")});
            this.detInWt.Font = new DevExpress.Drawing.DXFont("Arial", 7F);
            this.detInWt.Name = "detInWt";
            this.detInWt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detInWt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.detInWt.Weight = 55D;
            // 
            // detOutWt
            // 
            this.detOutWt.BorderColor = System.Drawing.Color.LightGray;
            this.detOutWt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detOutWt.BorderWidth = 0.5F;
            this.detOutWt.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OutWeight]")});
            this.detOutWt.Font = new DevExpress.Drawing.DXFont("Arial", 7F);
            this.detOutWt.Name = "detOutWt";
            this.detOutWt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detOutWt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.detOutWt.Weight = 55D;
            // 
            // detNet
            // 
            this.detNet.BorderColor = System.Drawing.Color.LightGray;
            this.detNet.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detNet.BorderWidth = 0.5F;
            this.detNet.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Net]")});
            this.detNet.Font = new DevExpress.Drawing.DXFont("Arial", 7F);
            this.detNet.Name = "detNet";
            this.detNet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.detNet.Weight = 55D;
            // 
            // detProtein
            // 
            this.detProtein.BorderColor = System.Drawing.Color.LightGray;
            this.detProtein.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detProtein.BorderWidth = 0.5F;
            this.detProtein.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Protein]")});
            this.detProtein.Font = new DevExpress.Drawing.DXFont("Arial", 7F);
            this.detProtein.Name = "detProtein";
            this.detProtein.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detProtein.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.detProtein.Weight = 45D;
            // 
            // reportFooterBand1
            // 
            this.reportFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrTotalLoads,
            this.xrTotalNet});
            this.reportFooterBand1.HeightF = 77.12498F;
            this.reportFooterBand1.Name = "reportFooterBand1";
            this.reportFooterBand1.PrintAtBottom = true;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 29.99996F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(730F, 34.45832F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "THIS CERTIFICATE IS ISSUED BY A WEIGHER LICENSED UNDER THE UNITED STATES WAREHOUS" +
    "E ACT AND THE REGULATIONS THEREUNDER\r";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.WeightSheetSummaryDto);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // WeightSheetSummaryReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.reportHeaderBand1,
            this.detailBand1,
            this.reportFooterBand1,
            this.bottomMarginBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataMember = "Loads";
            this.DataSource = this.objectDataSource1;
            this.Font = new DevExpress.Drawing.DXFont("Arial", 8F);
            this.Margins = new DevExpress.Drawing.DXMargins(50F, 50F, 22.29166F, 40F);
            this.Version = "25.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTableHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        private XRPictureBox xrPictureBox1;
        private XRLabel xrLabel1;
        private XRLabel xrLabel2;
    }
}
