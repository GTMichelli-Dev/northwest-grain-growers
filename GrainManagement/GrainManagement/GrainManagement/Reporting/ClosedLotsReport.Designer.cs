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

    partial class ClosedLotsReport
    {
        // Designer-friendly skeleton — bound to ClosedLotsReportDto. The
        // Detail band iterates over Rows. Open in the DevExpress XtraReports
        // designer to refine layout.
        private IContainer components = null;

        private TopMarginBand topMarginBand1;
        private ReportHeaderBand reportHeaderBand1;
        private PageHeaderBand pageHeaderBand1;
        private DetailBand detailBand1;
        private ReportFooterBand reportFooterBand1;
        private BottomMarginBand bottomMarginBand1;

        private ObjectDataSource objectDataSource1;

        private XRLabel xrTitle;
        private XRLabel xrLocation;
        private XRLabel xrDateRange;

        private XRLabel capLot;
        private XRLabel capClosed;
        private XRLabel capCrop;
        private XRLabel capProducer;

        private XRLabel xrLot;
        private XRLabel xrClosed;
        private XRLabel xrCrop;
        private XRLabel xrProducer;

        private XRLabel xrTotalLabel;
        private XRLabel xrTotalCount;

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

            this.xrTitle      = new XRLabel();
            this.xrLocation   = new XRLabel();
            this.xrDateRange  = new XRLabel();

            this.capLot       = new XRLabel();
            this.capClosed    = new XRLabel();
            this.capCrop      = new XRLabel();
            this.capProducer  = new XRLabel();

            this.xrLot        = new XRLabel();
            this.xrClosed     = new XRLabel();
            this.xrCrop       = new XRLabel();
            this.xrProducer   = new XRLabel();

            this.xrTotalLabel = new XRLabel();
            this.xrTotalCount = new XRLabel();

            this.objectDataSource1.DataSource = typeof(global::GrainManagement.Dtos.Warehouse.ClosedLotsReportDto);
            this.objectDataSource1.Name = "objectDataSource1";

            // ── Title ───────────────────────────────────────────────────────
            this.xrTitle.Font = new Font("Tahoma", 14F, FontStyle.Bold);
            this.xrTitle.LocationFloat = new PointFloat(0, 0);
            this.xrTitle.SizeF = new SizeF(720, 26);
            this.xrTitle.TextAlignment = TextAlignment.MiddleCenter;
            this.xrTitle.Text = "Closed Lots";
            this.xrTitle.Name = "xrTitle";

            this.xrLocation.LocationFloat = new PointFloat(0, 28);
            this.xrLocation.SizeF = new SizeF(720, 22);
            this.xrLocation.TextAlignment = TextAlignment.MiddleCenter;
            this.xrLocation.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            this.xrLocation.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[LocationHeader]"));
            this.xrLocation.Name = "xrLocation";

            this.xrDateRange.LocationFloat = new PointFloat(0, 52);
            this.xrDateRange.SizeF = new SizeF(720, 20);
            this.xrDateRange.TextAlignment = TextAlignment.MiddleCenter;
            this.xrDateRange.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[DateRangeHeader]"));
            this.xrDateRange.Name = "xrDateRange";

            this.reportHeaderBand1.HeightF = 78;
            this.reportHeaderBand1.Controls.AddRange(new XRControl[] {
                this.xrTitle, this.xrLocation, this.xrDateRange });

            // ── Page header ────────────────────────────────────────────────
            ConfigureCaption(this.capLot,      "Lot",      0,    140);
            ConfigureCaption(this.capClosed,   "Closed",   140,  120);
            ConfigureCaption(this.capCrop,     "Crop",     260,  220);
            ConfigureCaption(this.capProducer, "Producer", 480,  240);

            this.pageHeaderBand1.HeightF = 22;
            this.pageHeaderBand1.Controls.AddRange(new XRControl[] {
                this.capLot, this.capClosed, this.capCrop, this.capProducer });

            // ── Detail band ────────────────────────────────────────────────
            this.detailBand1.HeightF = 22;
            ConfigureField(this.xrLot,      "LotNumber", 0,    140);
            ConfigureField(this.xrClosed,   "CloseDate", 140,  120);
            ConfigureField(this.xrCrop,     "Crop",      260,  220);
            ConfigureField(this.xrProducer, "Producer",  480,  240);
            this.detailBand1.Controls.AddRange(new XRControl[] {
                this.xrLot, this.xrClosed, this.xrCrop, this.xrProducer });

            // ── Footer (total count) ──────────────────────────────────────
            this.xrTotalLabel.LocationFloat = new PointFloat(0, 4);
            this.xrTotalLabel.SizeF = new SizeF(180, 20);
            this.xrTotalLabel.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            this.xrTotalLabel.Text = "Total # Closed Lots:";

            this.xrTotalCount.LocationFloat = new PointFloat(180, 4);
            this.xrTotalCount.SizeF = new SizeF(80, 20);
            this.xrTotalCount.Font = new Font("Tahoma", 9F, FontStyle.Bold);
            this.xrTotalCount.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[TotalCount]"));

            this.reportFooterBand1.HeightF = 32;
            this.reportFooterBand1.Controls.AddRange(new XRControl[] {
                this.xrTotalLabel, this.xrTotalCount });

            // ── Page setup ────────────────────────────────────────────────
            this.Bands.AddRange(new Band[] {
                this.topMarginBand1, this.reportHeaderBand1, this.pageHeaderBand1,
                this.detailBand1, this.reportFooterBand1, this.bottomMarginBand1 });
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] { this.objectDataSource1 });
            this.DataSource = this.objectDataSource1;
            this.DataMember = "Rows";
            this.Margins = new System.Drawing.Printing.Margins(40, 40, 40, 40);
            this.PageHeight = 1100;
            this.PageWidth = 850;
            this.PaperKind = DevExpress.Drawing.Printing.DXPaperKind.Letter;
            this.Version = "25.2";
        }

        private static void ConfigureCaption(XRLabel lbl, string text, int x, int width)
        {
            lbl.Text = text;
            lbl.Font = new Font("Tahoma", 8F, FontStyle.Bold);
            lbl.LocationFloat = new PointFloat(x, 0);
            lbl.SizeF = new SizeF(width, 20);
            lbl.Borders = BorderSide.Bottom;
        }

        private static void ConfigureField(XRLabel lbl, string fieldExpression, int x, int width)
        {
            lbl.Font = new Font("Tahoma", 8F);
            lbl.LocationFloat = new PointFloat(x, 0);
            lbl.SizeF = new SizeF(width, 20);
            lbl.ExpressionBindings.Add(new ExpressionBinding("BeforePrint", "Text", "[" + fieldExpression + "]"));
        }
    }
}
