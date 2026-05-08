namespace GrainManagement.Reporting
{
#nullable disable

    using System;
    using System.ComponentModel;
    using System.Drawing;
    using DevExpress.DataAccess.ObjectBinding;
    using DevExpress.Utils;
    using DevExpress.XtraPrinting;
    using DevExpress.XtraReports.UI;

    partial class DailyIntakeReport
    {
        // Designer-friendly skeleton for the End Of Day Intake summary report.
        // Bound to GrainManagement.Dtos.Warehouse.DailyIntakeReportDto via
        // ObjectDataSource. The Detail band iterates over the parent DTO's
        // "Rows" collection — XRLabels in Detail can reference row fields
        // directly (e.g. WeightSheetId, Commodity, Net), while bands above
        // Detail can reference the parent DTO header fields (LocationName,
        // CreationDate, etc.). Open in the DevExpress XtraReports designer
        // to position controls, add Commodity grouping, and add totals.
        private IContainer components = null;

        private TopMarginBand topMarginBand1;
        private ReportHeaderBand reportHeaderBand1;
        private PageHeaderBand pageHeaderBand1;
        private DetailBand detailBand1;
        private ReportFooterBand reportFooterBand1;
        private BottomMarginBand bottomMarginBand1;

        private ObjectDataSource objectDataSource1;

        // Header
        private XRLabel xrTitle;
        private XRLabel xrLocation;
        private XRLabel xrCreationDate;

        // Page header / column captions
        private XRLabel capWs;
        private XRLabel capCommodity;
        private XRLabel capLot;
        private XRLabel capCustomer;
        private XRLabel capLandlord;
        private XRLabel capFsa;
        private XRLabel capNetLbs;
        private XRLabel capNetUnits;

        // Detail
        private XRLabel xrWs;
        private XRLabel xrCommodity;
        private XRLabel xrLot;
        private XRLabel xrCustomer;
        private XRLabel xrLandlord;
        private XRLabel xrFsa;
        private XRLabel xrNetLbs;
        private XRLabel xrNetUnits;

        // Footer
        private XRLabel xrPageInfo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            this.topMarginBand1     = new TopMarginBand();
            this.reportHeaderBand1  = new ReportHeaderBand();
            this.pageHeaderBand1    = new PageHeaderBand();
            this.detailBand1        = new DetailBand();
            this.reportFooterBand1  = new ReportFooterBand();
            this.bottomMarginBand1  = new BottomMarginBand();
            this.objectDataSource1  = new ObjectDataSource(this.components);

            this.xrTitle           = new XRLabel();
            this.xrLocation        = new XRLabel();
            this.xrCreationDate    = new XRLabel();

            this.capWs        = new XRLabel();
            this.capCommodity = new XRLabel();
            this.capLot       = new XRLabel();
            this.capCustomer  = new XRLabel();
            this.capLandlord  = new XRLabel();
            this.capFsa       = new XRLabel();
            this.capNetLbs    = new XRLabel();
            this.capNetUnits  = new XRLabel();

            this.xrWs        = new XRLabel();
            this.xrCommodity = new XRLabel();
            this.xrLot       = new XRLabel();
            this.xrCustomer  = new XRLabel();
            this.xrLandlord  = new XRLabel();
            this.xrFsa       = new XRLabel();
            this.xrNetLbs    = new XRLabel();
            this.xrNetUnits  = new XRLabel();

            this.xrPageInfo  = new XRLabel();

            // ── ObjectDataSource ────────────────────────────────────────────
            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.DailyIntakeReportDto);
            this.objectDataSource1.Name = "objectDataSource1";

            // ── Title ───────────────────────────────────────────────────────
            this.xrTitle.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            this.xrTitle.LocationFloat = new PointFloat(0, 0);
            this.xrTitle.SizeF = new SizeF(1000, 26);
            this.xrTitle.TextAlignment = TextAlignment.MiddleCenter;
            this.xrTitle.Text = "END OF DAY INTAKE REPORT";
            this.xrTitle.Name = "xrTitle";

            this.xrLocation.LocationFloat = new PointFloat(0, 28);
            this.xrLocation.SizeF = new SizeF(1000, 22);
            this.xrLocation.TextAlignment = TextAlignment.MiddleCenter;
            this.xrLocation.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            this.xrLocation.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text",
                "[LocationName] + ' / ' + [LocationId]"));
            this.xrLocation.Name = "xrLocation";

            this.xrCreationDate.LocationFloat = new PointFloat(0, 52);
            this.xrCreationDate.SizeF = new SizeF(1000, 20);
            this.xrCreationDate.TextAlignment = TextAlignment.MiddleCenter;
            this.xrCreationDate.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[CreationDate]"));
            this.xrCreationDate.Name = "xrCreationDate";

            this.reportHeaderBand1.HeightF = 78;
            this.reportHeaderBand1.Controls.AddRange(new XRControl[] {
                this.xrTitle, this.xrLocation, this.xrCreationDate });

            // ── Page header (column captions). Layout out via the designer
            //   for spacing — left at sequential X positions for now. ──────
            ConfigureCaption(this.capWs,        "WS #",      0,    100);
            ConfigureCaption(this.capCommodity, "Commod",    100,  120);
            ConfigureCaption(this.capLot,       "Lot #",     220,  90);
            ConfigureCaption(this.capCustomer,  "Customer",  310,  220);
            ConfigureCaption(this.capLandlord,  "Landlord",  530,  140);
            ConfigureCaption(this.capFsa,       "Farm",      670,  100);
            ConfigureCaption(this.capNetLbs,    "Net Lbs",   770,  100, TextAlignment.MiddleRight);
            ConfigureCaption(this.capNetUnits,  "Net Units", 870,  100, TextAlignment.MiddleRight);

            this.pageHeaderBand1.HeightF = 22;
            this.pageHeaderBand1.Controls.AddRange(new XRControl[] {
                this.capWs, this.capCommodity, this.capLot, this.capCustomer,
                this.capLandlord, this.capFsa, this.capNetLbs, this.capNetUnits });

            // ── Detail band — bound to the parent DTO's Rows collection ───
            this.detailBand1.HeightF = 22;
            ConfigureField(this.xrWs,        "[WeightSheetId]",                       0,    100);
            ConfigureField(this.xrCommodity, "[Commodity]",                           100,  120);
            ConfigureField(this.xrLot,       "[LotNumber]",                           220,  90);
            ConfigureField(this.xrCustomer,  "[Customer]",                            310,  220);
            ConfigureField(this.xrLandlord,  "[Landlord]",                            530,  140);
            ConfigureField(this.xrFsa,       "[FsaNumber]",                           670,  100);
            ConfigureField(this.xrNetLbs,    "FormatString('{0:n0} lbs', [Net])",     770,  100, TextAlignment.MiddleRight);
            ConfigureField(this.xrNetUnits,  "FormatString('{0:n2} ', [Units]) + [Uom]", 870, 100, TextAlignment.MiddleRight);
            this.detailBand1.Controls.AddRange(new XRControl[] {
                this.xrWs, this.xrCommodity, this.xrLot, this.xrCustomer,
                this.xrLandlord, this.xrFsa, this.xrNetLbs, this.xrNetUnits });

            // ── Footer ────────────────────────────────────────────────────
            this.xrPageInfo.LocationFloat = new PointFloat(800, 0);
            this.xrPageInfo.SizeF = new SizeF(170, 20);
            this.xrPageInfo.TextAlignment = TextAlignment.MiddleRight;
            this.xrPageInfo.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text",
                "'Page ' + [PageNumber] + ' of ' + [TotalPageCount]"));
            this.xrPageInfo.Name = "xrPageInfo";
            this.reportFooterBand1.HeightF = 22;
            this.reportFooterBand1.Controls.AddRange(new XRControl[] { this.xrPageInfo });

            // ── Page setup ────────────────────────────────────────────────
            this.Bands.AddRange(new Band[] {
                this.topMarginBand1, this.reportHeaderBand1, this.pageHeaderBand1,
                this.detailBand1, this.reportFooterBand1, this.bottomMarginBand1 });
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] { this.objectDataSource1 });
            this.DataSource = this.objectDataSource1;
            this.DataMember = "Rows";
            this.Margins = new System.Drawing.Printing.Margins(40, 40, 40, 40);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Letter;
            this.Landscape = true;
            this.Version = "25.2";
        }

        private static void ConfigureCaption(XRLabel lbl, string text, int x, int width,
            TextAlignment alignment = TextAlignment.MiddleLeft)
        {
            lbl.Text = text;
            lbl.Font = new Font("Tahoma", 8F, FontStyle.Bold);
            lbl.LocationFloat = new PointFloat(x, 0);
            lbl.SizeF = new SizeF(width, 20);
            lbl.Borders = BorderSide.Bottom;
            lbl.TextAlignment = alignment;
        }

        // expression is a full XtraReports criteria expression — caller provides
        // the brackets (e.g. "[WeightSheetId]") and any FormatString wrapping.
        private static void ConfigureField(XRLabel lbl, string expression, int x, int width,
            TextAlignment alignment = TextAlignment.MiddleLeft)
        {
            lbl.Font = new Font("Tahoma", 8F);
            lbl.LocationFloat = new PointFloat(x, 0);
            lbl.SizeF = new SizeF(width, 20);
            lbl.TextAlignment = alignment;
            lbl.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", expression));
        }
    }
}
