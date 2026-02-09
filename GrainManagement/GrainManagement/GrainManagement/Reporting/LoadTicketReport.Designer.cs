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
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator2 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            this.topMarginBand1 = new DevExpress.XtraReports.UI.TopMarginBand();
            this.bottomMarginBand1 = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.detailBand1 = new DevExpress.XtraReports.UI.DetailBand();
            this.lblTitle = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTicket = new DevExpress.XtraReports.UI.XRLabel();
            this.barTicket = new DevExpress.XtraReports.UI.XRBarCode();
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
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // topMarginBand1
            // 
            this.topMarginBand1.HeightF = 116.8056F;
            this.topMarginBand1.Name = "topMarginBand1";
            this.topMarginBand1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 100F);
            this.topMarginBand1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // bottomMarginBand1
            // 
            this.bottomMarginBand1.HeightF = 165.4165F;
            this.bottomMarginBand1.Name = "bottomMarginBand1";
            this.bottomMarginBand1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 100F);
            this.bottomMarginBand1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // detailBand1
            // 
            this.detailBand1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblTitle,
            this.lblTicket,
            this.barTicket,
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
            this.detailBand1.HeightF = 509.8613F;
            this.detailBand1.Name = "detailBand1";
            this.detailBand1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 100F);
            this.detailBand1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblTitle
            // 
            this.lblTitle.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.lblTitle.SizeF = new System.Drawing.SizeF(295F, 22F);
            this.lblTitle.Text = "Load Ticket";
            this.lblTitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lblTicket
            // 
            this.lblTicket.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Ticket]")});
            this.lblTicket.LocationFloat = new DevExpress.Utils.PointFloat(0F, 24F);
            this.lblTicket.Name = "lblTicket";
            this.lblTicket.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.lblTicket.SizeF = new System.Drawing.SizeF(295F, 26F);
            this.lblTicket.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // barTicket
            // 
            this.barTicket.AutoModule = true;
            this.barTicket.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Ticket]")});
            this.barTicket.LocationFloat = new DevExpress.Utils.PointFloat(19.99999F, 54.00001F);
            this.barTicket.Name = "barTicket";
            this.barTicket.Padding = new DevExpress.XtraPrinting.PaddingInfo(10F, 10F, 0F, 0F, 100F);
            this.barTicket.ShowText = false;
            this.barTicket.SizeF = new System.Drawing.SizeF(255F, 79.30556F);
            this.barTicket.Symbology = code128Generator2;
            // 
            // line1
            // 
            this.line1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 146.639F);
            this.line1.Name = "line1";
            this.line1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 100F);
            this.line1.SizeF = new System.Drawing.SizeF(295F, 2F);
            // 
            // capLocation
            // 
            this.capLocation.LocationFloat = new DevExpress.Utils.PointFloat(0F, 156.639F);
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
            this.valLocation.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 156.639F);
            this.valLocation.Name = "valLocation";
            this.valLocation.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valLocation.SizeF = new System.Drawing.SizeF(205F, 18F);
            this.valLocation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capCustomer
            // 
            this.capCustomer.LocationFloat = new DevExpress.Utils.PointFloat(0F, 176.639F);
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
            this.valCustomer.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 176.639F);
            this.valCustomer.Name = "valCustomer";
            this.valCustomer.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valCustomer.SizeF = new System.Drawing.SizeF(205F, 18F);
            this.valCustomer.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capCommodity
            // 
            this.capCommodity.LocationFloat = new DevExpress.Utils.PointFloat(0F, 196.639F);
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
            this.valCommodity.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 196.639F);
            this.valCommodity.Name = "valCommodity";
            this.valCommodity.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valCommodity.SizeF = new System.Drawing.SizeF(205F, 18F);
            this.valCommodity.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capHauler
            // 
            this.capHauler.LocationFloat = new DevExpress.Utils.PointFloat(0F, 216.639F);
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
            this.valHauler.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 216.639F);
            this.valHauler.Name = "valHauler";
            this.valHauler.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valHauler.SizeF = new System.Drawing.SizeF(205F, 18F);
            this.valHauler.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capTruck
            // 
            this.capTruck.LocationFloat = new DevExpress.Utils.PointFloat(0F, 236.639F);
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
            this.valTruck.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 236.639F);
            this.valTruck.Name = "valTruck";
            this.valTruck.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valTruck.SizeF = new System.Drawing.SizeF(205F, 18F);
            this.valTruck.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capSheetId
            // 
            this.capSheetId.LocationFloat = new DevExpress.Utils.PointFloat(0F, 256.6389F);
            this.capSheetId.Name = "capSheetId";
            this.capSheetId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capSheetId.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capSheetId.Text = "Sheet Id:";
            this.capSheetId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valSheetId
            // 
            this.valSheetId.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WeightSheetId]")});
            this.valSheetId.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 256.6389F);
            this.valSheetId.Name = "valSheetId";
            this.valSheetId.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valSheetId.SizeF = new System.Drawing.SizeF(205F, 18F);
            this.valSheetId.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.valSheetId.TextFormatString = "{0}";
            // 
            // capIn
            // 
            this.capIn.LocationFloat = new DevExpress.Utils.PointFloat(0F, 276.639F);
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
            this.valIn.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 276.639F);
            this.valIn.Name = "valIn";
            this.valIn.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valIn.SizeF = new System.Drawing.SizeF(205F, 18F);
            this.valIn.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.valIn.TextFormatString = "{0:g}";
            // 
            // capOut
            // 
            this.capOut.LocationFloat = new DevExpress.Utils.PointFloat(0F, 296.639F);
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
            this.valOut.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 296.639F);
            this.valOut.Name = "valOut";
            this.valOut.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valOut.SizeF = new System.Drawing.SizeF(205F, 18F);
            this.valOut.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.valOut.TextFormatString = "{0:g}";
            // 
            // line2
            // 
            this.line2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 324.639F);
            this.line2.Name = "line2";
            this.line2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0F, 0F, 0F, 0F, 100F);
            this.line2.SizeF = new System.Drawing.SizeF(295F, 2F);
            // 
            // lblLoadSection
            // 
            this.lblLoadSection.LocationFloat = new DevExpress.Utils.PointFloat(0F, 336.639F);
            this.lblLoadSection.Name = "lblLoadSection";
            this.lblLoadSection.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.lblLoadSection.SizeF = new System.Drawing.SizeF(295F, 20F);
            this.lblLoadSection.Text = "Load";
            this.lblLoadSection.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // capBin
            // 
            this.capBin.LocationFloat = new DevExpress.Utils.PointFloat(0F, 362.639F);
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
            this.valBin.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 362.639F);
            this.valBin.Name = "valBin";
            this.valBin.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valBin.SizeF = new System.Drawing.SizeF(205F, 18F);
            this.valBin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // capProtein
            // 
            this.capProtein.LocationFloat = new DevExpress.Utils.PointFloat(0F, 382.6391F);
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
            this.valProtein.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 382.6391F);
            this.valProtein.Name = "valProtein";
            this.valProtein.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valProtein.SizeF = new System.Drawing.SizeF(205F, 18F);
            this.valProtein.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.valProtein.TextFormatString = "{0:n1}";
            // 
            // capGross
            // 
            this.capGross.LocationFloat = new DevExpress.Utils.PointFloat(0F, 406.639F);
            this.capGross.Name = "capGross";
            this.capGross.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capGross.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capGross.Text = "Gross:";
            this.capGross.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valGross
            // 
            this.valGross.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Gross]")});
            this.valGross.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 406.639F);
            this.valGross.Name = "valGross";
            this.valGross.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valGross.SizeF = new System.Drawing.SizeF(205F, 18F);
            this.valGross.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.valGross.TextFormatString = "{0:n0}";
            // 
            // capTare
            // 
            this.capTare.LocationFloat = new DevExpress.Utils.PointFloat(0F, 426.639F);
            this.capTare.Name = "capTare";
            this.capTare.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capTare.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capTare.Text = "Tare:";
            this.capTare.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valTare
            // 
            this.valTare.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Tare]")});
            this.valTare.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 426.639F);
            this.valTare.Name = "valTare";
            this.valTare.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valTare.SizeF = new System.Drawing.SizeF(205F, 18F);
            this.valTare.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.valTare.TextFormatString = "{0:n0}";
            // 
            // capNet
            // 
            this.capNet.LocationFloat = new DevExpress.Utils.PointFloat(0F, 446.639F);
            this.capNet.Name = "capNet";
            this.capNet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.capNet.SizeF = new System.Drawing.SizeF(90F, 18F);
            this.capNet.Text = "Net:";
            this.capNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // valNet
            // 
            this.valNet.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Net]")});
            this.valNet.LocationFloat = new DevExpress.Utils.PointFloat(89.99998F, 446.639F);
            this.valNet.Name = "valNet";
            this.valNet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.valNet.SizeF = new System.Drawing.SizeF(205F, 22F);
            this.valNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.valNet.TextFormatString = "{0:n0}";
            // 
            // lblFooter
            // 
            this.lblFooter.LocationFloat = new DevExpress.Utils.PointFloat(0F, 479.8613F);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Padding = new DevExpress.XtraPrinting.PaddingInfo(2F, 2F, 0F, 0F, 100F);
            this.lblFooter.SizeF = new System.Drawing.SizeF(295F, 30F);
            this.lblFooter.Text = "Thank you";
            this.lblFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // objectDataSource1
            // 
            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.LoadTicketPrintDto);
            this.objectDataSource1.Name = "objectDataSource1";
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
            this.Margins = new DevExpress.Drawing.DXMargins(10F, 10F, 116.8056F, 165.4165F);
            this.PageWidthF = 315F;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Custom;
            this.RollPaper = true;
            this.Version = "25.2";
            ((System.ComponentModel.ISupportInitialize)(this.objectDataSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
    }

}
