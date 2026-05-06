namespace GrainManagement.Reporting
{
#nullable disable

    using System;
    using System.ComponentModel;
    using DevExpress.DataAccess.ObjectBinding;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraReports.UI;

    partial class TransferWeightSheetReport
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
        private XRLine xrHeaderLine1;
private XRLabel xrLotId;
        private XRLabel capAccount;    private XRLabel xrAccount;
        private XRLabel capCrop;       private XRLabel xrCropName;
private XRLabel xrSplitId;
private XRLabel xrSplitName;
        private XRLabel capBol;        private XRLabel xrBolType;
        private XRLabel capHauler;     private XRLabel xrHauler;
        private XRLabel capMiles;      private XRLabel xrMiles;
        private XRLabel capRate;       private XRLabel xrRate;
        private XRLabel capState;      private XRLabel xrState;
        private XRLabel capCounty;     private XRLabel xrCounty;
        private XRLine xrHeaderLine2;
        private XRLabel xrTotalLoads;
        private XRLabel xrTotalNet;

        // Table header
        private XRTable xrTableHeader;
        private XRTableRow xrTableHeaderRow;
        private XRTableCell hdrLoadId;
        private XRTableCell hdrTruckId;
        private XRTableCell hdrBOL;
        private XRTableCell hdrTimeIn;
        private XRTableCell hdrTimeOut;
        private XRTableCell hdrBin;
        private XRTableCell hdrInWt;
        private XRTableCell hdrOutWt;
        private XRTableCell hdrNet;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TransferWeightSheetReport));
            DevExpress.XtraPrinting.BarCode.QRCodeGS1Generator qrCodeGS1Generator1 = new DevExpress.XtraPrinting.BarCode.QRCodeGS1Generator();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.reportHeaderBand1 = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCheckBox1 = new DevExpress.XtraReports.UI.XRCheckBox();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.capSplitId = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine4 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrReportTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrHeaderLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLotId = new DevExpress.XtraReports.UI.XRLabel();
            this.capAccount = new DevExpress.XtraReports.UI.XRLabel();
            this.xrAccount = new DevExpress.XtraReports.UI.XRLabel();
            this.capCrop = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCropName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSplitId = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSplitName = new DevExpress.XtraReports.UI.XRLabel();
            this.capBol = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBolType = new DevExpress.XtraReports.UI.XRLabel();
            this.capHauler = new DevExpress.XtraReports.UI.XRLabel();
            this.xrHauler = new DevExpress.XtraReports.UI.XRLabel();
            this.capMiles = new DevExpress.XtraReports.UI.XRLabel();
            this.xrMiles = new DevExpress.XtraReports.UI.XRLabel();
            this.capRate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrRate = new DevExpress.XtraReports.UI.XRLabel();
            this.capState = new DevExpress.XtraReports.UI.XRLabel();
            this.xrState = new DevExpress.XtraReports.UI.XRLabel();
            this.capCounty = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCounty = new DevExpress.XtraReports.UI.XRLabel();
            this.xrHeaderLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrTableHeader = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableHeaderRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrLoadId = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrTruckId = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrBOL = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrTimeIn = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrTimeOut = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrBin = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrInWt = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrOutWt = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.hdrNet = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrBarCode1 = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrTotalLoads = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTotalNet = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTableDetail = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableDetailRow = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.detLoadId = new DevExpress.XtraReports.UI.XRTableCell();
            this.detTruckId = new DevExpress.XtraReports.UI.XRTableCell();
            this.detBOL = new DevExpress.XtraReports.UI.XRTableCell();
            this.detTimeIn = new DevExpress.XtraReports.UI.XRTableCell();
            this.detTimeOut = new DevExpress.XtraReports.UI.XRTableCell();
            this.detBin = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.detInWt = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.detOutWt = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.detNet = new DevExpress.XtraReports.UI.XRTableCell();
            this.reportFooterBand1 = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrWeighmaster = new DevExpress.XtraReports.UI.XRLabel();
            this.capWeighmaster = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.xrTableHeader)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.HeightF = 18.12499F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.HeightF = 13.33338F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // reportHeaderBand1
            // 
            this.reportHeaderBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel14,
            this.xrLabel24,
            this.xrLabel23,
            this.xrLabel22,
            this.xrLabel21,
            this.xrLabel13,
            this.xrLabel12,
            this.xrLabel17,
            this.xrLabel16,
            this.xrLabel11,
            this.xrCheckBox1,
            this.xrLabel9,
            this.xrLabel10,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel5,
            this.capSplitId,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLine4,
            this.xrLine2,
            this.xrLabel2,
            this.xrPictureBox1,
            this.xrReportTitle,
            this.xrHeaderLine1,
            this.xrLotId,
            this.capAccount,
            this.xrAccount,
            this.capCrop,
            this.xrCropName,
            this.xrSplitId,
            this.xrSplitName,
            this.capBol,
            this.xrBolType,
            this.capHauler,
            this.xrHauler,
            this.capMiles,
            this.xrMiles,
            this.capRate,
            this.xrRate,
            this.capState,
            this.xrState,
            this.capCounty,
            this.xrCounty,
            this.xrHeaderLine2,
            this.xrTableHeader});
            this.reportHeaderBand1.HeightF = 363.7501F;
            this.reportHeaderBand1.Name = "reportHeaderBand1";
            // 
            // xrLabel14
            // 
            this.xrLabel14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WeightSheetNotes]")});
            this.xrLabel14.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(0F, 273.5417F);
            this.xrLabel14.Multiline = true;
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(743.676F, 18F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "xrLabel6";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel14.WordWrap = false;
            // 
            // xrLabel24
            // 
            this.xrLabel24.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CropName]")});
            this.xrLabel24.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(145.2662F, 103.4584F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(578.9214F, 17.99999F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel24.WordWrap = false;
            // 
            // xrLabel23
            // 
            this.xrLabel23.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Location]")});
            this.xrLabel23.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(142.2663F, 121.4584F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(558.7848F, 18F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel23.WordWrap = false;
            // 
            // xrLabel22
            // 
            this.xrLabel22.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PrimaryAccountId]")});
            this.xrLabel22.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(88.00002F, 177.5417F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(79.40277F, 18F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel22.WordWrap = false;
            // 
            // xrLabel21
            // 
            this.xrLabel21.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LotNotes]")});
            this.xrLabel21.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(0F, 255.5417F);
            this.xrLabel21.Multiline = true;
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(743.676F, 18F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "xrLabel6";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel21.WordWrap = false;
            // 
            // xrLabel13
            // 
            this.xrLabel13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LocationId]")});
            this.xrLabel13.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(88.00002F, 121.4584F);
            this.xrLabel13.Multiline = true;
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(53.36113F, 18F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(0F, 121.4584F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(88F, 18.00002F);
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.Text = "Location:";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(0F, 83.45841F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(88F, 18.00002F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.Text = "Lot:";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel16
            // 
            this.xrLabel16.Font = new DevExpress.Drawing.DXFont("Arial", 8F);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(596.9213F, 6.29166F);
            this.xrLabel16.Multiline = true;
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(145.3344F, 15.625F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "Weight Sheet #";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            // 
            // xrLabel11
            // 
            this.xrLabel11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[As400Id]")});
            this.xrLabel11.Font = new DevExpress.Drawing.DXFont("Arial", 16F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(596.9213F, 21.91669F);
            this.xrLabel11.Multiline = true;
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(145.3345F, 19F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "xrLabel11";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrCheckBox1
            // 
            this.xrCheckBox1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "CheckBoxState", "[IsFinalWeightSheet]")});
            this.xrCheckBox1.Font = new DevExpress.Drawing.DXFont("Arial", 12F);
            this.xrCheckBox1.LocationFloat = new DevExpress.Utils.PointFloat(620.1296F, 83.4584F);
            this.xrCheckBox1.Name = "xrCheckBox1";
            this.xrCheckBox1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrCheckBox1.SizeF = new System.Drawing.SizeF(119.7917F, 18F);
            this.xrCheckBox1.StylePriority.UseFont = false;
            this.xrCheckBox1.StylePriority.UseTextAlignment = false;
            this.xrCheckBox1.Text = "Lot Closed";
            // 
            // xrLabel9
            // 
            this.xrLabel9.CanShrink = true;
            this.xrLabel9.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(596.1295F, 319.4584F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(82.79175F, 18F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "Total Billed:";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel10
            // 
            this.xrLabel10.CanShrink = true;
            this.xrLabel10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Rate]*[Miles]")});
            this.xrLabel10.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(682.3395F, 319.4584F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(60.66052F, 18F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel10.TextFormatString = "${0:F2}";
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 237.5417F);
            this.xrLabel8.Multiline = true;
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(88F, 18.00002F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Landlord :";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(0F, 217.5417F);
            this.xrLabel7.Multiline = true;
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(88F, 18.00002F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Farm #:";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LandlordName]")});
            this.xrLabel6.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(88.00002F, 235.5417F);
            this.xrLabel6.Multiline = true;
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(655.6761F, 18F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "xrLabel6";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            this.xrLabel6.WordWrap = false;
            // 
            // xrLabel5
            // 
            this.xrLabel5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FarmNumber]")});
            this.xrLabel5.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(88.00002F, 217.5417F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(638.8635F, 18F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "xrLabel5";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            this.xrLabel5.WordWrap = false;
            // 
            // capSplitId
            // 
            this.capSplitId.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.capSplitId.LocationFloat = new DevExpress.Utils.PointFloat(0F, 197.5417F);
            this.capSplitId.Name = "capSplitId";
            this.capSplitId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capSplitId.SizeF = new System.Drawing.SizeF(88F, 18.00002F);
            this.capSplitId.StylePriority.UseFont = false;
            this.capSplitId.Text = "Split :";
            this.capSplitId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CertificateTitle]")});
            this.xrLabel4.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(98.2954F, 41.45841F);
            this.xrLabel4.Multiline = true;
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(596.7358F, 20F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "xrLabel4";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel4.WordWrap = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CreationDate]")});
            this.xrLabel3.Font = new DevExpress.Drawing.DXFont("Arial", 11F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(642.2558F, 61.4584F);
            this.xrLabel3.Multiline = true;
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 16F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "xrLabel3";
            this.xrLabel3.TextFormatString = "{0:d}";
            // 
            // xrLine4
            // 
            this.xrLine4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 339.5417F);
            this.xrLine4.Name = "xrLine4";
            this.xrLine4.SizeF = new System.Drawing.SizeF(750F, 2F);
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 292.5417F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(750F, 2F);
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(182.7438F, 61.4584F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(367.7084F, 20F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "[Location] - [LocationId]";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(98.2954F, 72.83341F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrReportTitle
            // 
            this.xrReportTitle.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrReportTitle.LocationFloat = new DevExpress.Utils.PointFloat(222.2386F, 0F);
            this.xrReportTitle.Multiline = true;
            this.xrReportTitle.Name = "xrReportTitle";
            this.xrReportTitle.SizeF = new System.Drawing.SizeF(305.5227F, 40F);
            this.xrReportTitle.StylePriority.UseFont = false;
            this.xrReportTitle.StylePriority.UseTextAlignment = false;
            this.xrReportTitle.Text = "NORTHWEST GRAIN GROWERS, INC\r\nWALLA WALLA. WA";
            this.xrReportTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrHeaderLine1
            // 
            this.xrHeaderLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 81.4584F);
            this.xrHeaderLine1.Name = "xrHeaderLine1";
            this.xrHeaderLine1.SizeF = new System.Drawing.SizeF(750F, 2F);
            // 
            // xrLotId
            // 
            this.xrLotId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LotId]")});
            this.xrLotId.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLotId.LocationFloat = new DevExpress.Utils.PointFloat(88F, 83.4584F);
            this.xrLotId.Name = "xrLotId";
            this.xrLotId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLotId.SizeF = new System.Drawing.SizeF(171.4881F, 18F);
            this.xrLotId.StylePriority.UseFont = false;
            this.xrLotId.StylePriority.UseTextAlignment = false;
            this.xrLotId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // capAccount
            // 
            this.capAccount.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.capAccount.LocationFloat = new DevExpress.Utils.PointFloat(0F, 177.5417F);
            this.capAccount.Name = "capAccount";
            this.capAccount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capAccount.SizeF = new System.Drawing.SizeF(88F, 18.00002F);
            this.capAccount.StylePriority.UseFont = false;
            this.capAccount.Text = "Primary:";
            this.capAccount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrAccount
            // 
            this.xrAccount.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", " [PrimaryAccountName]")});
            this.xrAccount.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrAccount.LocationFloat = new DevExpress.Utils.PointFloat(167.4028F, 177.5417F);
            this.xrAccount.Name = "xrAccount";
            this.xrAccount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrAccount.SizeF = new System.Drawing.SizeF(575.5972F, 17.99998F);
            this.xrAccount.StylePriority.UseFont = false;
            this.xrAccount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrAccount.WordWrap = false;
            // 
            // capCrop
            // 
            this.capCrop.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.capCrop.LocationFloat = new DevExpress.Utils.PointFloat(0F, 103.4584F);
            this.capCrop.Name = "capCrop";
            this.capCrop.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capCrop.SizeF = new System.Drawing.SizeF(88F, 18.00002F);
            this.capCrop.StylePriority.UseFont = false;
            this.capCrop.Text = "Crop:";
            this.capCrop.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrCropName
            // 
            this.xrCropName.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ItemId]")});
            this.xrCropName.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrCropName.LocationFloat = new DevExpress.Utils.PointFloat(88.00002F, 103.4584F);
            this.xrCropName.Name = "xrCropName";
            this.xrCropName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrCropName.SizeF = new System.Drawing.SizeF(53.3611F, 18F);
            this.xrCropName.StylePriority.UseFont = false;
            this.xrCropName.StylePriority.UseTextAlignment = false;
            this.xrCropName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrCropName.WordWrap = false;
            // 
            // xrSplitId
            // 
            this.xrSplitId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SplitGroupId]")});
            this.xrSplitId.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrSplitId.LocationFloat = new DevExpress.Utils.PointFloat(88F, 197.5417F);
            this.xrSplitId.Name = "xrSplitId";
            this.xrSplitId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrSplitId.SizeF = new System.Drawing.SizeF(79.40277F, 18F);
            this.xrSplitId.StylePriority.UseFont = false;
            this.xrSplitId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrSplitId.WordWrap = false;
            // 
            // xrSplitName
            // 
            this.xrSplitName.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SplitName]")});
            this.xrSplitName.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrSplitName.LocationFloat = new DevExpress.Utils.PointFloat(168.0787F, 197.5417F);
            this.xrSplitName.Name = "xrSplitName";
            this.xrSplitName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrSplitName.SizeF = new System.Drawing.SizeF(575.5974F, 18F);
            this.xrSplitName.StylePriority.UseFont = false;
            this.xrSplitName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrSplitName.WordWrap = false;
            // 
            // capBol
            // 
            this.capBol.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.capBol.LocationFloat = new DevExpress.Utils.PointFloat(0F, 297.5417F);
            this.capBol.Name = "capBol";
            this.capBol.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capBol.SizeF = new System.Drawing.SizeF(73F, 18F);
            this.capBol.StylePriority.UseFont = false;
            this.capBol.Text = "BOL Type:";
            this.capBol.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrBolType
            // 
            this.xrBolType.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RateType]")});
            this.xrBolType.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrBolType.LocationFloat = new DevExpress.Utils.PointFloat(73.11897F, 297.5417F);
            this.xrBolType.Name = "xrBolType";
            this.xrBolType.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrBolType.SizeF = new System.Drawing.SizeF(243.25F, 18F);
            this.xrBolType.StylePriority.UseFont = false;
            this.xrBolType.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrBolType.WordWrap = false;
            // 
            // capHauler
            // 
            this.capHauler.CanShrink = true;
            this.capHauler.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.capHauler.LocationFloat = new DevExpress.Utils.PointFloat(336.517F, 297.5417F);
            this.capHauler.Name = "capHauler";
            this.capHauler.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capHauler.SizeF = new System.Drawing.SizeF(49F, 18F);
            this.capHauler.StylePriority.UseFont = false;
            this.capHauler.Text = "Hauler:";
            this.capHauler.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrHauler
            // 
            this.xrHauler.CanShrink = true;
            this.xrHauler.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[HaulerName]")});
            this.xrHauler.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrHauler.LocationFloat = new DevExpress.Utils.PointFloat(385.517F, 297.5417F);
            this.xrHauler.Name = "xrHauler";
            this.xrHauler.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrHauler.SizeF = new System.Drawing.SizeF(356.1558F, 18F);
            this.xrHauler.StylePriority.UseFont = false;
            this.xrHauler.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrHauler.WordWrap = false;
            // 
            // capMiles
            // 
            this.capMiles.CanShrink = true;
            this.capMiles.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.capMiles.LocationFloat = new DevExpress.Utils.PointFloat(0F, 319.4584F);
            this.capMiles.Name = "capMiles";
            this.capMiles.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capMiles.SizeF = new System.Drawing.SizeF(50.62497F, 18F);
            this.capMiles.StylePriority.UseFont = false;
            this.capMiles.Text = "Miles:";
            this.capMiles.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrMiles
            // 
            this.xrMiles.CanShrink = true;
            this.xrMiles.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Miles]")});
            this.xrMiles.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrMiles.LocationFloat = new DevExpress.Utils.PointFloat(52.62502F, 319.4584F);
            this.xrMiles.Name = "xrMiles";
            this.xrMiles.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrMiles.SizeF = new System.Drawing.SizeF(54.40161F, 18F);
            this.xrMiles.StylePriority.UseFont = false;
            this.xrMiles.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrMiles.TextFormatString = "{0:N0}";
            // 
            // capRate
            // 
            this.capRate.CanShrink = true;
            this.capRate.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.capRate.LocationFloat = new DevExpress.Utils.PointFloat(116.4449F, 319.4584F);
            this.capRate.Name = "capRate";
            this.capRate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capRate.SizeF = new System.Drawing.SizeF(44.65692F, 18F);
            this.capRate.StylePriority.UseFont = false;
            this.capRate.Text = "Rate:";
            this.capRate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrRate
            // 
            this.xrRate.CanShrink = true;
            this.xrRate.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Rate]")});
            this.xrRate.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrRate.LocationFloat = new DevExpress.Utils.PointFloat(161.1018F, 319.4584F);
            this.xrRate.Name = "xrRate";
            this.xrRate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrRate.SizeF = new System.Drawing.SizeF(48.46985F, 18F);
            this.xrRate.StylePriority.UseFont = false;
            this.xrRate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrRate.TextFormatString = "${0:F2}";
            // 
            // capState
            // 
            this.capState.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.capState.LocationFloat = new DevExpress.Utils.PointFloat(0F, 139.5417F);
            this.capState.Name = "capState";
            this.capState.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capState.SizeF = new System.Drawing.SizeF(88F, 18.00002F);
            this.capState.StylePriority.UseFont = false;
            this.capState.Text = "State:";
            this.capState.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrState
            // 
            this.xrState.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[State]")});
            this.xrState.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrState.LocationFloat = new DevExpress.Utils.PointFloat(88F, 139.5417F);
            this.xrState.Name = "xrState";
            this.xrState.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrState.SizeF = new System.Drawing.SizeF(87.1074F, 18F);
            this.xrState.StylePriority.UseFont = false;
            this.xrState.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrState.WordWrap = false;
            // 
            // capCounty
            // 
            this.capCounty.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.capCounty.LocationFloat = new DevExpress.Utils.PointFloat(0F, 159.5417F);
            this.capCounty.Name = "capCounty";
            this.capCounty.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capCounty.SizeF = new System.Drawing.SizeF(88F, 18.00002F);
            this.capCounty.StylePriority.UseFont = false;
            this.capCounty.Text = "County:";
            this.capCounty.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrCounty
            // 
            this.xrCounty.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[County]")});
            this.xrCounty.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrCounty.LocationFloat = new DevExpress.Utils.PointFloat(88.00002F, 159.5417F);
            this.xrCounty.Name = "xrCounty";
            this.xrCounty.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrCounty.SizeF = new System.Drawing.SizeF(638.1876F, 18F);
            this.xrCounty.StylePriority.UseFont = false;
            this.xrCounty.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrCounty.WordWrap = false;
            // 
            // xrHeaderLine2
            // 
            this.xrHeaderLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 361.7501F);
            this.xrHeaderLine2.Name = "xrHeaderLine2";
            this.xrHeaderLine2.SizeF = new System.Drawing.SizeF(750F, 2F);
            // 
            // xrTableHeader
            // 
            this.xrTableHeader.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrTableHeader.LocationFloat = new DevExpress.Utils.PointFloat(0F, 341.7501F);
            this.xrTableHeader.Name = "xrTableHeader";
            this.xrTableHeader.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableHeaderRow});
            this.xrTableHeader.SizeF = new System.Drawing.SizeF(750F, 20F);
            this.xrTableHeader.StylePriority.UseFont = false;
            // 
            // xrTableHeaderRow
            // 
            this.xrTableHeaderRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell8,
            this.hdrLoadId,
            this.hdrTruckId,
            this.hdrBOL,
            this.hdrTimeIn,
            this.hdrTimeOut,
            this.xrTableCell1,
            this.hdrBin,
            this.hdrInWt,
            this.xrTableCell3,
            this.hdrOutWt,
            this.xrTableCell2,
            this.hdrNet});
            this.xrTableHeaderRow.Name = "xrTableHeaderRow";
            this.xrTableHeaderRow.Weight = 1D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell8.BorderWidth = 1F;
            this.xrTableCell8.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell8.Multiline = true;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell8.Weight = 22.71106121444133D;
            // 
            // hdrLoadId
            // 
            this.hdrLoadId.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrLoadId.BorderWidth = 1F;
            this.hdrLoadId.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrLoadId.Name = "hdrLoadId";
            this.hdrLoadId.Padding = new DevExpress.XtraPrinting.PaddingInfo(10F, 2F, 0F, 0F, 100F);
            this.hdrLoadId.StylePriority.UseFont = false;
            this.hdrLoadId.StylePriority.UsePadding = false;
            this.hdrLoadId.Text = "Load ID";
            this.hdrLoadId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.hdrLoadId.Weight = 109.51831153237285D;
            // 
            // hdrTruckId
            // 
            this.hdrTruckId.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrTruckId.BorderWidth = 1F;
            this.hdrTruckId.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrTruckId.Name = "hdrTruckId";
            this.hdrTruckId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrTruckId.StylePriority.UseFont = false;
            this.hdrTruckId.Text = "Trk ID";
            this.hdrTruckId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.hdrTruckId.Weight = 58.632869782221455D;
            // 
            // hdrBOL
            // 
            this.hdrBOL.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrBOL.BorderWidth = 1F;
            this.hdrBOL.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrBOL.Name = "hdrBOL";
            this.hdrBOL.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrBOL.StylePriority.UseFont = false;
            this.hdrBOL.Text = "BOL";
            this.hdrBOL.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.hdrBOL.Weight = 121.32545851453526D;
            // 
            // hdrTimeIn
            // 
            this.hdrTimeIn.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrTimeIn.BorderWidth = 1F;
            this.hdrTimeIn.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrTimeIn.Name = "hdrTimeIn";
            this.hdrTimeIn.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrTimeIn.StylePriority.UseFont = false;
            this.hdrTimeIn.Text = "In";
            this.hdrTimeIn.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.hdrTimeIn.Weight = 73.810948192760577D;
            // 
            // hdrTimeOut
            // 
            this.hdrTimeOut.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrTimeOut.BorderWidth = 1F;
            this.hdrTimeOut.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrTimeOut.Name = "hdrTimeOut";
            this.hdrTimeOut.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrTimeOut.StylePriority.UseFont = false;
            this.hdrTimeOut.Text = "Out";
            this.hdrTimeOut.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.hdrTimeOut.Weight = 73.810948264891579D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell1.BorderWidth = 1F;
            this.xrTableCell1.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell1.Multiline = true;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Pro";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell1.Weight = 51.09988531663867D;
            // 
            // hdrBin
            // 
            this.hdrBin.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrBin.BorderWidth = 1F;
            this.hdrBin.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrBin.Name = "hdrBin";
            this.hdrBin.Padding = new DevExpress.XtraPrinting.PaddingInfo(15F, 2F, 0F, 0F, 100F);
            this.hdrBin.StylePriority.UseFont = false;
            this.hdrBin.StylePriority.UsePadding = false;
            this.hdrBin.Text = "Bin";
            this.hdrBin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.hdrBin.Weight = 73.810942174573853D;
            // 
            // hdrInWt
            // 
            this.hdrInWt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrInWt.BorderWidth = 1F;
            this.hdrInWt.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrInWt.Name = "hdrInWt";
            this.hdrInWt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrInWt.StylePriority.UseFont = false;
            this.hdrInWt.Text = "Gross";
            this.hdrInWt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.hdrInWt.Weight = 79.488711134430332D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell3.BorderWidth = 1F;
            this.xrTableCell3.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell3.Weight = 13.626636381865989D;
            // 
            // hdrOutWt
            // 
            this.hdrOutWt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrOutWt.BorderWidth = 1F;
            this.hdrOutWt.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrOutWt.Name = "hdrOutWt";
            this.hdrOutWt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrOutWt.StylePriority.UseFont = false;
            this.hdrOutWt.Text = "Tare";
            this.hdrOutWt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.hdrOutWt.Weight = 79.488712541668946D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell2.BorderWidth = 1F;
            this.xrTableCell2.Font = new DevExpress.Drawing.DXFont("Arial", 7F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell2.Weight = 13.626635863246587D;
            // 
            // hdrNet
            // 
            this.hdrNet.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.hdrNet.BorderWidth = 1F;
            this.hdrNet.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.hdrNet.Name = "hdrNet";
            this.hdrNet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.hdrNet.StylePriority.UseFont = false;
            this.hdrNet.Text = "Net";
            this.hdrNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.hdrNet.Weight = 80.71364096418111D;
            // 
            // xrBarCode1
            // 
            this.xrBarCode1.Alignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            this.xrBarCode1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WeightSheetId]")});
            this.xrBarCode1.LocationFloat = new DevExpress.Utils.PointFloat(634.6728F, 28.58332F);
            this.xrBarCode1.Name = "xrBarCode1";
            this.xrBarCode1.Padding = new DevExpress.XtraPrinting.PaddingInfo(10F, 10F, 0F, 0F, 100F);
            this.xrBarCode1.SizeF = new System.Drawing.SizeF(106F, 81.83306F);
            this.xrBarCode1.StylePriority.UseTextAlignment = false;
            this.xrBarCode1.Symbology = qrCodeGS1Generator1;
            this.xrBarCode1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            // 
            // xrTotalLoads
            // 
            this.xrTotalLoads.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Loads: \' + [TotalLoads]")});
            this.xrTotalLoads.Font = new DevExpress.Drawing.DXFont("Arial", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTotalLoads.LocationFloat = new DevExpress.Utils.PointFloat(8.577283F, 4.04158F);
            this.xrTotalLoads.Name = "xrTotalLoads";
            this.xrTotalLoads.SizeF = new System.Drawing.SizeF(186.6667F, 20F);
            this.xrTotalLoads.StylePriority.UseFont = false;
            // 
            // xrTotalNet
            // 
            this.xrTotalNet.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "\'Net: \' + [TotalNetWeight]")});
            this.xrTotalNet.Font = new DevExpress.Drawing.DXFont("Arial", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrTotalNet.LocationFloat = new DevExpress.Utils.PointFloat(550F, 4.04158F);
            this.xrTotalNet.Name = "xrTotalNet";
            this.xrTotalNet.SizeF = new System.Drawing.SizeF(200F, 20F);
            this.xrTotalNet.StylePriority.UseFont = false;
            this.xrTotalNet.StylePriority.UseTextAlignment = false;
            this.xrTotalNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25.04161F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(750F, 2F);
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
            this.xrTableDetail.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrTableDetail.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTableDetail.Name = "xrTableDetail";
            this.xrTableDetail.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableDetailRow});
            this.xrTableDetail.SizeF = new System.Drawing.SizeF(750F, 18F);
            this.xrTableDetail.StylePriority.UseFont = false;
            // 
            // xrTableDetailRow
            // 
            this.xrTableDetailRow.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell7,
            this.detLoadId,
            this.detTruckId,
            this.detBOL,
            this.detTimeIn,
            this.detTimeOut,
            this.detBin,
            this.xrTableCell4,
            this.detInWt,
            this.xrTableCell5,
            this.detOutWt,
            this.xrTableCell6,
            this.detNet});
            this.xrTableDetailRow.Name = "xrTableDetailRow";
            this.xrTableDetailRow.Weight = 1D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.BorderColor = System.Drawing.Color.LightGray;
            this.xrTableCell7.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell7.BorderWidth = 0.5F;
            this.xrTableCell7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[RowNumber]")});
            this.xrTableCell7.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrTableCell7.Multiline = true;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrTableCell7.StylePriority.UseFont = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell7.Weight = 26.222167327512864D;
            // 
            // detLoadId
            // 
            this.detLoadId.BorderColor = System.Drawing.Color.LightGray;
            this.detLoadId.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detLoadId.BorderWidth = 0.5F;
            this.detLoadId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LoadNumber]")});
            this.detLoadId.Font = new DevExpress.Drawing.DXFont("Arial", 8F);
            this.detLoadId.Name = "detLoadId";
            this.detLoadId.Padding = new DevExpress.XtraPrinting.PaddingInfo(10F, 2F, 0F, 0F, 100F);
            this.detLoadId.StylePriority.UseFont = false;
            this.detLoadId.StylePriority.UsePadding = false;
            this.detLoadId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.detLoadId.Weight = 126.44975421650352D;
            this.detLoadId.WordWrap = false;
            // 
            // detTruckId
            // 
            this.detTruckId.BorderColor = System.Drawing.Color.LightGray;
            this.detTruckId.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detTruckId.BorderWidth = 0.5F;
            this.detTruckId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TruckId]")});
            this.detTruckId.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.detTruckId.Name = "detTruckId";
            this.detTruckId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detTruckId.StylePriority.UseFont = false;
            this.detTruckId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.detTruckId.Weight = 67.69747065886682D;
            this.detTruckId.WordWrap = false;
            // 
            // detBOL
            // 
            this.detBOL.BorderColor = System.Drawing.Color.LightGray;
            this.detBOL.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detBOL.BorderWidth = 0.5F;
            this.detBOL.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BOL]")});
            this.detBOL.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.detBOL.Name = "detBOL";
            this.detBOL.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detBOL.StylePriority.UseFont = false;
            this.detBOL.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.detBOL.Weight = 140.08226710601622D;
            this.detBOL.WordWrap = false;
            // 
            // detTimeIn
            // 
            this.detTimeIn.BorderColor = System.Drawing.Color.LightGray;
            this.detTimeIn.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detTimeIn.BorderWidth = 0.5F;
            this.detTimeIn.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TimeIn]")});
            this.detTimeIn.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.detTimeIn.Name = "detTimeIn";
            this.detTimeIn.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detTimeIn.StylePriority.UseFont = false;
            this.detTimeIn.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.detTimeIn.TextFormatString = "{0:h:mm tt}";
            this.detTimeIn.Weight = 85.222048775495608D;
            this.detTimeIn.WordWrap = false;
            // 
            // detTimeOut
            // 
            this.detTimeOut.BorderColor = System.Drawing.Color.LightGray;
            this.detTimeOut.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detTimeOut.BorderWidth = 0.5F;
            this.detTimeOut.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TimeOut]")});
            this.detTimeOut.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.detTimeOut.Name = "detTimeOut";
            this.detTimeOut.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detTimeOut.StylePriority.UseFont = false;
            this.detTimeOut.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.detTimeOut.TextFormatString = "{0:h:mm tt}";
            this.detTimeOut.Weight = 85.222047441890538D;
            this.detTimeOut.WordWrap = false;
            // 
            // detBin
            // 
            this.detBin.BorderColor = System.Drawing.Color.LightGray;
            this.detBin.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detBin.BorderWidth = 0.5F;
            this.detBin.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Protein]")});
            this.detBin.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.detBin.Name = "detBin";
            this.detBin.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detBin.StylePriority.UseFont = false;
            this.detBin.StylePriority.UseTextAlignment = false;
            this.detBin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.detBin.TextFormatString = "{0:#.00}";
            this.detBin.Weight = 58.999882072792843D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.BorderColor = System.Drawing.Color.LightGray;
            this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell4.BorderWidth = 0.5F;
            this.xrTableCell4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Bin]")});
            this.xrTableCell4.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.xrTableCell4.Multiline = true;
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(15F, 2F, 0F, 0F, 100F);
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell4.Weight = 85.2220472212187D;
            // 
            // detInWt
            // 
            this.detInWt.BorderColor = System.Drawing.Color.LightGray;
            this.detInWt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detInWt.BorderWidth = 0.5F;
            this.detInWt.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[InWeight]")});
            this.detInWt.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.detInWt.Name = "detInWt";
            this.detInWt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detInWt.StylePriority.UseFont = false;
            this.detInWt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.detInWt.TextFormatString = "{0:#,#}";
            this.detInWt.Weight = 91.777594783901279D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.BorderColor = System.Drawing.Color.LightGray;
            this.xrTableCell5.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell5.BorderWidth = 0.5F;
            this.xrTableCell5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[StartManualFlag]")});
            this.xrTableCell5.Font = new DevExpress.Drawing.DXFont("Arial", 7F);
            this.xrTableCell5.Multiline = true;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrTableCell5.Text = "xrTableCell5";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell5.Weight = 15.733301141092818D;
            // 
            // detOutWt
            // 
            this.detOutWt.BorderColor = System.Drawing.Color.LightGray;
            this.detOutWt.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.detOutWt.BorderWidth = 0.5F;
            this.detOutWt.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OutWeight]")});
            this.detOutWt.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.detOutWt.Name = "detOutWt";
            this.detOutWt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detOutWt.StylePriority.UseFont = false;
            this.detOutWt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.detOutWt.TextFormatString = "{0:#,#}";
            this.detOutWt.Weight = 91.777594042426713D;
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.BorderColor = System.Drawing.Color.LightGray;
            this.xrTableCell6.Borders = DevExpress.XtraPrinting.BorderSide.Bottom;
            this.xrTableCell6.BorderWidth = 0.5F;
            this.xrTableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[EndManualFlag]")});
            this.xrTableCell6.Font = new DevExpress.Drawing.DXFont("Arial", 7F);
            this.xrTableCell6.Multiline = true;
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrTableCell6.Text = "xrTableCell6";
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell6.Weight = 15.733301772818658D;
            // 
            // detNet
            // 
            this.detNet.BorderColor = System.Drawing.Color.LightGray;
            this.detNet.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.detNet.BorderWidth = 0.5F;
            this.detNet.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Net]")});
            this.detNet.Font = new DevExpress.Drawing.DXFont("Arial", 12F);
            this.detNet.Name = "detNet";
            this.detNet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.detNet.StylePriority.UseBorders = false;
            this.detNet.StylePriority.UseFont = false;
            this.detNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.detNet.TextFormatString = "{0:#,#}";
            this.detNet.Weight = 93.1918791731357D;
            // 
            // reportFooterBand1
            // 
            this.reportFooterBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel19,
            this.xrLabel18,
            this.xrBarCode1,
            this.xrTotalNet,
            this.xrTotalLoads,
            this.xrLine3,
            this.xrLine1,
            this.xrWeighmaster,
            this.capWeighmaster,
            this.xrLabel1});
            this.reportFooterBand1.HeightF = 112.208F;
            this.reportFooterBand1.Name = "reportFooterBand1";
            this.reportFooterBand1.PrintAtBottom = true;
            // 
            // xrLabel19
            // 
            this.xrLabel19.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CopyType]")});
            this.xrLabel19.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(299.0756F, 66.58335F);
            this.xrLabel19.Multiline = true;
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(151.8488F, 14F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = "xrLabel19";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel18
            // 
            this.xrLabel18.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(299.0756F, 47.58332F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(151.8488F, 18F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "NON-NEGOTIABLE";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(750F, 2F);
            // 
            // xrWeighmaster
            // 
            this.xrWeighmaster.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WeightmasterName]")});
            this.xrWeighmaster.Font = new DevExpress.Drawing.DXFont("Arial", 10F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrWeighmaster.LocationFloat = new DevExpress.Utils.PointFloat(163.7438F, 29.58329F);
            this.xrWeighmaster.Name = "xrWeighmaster";
            this.xrWeighmaster.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrWeighmaster.SizeF = new System.Drawing.SizeF(280F, 18F);
            this.xrWeighmaster.StylePriority.UseFont = false;
            this.xrWeighmaster.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrWeighmaster.WordWrap = false;
            // 
            // capWeighmaster
            // 
            this.capWeighmaster.Font = new DevExpress.Drawing.DXFont("Arial", 10F);
            this.capWeighmaster.LocationFloat = new DevExpress.Utils.PointFloat(0F, 29.58329F);
            this.capWeighmaster.Name = "capWeighmaster";
            this.capWeighmaster.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capWeighmaster.SizeF = new System.Drawing.SizeF(161.1018F, 18F);
            this.capWeighmaster.StylePriority.UseFont = false;
            this.capWeighmaster.Text = "Licensed Weighmaster:";
            this.capWeighmaster.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(116.4449F, 81.8747F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(517.11F, 30F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "THIS CERTIFICATE IS ISSUED BY A WEIGHER LICENSED UNDER THE UNITED STATES WAREHOUS" +
    "E ACT AND THE REGULATIONS THEREUNDER\r";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.TransferWeightSheetDto);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // TransferWeightSheetReport
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
            this.Margins = new DevExpress.Drawing.DXMargins(50F, 50F, 18.12499F, 13.33338F);
            this.Version = "25.2";
            ((System.ComponentModel.ISupportInitialize)(this.xrTableHeader)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTableDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        private XRPictureBox xrPictureBox1;
        private XRLabel xrLabel1;
        private XRLabel xrLabel2;
        private XRLabel xrWeighmaster;
        private XRLabel capWeighmaster;
        private XRLine xrLine3;
        private XRLine xrLine2;
        private XRLine xrLine1;
        private XRTable xrTableDetail;
        private XRTableRow xrTableDetailRow;
        private XRTableCell detLoadId;
        private XRTableCell detTruckId;
        private XRTableCell detBOL;
        private XRTableCell detTimeIn;
        private XRTableCell detTimeOut;
        private XRTableCell detBin;
        private XRTableCell detInWt;
        private XRTableCell detOutWt;
        private XRTableCell detNet;
        private XRTableCell xrTableCell1;
        private XRTableCell xrTableCell4;
        private XRLine xrLine4;
        private XRTableCell xrTableCell3;
        private XRTableCell xrTableCell2;
        private XRTableCell xrTableCell5;
        private XRTableCell xrTableCell6;
        private XRLabel xrLabel3;
        private XRLabel xrLabel4;
        private XRTableCell xrTableCell8;
        private XRTableCell xrTableCell7;
        private XRBarCode xrBarCode1;
        private XRLabel capSplitId;
        private XRLabel xrLabel6;
        private XRLabel xrLabel5;
        private XRLabel xrLabel8;
        private XRLabel xrLabel7;
        private XRLabel xrLabel9;
        private XRLabel xrLabel10;
        private XRCheckBox xrCheckBox1;
        private XRLabel xrLabel11;
        private XRLabel xrLabel16;
        private XRLabel xrLabel17;
        private XRLabel xrLabel18;
        private XRLabel xrLabel19;
        private XRLabel xrLabel12;
        private XRLabel xrLabel13;
        private XRLabel xrLabel21;
        private XRLabel xrLabel22;
        private XRLabel xrLabel23;
        private XRLabel xrLabel24;
        private XRLabel xrLabel14;
    }
}
