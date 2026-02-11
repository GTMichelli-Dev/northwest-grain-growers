namespace GrainManagement.Reporting
{
#nullable disable

    using System;
    using System.ComponentModel;
    using DevExpress.DataAccess.ObjectBinding;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraPrinting.BarCode;
    using DevExpress.XtraReports.UI;

    partial class LoadTicketReport
    {
        private IContainer components = null;

        private TopMarginBand topMarginBand1;
        private BottomMarginBand bottomMarginBand1;
        private DetailBand detailBand1;

        private ObjectDataSource objectDataSource1;

        private XRLabel lblTitle;
        private XRLabel lblTicket;
        private XRBarCode barTicket;
        private XRLine line1;
        private XRLine line2;

        private XRLabel capLocation; private XRLabel valLocation;
        private XRLabel capCustomer; private XRLabel valCustomer;
        private XRLabel capCommodity; private XRLabel valCommodity;
        private XRLabel capHauler; private XRLabel valHauler;
        private XRLabel capTruck; private XRLabel valTruck;
        private XRLabel capSheetId; private XRLabel valSheetId;
        private XRLabel capIn; private XRLabel valIn;
        private XRLabel capOut; private XRLabel valOut;

        private XRLabel lblLoadSection;

        private XRLabel capBin; private XRLabel valBin;
        private XRLabel capProtein; private XRLabel valProtein;
        private XRLabel capGross; private XRLabel valGross;
        private XRLabel capTare; private XRLabel valTare;
        private XRLabel capNet; private XRLabel valNet;

        private XRLabel lblFooter;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadTicketReport));
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator2 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.barTicket = new DevExpress.XtraReports.UI.XRBarCode();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.lblTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTicket = new DevExpress.XtraReports.UI.XRLabel();
            this.line1 = new DevExpress.XtraReports.UI.XRLine();
            this.capLocation = new DevExpress.XtraReports.UI.XRLabel();
            this.valLocation = new DevExpress.XtraReports.UI.XRLabel();
            this.capCustomer = new DevExpress.XtraReports.UI.XRLabel();
            this.valCustomer = new DevExpress.XtraReports.UI.XRLabel();
            this.capCommodity = new DevExpress.XtraReports.UI.XRLabel();
            this.valCommodity = new DevExpress.XtraReports.UI.XRLabel();
            this.capHauler = new DevExpress.XtraReports.UI.XRLabel();
            this.valHauler = new DevExpress.XtraReports.UI.XRLabel();
            this.capTruck = new DevExpress.XtraReports.UI.XRLabel();
            this.valTruck = new DevExpress.XtraReports.UI.XRLabel();
            this.capSheetId = new DevExpress.XtraReports.UI.XRLabel();
            this.valSheetId = new DevExpress.XtraReports.UI.XRLabel();
            this.capIn = new DevExpress.XtraReports.UI.XRLabel();
            this.valIn = new DevExpress.XtraReports.UI.XRLabel();
            this.capOut = new DevExpress.XtraReports.UI.XRLabel();
            this.valOut = new DevExpress.XtraReports.UI.XRLabel();
            this.line2 = new DevExpress.XtraReports.UI.XRLine();
            this.lblLoadSection = new DevExpress.XtraReports.UI.XRLabel();
            this.capBin = new DevExpress.XtraReports.UI.XRLabel();
            this.valBin = new DevExpress.XtraReports.UI.XRLabel();
            this.capProtein = new DevExpress.XtraReports.UI.XRLabel();
            this.valProtein = new DevExpress.XtraReports.UI.XRLabel();
            this.capGross = new DevExpress.XtraReports.UI.XRLabel();
            this.valGross = new DevExpress.XtraReports.UI.XRLabel();
            this.capTare = new DevExpress.XtraReports.UI.XRLabel();
            this.valTare = new DevExpress.XtraReports.UI.XRLabel();
            this.capNet = new DevExpress.XtraReports.UI.XRLabel();
            this.valNet = new DevExpress.XtraReports.UI.XRLabel();
            this.lblFooter = new DevExpress.XtraReports.UI.XRLabel();
            this.objectDataSource1 = new DevExpress.DataAccess.ObjectBinding.ObjectDataSource(this.components);
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrPictureBox2});
            this.topMarginBand1.HeightF = 153.75F;
            this.topMarginBand1.Name = "topMarginBand1";
            this.topMarginBand1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 100F);
            this.topMarginBand1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new DevExpress.Drawing.DXFont("Arial", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(5.000015F, 28.27778F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(264F, 22F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Grain Management";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.ImageSource = new DevExpress.XtraPrinting.Drawing.ImageSource("img", resources.GetString("xrPictureBox2.ImageSource"));
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(87F, 50.27777F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(100F, 100F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.barTicket});
            this.bottomMarginBand1.HeightF = 165.4165F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            this.bottomMarginBand1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 100F);
            this.bottomMarginBand1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // barTicket
            // 
            this.barTicket.AutoModule = true;
            this.barTicket.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Ticket]")});
            this.barTicket.LocationFloat = new DevExpress.Utils.PointFloat(10.00002F, 10.00002F);
            this.barTicket.Name = "barTicket";
            this.barTicket.Padding = new DevExpress.XtraPrinting.PaddingInfo(10F, 10F, 0F, 0F, 100F);
            this.barTicket.SizeF = new System.Drawing.SizeF(254F, 111.9444F);
            this.barTicket.Symbology = code128Generator2;
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine1,
            this.lblTitle,
            this.lblTicket,
            this.line1,
            this.capLocation,
            this.valLocation,
            this.capCustomer,
            this.valCustomer,
            this.capCommodity,
            this.valCommodity,
            this.capHauler,
            this.valHauler,
            this.capTruck,
            this.valTruck,
            this.capSheetId,
            this.valSheetId,
            this.capIn,
            this.valIn,
            this.capOut,
            this.valOut,
            this.line2,
            this.lblLoadSection,
            this.capBin,
            this.valBin,
            this.capProtein,
            this.valProtein,
            this.capGross,
            this.valGross,
            this.capTare,
            this.valTare,
            this.capNet,
            this.valNet,
            this.lblFooter});
            this.detailBand1.HeightF = 450.4169F;
            this.detailBand1.Name = "detailBand1";
            this.detailBand1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 100F);
            this.detailBand1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblTitle
            // 
            this.lblTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.lblTitle.SizeF = new System.Drawing.SizeF(269F, 22F);
            this.lblTitle.Text = "Load Ticket";
            this.lblTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblTicket
            // 
            this.lblTicket.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Ticket]")});
            this.lblTicket.Font = new DevExpress.Drawing.DXFont("Arial", 14F, DevExpress.Drawing.DXFontStyle.Bold);
            this.lblTicket.LocationFloat = new DevExpress.Utils.PointFloat(0F, 24F);
            this.lblTicket.Name = "lblTicket";
            this.lblTicket.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.lblTicket.SizeF = new System.Drawing.SizeF(269F, 26F);
            this.lblTicket.StylePriority.UseFont = false;
            this.lblTicket.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // line1
            // 
            this.line1.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 54.27789F);
            this.line1.Name = "line1";
            this.line1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 100F);
            this.line1.SizeF = new System.Drawing.SizeF(269F, 2F);
            // 
            // capLocation
            // 
            this.capLocation.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 64.2779F);
            this.capLocation.Name = "capLocation";
            this.capLocation.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capLocation.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capLocation.Text = "Location:";
            this.capLocation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valLocation
            // 
            this.valLocation.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Location]")});
            this.valLocation.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.valLocation.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 64.2779F);
            this.valLocation.Name = "valLocation";
            this.valLocation.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valLocation.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.valLocation.StylePriority.UseFont = false;
            this.valLocation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capCustomer
            // 
            this.capCustomer.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 84.27789F);
            this.capCustomer.Name = "capCustomer";
            this.capCustomer.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capCustomer.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capCustomer.Text = "Customer:";
            this.capCustomer.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valCustomer
            // 
            this.valCustomer.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Customer]")});
            this.valCustomer.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.valCustomer.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 84.27789F);
            this.valCustomer.Name = "valCustomer";
            this.valCustomer.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valCustomer.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.valCustomer.StylePriority.UseFont = false;
            this.valCustomer.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capCommodity
            // 
            this.capCommodity.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 104.2779F);
            this.capCommodity.Name = "capCommodity";
            this.capCommodity.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capCommodity.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capCommodity.Text = "Commodity:";
            this.capCommodity.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valCommodity
            // 
            this.valCommodity.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Commodity]")});
            this.valCommodity.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.valCommodity.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 104.2779F);
            this.valCommodity.Name = "valCommodity";
            this.valCommodity.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valCommodity.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.valCommodity.StylePriority.UseFont = false;
            this.valCommodity.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capHauler
            // 
            this.capHauler.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 124.2779F);
            this.capHauler.Name = "capHauler";
            this.capHauler.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capHauler.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capHauler.Text = "Hauler:";
            this.capHauler.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valHauler
            // 
            this.valHauler.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Hauler]")});
            this.valHauler.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.valHauler.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 124.2779F);
            this.valHauler.Name = "valHauler";
            this.valHauler.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valHauler.SizeF = new System.Drawing.SizeF(179F, 17.99999F);
            this.valHauler.StylePriority.UseFont = false;
            this.valHauler.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capTruck
            // 
            this.capTruck.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 144.2779F);
            this.capTruck.Name = "capTruck";
            this.capTruck.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capTruck.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capTruck.Text = "Truck:";
            this.capTruck.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valTruck
            // 
            this.valTruck.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[TruckId]")});
            this.valTruck.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.valTruck.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 144.2779F);
            this.valTruck.Name = "valTruck";
            this.valTruck.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valTruck.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.valTruck.StylePriority.UseFont = false;
            this.valTruck.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capSheetId
            // 
            this.capSheetId.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 164.2778F);
            this.capSheetId.Name = "capSheetId";
            this.capSheetId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capSheetId.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capSheetId.Text = "Weight Sheet:";
            this.capSheetId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valSheetId
            // 
            this.valSheetId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WeightSheetId]")});
            this.valSheetId.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.valSheetId.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 164.2778F);
            this.valSheetId.Name = "valSheetId";
            this.valSheetId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valSheetId.SizeF = new System.Drawing.SizeF(179F, 18F);
            this.valSheetId.StylePriority.UseFont = false;
            this.valSheetId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.valSheetId.TextFormatString = "{0}";
            // 
            // capIn
            // 
            this.capIn.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 184.2779F);
            this.capIn.Name = "capIn";
            this.capIn.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capIn.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capIn.Text = "In:";
            this.capIn.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valIn
            // 
            this.valIn.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DateTimeIn]")});
            this.valIn.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.valIn.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 184.2779F);
            this.valIn.Name = "valIn";
            this.valIn.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valIn.SizeF = new System.Drawing.SizeF(179F, 17.99998F);
            this.valIn.StylePriority.UseFont = false;
            this.valIn.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.valIn.TextFormatString = "{0:g}";
            // 
            // capOut
            // 
            this.capOut.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 204.2779F);
            this.capOut.Name = "capOut";
            this.capOut.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capOut.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capOut.Text = "Out:";
            this.capOut.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valOut
            // 
            this.valOut.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DateTimeOut]")});
            this.valOut.Font = new DevExpress.Drawing.DXFont("Arial", 9F, DevExpress.Drawing.DXFontStyle.Bold);
            this.valOut.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 204.2779F);
            this.valOut.Name = "valOut";
            this.valOut.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valOut.SizeF = new System.Drawing.SizeF(179F, 18.00002F);
            this.valOut.StylePriority.UseFont = false;
            this.valOut.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.valOut.TextFormatString = "{0:g}";
            // 
            // line2
            // 
            this.line2.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 232.2779F);
            this.line2.Name = "line2";
            this.line2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 100F);
            this.line2.SizeF = new System.Drawing.SizeF(269F, 2.000015F);
            // 
            // lblLoadSection
            // 
            this.lblLoadSection.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 244.2779F);
            this.lblLoadSection.Name = "lblLoadSection";
            this.lblLoadSection.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.lblLoadSection.SizeF = new System.Drawing.SizeF(269F, 20F);
            this.lblLoadSection.Text = "Load";
            this.lblLoadSection.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // capBin
            // 
            this.capBin.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 270.2779F);
            this.capBin.Name = "capBin";
            this.capBin.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capBin.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capBin.Text = "Bin:";
            this.capBin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valBin
            // 
            this.valBin.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Bin]")});
            this.valBin.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 270.2779F);
            this.valBin.Name = "valBin";
            this.valBin.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valBin.SizeF = new System.Drawing.SizeF(139.7222F, 18F);
            this.valBin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capProtein
            // 
            this.capProtein.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 290.278F);
            this.capProtein.Name = "capProtein";
            this.capProtein.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capProtein.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capProtein.Text = "Protein:";
            this.capProtein.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valProtein
            // 
            this.valProtein.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Protein]")});
            this.valProtein.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 290.278F);
            this.valProtein.Name = "valProtein";
            this.valProtein.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valProtein.SizeF = new System.Drawing.SizeF(139.7222F, 18F);
            this.valProtein.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.valProtein.TextFormatString = "{0:n1}";
            // 
            // capGross
            // 
            this.capGross.Font = new DevExpress.Drawing.DXFont("Arial", 16F);
            this.capGross.LocationFloat = new DevExpress.Utils.PointFloat(22.13894F, 322.0557F);
            this.capGross.Name = "capGross";
            this.capGross.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capGross.SizeF = new System.Drawing.SizeF(90F, 26.86115F);
            this.capGross.StylePriority.UseFont = false;
            this.capGross.Text = "Gross:";
            this.capGross.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valGross
            // 
            this.valGross.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Gross]")});
            this.valGross.Font = new DevExpress.Drawing.DXFont("Arial", 16F);
            this.valGross.LocationFloat = new DevExpress.Utils.PointFloat(112.1389F, 322.0557F);
            this.valGross.Name = "valGross";
            this.valGross.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valGross.SizeF = new System.Drawing.SizeF(139.7222F, 26.86115F);
            this.valGross.StylePriority.UseFont = false;
            this.valGross.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.valGross.TextFormatString = "{0:n0}";
            // 
            // capTare
            // 
            this.capTare.Font = new DevExpress.Drawing.DXFont("Arial", 16F);
            this.capTare.LocationFloat = new DevExpress.Utils.PointFloat(22.13894F, 350.9169F);
            this.capTare.Name = "capTare";
            this.capTare.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capTare.SizeF = new System.Drawing.SizeF(90F, 24.86108F);
            this.capTare.StylePriority.UseFont = false;
            this.capTare.Text = "Tare:";
            this.capTare.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valTare
            // 
            this.valTare.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Tare]")});
            this.valTare.Font = new DevExpress.Drawing.DXFont("Tahoma", 16F);
            this.valTare.LocationFloat = new DevExpress.Utils.PointFloat(112.1389F, 348.9168F);
            this.valTare.Name = "valTare";
            this.valTare.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valTare.SizeF = new System.Drawing.SizeF(139.7222F, 26.86115F);
            this.valTare.StylePriority.UseFont = false;
            this.valTare.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.valTare.TextFormatString = "{0:n0}";
            // 
            // capNet
            // 
            this.capNet.Font = new DevExpress.Drawing.DXFont("Arial", 16F);
            this.capNet.LocationFloat = new DevExpress.Utils.PointFloat(22.13894F, 382.2778F);
            this.capNet.Name = "capNet";
            this.capNet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capNet.SizeF = new System.Drawing.SizeF(90F, 26.86115F);
            this.capNet.StylePriority.UseFont = false;
            this.capNet.Text = "Net:";
            this.capNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valNet
            // 
            this.valNet.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Net]")});
            this.valNet.Font = new DevExpress.Drawing.DXFont("Tahoma", 16F);
            this.valNet.LocationFloat = new DevExpress.Utils.PointFloat(112.1388F, 382.2778F);
            this.valNet.Name = "valNet";
            this.valNet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valNet.SizeF = new System.Drawing.SizeF(139.7222F, 26.86115F);
            this.valNet.StylePriority.UseFont = false;
            this.valNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.valNet.TextFormatString = "{0:n0}";
            // 
            // lblFooter
            // 
            this.lblFooter.LocationFloat = new DevExpress.Utils.PointFloat(4.238552E-05F, 420.4169F);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.lblFooter.SizeF = new System.Drawing.SizeF(269F, 30F);
            this.lblFooter.Text = "Thank you";
            this.lblFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.LoadTicketPrintDto);
            this.objectDataSource1.Name = "objectDataSource1";
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(8.555561F, 380.2778F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(250.6944F, 2F);
            // 
            // LoadTicketReport
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.topMarginBand1,
            this.detailBand1,
            this.bottomMarginBand1});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.objectDataSource1});
            this.DataSource = this.objectDataSource1;
            this.Font = new DevExpress.Drawing.DXFont("Arial", 9F);
            this.Margins = new DevExpress.Drawing.DXMargins(22F, 19F, 153.75F, 165.4165F);
            this.PageWidthF = 315F;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            this.RollPaper = true;
            this.Version = "25.2";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        private XRLabel xrLabel1;
        private XRPictureBox xrPictureBox2;
        private XRLine xrLine1;
    }

}
