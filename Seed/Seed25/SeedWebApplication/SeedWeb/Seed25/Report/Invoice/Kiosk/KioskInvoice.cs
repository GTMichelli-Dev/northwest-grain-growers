using DevExpress.XtraReports.UI;
using Seed25.DTO;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using DevExpress.XtraPrinting.BarCode;

namespace Seed25.Report
{
    public partial class KioskInvoice : DevExpress.XtraReports.UI.XtraReport
    {
        public KioskInvoice()
        {
            InitializeComponent();
        }


        public KioskInvoice(InvoiceDTO data) : this()
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            if (xrBarCode1.Symbology is QRCodeGenerator gen)
            {
                gen.CompactionMode = QRCodeCompactionMode.Byte; // <- key line
                gen.Version = QRCodeVersion.AutoVersion;
                gen.ErrorCorrectionLevel = QRCodeErrorCorrectionLevel.Q;
            }
            xrBarCode1.Text = "https://inetsgi.com/customer/780/f32d320d.pdf";

            this.DataSource = new List<InvoiceDTO> { data };
            this.DataMember = string.Empty;
            if (data.Clearfield)
            {
                this.WatermarkId = "Clearfield";
            }
            else if (data.Coaxium)
            {
                this.WatermarkId = "Coaxium";
            }
            else
            {
                this.Watermarks.Clear();
            }

            // Ensure the subreport has a report instance
            if (this.xrVarietiesSubReport?.ReportSource == null)
            {
                this.xrVarietiesSubReport.ReportSource = new KioskInvoiceVarietySubReport();
            }


            this.xrVarietiesSubReport.BeforePrint += (sender, e) =>
            {
                var current = this.GetCurrentRow() as InvoiceDTO;
                if (current == null) return;


                var sub = (XRSubreport)sender;
                var subReport = (KioskInvoiceVarietySubReport)sub.ReportSource;


                subReport.DataSource = current.InvoiceVarietyDTOs;
                subReport.DataMember = null;

            };


            if (this.xrTreatmentSubReport?.ReportSource == null)
            {
                this.xrTreatmentSubReport.ReportSource = new KioskInvoiceTreatmentSubReport();
            }


            this.xrTreatmentSubReport.BeforePrint += (sender, e) =>
            {
                var current = this.GetCurrentRow() as InvoiceDTO;
                if (current == null) return;


                var sub = (XRSubreport)sender;
                var subReport = (KioskInvoiceTreatmentSubReport)sub.ReportSource;

                subReport.DataSource = current.invoiceTreatmentDTOs;
                subReport.DataMember = null;

            };



            if (this.xrMiscSubReport?.ReportSource == null)
            {
                this.xrMiscSubReport.ReportSource = new KioskInvoiceMiscSubReport();
            }


            this.xrMiscSubReport.BeforePrint += (sender, e) =>
            {
                var current = this.GetCurrentRow() as InvoiceDTO;
                if (current == null) return;


                var sub = (XRSubreport)sender;
                var subReport = (KioskInvoiceMiscSubReport)sub.ReportSource;

                subReport.DataSource = current.invoiceMiscDTOs;
                subReport.DataMember = null;

            };



            if (this.xrAnalysisSubreport?.ReportSource == null)
            {
                this.xrAnalysisSubreport.ReportSource = new KioskInvoiceAnalysisSubReport();
            }


            this.xrAnalysisSubreport.BeforePrint += (sender, e) =>
            {
                var current = this.GetCurrentRow() as InvoiceDTO;
                if (current == null) return;


                var sub = (XRSubreport)sender;
                var subReport = (KioskInvoiceAnalysisSubReport)sub.ReportSource;

                subReport.DataSource = current.invoiceAnalysisDTOs;
                subReport.DataMember = null;

            };



            if (data.Type == "TRUCK")
            {

                if (this.xrWeightSubReport?.ReportSource == null)
                {
                    this.xrWeightSubReport.ReportSource = new KioskInvoiceTruckWeightSubReport();
                }


                this.xrWeightSubReport.BeforePrint += (sender, e) =>
                {
                    var current = this.GetCurrentRow() as InvoiceDTO;
                    if (current == null) return;


                    var sub = (XRSubreport)sender;
                    var subReport = (KioskInvoiceTruckWeightSubReport)sub.ReportSource;

                    subReport.DataSource = current.invoiceWeightDTOs;
                    subReport.DataMember = null;

                };
            }
            else if (data.Type == "TOTE")
            {
                if (this.xrWeightSubReport?.ReportSource == null)
                {
                    this.xrWeightSubReport.ReportSource = new KioskInvoiceToteWeightsSubReport();
                }


                this.xrWeightSubReport.BeforePrint += (sender, e) =>
                {
                    var current = this.GetCurrentRow() as InvoiceDTO;
                    if (current == null) return;


                    var sub = (XRSubreport)sender;
                    var subReport = (KioskInvoiceToteWeightsSubReport)sub.ReportSource;

                    subReport.DataSource = current.invoiceWeightDTOs;
                    subReport.DataMember = null;

                };
            }
            else if (data.Type == "BAG")
            {
                if (this.xrWeightSubReport?.ReportSource == null)
                {
                    this.xrWeightSubReport.ReportSource = new KioskInvoiceBagWeightsSubReport();
                }


                this.xrWeightSubReport.BeforePrint += (sender, e) =>
                {
                    var current = this.GetCurrentRow() as InvoiceDTO;
                    if (current == null) return;


                    var sub = (XRSubreport)sender;
                    var subReport = (KioskInvoiceBagWeightsSubReport)sub.ReportSource;

                    subReport.DataSource = current.invoiceBagsDTOs;
                    subReport.DataMember = null;

                };
            }
            else
            {

            }
        }
    }

}
