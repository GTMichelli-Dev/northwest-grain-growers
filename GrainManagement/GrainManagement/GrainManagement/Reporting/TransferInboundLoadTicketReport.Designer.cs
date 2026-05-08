namespace GrainManagement.Reporting
{
#nullable disable

    using System;
    using System.ComponentModel;
    using DevExpress.DataAccess.ObjectBinding;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraPrinting.BarCode;
    using DevExpress.XtraReports.UI;

    partial class TransferInboundLoadTicketReport
    {
        private IContainer components = null;

        private TopMarginBand topMarginBand1;
        private BottomMarginBand bottomMarginBand1;
        private DetailBand detailBand1;

        private ObjectDataSource objectDataSource1;

        // Detail controls — named meaningfully for Designer editing
        private XRLabel xrTitle;
        private XRLabel xrLoadId;
        private XRLine xrLine1;

        private XRLabel capLocation;    private XRLabel xrLocation;
        private XRLabel capWeightSheet; private XRLabel xrWeightSheetId;
        private XRLabel capAccount;     private XRLabel xrCropAccount;
        private XRLabel capHauler;      private XRLabel xrHauler;

        private XRLine xrLine2;
        private XRLabel capInTime;      private XRLabel xrInboundTime;
        private XRLine xrLine3;

        private XRLabel capInWeight;
        private XRLabel xrInboundWeight;
        private XRLabel xrFooter;

        // Bottom margin
        private XRBarCode xrBarcodeLoadId;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator2 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrBarcodeLoadId = new DevExpress.XtraReports.UI.XRBarCode();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrFooter = new DevExpress.XtraReports.UI.XRLabel();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLoadId = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.capLocation = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLocation = new DevExpress.XtraReports.UI.XRLabel();
            this.capWeightSheet = new DevExpress.XtraReports.UI.XRLabel();
            this.xrWeightSheetId = new DevExpress.XtraReports.UI.XRLabel();
            this.capAccount = new DevExpress.XtraReports.UI.XRLabel();
            this.xrCropAccount = new DevExpress.XtraReports.UI.XRLabel();
            this.capHauler = new DevExpress.XtraReports.UI.XRLabel();
            this.xrHauler = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.capInTime = new DevExpress.XtraReports.UI.XRLabel();
            this.xrInboundTime = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine3 = new DevExpress.XtraReports.UI.XRLine();
            this.capInWeight = new DevExpress.XtraReports.UI.XRLabel();
            this.xrInboundWeight = new DevExpress.XtraReports.UI.XRLabel();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrBarcodeLoadId});
            this.topMarginBand1.HeightF = 182.2917F;
            this.topMarginBand1.Name = "topMarginBand1";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Arial", 12F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 156.25F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(269F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Northwest Grain Growers";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrBarcodeLoadId
            // 
            this.xrBarcodeLoadId.AutoModule = true;
            this.xrBarcodeLoadId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[LoadId]")});
            this.xrBarcodeLoadId.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 33.45833F);
            this.xrBarcodeLoadId.Name = "xrBarcodeLoadId";
            this.xrBarcodeLoadId.Padding = new DevExpress.XtraPrinting.PaddingInfo(10F, 10F, 0F, 0F, 100F);
            this.xrBarcodeLoadId.SizeF = new System.Drawing.SizeF(254F, 112F);
            this.xrBarcodeLoadId.Symbology = code128Generator2;
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrFooter});
            this.bottomMarginBand1.HeightF = 40F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
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
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel3,
            this.xrLabel2,
            this.xrTitle,
            this.xrLoadId,
            this.xrLine1,
            this.capLocation,
            this.xrLocation,
            this.capWeightSheet,
            this.xrWeightSheetId,
            this.capAccount,
            this.xrCropAccount,
            this.capHauler,
            this.xrHauler,
            this.xrLine2,
            this.capInTime,
            this.xrInboundTime,
            this.xrLine3,
            this.capInWeight,
            this.xrInboundWeight});
            this.detailBand1.HeightF = 295.2082F;
            this.detailBand1.Name = "detailBand1";
            // 
            // xrLabel3
            // 
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Commodity]")});
            this.xrLabel3.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(91F, 105F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 105F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.xrLabel2.Text = "Commodity:";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTitle
            // 
            this.xrTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTitle.Name = "xrTitle";
            this.xrTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrTitle.SizeF = new System.Drawing.SizeF(269F, 22F);
            this.xrTitle.Text = "INBOUND TRANSFER TICKET";
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
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Location]+\'-\'+[LocationId]")});
            this.xrLocation.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLocation.LocationFloat = new DevExpress.Utils.PointFloat(90F, 64F);
            this.xrLocation.Name = "xrLocation";
            this.xrLocation.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLocation.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrLocation.StylePriority.UseFont = false;
            this.xrLocation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capWeightSheet
            // 
            this.capWeightSheet.LocationFloat = new DevExpress.Utils.PointFloat(0F, 85F);
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
            this.xrWeightSheetId.LocationFloat = new DevExpress.Utils.PointFloat(90F, 85F);
            this.xrWeightSheetId.Name = "xrWeightSheetId";
            this.xrWeightSheetId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrWeightSheetId.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrWeightSheetId.StylePriority.UseFont = false;
            this.xrWeightSheetId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capAccount
            // 
            this.capAccount.LocationFloat = new DevExpress.Utils.PointFloat(0F, 125F);
            this.capAccount.Name = "capAccount";
            this.capAccount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capAccount.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capAccount.Text = "Destination:";
            this.capAccount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrCropAccount
            // 
            this.xrCropAccount.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DestinationLocation]+\'-\'+ [DestinationLocationId]")});
            this.xrCropAccount.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrCropAccount.LocationFloat = new DevExpress.Utils.PointFloat(90F, 125F);
            this.xrCropAccount.Name = "xrCropAccount";
            this.xrCropAccount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrCropAccount.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrCropAccount.StylePriority.UseFont = false;
            this.xrCropAccount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capHauler
            // 
            this.capHauler.CanShrink = true;
            this.capHauler.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Len(Trim([Hauler])) > 0")});
            this.capHauler.LocationFloat = new DevExpress.Utils.PointFloat(0F, 169F);
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
            this.xrHauler.LocationFloat = new DevExpress.Utils.PointFloat(90F, 169F);
            this.xrHauler.Name = "xrHauler";
            this.xrHauler.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrHauler.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrHauler.StylePriority.UseFont = false;
            this.xrHauler.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 187F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(269F, 2F);
            // 
            // capInTime
            // 
            this.capInTime.LocationFloat = new DevExpress.Utils.PointFloat(0F, 197F);
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
            this.xrInboundTime.LocationFloat = new DevExpress.Utils.PointFloat(90F, 197F);
            this.xrInboundTime.Name = "xrInboundTime";
            this.xrInboundTime.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrInboundTime.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrInboundTime.StylePriority.UseFont = false;
            this.xrInboundTime.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrInboundTime.TextFormatString = "{0:g}";
            // 
            // xrLine3
            // 
            this.xrLine3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 223F);
            this.xrLine3.Name = "xrLine3";
            this.xrLine3.SizeF = new System.Drawing.SizeF(269F, 2F);
            // 
            // capInWeight
            // 
            this.capInWeight.LocationFloat = new DevExpress.Utils.PointFloat(0F, 233F);
            this.capInWeight.Name = "capInWeight";
            this.capInWeight.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capInWeight.SizeF = new System.Drawing.SizeF(269F, 20F);
            this.capInWeight.Text = "Inbound Weight";
            this.capInWeight.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrInboundWeight
            // 
            this.xrInboundWeight.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[InboundWeight]+[StartManualFlag]")});
            this.xrInboundWeight.Font = new DevExpress.Drawing.DXFont("Arial", 16F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrInboundWeight.LocationFloat = new DevExpress.Utils.PointFloat(22F, 257F);
            this.xrInboundWeight.Name = "xrInboundWeight";
            this.xrInboundWeight.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrInboundWeight.SizeF = new System.Drawing.SizeF(230F, 30F);
            this.xrInboundWeight.StylePriority.UseFont = false;
            this.xrInboundWeight.StylePriority.UseTextAlignment = false;
            this.xrInboundWeight.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrInboundWeight.TextFormatString = "{0:n0}";
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.TransferLoadTicketDataModel);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // xrLabel10
            // 
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(0F, 148F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.xrLabel10.Text = "Truck Id:";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel9
            // 
            this.xrLabel9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TruckId]")});
            this.xrLabel9.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(90F, 148F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // TransferInboundLoadTicketReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.Margins = new DevExpress.Drawing.DXMargins(22F, 19F, 182.2917F, 40F);
            this.PageWidthF = 315F;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            this.RollPaper = true;
            this.Version = "25.2";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        private XRLabel xrLabel1;
        private XRLabel xrLabel3;
        private XRLabel xrLabel2;
        private XRLabel xrLabel10;
        private XRLabel xrLabel9;
    }
}
