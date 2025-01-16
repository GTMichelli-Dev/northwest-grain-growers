using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for pdfGenerator
/// </summary>
public class pdfGenerator
{
    public pdfGenerator()
    {
        //
        // TODO: Add constructor logic here
        //
    }



    public static MemoryStream GetIntakeWeightSheet(Guid Weight_Sheet_UID,bool ShowAsOriginalPrinted, HttpContext context)
    {


        MemoryStream stream = new MemoryStream();


        try
        {



            using (ReportDocument rptWeigh_Sheet = new ReportDocument())
            {


                using (NWDataSet NWDataset = new NWDataSet())
                {

                    rptWeigh_Sheet.Load(context.Server.MapPath("Weigh_Sheet.rpt"));
                    {
                        bool Closed = true;

                        using (NWDataSetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new NWDataSetTableAdapters.vwWeigh_SheetTableAdapter())
                        {
                            vwWeigh_SheetTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwWeigh_Sheet, Weight_Sheet_UID);
                            {
                                using (NWDataSetTableAdapters.LocationsTableAdapter locationsTableAdapter = new NWDataSetTableAdapters.LocationsTableAdapter())
                                {
                                    if (NWDataset.vwWeigh_Sheet.Count > 0)
                                    {
                                        locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.vwWeigh_Sheet[0].Location_Id);
                                        Closed = NWDataset.vwWeigh_Sheet[0].Closed;
                                        NWDataset.vwWeigh_Sheet[0].Original_Printed = !ShowAsOriginalPrinted;
                                    }
                                    else
                                    {
                                        using (NWDataSetTableAdapters.Weight_SheetsTableAdapter weight_SheetsTableAdapter = new NWDataSetTableAdapters.Weight_SheetsTableAdapter())
                                        {
                                            if (weight_SheetsTableAdapter.Fill(NWDataset.Weight_Sheets, Weight_Sheet_UID) > 0)
                                            {
                                                locationsTableAdapter.FillById(NWDataset.Locations, NWDataset.Weight_Sheets[0].Location_Id);
                                                Closed = NWDataset.Weight_Sheets[0].Closed;

                                            }
                                        }
                                    }
                                }
                                using (NWDataSetTableAdapters.vwWeight_Sheet_InformationTableAdapter vwWeight_Sheet_InformationTableAdapter = new NWDataSetTableAdapters.vwWeight_Sheet_InformationTableAdapter())
                                {
                                    if (vwWeight_Sheet_InformationTableAdapter.FillByUID(NWDataset.vwWeight_Sheet_Information, Weight_Sheet_UID) > 0)
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

                                            rptWeigh_Sheet.SetParameterValue("Unofficial", !ShowAsOriginalPrinted);
                                            rptWeigh_Sheet.SetParameterValue("Customer_Copy", false);
                                            rptWeigh_Sheet.SetParameterValue("Lot", NWDataset.vwWeight_Sheet_Information[0].Lot_Number);
                                            rptWeigh_Sheet.SetParameterValue("StrWS_Id", NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0'));

                                            //var parameterRow = NWDataset.ReportParameters.NewReportParametersRow();
                                            //parameterRow.WSUID = Weight_Sheet_UID;
                                            //parameterRow.CustomRate = Custom;

                                            //parameterRow.Closed = Closed;
                                            //parameterRow.End_Of_Lot = NWDataset.vwWeight_Sheet_Information[0].Is_End_Lot;
                                            //parameterRow.New_Lot = NWDataset.vwWeight_Sheet_Information[0].Is_New_Lot;
                                            //parameterRow.Total_Billed = TotalBilled;

                                            //parameterRow.Location_Description = NWDataset.vwWeight_Sheet_Information[0].Location_Description;
                                            //parameterRow.Creation_Date = NWDataset.vwWeight_Sheet_Information[0].Creation_Date;
                                            //// LOOOOOOOOK
                                            //parameterRow.Unofficial = !ShowAsOriginalPrinted;
                                            //parameterRow.Customer_Copy = false;
                                            //parameterRow.Lot = NWDataset.vwWeight_Sheet_Information[0].Lot_Number;
                                            //parameterRow.StrWS_Id = NWDataset.vwWeight_Sheet_Information[0].WS_Id.ToString().PadLeft(8, '0');
                                            //NWDataset.ReportParameters.AddReportParametersRow(parameterRow);

                                          






                                            // NWDataset.vwWeight_Sheet_Information[0].Original_Printed = !Official;



                                            //

                                            ExportOptions exportOpts = new ExportOptions();
                                            exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;

                                            EditableRTFExportFormatOptions exportFormatOptions =
                                              ExportOptions.CreateEditableRTFExportFormatOptions();
                                            exportFormatOptions.FirstPageNumber = 1;
                                            exportFormatOptions.InsertPageBreaks = true;
                                            exportFormatOptions.LastPageNumber = 1;
                                            exportFormatOptions.UsePageRange = true;
                                            exportOpts.ExportFormatOptions = exportFormatOptions;


                                            //rptWeigh_Sheet.ExportToHttpResponse(exportOpts, Response, true, string.Format("WeightSheet_{0}", NWDataset.vwWeight_Sheet_Information[0].WS_Id));



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


    public static void DownloadIntakeWeightSheet( Guid Weight_Sheet_UID, HttpContext context)
    {

        var stream = GetIntakeWeightSheet(Weight_Sheet_UID, false, context);

        
        using (NWDataSetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new NWDataSetTableAdapters.vwWeigh_SheetTableAdapter())
        {
            using (NWDataSet NWDataset = new NWDataSet())
            {
                if (vwWeigh_SheetTableAdapter.FillByWeight_Sheet_UID(NWDataset.vwWeigh_Sheet, Weight_Sheet_UID)>0)
                {
                    var filename = $"Weight Sheet{ NWDataset.vwWeigh_Sheet[0].WS_Id}.pdf";
                    DownloadPdf(stream, filename);
                }
            }
        }
    }


    public static void DownloadPdf(Stream stream, string fileName)
    {
        HttpContext context = HttpContext.Current;

        // Clear any existing content from the response
        context.Response.Clear();

        // Set the content type and headers for the response
        context.Response.ContentType = "application/pdf";
        context.Response.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");

        // Copy the stream content to the response output stream
        stream.CopyTo(context.Response.OutputStream);

        // End the response to stop any further processing
        context.Response.End();
    }
}