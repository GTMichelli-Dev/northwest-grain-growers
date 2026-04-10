namespace GrainManagement.Reporting
{
#nullable disable

    using System;
    using System.ComponentModel;
    using DevExpress.DataAccess.ObjectBinding;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraReports.UI;

    partial class OutboundLoadTicketReport
    {
        private IContainer components = null;

        private TopMarginBand topMarginBand1;
        private BottomMarginBand bottomMarginBand1;
        private DetailBand detailBand1;
        private ObjectDataSource objectDataSource1;

        // Detail controls
        private XRLabel xrTitle;
        private XRLabel xrLoadId;
        private XRLine xrLine1;

        private XRLabel capLocation;    private XRLabel xrLocation;
        private XRLabel capLocationId;  private XRLabel xrLocationId;
        private XRLabel capWeightSheet; private XRLabel xrWeightSheetId;
        private XRLabel capLot;         private XRLabel xrLotNumber;
        private XRLabel capAccount;     private XRLabel xrCropAccount;
        private XRLabel capSplitNum;    private XRLabel xrSplitNumber;
        private XRLabel capSplit;       private XRLabel xrSplitDescription;
        private XRLabel capBol;         private XRLabel xrBolType;
        private XRLabel capHauler;      private XRLabel xrHauler;

        private XRLine xrLine2;
        private XRLabel capInTime;      private XRLabel xrInboundTime;
        private XRLabel capOutTime;     private XRLabel xrOutboundTime;
        private XRLine xrLine3;

        private XRLabel capInWeight;    private XRLabel xrInboundWeight;
        private XRLabel capOutWeight;   private XRLabel xrOutboundWeight;
        private XRLine xrLineNet;
        private XRLabel capNet;         private XRLabel xrNetWeight;
        private XRLabel xrFooter;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OutboundLoadTicketReport));
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLoadId = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.capLocation = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLocation = new DevExpress.XtraReports.UI.XRLabel();
            this.capLocationId = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLocationId = new DevExpress.XtraReports.UI.XRLabel();
            this.capWeightSheet = new DevExpress.XtraReports.UI.XRLabel();
            this.xrWeightSheetId = new DevExpress.XtraReports.UI.XRLabel();
            this.capLot = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLotNumber = new DevExpress.XtraReports.UI.XRLabel();
            this.capAccount = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCropAccount = new DevExpress.XtraReports.UI.XRLabel();
            this.capSplitNum = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSplitNumber = new DevExpress.XtraReports.UI.XRLabel();
            this.capSplit = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSplitDescription = new DevExpress.XtraReports.UI.XRLabel();
            this.capBol = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBolType = new DevExpress.XtraReports.UI.XRLabel();
            this.capHauler = new DevExpress.XtraReports.UI.XRLabel();
            this.xrHauler = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.capInTime = new DevExpress.XtraReports.UI.XRLabel();
            this.xrInboundTime = new DevExpress.XtraReports.UI.XRLabel();
            this.capOutTime = new DevExpress.XtraReports.UI.XRLabel();
            this.xrOutboundTime = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.capInWeight = new DevExpress.XtraReports.UI.XRLabel();
            this.xrInboundWeight = new DevExpress.XtraReports.UI.XRLabel();
            this.capOutWeight = new DevExpress.XtraReports.UI.XRLabel();
            this.xrOutboundWeight = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLineNet = new DevExpress.XtraReports.UI.XRLine();
            this.capNet = new DevExpress.XtraReports.UI.XRLabel();
            this.xrNetWeight = new DevExpress.XtraReports.UI.XRLabel();
            this.xrFooter = new DevExpress.XtraReports.UI.XRLabel();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1});
            this.topMarginBand1.HeightF = 157.2917F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrFooter});
            this.bottomMarginBand1.HeightF = 40F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTitle,
            this.xrLoadId,
            this.xrLine1,
            this.capLocation,
            this.xrLocation,
            this.capLocationId,
            this.xrLocationId,
            this.capWeightSheet,
            this.xrWeightSheetId,
            this.capLot,
            this.xrLotNumber,
            this.capAccount,
            this.xrCropAccount,
            this.capSplitNum,
            this.xrSplitNumber,
            this.capSplit,
            this.xrSplitDescription,
            this.capBol,
            this.xrBolType,
            this.capHauler,
            this.xrHauler,
            this.xrLine2,
            this.capInTime,
            this.xrInboundTime,
            this.capOutTime,
            this.xrOutboundTime,
            this.xrLine3,
            this.capInWeight,
            this.xrInboundWeight,
            this.capOutWeight,
            this.xrOutboundWeight,
            this.xrLineNet,
            this.capNet,
            this.xrNetWeight});
            this.detailBand1.HeightF = 456.6667F;
            this.detailBand1.Name = "detailBand1";
            // 
            // xrTitle
            // 
            this.xrTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTitle.Name = "xrTitle";
            this.xrTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrTitle.SizeF = new System.Drawing.SizeF(269F, 22F);
            this.xrTitle.Text = "OUTBOUND TICKET";
            this.xrTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLoadId
            // 
            this.xrLoadId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LoadId]")});
            this.xrLoadId.Font = new DevExpress.Drawing.DXFont("Arial", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLoadId.LocationFloat = new DevExpress.Utils.PointFloat(0F, 24F);
            this.xrLoadId.Name = "xrLoadId";
            this.xrLoadId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLoadId.SizeF = new System.Drawing.SizeF(269F, 26F);
            this.xrLoadId.StylePriority.UseFont = false;
            this.xrLoadId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 54F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(269F, 2F);
            // 
            // capLocation
            // 
            this.capLocation.LocationFloat = new DevExpress.Utils.PointFloat(0F, 64F);
            this.capLocation.Name = "capLocation";
            this.capLocation.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capLocation.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capLocation.Text = "Location:";
            this.capLocation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLocation
            // 
            this.xrLocation.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Location]")});
            this.xrLocation.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLocation.LocationFloat = new DevExpress.Utils.PointFloat(90F, 64F);
            this.xrLocation.Name = "xrLocation";
            this.xrLocation.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLocation.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrLocation.StylePriority.UseFont = false;
            this.xrLocation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capLocationId
            // 
            this.capLocationId.LocationFloat = new DevExpress.Utils.PointFloat(0F, 84F);
            this.capLocationId.Name = "capLocationId";
            this.capLocationId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capLocationId.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capLocationId.Text = "Location ID:";
            this.capLocationId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLocationId
            // 
            this.xrLocationId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LocationId]")});
            this.xrLocationId.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLocationId.LocationFloat = new DevExpress.Utils.PointFloat(90F, 84F);
            this.xrLocationId.Name = "xrLocationId";
            this.xrLocationId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLocationId.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrLocationId.StylePriority.UseFont = false;
            this.xrLocationId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capWeightSheet
            // 
            this.capWeightSheet.LocationFloat = new DevExpress.Utils.PointFloat(0F, 104F);
            this.capWeightSheet.Name = "capWeightSheet";
            this.capWeightSheet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capWeightSheet.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capWeightSheet.Text = "Weight Sheet:";
            this.capWeightSheet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrWeightSheetId
            // 
            this.xrWeightSheetId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WeightSheetId]")});
            this.xrWeightSheetId.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrWeightSheetId.LocationFloat = new DevExpress.Utils.PointFloat(90F, 104F);
            this.xrWeightSheetId.Name = "xrWeightSheetId";
            this.xrWeightSheetId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrWeightSheetId.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrWeightSheetId.StylePriority.UseFont = false;
            this.xrWeightSheetId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capLot
            // 
            this.capLot.LocationFloat = new DevExpress.Utils.PointFloat(0F, 124F);
            this.capLot.Name = "capLot";
            this.capLot.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capLot.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capLot.Text = "Lot #:";
            this.capLot.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLotNumber
            // 
            this.xrLotNumber.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LotNumber]")});
            this.xrLotNumber.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLotNumber.LocationFloat = new DevExpress.Utils.PointFloat(90F, 124F);
            this.xrLotNumber.Name = "xrLotNumber";
            this.xrLotNumber.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLotNumber.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrLotNumber.StylePriority.UseFont = false;
            this.xrLotNumber.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capAccount
            // 
            this.capAccount.LocationFloat = new DevExpress.Utils.PointFloat(0F, 144F);
            this.capAccount.Name = "capAccount";
            this.capAccount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capAccount.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capAccount.Text = "Account:";
            this.capAccount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrCropAccount
            // 
            this.xrCropAccount.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[CropAccount]")});
            this.xrCropAccount.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrCropAccount.LocationFloat = new DevExpress.Utils.PointFloat(90F, 144F);
            this.xrCropAccount.Name = "xrCropAccount";
            this.xrCropAccount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrCropAccount.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrCropAccount.StylePriority.UseFont = false;
            this.xrCropAccount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capSplitNum
            // 
            this.capSplitNum.LocationFloat = new DevExpress.Utils.PointFloat(0F, 164F);
            this.capSplitNum.Name = "capSplitNum";
            this.capSplitNum.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capSplitNum.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capSplitNum.Text = "Split #:";
            this.capSplitNum.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrSplitNumber
            // 
            this.xrSplitNumber.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SplitNumber]")});
            this.xrSplitNumber.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrSplitNumber.LocationFloat = new DevExpress.Utils.PointFloat(90F, 164F);
            this.xrSplitNumber.Name = "xrSplitNumber";
            this.xrSplitNumber.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrSplitNumber.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrSplitNumber.StylePriority.UseFont = false;
            this.xrSplitNumber.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capSplit
            // 
            this.capSplit.LocationFloat = new DevExpress.Utils.PointFloat(0F, 184F);
            this.capSplit.Name = "capSplit";
            this.capSplit.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capSplit.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capSplit.Text = "Split:";
            this.capSplit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrSplitDescription
            // 
            this.xrSplitDescription.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SplitDescription]")});
            this.xrSplitDescription.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrSplitDescription.LocationFloat = new DevExpress.Utils.PointFloat(90F, 184F);
            this.xrSplitDescription.Name = "xrSplitDescription";
            this.xrSplitDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrSplitDescription.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrSplitDescription.StylePriority.UseFont = false;
            this.xrSplitDescription.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capBol
            // 
            this.capBol.LocationFloat = new DevExpress.Utils.PointFloat(0F, 204F);
            this.capBol.Name = "capBol";
            this.capBol.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capBol.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capBol.Text = "BOL Type:";
            this.capBol.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrBolType
            // 
            this.xrBolType.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[BolType]")});
            this.xrBolType.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrBolType.LocationFloat = new DevExpress.Utils.PointFloat(90F, 204F);
            this.xrBolType.Name = "xrBolType";
            this.xrBolType.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrBolType.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrBolType.StylePriority.UseFont = false;
            this.xrBolType.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capHauler
            // 
            this.capHauler.CanShrink = true;
            this.capHauler.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Len(Trim([Hauler])) > 0")});
            this.capHauler.LocationFloat = new DevExpress.Utils.PointFloat(0F, 224F);
            this.capHauler.Name = "capHauler";
            this.capHauler.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capHauler.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capHauler.Text = "Hauler:";
            this.capHauler.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrHauler
            // 
            this.xrHauler.CanShrink = true;
            this.xrHauler.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Hauler]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Len(Trim([Hauler])) > 0")});
            this.xrHauler.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrHauler.LocationFloat = new DevExpress.Utils.PointFloat(90F, 224F);
            this.xrHauler.Name = "xrHauler";
            this.xrHauler.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrHauler.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrHauler.StylePriority.UseFont = false;
            this.xrHauler.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 250F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(269F, 2F);
            // 
            // capInTime
            // 
            this.capInTime.LocationFloat = new DevExpress.Utils.PointFloat(0F, 260F);
            this.capInTime.Name = "capInTime";
            this.capInTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capInTime.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capInTime.Text = "Inbound Time:";
            this.capInTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrInboundTime
            // 
            this.xrInboundTime.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[InboundTime]")});
            this.xrInboundTime.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrInboundTime.LocationFloat = new DevExpress.Utils.PointFloat(90F, 260F);
            this.xrInboundTime.Name = "xrInboundTime";
            this.xrInboundTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrInboundTime.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrInboundTime.StylePriority.UseFont = false;
            this.xrInboundTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrInboundTime.TextFormatString = "{0:g}";
            // 
            // capOutTime
            // 
            this.capOutTime.LocationFloat = new DevExpress.Utils.PointFloat(0F, 280F);
            this.capOutTime.Name = "capOutTime";
            this.capOutTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capOutTime.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capOutTime.Text = "Outbound Time:";
            this.capOutTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrOutboundTime
            // 
            this.xrOutboundTime.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OutboundTime]")});
            this.xrOutboundTime.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrOutboundTime.LocationFloat = new DevExpress.Utils.PointFloat(90F, 280F);
            this.xrOutboundTime.Name = "xrOutboundTime";
            this.xrOutboundTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrOutboundTime.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrOutboundTime.StylePriority.UseFont = false;
            this.xrOutboundTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrOutboundTime.TextFormatString = "{0:g}";
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 306F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(269F, 2F);
            // 
            // capInWeight
            // 
            this.capInWeight.Font = new DevExpress.Drawing.DXFont("Arial", 16F);
            this.capInWeight.LocationFloat = new DevExpress.Utils.PointFloat(22F, 316F);
            this.capInWeight.Name = "capInWeight";
            this.capInWeight.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capInWeight.SizeF = new System.Drawing.SizeF(90F, 27F);
            this.capInWeight.StylePriority.UseFont = false;
            this.capInWeight.Text = "Gross:";
            this.capInWeight.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrInboundWeight
            // 
            this.xrInboundWeight.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[InboundWeight]")});
            this.xrInboundWeight.Font = new DevExpress.Drawing.DXFont("Arial", 16F);
            this.xrInboundWeight.LocationFloat = new DevExpress.Utils.PointFloat(112F, 316F);
            this.xrInboundWeight.Name = "xrInboundWeight";
            this.xrInboundWeight.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrInboundWeight.SizeF = new System.Drawing.SizeF(140F, 27F);
            this.xrInboundWeight.StylePriority.UseFont = false;
            this.xrInboundWeight.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrInboundWeight.TextFormatString = "{0:n0}";
            // 
            // capOutWeight
            // 
            this.capOutWeight.Font = new DevExpress.Drawing.DXFont("Arial", 16F);
            this.capOutWeight.LocationFloat = new DevExpress.Utils.PointFloat(22F, 345F);
            this.capOutWeight.Name = "capOutWeight";
            this.capOutWeight.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capOutWeight.SizeF = new System.Drawing.SizeF(90F, 27F);
            this.capOutWeight.StylePriority.UseFont = false;
            this.capOutWeight.Text = "Tare:";
            this.capOutWeight.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrOutboundWeight
            // 
            this.xrOutboundWeight.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[OutboundWeight]")});
            this.xrOutboundWeight.Font = new DevExpress.Drawing.DXFont("Tahoma", 16F);
            this.xrOutboundWeight.LocationFloat = new DevExpress.Utils.PointFloat(112F, 345F);
            this.xrOutboundWeight.Name = "xrOutboundWeight";
            this.xrOutboundWeight.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrOutboundWeight.SizeF = new System.Drawing.SizeF(140F, 27F);
            this.xrOutboundWeight.StylePriority.UseFont = false;
            this.xrOutboundWeight.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrOutboundWeight.TextFormatString = "{0:n0}";
            // 
            // xrLineNet
            // 
            this.xrLineNet.LocationFloat = new DevExpress.Utils.PointFloat(8F, 376F);
            this.xrLineNet.Name = "xrLineNet";
            this.xrLineNet.SizeF = new System.Drawing.SizeF(251F, 2F);
            // 
            // capNet
            // 
            this.capNet.Font = new DevExpress.Drawing.DXFont("Arial", 16F);
            this.capNet.LocationFloat = new DevExpress.Utils.PointFloat(22F, 382F);
            this.capNet.Name = "capNet";
            this.capNet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capNet.SizeF = new System.Drawing.SizeF(90F, 27F);
            this.capNet.StylePriority.UseFont = false;
            this.capNet.Text = "Net:";
            this.capNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrNetWeight
            // 
            this.xrNetWeight.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[NetWeight]")});
            this.xrNetWeight.Font = new DevExpress.Drawing.DXFont("Tahoma", 16F);
            this.xrNetWeight.LocationFloat = new DevExpress.Utils.PointFloat(112F, 382F);
            this.xrNetWeight.Name = "xrNetWeight";
            this.xrNetWeight.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrNetWeight.SizeF = new System.Drawing.SizeF(140F, 27F);
            this.xrNetWeight.StylePriority.UseFont = false;
            this.xrNetWeight.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrNetWeight.TextFormatString = "{0:n0}";
            // 
            // xrFooter
            // 
            this.xrFooter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 5F);
            this.xrFooter.Name = "xrFooter";
            this.xrFooter.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrFooter.SizeF = new System.Drawing.SizeF(269F, 30F);
            this.xrFooter.Text = "Thank you";
            this.xrFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.LoadTicketDataModel);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox1.ImageSource"));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 23.95833F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(255.2083F, 129.1667F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // OutboundLoadTicketReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.Margins = new DevExpress.Drawing.DXMargins(22F, 19F, 157.2917F, 30F);
            this.PageWidthF = 315F;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            this.RollPaper = true;
            this.Version = "25.2";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        private XRPictureBox xrPictureBox1;
    }
}
