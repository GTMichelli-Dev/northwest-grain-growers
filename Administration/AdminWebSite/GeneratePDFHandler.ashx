<%@ WebHandler Language="C#" Class="GeneratePDFHandler" %>

using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
    using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Linq;

using System.Web.UI;
using System.Web.UI.WebControls;
public class GeneratePDFHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        try { 
            
        Guid[] uidList = (Guid[])context.Session["UIDList"];
        if (uidList != null && uidList.Length > 0)
        {
            PdfDocument outputDocument = new PdfDocument();

            List<MemoryStream> pdfStreams = new List<MemoryStream>();

            foreach (var uid in uidList)
            {
                pdfStreams.Add(GetIntakeWeightSheet(uid));
            }

            foreach (var pdfStream in pdfStreams)
            {
                pdfStream.Position = 0; // Reset stream position for reading
                PdfDocument inputDocument = PdfReader.Open(pdfStream, PdfDocumentOpenMode.Import);

                // Import pages from each report PDF and add them to the output document
                for (int i = 0; i < inputDocument.PageCount; i++)
                {
                    outputDocument.AddPage(inputDocument.Pages[i]);
                }
            }

            // Save combined PDF to a memory stream
            using (MemoryStream combinedPDF = new MemoryStream())
            {
                outputDocument.Save(combinedPDF, false);
                combinedPDF.Position = 0; // Reset stream position for reading

                // Send the combined PDF as an HTTP response
                context.Response.Clear();
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("Content-Disposition", "inline; filename=IntakeCombinedReport.pdf");
                context.Response.AddHeader("Content-Length", combinedPDF.Length.ToString());
                context.Response.OutputStream.Write(combinedPDF.ToArray(), 0, combinedPDF.ToArray().Length);
                context.Response.Flush();
                context.Response.End();
            }
        }
        }
            catch
        {

        }
    }

    public bool IsReusable
    {
        get { return false; }
    }

    private MemoryStream GetIntakeWeightSheet(Guid uid)
    {
   MemoryStream stream = new MemoryStream();


        try
        {



            {


                using (NWDataSet NWDataset = new NWDataSet())
                {
                    var rptWeigh_Sheet = new ReportDocument();
                        rptWeigh_Sheet.Load(HttpContext.Current.Server.MapPath("~/Reports/Weigh_Sheet.rpt")); // Adjusted path

                           {
                        bool Closed = true;

                        using (NWDataSetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new NWDataSetTableAdapters.vwWeigh_SheetTableAdapter())
                        {
                            vwWeigh_SheetTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwWeigh_Sheet, uid);
                            {
                                using (NWDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDataSetTableAdapters.LocationsTableAdapter())
                                {
                                    if (NWDataset.vwWeigh_Sheet.Count > 0)
                                    {
                                        locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.vwWeigh_Sheet[0].Location_Id);
                                        Closed = NWDataset.vwWeigh_Sheet[0].Closed;
                                       
                                    }
                                    else
                                    {
                                        using (NWDataSetTableAdapters.Weight_SheetsTableAdapter weight_SheetsTableAdapter = new NWDataSetTableAdapters.Weight_SheetsTableAdapter())
                                        {
                                            if (weight_SheetsTableAdapter.Fill(NWDataset.Weight_Sheets, uid) > 0)
                                            {
                                                locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.Weight_Sheets[0].Location_Id);
                                                Closed = NWDataset.Weight_Sheets[0].Closed;

                                            }
                                        }
                                    }
                                }
                                using (NWDataSetTableAdapters.vwWeight_Sheet_InformationTableAdapter vwWeight_Sheet_InformationTableAdapter = new NWDataSetTableAdapters.vwWeight_Sheet_InformationTableAdapter())
                                {
                                    if (vwWeight_Sheet_InformationTableAdapter.FillByUID(NWDataset.vwWeight_Sheet_Information, uid) > 0)
                                    {

                                        {
                                            rptWeigh_Sheet.SetDataSource(NWDataset);
                                            rptWeigh_Sheet.Subreports[0].SetDataSource(NWDataset);
                                            decimal TotalBilled = 0;
                                            if (!NWDataset.vwWeight_Sheet_Information[0].IsTotal_BilledNull())
                                            {
                                                TotalBilled = NWDataset.vwWeight_Sheet_Information[0].Total_Billed;
                                            }

                                            var Custom = (!NWDataset.vwWeight_Sheet_Information[0].IsBOL_TypeNull() && NWDataset.vwWeight_Sheet_Information[0].BOL_Type.ToUpper().Trim() == "C");

                                            rptWeigh_Sheet.SetParameterValue("CustomRate", Custom);

                                            rptWeigh_Sheet.SetParameterValue("Closed", Closed);
                                            rptWeigh_Sheet.SetParameterValue("End_Of_Lot", NWDataset.vwWeight_Sheet_Information[0].Is_End_Lot);
                                            rptWeigh_Sheet.SetParameterValue("New_Lot", NWDataset.vwWeight_Sheet_Information[0].Is_New_Lot);
                                            rptWeigh_Sheet.SetParameterValue("Total_Billed", TotalBilled);

                                            rptWeigh_Sheet.SetParameterValue("Location_Description", NWDataset.vwWeight_Sheet_Information[0].Location_Description);
                                            rptWeigh_Sheet.SetParameterValue("Creation_Date", NWDataset.vwWeight_Sheet_Information[0].Creation_Date);

                                            rptWeigh_Sheet.SetParameterValue("Unofficial", true);
                                            rptWeigh_Sheet.SetParameterValue("Customer_Copy", false);
                                            rptWeigh_Sheet.SetParameterValue("Lot", NWDataset.vwWeight_Sheet_Information[0].Lot_Number);
                                            rptWeigh_Sheet.SetParameterValue("StrWS_Id", NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));
                                            ExportOptions exportOpts = new ExportOptions();
                                            exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                            EditableRTFExportFormatOptions exportFormatOptions =
                                              ExportOptions.CreateEditableRTFExportFormatOptions();
                                            exportFormatOptions.FirstPageNumber = 1;
                                            exportFormatOptions.InsertPageBreaks = true;
                                            exportFormatOptions.LastPageNumber = 1;
                                            exportFormatOptions.UsePageRange = true;
                                            exportOpts.ExportFormatOptions = exportFormatOptions;





                                            ExportRequestContext req = new ExportRequestContext();

                                            req.ExportInfo = exportOpts;


                                            Stream s = rptWeigh_Sheet.FormatEngine.ExportToStream(req);
                                            s.CopyTo(stream);




                                        }
                                    }

                                }

                            }
                        }
                    }
                }

            }
       
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.Print(ex.Message);
            
        }
        return stream;
    }
}
