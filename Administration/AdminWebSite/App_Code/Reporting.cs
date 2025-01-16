using ClosedXML.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;


/// <summary>
/// Summary description for Reporting
/// </summary>
public class Reporting
{
    public Reporting()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static void DownloadProducersCommodityDeliveredReport(string District,string Commodity, string Producer,string Landlord, DateTime StartDate, DateTime EndDate,int? Location, int? VarietyID, HttpResponse Response)
    {
        try
        {
            MemoryStream ms = CreateProducersCommodityDeliveredReport(District,Commodity, Producer, Landlord,StartDate, EndDate,Location,VarietyID );
            if (ms.Length > 0)
            {
                DownloadExcellFile(ms, "ProducersCommodityDeliveredReport_" + GetDateForFileName(StartDate) + "_" + GetDateForFileName(EndDate) + ".xlsx", Response);
            }
        }
        catch
        {

        }
    }


    public static void DownloadLoadsByCropAndDate(int? LocationId, DateTime StartDate,DateTime EndDate, HttpResponse Response)
    {
        try
        {
            MemoryStream ms = CreateLoadsByCropAndDateReport(LocationId,StartDate,EndDate);
            if (ms.Length > 0)
            {
                DownloadExcellFile(ms, "LoadsByCropAndDateReport_" + GetDateForFileName(StartDate) + "_" + GetDateForFileName(EndDate) + ".xlsx", Response);
            }
        }
        catch
        {

        }
    }


    public static void DownloadProducersDeliveredReport(string District,string Producer,DateTime StartDate ,DateTime EndDate,HttpResponse  Response  )
    {
        try
        {
            MemoryStream ms = CreateProducersDeliveredReport( District,  Producer,  StartDate,  EndDate);
            if (ms.Length > 0)
            {
                DownloadExcellFile(ms, "ProducersDeliveredReport_" + GetDateForFileName(StartDate) + "_"+ GetDateForFileName(EndDate) + ".xlsx", Response);
            }
        }
        catch
        {

        }
    }



    public static void DownloadLocationIntakeReport( DateTime StartDate, DateTime EndDate, HttpResponse Response)
    {
        try
        {
            MemoryStream ms = CreateLocationIntakeReport(StartDate, EndDate);
            if (ms.Length > 0)
            {
                DownloadExcellFile(ms, "LocationIntakeReport_" + GetDateForFileName(StartDate) + "_" + GetDateForFileName(EndDate) + ".xlsx", Response);
            }
        }
        catch
        {

        }
    }




    private static MemoryStream CreateLocationIntakeReport(DateTime StartDate, DateTime EndDate)
    {
        MemoryStream stream = new MemoryStream();
        try
        {

            using (ReportDataSetTableAdapters.LoadsByCommodityTableAdapter  loadsByCommodityTableAdapter = new ReportDataSetTableAdapters.LoadsByCommodityTableAdapter())

            {

                using (ReportDataSet reportDataSet = new ReportDataSet())
                {

                    if (loadsByCommodityTableAdapter.Fill(reportDataSet.LoadsByCommodity ,  StartDate, EndDate) > 0)
                    {





                        int Row;
                        int FreezeRow;


                        XLWorkbook workbook = new XLWorkbook();


                        #region Report


                        IXLWorksheet worksheet = workbook.Worksheets.Add("Summary");



                        worksheet.Range("A1:E1").Merge();
                        worksheet.Range("A1:E1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A1:E1").Style.Font.FontSize = 14;
                        worksheet.Range("A1:E1").Style.Font.Bold = true;
                        worksheet.Range("A1:E1").Value = "LOCATION INTAKE REPORT SUMMARY";

                        worksheet.Range("A2:E2").Merge();
                        worksheet.Range("A2:E2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A2:E2").Style.Font.FontSize = 12;
                        worksheet.Range("A2:E2").Style.Font.Bold = false;
                        worksheet.Range("A2:E2").Value = "FROM " + StartDate.ToShortDateString() + " TO " + EndDate.ToShortDateString();




                        worksheet.Cell("A3").Value = "COMMODITY";
                        worksheet.Cell("B3").Value = "UOM";
                        worksheet.Cell("C3").Value = "AMOUNT";
                      




                        Row = 4;
                        FreezeRow = 3;

                   
                      
                        

                        foreach (var loadRow in reportDataSet.LoadsByCommodity )
                        {
                      

                            worksheet.SheetView.FreezeRows(FreezeRow);
                            worksheet.Cell(Row, 1).Value = loadRow.Commodity ;
                            worksheet.Cell(Row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 2).Value = loadRow.UOM ;
                            worksheet.Cell(Row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 3).Value = loadRow.Net_UOM ;
                            worksheet.Cell(Row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(Row,3).Style.NumberFormat.Format = "#,##0.00";

                            Row += 1;
                        }

                        worksheet.Columns().AdjustToContents();






                        using (ReportDataSetTableAdapters.LoadsByCommodityLocationTableAdapter loadsByCommodityLocationTableAdapte = new ReportDataSetTableAdapters.LoadsByCommodityLocationTableAdapter() )
                        {
                            loadsByCommodityLocationTableAdapte.Fill(reportDataSet.LoadsByCommodityLocation, StartDate, EndDate);
                        }

                        string Location = "";
                        string CurrentLocation = "";

                        worksheet = workbook.Worksheets.Add("Location");
                        worksheet.Range("A1:E1").Merge();
                        worksheet.Range("A1:E1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A1:E1").Style.Font.FontSize = 14;
                        worksheet.Range("A1:E1").Style.Font.Bold = true;
                        worksheet.Range("A1:E1").Value = "LOCATION INTAKE REPORT BY LOCATION";

                        worksheet.Range("A2:E2").Merge();
                        worksheet.Range("A2:E2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A2:E2").Style.Font.FontSize = 12;
                        worksheet.Range("A2:E2").Style.Font.Bold = false;
                        worksheet.Range("A2:E2").Value = "FROM " + StartDate.ToShortDateString() + " TO " + EndDate.ToShortDateString();



                        worksheet.Cell("A3").Value = "LOCATION";
                        worksheet.Cell("B3").Value = "COMMODITY";
                        worksheet.Cell("C3").Value = "UOM";
                        worksheet.Cell("D3").Value = "AMOUNT";


                        Row = 4;
                        foreach (ReportDataSet.LoadsByCommodityLocationRow  lCrow in reportDataSet.LoadsByCommodityLocation )
                        {
                            if (string.IsNullOrWhiteSpace(Location )|| Location != lCrow.Location )
                            {
                                if (!string.IsNullOrWhiteSpace(Location )) Row += 2;
                                Location = lCrow.Location;
                                CurrentLocation = lCrow.Location;


                            }
                            worksheet.SheetView.FreezeRows(FreezeRow);
                            worksheet.Cell(Row, 1).Value = CurrentLocation ;
                            CurrentLocation = "";
                            worksheet.Cell(Row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 2).Value = lCrow.Commodity ;
                            worksheet.Cell(Row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 3).Value = lCrow.UOM ;
                            worksheet.Cell(Row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 4).Value = lCrow.Net_UOM ;
                            worksheet.Cell(Row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(Row, 4).Style.NumberFormat.Format = "#,##0.00";


                            Row += 1;
                        }

                        worksheet.Columns().AdjustToContents();







                        #endregion

                        workbook.SaveAs(stream);
                    }
                }
            }

        }

        catch (Exception ex)
        {

            //  Logging.Log_Event("CreateRailCarReport", ex.Message);
        }

        return stream;



    }



    private static MemoryStream CreateProducersCommodityDeliveredReport(string District, string Commodity ,string Producer,string Landlord, DateTime StartDate, DateTime EndDate, int? Location, int? VarietyID)
    {
        MemoryStream stream = new MemoryStream();
        try
        {

            using (ReportDataSetTableAdapters.ProducersDeliveredCommodityTableAdapter  producersDeliveredCommodityTableAdapter = new ReportDataSetTableAdapters.ProducersDeliveredCommodityTableAdapter())

            {

                using (ReportDataSet reportDataSet = new ReportDataSet())
                {

                    if (producersDeliveredCommodityTableAdapter.Fill(
                        reportDataSet.ProducersDeliveredCommodity,
                        (string.IsNullOrEmpty(Landlord) ? null : Landlord), 
                        (string.IsNullOrEmpty(District) ? null : District),
                        (string.IsNullOrEmpty(Commodity) ? null : Commodity),
                        StartDate,
                        EndDate,
                        (string.IsNullOrEmpty(Producer) ? null : Producer),
                        (Location == null)?null:Location,
                        (VarietyID == null)?null:VarietyID ) > 0)
                    {





                        int Row;
                        int FreezeRow;


                        XLWorkbook workbook = new XLWorkbook();


                        #region Report





                        var worksheet = workbook.Worksheets.Add("Report");




                        worksheet.Range("A1:I1").Merge();
                        worksheet.Range("A1:I1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A1:I1").Style.Font.FontSize = 14;
                        worksheet.Range("A1:I1").Style.Font.Bold = true;
                        worksheet.Range("A1:I1").Value = "PRODUCER COMMODITY DELIVERY REPORT";

                        worksheet.Range("A2:I2").Merge();
                        worksheet.Range("A2:I2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A2:I2").Style.Font.FontSize = 12;
                        worksheet.Range("A2:I2").Style.Font.Bold = false;
                        worksheet.Range("A2:I2").Value = "FROM " + StartDate.ToShortDateString() + " TO " + EndDate.ToShortDateString();




                        worksheet.Cell("A3").Value = "DISTRICT";
                        worksheet.Cell("B3").Value = "LOCATION";
                        worksheet.Cell("C3").Value = "PRODUCER";
                        worksheet.Cell("D3").Value = "LANDLORD";
                        worksheet.Cell("E3").Value = "COMMODITY";
                        worksheet.Cell("F3").Value = "VARIETY";
                        worksheet.Cell("G3").Value = "UOM";
                        worksheet.Cell("H3").Value = "NET LBS.";
                        worksheet.Cell("I3").Value = "NET UOM";
                        
                        worksheet.Range(3, 1, 3, 9).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        worksheet.Range(3, 1, 3, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;





                        Row = 4;
                        FreezeRow = 3;

                       
                       
                        foreach (var Prow in reportDataSet.ProducersDeliveredCommodity )
                        {
                            

                            worksheet.SheetView.FreezeRows(FreezeRow);
                            worksheet.Cell(Row, 1).Value = Prow.District;
                            worksheet.Cell(Row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 2).Value = Prow.Location ;
                            worksheet.Cell(Row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 3).Value = Prow.Producer ;
                            worksheet.Cell(Row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 4).Value = Prow.Landlord;
                            worksheet.Cell(Row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 5).Value = Prow.Commodity ;
                            worksheet.Cell(Row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 6).Value = (Prow.IsVarietyNull()) ? "": Prow.Variety ;
                            worksheet.Cell(Row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                            worksheet.Cell(Row, 7).Value = Prow.Units;
                            worksheet.Cell(Row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 8).Value = Prow.Net ;
                            worksheet.Cell(Row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
//                            worksheet.Cell(Row, 8).Style.NumberFormat.NumberFormatId = 3;
                            worksheet.Cell(Row, 9).Value = Prow.NetUnits  ;
                            worksheet.Cell(Row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(Row, 9).Style.NumberFormat.Format = "#,##0.0"; 

                            Row += 1;
                        }

                        worksheet.Columns().AdjustToContents();

















                        #endregion

                        workbook.SaveAs(stream);
                    }
                }
            }

        }

        catch (Exception ex)
        {

            //  Logging.Log_Event("CreateRailCarReport", ex.Message);
        }

        return stream;



    }



    private static MemoryStream CreateCommodityTallyReport(int?LocationId, DateTime startDate ,DateTime endDate)
    {
        MemoryStream stream = new MemoryStream();
        try
        {

            using (TallyDatasetTableAdapters.IntakeTransferLoadsTableAdapter dlcta = new TallyDatasetTableAdapters.IntakeTransferLoadsTableAdapter())

            {

                using (TallyDataset.IntakeTransferLoadsDataTable table = new TallyDataset.IntakeTransferLoadsDataTable())
                {

                    if (dlcta.Fill(table,LocationId  ?? null,endDate,startDate) > 0)
                    {




                        int Row;
                        int FreezeRow;


                        XLWorkbook workbook = new XLWorkbook();


                        #region Report





                        var worksheet = workbook.Worksheets.Add("Report");




                        worksheet.Range("A1:I1").Merge();
                        worksheet.Range("A1:I1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A1:I1").Style.Font.FontSize = 14;
                        worksheet.Range("A1:I1").Style.Font.Bold = true;
                        worksheet.Range("A1:I1").Value = "PRODUCER COMMODITY DELIVERY REPORT";

                        worksheet.Range("A2:I2").Merge();
                        worksheet.Range("A2:I2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A2:I2").Style.Font.FontSize = 12;
                        worksheet.Range("A2:I2").Style.Font.Bold = false;
                        worksheet.Range("A2:I2").Value = "FROM " + startDate.ToShortDateString() + " TO " + endDate.ToShortDateString();




                        worksheet.Cell("A3").Value = "LOCATION";
                        worksheet.Cell("B3").Value = "CROP";
                        worksheet.Cell("C3").Value = "PRODUCER";
                        worksheet.Cell("D3").Value = "LANDLORD";
                        worksheet.Cell("E3").Value = "COMMODITY";
                        worksheet.Cell("F3").Value = "VARIETY";
                        worksheet.Cell("G3").Value = "UOM";
                        worksheet.Cell("H3").Value = "NET LBS.";
                        worksheet.Cell("I3").Value = "NET UOM";

                        worksheet.Range(3, 1, 3, 9).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        worksheet.Range(3, 1, 3, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;





                        Row = 4;
                        FreezeRow = 3;



                        foreach (var Prow in table)
                        {
                            

                            //worksheet.SheetView.FreezeRows(FreezeRow);
                            //worksheet.Cell(Row, 1).Value = Prow.District;
                            //worksheet.Cell(Row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            //worksheet.Cell(Row, 2).Value = Prow.Location;
                            //worksheet.Cell(Row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            //worksheet.Cell(Row, 3).Value = Prow.Producer;
                            //worksheet.Cell(Row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            //worksheet.Cell(Row, 4).Value = Prow.Landlord;
                            //worksheet.Cell(Row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            //worksheet.Cell(Row, 5).Value = Prow.Commodity;
                            //worksheet.Cell(Row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            //worksheet.Cell(Row, 6).Value = (Prow.IsVarietyNull()) ? "" : Prow.Variety;
                            //worksheet.Cell(Row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;

                            //worksheet.Cell(Row, 7).Value = Prow.Units;
                            //worksheet.Cell(Row, 7).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            //worksheet.Cell(Row, 8).Value = Prow.Net;
                            //worksheet.Cell(Row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            ////                            worksheet.Cell(Row, 8).Style.NumberFormat.NumberFormatId = 3;
                            //worksheet.Cell(Row, 9).Value = Prow.NetUnits;
                            //worksheet.Cell(Row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            //worksheet.Cell(Row, 9).Style.NumberFormat.Format = "#,##0.0";

                            Row += 1;
                        }

                        worksheet.Columns().AdjustToContents();


                        #endregion

                        workbook.SaveAs(stream);
                    }
                }
            }

        }

        catch (Exception ex)
        {


        }

        return stream;



    }

    private static MemoryStream CreateProducersDeliveredReport(string District, string Producer, DateTime StartDate, DateTime EndDate)
    {
        MemoryStream stream = new MemoryStream();
        try
        {

            using (ReportDataSetTableAdapters.ProducersDeliveredTableAdapter   producersDeliveredTableAdapter = new ReportDataSetTableAdapters.ProducersDeliveredTableAdapter())

            {

                using (ReportDataSet reportDataSet = new ReportDataSet())
                {

                    if (producersDeliveredTableAdapter.Fill(reportDataSet.ProducersDelivered,(string.IsNullOrEmpty(District)?null:District),StartDate,EndDate,(string.IsNullOrEmpty(Producer)?null:Producer)) > 0)
                    {




                       
                        int Row;
                        int FreezeRow;
                     

                        XLWorkbook workbook = new XLWorkbook();


                        #region Report

                        

                    




                      
                        //worksheet.Range("A1:E1").Merge();
                        //worksheet.Range("A1:E1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        //worksheet.Range("A1:E1").Style.Font.FontSize = 14;
                        //worksheet.Range("A1:E1").Style.Font.Bold = true;
                        //worksheet.Range("A1:E1").Value = "PRODUCERS DELIVERED REPORT";

                        //worksheet.Range("A2:E2").Merge();
                        //worksheet.Range("A2:E2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        //worksheet.Range("A2:E2").Style.Font.FontSize = 12;
                        //worksheet.Range("A2:E2").Style.Font.Bold = false;
                        //worksheet.Range("A2:E2").Value ="FROM "+ StartDate.ToShortDateString()+" TO " + EndDate.ToShortDateString();

                       

                        
                        //worksheet.Cell("A3").Value = "DISTRICT";
                        //worksheet.Cell("B3").Value = "PRODUCER ID" ;
                        //worksheet.Cell("C3").Value = "PRODUCER";
                        //worksheet.Cell("D3").Value = "NET LBS.";
                        //worksheet.Cell("E3").Value = "# LOADS";
                        //worksheet.Range(3, 1, 3, 5).Style.Border.BottomBorder= XLBorderStyleValues.Medium;
                        //worksheet.Range(3, 1, 3, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;



                        
                       
                        Row = 4;
                        FreezeRow =  3;

                        IXLWorksheet worksheet= null;
                        string worksheetname = "";
                        foreach (ReportDataSet.ProducersDeliveredRow   LotRow in reportDataSet.ProducersDelivered  )
                        {
                            if (LotRow.District != worksheetname )
                            {
                                if (worksheet != null) worksheet.Columns().AdjustToContents();
                                worksheetname = LotRow.District;
                                worksheet = CreateWorksheet(workbook, worksheetname, StartDate, EndDate);
                                
                                Row = 4;

                            }

                            worksheet.SheetView.FreezeRows(FreezeRow);
                            worksheet.Cell(Row, 1).Value = LotRow.District;
                            worksheet.Cell(Row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 2).Value = LotRow.Producer_Id;
                            worksheet.Cell(Row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 3).Value = LotRow.Producer;
                            worksheet.Cell(Row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 4).Value = LotRow.Net;
                            worksheet.Cell(Row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(Row, 4).Style.NumberFormat.NumberFormatId = 3;
                            worksheet.Cell(Row, 5).Value = LotRow.Loads;
                            worksheet.Cell(Row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(Row, 5).Style.NumberFormat.NumberFormatId = 3;
                            worksheet.Cell(Row, 6).Value = LotRow.Company_Name;
                            worksheet.Cell(Row, 7).Value = LotRow.Customer_Name1;
                            worksheet.Cell(Row, 8).Value = LotRow.Customer_Name2;
                            worksheet.Cell(Row, 9).Value = LotRow.Address1;
                            worksheet.Cell(Row, 10).Value = LotRow.Address2;
                            worksheet.Cell(Row, 11).Value = LotRow.City;
                            worksheet.Cell(Row, 12).Value = LotRow.State;
                            worksheet.Cell(Row, 13).Value = LotRow.Zip1;
                            worksheet.Cell(Row, 14).Value = LotRow.Zip2;
                            worksheet.Cell(Row, 15).Value = LotRow.Home_Phone;
                            worksheet.Cell(Row, 16).Value = LotRow.Work_Phone;
                            worksheet.Cell(Row, 17).Value = LotRow.Mobile_Phone;
                            worksheet.Cell(Row, 18).Value = LotRow.Phone;
                            worksheet.Cell(Row, 19).Value = LotRow.Member;
                            worksheet.Cell(Row, 20).Value = LotRow.Email_Address;


                            Row += 1;
                        }

                        worksheet.Columns().AdjustToContents();











                        worksheet = CreateWorksheet(workbook, "All", StartDate, EndDate);
                        Row = 4;
                        foreach (ReportDataSet.ProducersDeliveredRow LotRow in reportDataSet.ProducersDelivered)
                        {
                            worksheet.SheetView.FreezeRows(FreezeRow);
                            worksheet.Cell(Row, 1).Value = LotRow.District;
                            worksheet.Cell(Row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 2).Value = LotRow.Producer_Id;
                            worksheet.Cell(Row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 3).Value = LotRow.Producer;
                            worksheet.Cell(Row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 4).Value = LotRow.Net;
                            worksheet.Cell(Row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(Row, 4).Style.NumberFormat.NumberFormatId = 3;
                            worksheet.Cell(Row, 5).Value = LotRow.Loads;
                            worksheet.Cell(Row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(Row, 5).Style.NumberFormat.NumberFormatId = 3;
                            worksheet.Cell(Row, 6).Value = LotRow.Company_Name;
                            worksheet.Cell(Row, 7).Value = LotRow.Customer_Name1;
                            worksheet.Cell(Row, 8).Value = LotRow.Customer_Name2;
                            worksheet.Cell(Row, 9).Value = LotRow.Address1;
                            worksheet.Cell(Row, 10).Value = LotRow.Address2;
                            worksheet.Cell(Row, 11).Value = LotRow.City;
                            worksheet.Cell(Row, 12).Value = LotRow.State;
                            worksheet.Cell(Row, 13).Value = LotRow.Zip1;
                            worksheet.Cell(Row, 14).Value = LotRow.Zip2;
                            worksheet.Cell(Row, 15).Value = LotRow.Home_Phone;
                            worksheet.Cell(Row, 16).Value = LotRow.Work_Phone;
                            worksheet.Cell(Row, 17).Value = LotRow.Mobile_Phone;
                            worksheet.Cell(Row, 18).Value = LotRow.Phone;
                            worksheet.Cell(Row, 19).Value = LotRow.Member;
                            worksheet.Cell(Row, 20).Value = LotRow.Email_Address;

                            Row += 1;
                        }

                        worksheet.Columns().AdjustToContents();







                        #endregion

                        workbook.SaveAs(stream);
                    }
                }
            }

            }
           
        catch (Exception ex )
        {

           
        }

        return stream;

    

    }



    private static MemoryStream CreateLoadsByCropAndDateReport(int? LocationId, DateTime StartDate, DateTime EndDate)
    {
        MemoryStream stream = new MemoryStream();
        try
        {
            using (LoadReportDataSetTableAdapters.LoadsByCropAndDataTableAdapter dataTableAdapter = new LoadReportDataSetTableAdapters.LoadsByCropAndDataTableAdapter())
            {





                LoadReportDataSet.LoadsByCropAndDataDataTable reportDataSet = new LoadReportDataSet.LoadsByCropAndDataDataTable();


                {

                    if (dataTableAdapter.Fill(reportDataSet, LocationId, StartDate, EndDate) > 0)
                    {





                        int Row;
                        int FreezeRow;


                        XLWorkbook workbook = new XLWorkbook();


                        #region Report













                        
                        
                        var worksheetname = "Location Loads";
                        var worksheet = workbook.Worksheets.Add(worksheetname);

                        worksheet.Range("A1:J1").Merge();
                        worksheet.Range("A1:J1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A1:J1").Style.Font.FontSize = 14;
                        worksheet.Range("A1:J1").Style.Font.Bold = true;
                        worksheet.Range("A1:J1").Value = "COMMODITIES DELIVERED/TRANSFERED BY LOCATION ";

                        worksheet.Range("A2:J2").Merge();
                        worksheet.Range("A2:J2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                        worksheet.Range("A2:J2").Style.Font.FontSize = 12;
                        worksheet.Range("A2:J2").Style.Font.Bold = false;
                        worksheet.Range("A2:J2").Value = "FROM " + StartDate.ToShortDateString() + " TO " + EndDate.ToShortDateString();




                        worksheet.Cell("A3").Value = "LOCATION ID";
                        worksheet.Cell("B3").Value = "LOCATION";
                        worksheet.Cell("C3").Value = "CROP ID";
                        worksheet.Cell("D3").Value = "CROP";
                        worksheet.Cell("E3").Value = "UOM";
                        worksheet.Cell("F3").Value = "INTAKE";
                        worksheet.Cell("G3").Value = "# INTAKE LOADS";
                        worksheet.Cell("H3").Value = "TRANSFER DESTINATION";
                        worksheet.Cell("I3").Value = "TRANSFER";
                        worksheet.Cell("J3").Value = "# TRANSFER LOADS";

                        worksheet.Range(3, 1, 3, 10).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                        worksheet.Range(3, 1, 3, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;




                        Row = 4;
                        FreezeRow = 3;

                        foreach (LoadReportDataSet.LoadsByCropAndDataRow repRow in reportDataSet)
                        {
                            
        
                        



                            
                            worksheet.SheetView.FreezeRows(FreezeRow);
                            worksheet.Cell(Row, 1).Value = repRow.Location_Id;
                            worksheet.Cell(Row, 1).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 2).Value = repRow.Location;
                            worksheet.Cell(Row, 2).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 3).Value = repRow.Crop_Id;
                            worksheet.Cell(Row, 3).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 4).Value = repRow.Crop;
                            worksheet.Cell(Row, 4).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Left;
                            worksheet.Cell(Row, 5).Value = repRow.UOM;
                            worksheet.Cell(Row, 5).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(Row, 6).Value = repRow.Intake;
                            worksheet.Cell(Row, 6).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(Row, 7).Style.NumberFormat.NumberFormatId = 3;
                            worksheet.Cell(Row, 7).Value = repRow.IntakeLoads;
                            worksheet.Cell(Row, 8).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(Row, 8).Value = repRow.Outbound_Location;
                            worksheet.Cell(Row, 9).Value = repRow.Transfered;
                            worksheet.Cell(Row, 9).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;
                            worksheet.Cell(Row, 10).Style.NumberFormat.NumberFormatId = 3;
                            worksheet.Cell(Row, 10).Value = repRow.TransferLoads;
                            worksheet.Cell(Row, 10).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Right;




                            Row += 1;
                        }

                        worksheet.Columns().AdjustToContents();
















                        #endregion

                        workbook.SaveAs(stream);
                    }
                }
            }

        }

        catch (Exception ex)
        {


        }

        return stream;



    }


    private static IXLWorksheet CreateWorksheet(IXLWorkbook workbook, string District, DateTime StartDate, DateTime EndDate)
    {
        var worksheet = workbook.Worksheets.Add(District);




        worksheet.Range("A1:T1").Merge();
        worksheet.Range("A1:T1").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Range("A1:T1").Style.Font.FontSize = 14;
        worksheet.Range("A1:T1").Style.Font.Bold = true;
        worksheet.Range("A1:T1").Value = "PRODUCERS DELIVERED REPORT FOR " + District.ToUpper();

        worksheet.Range("A2:T2").Merge();
        worksheet.Range("A2:T2").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
        worksheet.Range("A2:T2").Style.Font.FontSize = 12;
        worksheet.Range("A2:T2").Style.Font.Bold = false;
        worksheet.Range("A2:T2").Value = "FROM " + StartDate.ToShortDateString() + " TO " + EndDate.ToShortDateString();




        worksheet.Cell("A3").Value = "DISTRICT";
        worksheet.Cell("B3").Value = "PRODUCER ID";
        worksheet.Cell("C3").Value = "PRODUCER";
        worksheet.Cell("D3").Value = "NET LBS.";
        worksheet.Cell("E3").Value = "# LOADS";
        worksheet.Cell("F3").Value = "Company Name";
        worksheet.Cell("G3").Value = "Customer Name 1";
        worksheet.Cell("H3").Value = "Customer Name 2";
        worksheet.Cell("I3").Value = "Address 1";
        worksheet.Cell("J3").Value = "Address 2";
        worksheet.Cell("K3").Value = "City";
        worksheet.Cell("L3").Value = "State";
        worksheet.Cell("M3").Value = "Zip 1";
        worksheet.Cell("N3").Value = "Zip 2";
        worksheet.Cell("O3").Value = "Home Phone";
        worksheet.Cell("P3").Value = "Work Phone";
        worksheet.Cell("Q3").Value = "Mobile Phone";
        worksheet.Cell("R3").Value = "Phone";
        worksheet.Cell("S3").Value = "Member";
        worksheet.Cell("T3").Value = "Email";
        worksheet.Range(3, 1, 3, 20).Style.Border.BottomBorder = XLBorderStyleValues.Medium;
        worksheet.Range(3, 1, 3, 20).Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;


        return worksheet;


    }





    /// <summary>
    /// Download an xlsx file directly to the browser
    /// </summary>
    /// <param name="stream">memory stream of the excel file</param>
    /// <param name="FileName">Name of file like something.xlsx </param>
    /// <param name="Response">HttprRsponse</param>
    public static void DownloadExcellFile(MemoryStream stream, string FileName, HttpResponse Response)
    {
        try
        {
            stream.Seek(0, SeekOrigin.Begin);

            Response.Clear();

            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment; filename=" + FileName);
            Response.AddHeader("content-length", stream.Length.ToString());

            Response.BinaryWrite(stream.ToArray());
            Response.Flush();
            Response.End();
        }
        catch (Exception ex)
        {

        }
    }

    public static string GetDateForFileName(DateTime D)
    {

        return string.Format("{0}_{1}_{2}", D.Month, D.Day, D.Year);
    }



    public static MemoryStream CreateXLSXReport(DataTable Dt)
    {
        MemoryStream stream = new MemoryStream();
        try
        {
            XLWorkbook wb = new XLWorkbook();
            DataRow Dr = Dt.NewRow();
            wb.Worksheets.Add(Dt);
            wb.SaveAs(stream);
        }
        catch (Exception )
        {
          //  Logging.Log_Event("CreateXLSXReport", ex.Message);
        }

        return stream;


    }


    public class UsedLots
    {
        public int VarietyId { get; set; } // Corresponds to Seed_Ticket_Varieties.Variety_ID
        public string Lot { get; set; } // Corresponds to Seed_Ticket_Varieties.Lot
        public string VarietyDescription { get; set; } // Corresponds to Seed_Varieties.Description
        public int LocationId { get; set; } // Corresponds to Seed_Varieties.Location_ID
        public string Location { get; set; } // Corresponds to Locations.Description
    }



    public static void getUsedLots(int? location)
    {
        //using (LotsDataSetTableAdapters.UsedSeedLotsTableAdapter = new UsedSeedLotsTableAdapter())
        //{
        //    using (LotsDataSet ds = new LotsDataSet())
        //    {
        //        if (location == null)
        //        {
        //            UsedSeedLotsTableAdapter.Fill(ds.UsedSeedLots);
        //        }
        //        else
        //        {
        //            UsedSeedLotsTableAdapter.FillByLocation(ds.UsedSeedLots, location);
        //        }

        //        List<UsedLots> lots = new List<UsedLots>();
        //        foreach (LotsDataSet.UsedSeedLotsRow row in ds.UsedSeedLots)
        //        {
        //            lots.Add(new UsedLots
        //            {
        //                VarietyId = row.Variety_ID,
        //                Lot = row.Lot,
        //                VarietyDescription = row.Description,
        //                LocationId = row.Location_ID,
        //                Location = row.Location
        //            });
        //        }
        //    }
        //}
    }
}