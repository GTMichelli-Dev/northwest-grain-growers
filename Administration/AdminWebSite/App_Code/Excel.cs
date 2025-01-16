using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ClosedXML.Excel;

using System.Data;
using System.IO;
using System.Web.UI.WebControls;



/// <summary>
/// Summary description for Excel
/// </summary>
public class Excel
{
    public Excel()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    /// <summary>
    /// Export the Gridview to excell
    /// </summary>
    /// <param name="GridView1"></param>
    /// <param name="Response"></param>
    /// <param name="Filename">xlsx</param>
    public static void ExportToExcel( GridView GridView1, HttpResponse Response,string Filename) {
        // Create a new workbook and worksheet
        Filename = Filename + $"_{DateTime.Now.Month}-{DateTime.Now.Day}-{DateTime.Now.Year}_{DateTime.Now.ToLongTimeString()}.xlsx";
        using (var wb = new XLWorkbook())
        {
            var ws = wb.Worksheets.Add("Sheet1");

            // Access the GridView data
            DataTable dt = new DataTable();
            foreach (DataControlField column in GridView1.Columns)
            {
                dt.Columns.Add(column.HeaderText);
            }

            foreach (GridViewRow row in GridView1.Rows)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    dr[i] =  row.Cells[i].Text.Replace("&nbsp","").Replace(";","");
                }
                dt.Rows.Add(dr);
            }

            // Add the data to the worksheet
            ws.Cell(1, 1).InsertTable(dt);
            ws.Columns().AdjustToContents();
            // Set the content type and headers for the response
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;filename="+Filename);

            // Write the workbook to the response stream
            using (MemoryStream memoryStream = new MemoryStream())
            {
                wb.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                memoryStream.Close();
            }

            Response.End();
        }
    }







}





