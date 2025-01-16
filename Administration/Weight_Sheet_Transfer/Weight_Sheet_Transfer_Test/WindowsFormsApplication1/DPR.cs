using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
using System.Windows.Forms;

namespace Weight_Sheet_Export
{
    class DPR
    {

        public static bool ExportDPR(DateTime SelectedDate,string filepath)//, List<int> Locations)
        {
            try
            {

       
                using (var dailyDPRTableAdapter = new NW_Data_MasterDataSetTableAdapters.Daily_DPRTableAdapter())
                {
                    using (var daily_DPRDataTable = new NW_Data_MasterDataSet.Daily_DPRDataTable())
                    {

                        dailyDPRTableAdapter.Fill(daily_DPRDataTable, SelectedDate);
                        XLWorkbook workbook = new XLWorkbook();
                        var Crop_Id = -1;
                        var Begin = true;

                        foreach (var row in daily_DPRDataTable)
                        {

                            if (Begin || Crop_Id != row.Crop_Id)
                            {
                                if (!Begin)
                                {
                                    var prevworksheet = workbook.Worksheets.Last();
                                    prevworksheet.Column(1).AdjustToContents();
                                    prevworksheet.Column(2).AdjustToContents();
                                    prevworksheet.Column(3).AdjustToContents();
                                    prevworksheet.Column(4).AdjustToContents();
                                    prevworksheet.Row(1).AdjustToContents();
                                    var TotalRow = prevworksheet.LastRowUsed().RowBelow();
                                    var Last = TotalRow.RowNumber() - 1;
                                   
                                    TotalRow.Cell("C").FormulaA1 = string.Format("SUM(C2:C{0})",Last) ;
                                    TotalRow.Cell("D").FormulaA1 = string.Format("SUM(D2:D{0})", Last);
                                    TotalRow.Style.Border.TopBorder = XLBorderStyleValues.Medium;


                                }
                                Begin = false;

                                Crop_Id = row.Crop_Id;
                                string worksheetname = string.Format("{0}-{1}", row.Crop_Id, row.Crop);
                                if (worksheetname.Length  > 30) worksheetname = worksheetname.Substring(0, 30);

                                var ws = workbook.Worksheets.Add(worksheetname);
                            
                                
          
                                ws.Cells("A1").Value = "Location Id";
                                ws.Cells("B1").Value = "Location";
                                ws.Cells("C1").Value = "Recieved";
                                ws.Cells("D1").Value = "Out";
                                ws.Cells("A1:D1").Style.Font.Bold = true;
                                ws.Cells("A1:D1").Style.Border.BottomBorder = XLBorderStyleValues.Medium;
                                ws.SheetView.Freeze(1, 4);
                            }


                            var worksheet = workbook.Worksheets.Last();
                            var CurRow = worksheet.LastRowUsed().RowBelow();
                            var RowNumber = CurRow.RowNumber().ToString();
                            worksheet.Cell("A" + RowNumber).Value = row.Location_Id;
                            worksheet.Cell("B" + RowNumber).Value = row.Location;
                            worksheet.Cell("C" + RowNumber).Value = row.Recieved;
                            worksheet.Cell("D" + RowNumber).Value = row.Out;


                        }

                        
                        if (System.IO.File.Exists(filepath ))
                        {
                            System.IO.File.Delete(filepath);
                        }
                        workbook.SaveAs(filepath);
                    }
                }
                return true;
            }
            catch(Exception ex)
            {

                MessageBox.Show("Error Exporting DPR " + ex.Message);
                return false;
            }
        }



    }
}
