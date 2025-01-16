using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.IO;

public partial class DownloadSeedTotals : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        // ... your ClosedXML code to generate Excel ...
        // Sample values, replace with actual input from UI
        int? locationId = null; // Assuming you get this from some control on the page
        //if (hfLocationID.Value != null)
        //{
        //    int LocID;
        //    if (int.TryParse(hfLocationID.Value, out LocID)) locationId = LocID;
        //}


        DateTime sd = DateTime.Now.AddDays(-30); // Sample start date
        DateTime ed = DateTime.Now; // Sample end date

        //DateTime.TryParse(hfStartDate.Value, out sd);
        //DateTime.TryParse(hfEndDate.Value, out ed);
        string Report = (locationId != null) ? $"Totals_For_Location{locationId}_" : "Totals_For_All_Locations_";
        Report += $"From_{sd.Month}-{sd.Day}-{sd.Year}_To_{ed.Month}-{ed.Day}-{ed.Year}.xlsx";
        // Get data from SignalR hub method
        List<VarietyTotalsForDateByDateRange> data = VarietyTotalsForDateByDateRange.GetVarietyTotalsForDatesByDate(locationId, sd, ed);

        // Convert data to Excel using ClosedXML
        using (XLWorkbook workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Totals By Day");
            var currentRow = 1;

            // Headers
            worksheet.Cell(currentRow, 1).Value = "Location ID";
            worksheet.Cell(currentRow, 2).Value = "Location";
            worksheet.Cell(currentRow, 3).Value = "Department";
            worksheet.Cell(currentRow, 4).Value = "Date";
            worksheet.Cell(currentRow, 5).Value = "Total_Lbs";
            worksheet.Cell(currentRow, 6).Value = "Total_Bushels";
            worksheet.Cell(currentRow, 7).Value = "Total_Loads";
            worksheet.Cell(currentRow, 8).Value = "Clean_Lbs";
            worksheet.Cell(currentRow, 9).Value = "Clean_Bushels";
            worksheet.Cell(currentRow, 10).Value = "Clean_Loads";
            worksheet.Cell(currentRow, 11).Value = "Treated_Lbs";
            worksheet.Cell(currentRow, 12).Value = "TreatedBushels";
            worksheet.Cell(currentRow, 13).Value = "Treated_Loads";

            currentRow++;

            foreach (var item in data)
            {

                worksheet.Cell(currentRow, 1).Value = item.Location_Id;
                worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                worksheet.Cell(currentRow, 2).Value = item.Department;
                worksheet.Cell(currentRow, 3).Value = item.Description;
                worksheet.Cell(currentRow, 4).Value = item.Date_Shipped;
                worksheet.Cell(currentRow, 5).Value = item.Total_Lbs;
                worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                worksheet.Cell(currentRow, 6).Value = item.Total_Bushels;
                worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                worksheet.Cell(currentRow, 7).Value = item.Total_Loads;
                worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                worksheet.Cell(currentRow, 8).Value = item.Clean_Lbs;
                worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                worksheet.Cell(currentRow, 9).Value = item.Clean_Bushels;
                worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                worksheet.Cell(currentRow, 10).Value = item.Clean_Loads;
                worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                worksheet.Cell(currentRow, 11).Value = item.Treated_Lbs;
                worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                worksheet.Cell(currentRow, 12).Value = item.TreatedBushels;
                worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                worksheet.Cell(currentRow, 13).Value = item.Treated_Loads;
                worksheet.Cell(currentRow, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                currentRow++;
            }

            worksheet.Columns().AdjustToContents();
            // Send Excel file to client
            using (MemoryStream stream = new MemoryStream())
            {
                workbook.SaveAs(stream);
                var bytes = stream.ToArray();
                Response.Clear();
                Response.AddHeader("content-disposition", "attachment;filename=" + Report);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.BinaryWrite(bytes);
                Response.End();
            }
        }
    }
}