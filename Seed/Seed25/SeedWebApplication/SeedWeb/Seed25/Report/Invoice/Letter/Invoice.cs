using DevExpress.XtraPrinting.BarCode;
using DevExpress.XtraReports.UI;
using Seed25.DTO;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace Seed25.Report
{
    public partial class Invoice : DevExpress.XtraReports.UI.XtraReport
    {
        public Invoice()
        {
            InitializeComponent();
        }


        public Invoice(InvoiceDTO data) : this()
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
                this.xrVarietiesSubReport.ReportSource = new InvoiceVarietySubReport();
            }


            this.xrVarietiesSubReport.BeforePrint += (sender, e) =>
            {
                var current = this.GetCurrentRow() as InvoiceDTO;
                if (current == null) return;


                var sub = (XRSubreport)sender;
                var subReport = (InvoiceVarietySubReport)sub.ReportSource;


                subReport.DataSource = current.InvoiceVarietyDTOs;
                subReport.DataMember = null;

            };


            if (this.xrTreatmentSubReport?.ReportSource == null)
            {
                this.xrTreatmentSubReport.ReportSource = new InvoiceTreatmentSubReport();
            }


            this.xrTreatmentSubReport.BeforePrint += (sender, e) =>
            {
                var current = this.GetCurrentRow() as InvoiceDTO;
                if (current == null) return;


                var sub = (XRSubreport)sender;
                var subReport = (InvoiceTreatmentSubReport)sub.ReportSource;

                subReport.DataSource = current.invoiceTreatmentDTOs;
                subReport.DataMember = null;

            };



            if (this.xrMiscSubReport?.ReportSource == null)
            {
                this.xrMiscSubReport.ReportSource = new InvoiceMiscSubReport();
            }


            this.xrMiscSubReport.BeforePrint += (sender, e) =>
            {
                var current = this.GetCurrentRow() as InvoiceDTO;
                if (current == null) return;


                var sub = (XRSubreport)sender;
                var subReport = (InvoiceMiscSubReport)sub.ReportSource;

                subReport.DataSource = current.invoiceMiscDTOs;
                subReport.DataMember = null;

            };



            if (this.xrAnalysisSubreport?.ReportSource == null)
            {
                this.xrAnalysisSubreport.ReportSource = new InvoiceAnalysisSubReport();
            }


            this.xrAnalysisSubreport.BeforePrint += (sender, e) =>
            {
                var current = this.GetCurrentRow() as InvoiceDTO;
                if (current == null) return;


                var sub = (XRSubreport)sender;
                var subReport = (InvoiceAnalysisSubReport)sub.ReportSource;

                subReport.DataSource = current.invoiceAnalysisDTOs;
                subReport.DataMember = null;

            };



            if (data.Type == "TRUCK")
            {

                if (this.xrWeightSubReport?.ReportSource == null)
                {
                    this.xrWeightSubReport.ReportSource = new InvoiceTruckWeightSubReport();
                }


                this.xrWeightSubReport.BeforePrint += (sender, e) =>
                {
                    var current = this.GetCurrentRow() as InvoiceDTO;
                    if (current == null) return;


                    var sub = (XRSubreport)sender;
                    var subReport = (InvoiceTruckWeightSubReport)sub.ReportSource;

                    subReport.DataSource = current.invoiceWeightDTOs;
                    subReport.DataMember = null;

                };
            }
            else if (data.Type == "TOTE")
            {
                if (this.xrWeightSubReport?.ReportSource == null)
                {
                    this.xrWeightSubReport.ReportSource = new InvoiceToteWeightsSubReport();
                }


                this.xrWeightSubReport.BeforePrint += (sender, e) =>
                {
                    var current = this.GetCurrentRow() as InvoiceDTO;
                    if (current == null) return;


                    var sub = (XRSubreport)sender;
                    var subReport = (InvoiceToteWeightsSubReport)sub.ReportSource;

                    subReport.DataSource = current.invoiceWeightDTOs;
                    subReport.DataMember = null;

                };
            }
            else if (data.Type == "BAG")
            {
                if (this.xrWeightSubReport?.ReportSource == null)
                {
                    this.xrWeightSubReport.ReportSource = new InvoiceBagWeightsSubReport();
                }


                this.xrWeightSubReport.BeforePrint += (sender, e) =>
                {
                    var current = this.GetCurrentRow() as InvoiceDTO;
                    if (current == null) return;


                    var sub = (XRSubreport)sender;
                    var subReport = (InvoiceBagWeightsSubReport)sub.ReportSource;

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
