using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;

/// <summary>
/// Summary description for ReportService
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class ReportService : System.Web.Services.WebService
{

    public ReportService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
   
    public string GetSeedTotalsByDateVarietyExcelFile(int? locationId, DateTime sd, DateTime ed)
    {
        // Sample values, replace with actual input from UI
       
        

        //DateTime sd = DateTime.Now.AddDays(-30); // Sample start date
        //DateTime ed = DateTime.Now; // Sample end date

        //DateTime.TryParse(startDate, out sd);
        //DateTime.TryParse(DateTime, out ed);
        string Report = (locationId != null) ? $"Variety_Totals_For_Location{locationId}_" : "Variety_Totals_For_All_Locations_";
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
            worksheet.Cell(currentRow, 5).Value = "Total Lbs";
            worksheet.Cell(currentRow, 6).Value = "Total Bushels";
            worksheet.Cell(currentRow, 7).Value = "Total Loads";
            worksheet.Cell(currentRow, 8).Value = "Clean Lbs";
            worksheet.Cell(currentRow, 9).Value = "Clean Bushels";
            worksheet.Cell(currentRow, 10).Value = "Clean Loads";
            worksheet.Cell(currentRow, 11).Value = "Treated Lbs";
            worksheet.Cell(currentRow, 12).Value = "Treated Bushels";
            worksheet.Cell(currentRow, 13).Value = "Treated Loads";

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
                byte[] byteArray = stream.ToArray();
                string base64String = Convert.ToBase64String(byteArray);
                return base64String;
            }
        }
    }

    [WebMethod]
    public string GetSeedTotalsByDateExcelFile(int? locationId, DateTime sd, DateTime ed, string group, string type)
    {

        if (type == "commodity" && group == "load")
        {
            return downLoadByCommodityByLoad(locationId, sd, ed);
        }

        else if (type == "commodity" && group == "day")
        {
            if (locationId == null)
            {
                return downLoadByCommodityByDay(sd, ed);
            }
            else
            {
                return downLoadByCommodityByDay(locationId, sd, ed);
            }

        }
        else if (type == "commodity" && group == "day_location")
        {
            return downLoadByCommodityByDay(locationId, sd, ed);

        }

        else if (type == "commodity" && group == "total")
        {
            if (locationId == null)
            {
                return downLoadByCommodityTotal(sd, ed);
            }
            else
            {
                return downLoadByCommodityTotal(locationId, sd, ed);
            }
        }
        else if (type == "commodity" && group == "total_location")
        {
            return downLoadByCommodityTotal(locationId, sd, ed);
        }


        else if (type == "commodityDetails" && group == "load")
        {
            return downLoadByCommodityByLoad(locationId, sd, ed);
        }


        else if (type == "commodityDetails" && group == "day")
        {

            if (locationId == null)
            {
                return downLoadByCommodityDetailsByDay(sd, ed);
            }
            else
            {
                return downLoadByCommodityDetailsByDay(locationId, sd, ed);
            }
        }
        else if (type == "commodityDetails" && group == "day_location")
        {

            return downLoadByCommodityDetailsByDay(locationId, sd, ed);
        }


        else if (type == "commodityDetails" && group == "total")
        {
            if (locationId == null)
            {
                return downLoadByCommodityDetailsTotal(sd, ed);
            }

            else
            {
                return downLoadByCommodityDetailsTotal(locationId, sd, ed);

            }
        }
        else if (type == "commodityDetails" && group == "total_location")
        {
            return downLoadByCommodityDetailsTotal(locationId, sd, ed);

        }


        else if (type == "variety" && group == "load")
        {
            return downLoadByCommodityByLoad(locationId, sd, ed);
        }


        else if (type == "variety" && group == "day")
        {
            if (locationId == null)
            {
                return downLoadByVarietyByDay(sd, ed);
            }
            else
            {
                return downLoadByVarietyByDay(locationId, sd, ed);
            }

        }
        else if (type == "variety" && group == "day_location")
        {
            return downLoadByVarietyByDay(locationId, sd, ed);

        }




        else if (type == "variety" && group == "total")
        {
            if (locationId == null)
            {
                return downLoadByVarietyByTotal(sd, ed);
            }
            else
            {
                return downLoadByVarietyByTotal(locationId, sd, ed);
            }

        }

        else if (type == "variety" && group == "total_location")
        {
            return downLoadByVarietyByTotal(locationId, sd, ed);

        }

        else if (type == "everything" && group == "load")
        {
            return downLoadByCommodityByLoad(locationId, sd, ed);
        }


        else if (type == "everything" && group == "day")
        {
            if (locationId == null)
            {
                return downLoadByEverythingByDay(sd, ed);
            }
            else
            {
                return downLoadByEverythingByDay(locationId, sd, ed);
            }

        }
        else if (type == "everything" && group == "day_location")
        {
            return downLoadByEverythingByDay(locationId, sd, ed);

        }




        else if (type == "everything" && group == "total")
        {
            if (locationId == null)
            {
                return downLoadByEverythingTotal(sd, ed);
            }
            else
            {
                return downLoadByEverythingTotal(locationId, sd, ed);
            }

        }

        else if (type == "everything" && group == "total_location")
        {
            return downLoadByEverythingTotal(locationId, sd, ed);

        }

        else return "";

        //DateTime sd = DateTime.Now.AddDays(-30); // Sample start date
        //DateTime ed = DateTime.Now; // Sample end date

    }

    #region Commodity
    public string downLoadByCommodityByLoad(int? locationId, DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"SELECT 
                        Ticket, Ticket_Date, Clean, Treated, CommodityDetails, Commodity,Location_ID ,Location
                        FROM SeedTicketInfo where Ticket_Date >= @StartDate and Ticket_Date<=@EndDate and Location_ID = isnull(@Location_ID,Location_Id)
                        order by Location_Id,Ticket";

            var data = context.Database.SqlQuery<CommodityDownloadByLoad>(query, sd, ed, lId).ToList();




            ////DateTime.TryParse(startDate, out sd);
            ////DateTime.TryParse(DateTime, out ed);
            //string Report = (locationId != null) ? $"Totals_For_Location{locationId}_" : "Totals_For_All_Locations_";
            //Report += $"From_{sd.Month}-{sd.Day}-{sd.Year}_To_{ed.Month}-{ed.Day}-{ed.Year}.xlsx";
            //// Get data from SignalR hub method
            //List<TotalsForDateByDateRange> data = TotalsForDateByDateRange.GetTotalsForDatesByDate(locationId, sd, ed);

            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Loads");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Location";
                worksheet.Cell(currentRow, 2).Value = "Location Id";
                worksheet.Cell(currentRow, 3).Value = "Ticket";
                worksheet.Cell(currentRow, 4).Value = "TicketDate";
                worksheet.Cell(currentRow, 5).Value = "Commodity";
                worksheet.Cell(currentRow, 6).Value = "Commodity Details";
                worksheet.Cell(currentRow, 7).Value = "Treated lbs";
                worksheet.Cell(currentRow, 8).Value = "Clean lbs";
                worksheet.Cell(currentRow, 9).Value = "Treated Bu";
                worksheet.Cell(currentRow, 10).Value = "Clean Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {

                    worksheet.Cell(currentRow, 1).Value = item.Location;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);
                    worksheet.Cell(currentRow, 2).Value = item.Location_ID;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 3).Value = item.Ticket;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 4).Value = item.Ticket_Date;
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 5).Value = item.Commodity;
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 6).Value = item.CommodityDetails;
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 7).Value = item.Treated;
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 8).Value = item.Clean;
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 9).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 10).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 10).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }


    public string downLoadByCommodityByDay(int? locationId, DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"	

;WITH DateSeries AS (
    SELECT @StartDate AS DateValue
    UNION ALL
    SELECT DATEADD(DAY, 1, DateValue)
    FROM DateSeries
    WHERE DateValue < @EndDate
),
Commodities AS (
    SELECT DISTINCT CommodityDetails,Commodity
    FROM SeedCommodityDetails
),
FilteredLocations AS (
    SELECT ID, Description
    FROM Locations
    WHERE ID <> 0 AND ID = isnull(@Location_ID,ID) -- Exclude Location_ID 0
)
SELECT 
    CAST(DS.DateValue AS DATE) AS Ticket_Date, 
    FL.ID as Location_ID, 
    FL.Description as Location, 
    C.Commodity, 
    COUNT(ST.Ticket_Date) AS Loads, 
    SUM(COALESCE(ST.Clean, 0)) AS Clean, 
    SUM(COALESCE(ST.Treated, 0)) AS Treated
FROM 
    DateSeries DS
CROSS JOIN 
    Commodities C
CROSS JOIN 
    FilteredLocations FL
LEFT JOIN 
    SeedTicketInfo ST ON DS.DateValue = ST.Ticket_Date AND C.CommodityDetails = ST.CommodityDetails and FL.ID = ST.Location_ID
GROUP BY 
    DS.DateValue, C.Commodity, FL.ID, FL.Description
ORDER BY 
    DS.DateValue, C.Commodity, FL.Description
OPTION (MAXRECURSION 0); -- Removes limitation on recursion depth, use cautiously

--SELECT        Ticket_Date, Location_ID, Location,  CommodityDetails as Commodity, COUNT(*) AS Loads, SUM(Clean) AS Clean, SUM(Treated) AS Treated
--                        FROM            SeedTicketInfo
--                        GROUP BY Ticket_Date,  CommodityDetails, Location_ID, Location
--                        HAVING        (Ticket_Date >= @StartDate) AND (Ticket_Date <= @EndDate) AND (Location_ID = ISNULL(@Location_ID, Location_ID))
--                        ORDER BY Location_ID,Ticket_Date,Commodity ";

            var data = context.Database.SqlQuery<CommodityDownloadByDay>(query, sd, ed, lId).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Commodities");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Location";
                worksheet.Cell(currentRow, 2).Value = "Location Id";
                worksheet.Cell(currentRow, 3).Value = "Date";
                worksheet.Cell(currentRow, 4).Value = "Commodity";
                worksheet.Cell(currentRow, 5).Value = "# Loads";
                worksheet.Cell(currentRow, 6).Value = "Treated lbs";
                worksheet.Cell(currentRow, 7).Value = "Clean lbs";
                worksheet.Cell(currentRow, 8).Value = "Total lbs";
                worksheet.Cell(currentRow, 9).Value = "Treated Bu";
                worksheet.Cell(currentRow, 10).Value = "Clean Bu";
                worksheet.Cell(currentRow, 11).Value = "Total Bu";
                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {

                    worksheet.Cell(currentRow, 1).Value = item.Location;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 2).Value = item.Location_ID;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 3).Value = item.Ticket_Date;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 4).Value = item.Commodity;
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    worksheet.Cell(currentRow, 5).Value = item.Loads;
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 6).Value = item.Treated;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 7).Value = item.Clean;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 8).Value = item.Clean+item.Treated;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 9).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 10).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 10).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 11).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 11).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }

    public string downLoadByCommodityByDay(DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);

            var query = @";WITH DateSeries AS (
    SELECT @StartDate AS DateValue
    UNION ALL
    SELECT DATEADD(DAY, 1, DateValue)
    FROM DateSeries
    WHERE DateValue < @EndDate
),
Commodities AS (
    SELECT DISTINCT CommodityDetails,Commodity
    FROM SeedCommodityDetails
)
SELECT 
    CAST(DS.DateValue AS DATE) AS Ticket_Date, 
    0 AS Location_ID, 
    '' AS Location, 
    C.Commodity, 
    COUNT(ST.Ticket_Date) AS Loads, 
    SUM(COALESCE(ST.Clean, 0)) AS Clean, 
    SUM(COALESCE(ST.Treated, 0)) AS Treated
FROM 
    DateSeries DS
CROSS JOIN 
    Commodities C
LEFT JOIN 
    SeedTicketInfo ST ON DS.DateValue = ST.Ticket_Date AND C.CommodityDetails = ST.CommodityDetails
GROUP BY 
    DS.DateValue, C.Commodity
ORDER BY 
    DS.DateValue, C.Commodity
OPTION (MAXRECURSION 0);";

            var data = context.Database.SqlQuery<CommodityDownloadByDay>(query, sd, ed).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Commodities");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Commodity";
                worksheet.Cell(currentRow, 3).Value = "# Loads";
                worksheet.Cell(currentRow, 4).Value = "Treated lbs";
                worksheet.Cell(currentRow, 5).Value = "Clean lbs";
                worksheet.Cell(currentRow, 6).Value = "Total lbs";
                worksheet.Cell(currentRow, 7).Value = "Treated Bu";
                worksheet.Cell(currentRow, 8).Value = "Clean Bu";
                worksheet.Cell(currentRow, 9).Value = "Total Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {

                    worksheet.Cell(currentRow, 1).Value = item.Ticket_Date;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 2).Value = item.Commodity;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    worksheet.Cell(currentRow, 3).Value = item.Loads;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 4).Value = item.Treated;
                    worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 5).Value = item.Clean;
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 6).Value = item.Clean+item.Treated;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 7).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 8).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 9).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }


    public string downLoadByCommodityTotal(int? locationId, DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"SELECT         Location_ID, Location,  Commodity, COUNT(*) AS Loads, SUM(Clean) AS Clean, SUM(Treated) AS Treated
                    FROM            SeedTicketInfo
                    where        (Ticket_Date >= @StartDate) AND (Ticket_Date <= @EndDate) AND (Location_ID = ISNULL(@Location_ID, Location_ID))
                    GROUP BY   Commodity, Location_ID, Location

                ORDER BY Location_ID,Commodity";

            var data = context.Database.SqlQuery<CommodityDownloadTotal>(query, sd, ed, lId).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Commodities");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Location";
                worksheet.Cell(currentRow, 2).Value = "Location Id";
                worksheet.Cell(currentRow, 3).Value = "Commodity";
                worksheet.Cell(currentRow, 4).Value = "# Loads";
                worksheet.Cell(currentRow, 5).Value = "Treated lbs";
                worksheet.Cell(currentRow, 6).Value = "Clean lbs";
                worksheet.Cell(currentRow, 7).Value = "Total lbs";
                worksheet.Cell(currentRow, 8).Value = "Treated Bu";
                worksheet.Cell(currentRow, 9).Value = "Clean Bu";
                worksheet.Cell(currentRow, 10).Value = "Total Bu";
                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {

                    worksheet.Cell(currentRow, 1).Value = item.Location;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 2).Value = item.Location_ID;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 3).Value = item.Commodity;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    worksheet.Cell(currentRow, 4).Value = item.Loads;
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 5).Value = item.Treated;
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 6).Value = item.Clean;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 7).Value = item.Clean+ item.Treated;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 8).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 9).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 10).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 10).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }



    public string downLoadByCommodityTotal(DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);

            var query = @"SELECT         0 as Location_ID, '' as Location,  Commodity, COUNT(*) AS Loads, SUM(Clean) AS Clean, SUM(Treated) AS Treated
                    FROM            SeedTicketInfo
                    where        (Ticket_Date >= @StartDate) AND (Ticket_Date <= @EndDate) 
                    GROUP BY   Commodity

                ORDER BY Location_ID,Commodity";

            var data = context.Database.SqlQuery<CommodityDownloadTotal>(query, sd, ed).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Commodities");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Commodity";
                worksheet.Cell(currentRow, 2).Value = "# Loads";
                worksheet.Cell(currentRow, 3).Value = "Treated lbs";
                worksheet.Cell(currentRow, 4).Value = "Clean lbs";
                worksheet.Cell(currentRow, 5).Value = "Total lbs";
                worksheet.Cell(currentRow, 6).Value = "Treated Bu";
                worksheet.Cell(currentRow, 7).Value = "Clean Bu";
                worksheet.Cell(currentRow, 8).Value = "Total Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {



                    worksheet.Cell(currentRow, 1).Value = item.Commodity;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    worksheet.Cell(currentRow, 2).Value = item.Loads;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 3).Value = item.Treated;
                    worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 4).Value = item.Clean;
                    worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 5).Value = item.Clean+item.Treated;
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 6).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 7).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 8).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }

    #endregion

    #region CommodityDetails
    public string downLoadByCommodityDetailsByDay(int? locationId, DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"
;WITH DateSeries AS (
    SELECT @StartDate AS DateValue
    UNION ALL
    SELECT DATEADD(DAY, 1, DateValue)
    FROM DateSeries
    WHERE DateValue < @EndDate
),
Commodities AS (
    SELECT DISTINCT CommodityDetails,Commodity
    FROM SeedCommodityDetails
),
FilteredLocations AS (
    SELECT ID, Description
    FROM Locations
    WHERE ID <> 0 AND ID = isnull(@Location_ID,ID) -- Exclude Location_ID 0
)
SELECT 
    CAST(DS.DateValue AS DATE) AS Ticket_Date, 
    FL.ID as Location_ID, 
    FL.Description as Location, 
    C.CommodityDetails AS Commodity, 
    COUNT(ST.Ticket_Date) AS Loads, 
    SUM(COALESCE(ST.Clean, 0)) AS Clean, 
    SUM(COALESCE(ST.Treated, 0)) AS Treated
FROM 
    DateSeries DS
CROSS JOIN 
    Commodities C
CROSS JOIN 
    FilteredLocations FL
LEFT JOIN 
    SeedTicketInfo ST ON DS.DateValue = ST.Ticket_Date AND C.CommodityDetails = ST.CommodityDetails and FL.ID = ST.Location_ID
GROUP BY 
    DS.DateValue, C.CommodityDetails, FL.ID, FL.Description
ORDER BY 
    DS.DateValue, C.CommodityDetails, FL.Description
OPTION (MAXRECURSION 0);";



            //var query = @"SELECT        Ticket_Date, Location_ID, Location,  CommodityDetails as Commodity, COUNT(*) AS Loads, SUM(Clean) AS Clean, SUM(Treated) AS Treated
            //            FROM            SeedTicketInfo
            //            GROUP BY Ticket_Date,  CommodityDetails, Location_ID, Location
            //            HAVING        (Ticket_Date >= @StartDate) AND (Ticket_Date <= @EndDate) AND (Location_ID = ISNULL(@Location_ID, Location_ID))
            //            ORDER BY Location_ID,Ticket_Date,Commodity";

            var data = context.Database.SqlQuery<CommodityDownloadByDay>(query, sd, ed, lId).ToList();




            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Commodities");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Location";
                worksheet.Cell(currentRow, 2).Value = "Location Id";
                worksheet.Cell(currentRow, 3).Value = "Date";
                worksheet.Cell(currentRow, 4).Value = "Commodity Details";
                worksheet.Cell(currentRow, 5).Value = "# Loads";
                worksheet.Cell(currentRow, 6).Value = "Treated lbs";
                worksheet.Cell(currentRow, 7).Value = "Clean lbs";
                worksheet.Cell(currentRow, 8).Value = "Total lbs";
                worksheet.Cell(currentRow, 9).Value = "Treated Bu";
                worksheet.Cell(currentRow, 10).Value = "Clean Bu";
                worksheet.Cell(currentRow, 11).Value = "Total Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {

                    worksheet.Cell(currentRow, 1).Value = item.Location;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 2).Value = item.Location_ID;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 3).Value = item.Ticket_Date;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 4).Value = item.Commodity;
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    worksheet.Cell(currentRow, 5).Value = item.Loads;
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 6).Value = item.Treated;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 7).Value = item.Clean;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 8).Value = item.Clean+ item.Treated;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 9).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 10).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 10).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 11).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 11).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }


    public string downLoadByCommodityDetailsByDay(DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);

            var query = @"
;WITH DateSeries AS (
    SELECT @StartDate AS DateValue
    UNION ALL
    SELECT DATEADD(DAY, 1, DateValue)
    FROM DateSeries
    WHERE DateValue < @EndDate
),
Commodities AS (
    SELECT DISTINCT CommodityDetails,Commodity
    FROM SeedCommodityDetails
)
SELECT 
    CAST(DS.DateValue AS DATE) AS Ticket_Date, 
    0 as Location_ID, 
    '' as Location, 
    C.CommodityDetails AS Commodity, 
    COUNT(ST.Ticket_Date) AS Loads, 
    SUM(COALESCE(ST.Clean, 0)) AS Clean, 
    SUM(COALESCE(ST.Treated, 0)) AS Treated
FROM 
    DateSeries DS
CROSS JOIN 
    Commodities C
LEFT JOIN 
    SeedTicketInfo ST ON DS.DateValue = ST.Ticket_Date AND C.CommodityDetails = ST.CommodityDetails 
GROUP BY 
    DS.DateValue, C.CommodityDetails
ORDER BY 
    DS.DateValue, C.CommodityDetails
OPTION (MAXRECURSION 0);";

            var data = context.Database.SqlQuery<CommodityDownloadByDay>(query, sd, ed).ToList();




            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Commodities");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Commodity Details";
                worksheet.Cell(currentRow, 3).Value = "# Loads";
                worksheet.Cell(currentRow, 4).Value = "Treated lbs";
                worksheet.Cell(currentRow, 5).Value = "Clean lbs";
                worksheet.Cell(currentRow, 6).Value = "Total lbs";
                worksheet.Cell(currentRow, 7).Value = "Treated Bu";
                worksheet.Cell(currentRow, 8).Value = "Clean Bu";
                worksheet.Cell(currentRow, 9).Value = "Total Bu";
                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {

                    worksheet.Cell(currentRow, 1).Value = item.Ticket_Date;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 2).Value = item.Commodity;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    worksheet.Cell(currentRow, 3).Value = item.Loads;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 4).Value = item.Treated;
                    worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 5).Value = item.Clean;
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 6).Value = item.Clean+item.Treated;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);



                    worksheet.Cell(currentRow, 7).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 8).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 9).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }


    public string downLoadByCommodityDetailsTotal(int? locationId, DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"SELECT         Location_ID, Location,  CommodityDetails as Commodity, COUNT(*) AS Loads, SUM(Clean) AS Clean, SUM(Treated) AS Treated
                        FROM            SeedTicketInfo
                        where        (Ticket_Date >= @StartDate) AND (Ticket_Date <= @EndDate) AND (Location_ID = ISNULL(@Location_ID, Location_ID))
                        GROUP BY   CommodityDetails, Location_ID, Location

                        ORDER BY Location_ID,CommodityDetails";

            var data = context.Database.SqlQuery<CommodityDownloadTotal>(query, sd, ed, lId).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Commodities");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Location";
                worksheet.Cell(currentRow, 2).Value = "Location Id";
                worksheet.Cell(currentRow, 3).Value = "Commodity Details";
                worksheet.Cell(currentRow, 4).Value = "# Loads";
                worksheet.Cell(currentRow, 5).Value = "Treated lbs";
                worksheet.Cell(currentRow, 6).Value = "Clean lbs";
                worksheet.Cell(currentRow, 7).Value = "Total lbs";
                worksheet.Cell(currentRow, 8).Value = "Treated Bu";
                worksheet.Cell(currentRow, 9).Value = "Clean Bu";
                worksheet.Cell(currentRow, 10).Value = "Total Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {

                    worksheet.Cell(currentRow, 1).Value = item.Location;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 2).Value = item.Location_ID;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 3).Value = item.Commodity;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    worksheet.Cell(currentRow, 4).Value = item.Loads;
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 5).Value = item.Treated;
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 6).Value = item.Clean;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 7).Value = item.Clean+item.Treated;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 8).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 9).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 10).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 10).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }



    public string downLoadByCommodityDetailsTotal(DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);

            var query = @"SELECT         0 as Location_ID,'' as  Location,  CommodityDetails as Commodity, COUNT(*) AS Loads, SUM(Clean) AS Clean, SUM(Treated) AS Treated
                        FROM            SeedTicketInfo
                        where        (Ticket_Date >= @StartDate) AND (Ticket_Date <= @EndDate) 
                        GROUP BY   CommodityDetails

                        ORDER BY CommodityDetails";

            var data = context.Database.SqlQuery<CommodityDownloadTotal>(query, sd, ed).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Commodities");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Commodity Details";
                worksheet.Cell(currentRow, 2).Value = "# Loads";
                worksheet.Cell(currentRow, 3).Value = "Treated lbs";
                worksheet.Cell(currentRow, 4).Value = "Clean lbs";
                worksheet.Cell(currentRow, 5).Value = "Total lbs";
                worksheet.Cell(currentRow, 6).Value = "Treated Bu";
                worksheet.Cell(currentRow, 7).Value = "Clean Bu";
                worksheet.Cell(currentRow, 8).Value = "Total Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {


                    worksheet.Cell(currentRow, 1).Value = item.Commodity;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);


                    worksheet.Cell(currentRow, 2).Value = item.Loads;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 3).Value = item.Treated;
                    worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 4).Value = item.Clean;
                    worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 5).Value = item.Clean+ item.Treated;
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 6).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 7).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 8).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }




    #endregion

    #region Variety

    public string downLoadByVarietyByDay(int? locationId, DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"SELECT      Ticket_Date,  Location_ID, Location, Commodity, COUNT(*) AS Loads, SUM(Clean) AS Clean, SUM(Treated) AS Treated, Variety, VarietyID, CommodityDetails
                    FROM            SeedTicketInfo
                    WHERE        (Ticket_Date >= @StartDate) AND (Ticket_Date <= @EndDate) AND (Location_ID = ISNULL(@Location_ID, Location_ID))
                    GROUP BY Commodity, Location_ID, Location, Variety, VarietyID, CommodityDetails, Ticket_Date
                    ORDER BY Location_ID, Commodity";

            var data = context.Database.SqlQuery<VarietyDownloadByDay>(query, sd, ed, lId).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Varieties");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Location";
                worksheet.Cell(currentRow, 2).Value = "Location Id";
                worksheet.Cell(currentRow, 3).Value = "Date";
                worksheet.Cell(currentRow, 4).Value = "Commodity";
                worksheet.Cell(currentRow, 5).Value = "Commodity Details";
                worksheet.Cell(currentRow, 6).Value = "Variety";
                worksheet.Cell(currentRow, 7).Value = "Variety ID";
                worksheet.Cell(currentRow, 8).Value = "# Loads";
                worksheet.Cell(currentRow, 9).Value = "Treated lbs";
                worksheet.Cell(currentRow, 10).Value = "Clean lbs";
                worksheet.Cell(currentRow, 11).Value = "Total lbs";
                worksheet.Cell(currentRow, 12).Value = "Treated Bu";
                worksheet.Cell(currentRow, 13).Value = "Clean Bu";
                worksheet.Cell(currentRow, 14).Value = "Total Bu";
                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {

                    worksheet.Cell(currentRow, 1).Value = item.Location;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 2).Value = item.Location_ID;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 3).Value = item.Ticket_Date;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 4).Value = item.Commodity;
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 5).Value = item.CommodityDetails;
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 6).Value = item.Variety;
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 7).Value = item.VarietyID;
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 8).Value = item.Loads;
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 9).Value = item.Treated;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 10).Value = item.Clean;
                    worksheet.Cell(currentRow, 10).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 11).Value = item.Clean+item.Treated;
                    worksheet.Cell(currentRow, 11).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);



                    worksheet.Cell(currentRow, 12).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 12).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 13).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 13).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 14).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 14).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 14).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }


    public string downLoadByVarietyByDay(DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);

            var query = @"SELECT      Ticket_Date, 0 as  Location_ID, '' as Location, Commodity, COUNT(*) AS Loads, SUM(Clean) AS Clean, SUM(Treated) AS Treated, Variety, VarietyID, CommodityDetails
                    FROM            SeedTicketInfo
                    WHERE        (Ticket_Date >= @StartDate) AND (Ticket_Date <= @EndDate) 
                    GROUP BY Commodity,  Variety, VarietyID, CommodityDetails, Ticket_Date
                    ORDER BY Commodity";

            var data = context.Database.SqlQuery<VarietyDownloadByDay>(query, sd, ed).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Varieties");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "Commodity";
                worksheet.Cell(currentRow, 3).Value = "Commodity Details";
                worksheet.Cell(currentRow, 4).Value = "Variety";
                worksheet.Cell(currentRow, 5).Value = "Variety ID";
                worksheet.Cell(currentRow, 6).Value = "# Loads";
                worksheet.Cell(currentRow, 7).Value = "Treated lbs";
                worksheet.Cell(currentRow, 8).Value = "Clean lbs";
                worksheet.Cell(currentRow, 9).Value = "Total lbs";
                worksheet.Cell(currentRow, 10).Value = "Treated Bu";
                worksheet.Cell(currentRow, 11).Value = "Clean Bu";
                worksheet.Cell(currentRow, 12).Value = "Treated Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {


                    worksheet.Cell(currentRow, 1).Value = item.Ticket_Date;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 2).Value = item.Commodity;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 3).Value = item.CommodityDetails;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 4).Value = item.Variety;
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 5).Value = item.VarietyID;
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 6).Value = item.Loads;
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 7).Value = item.Treated;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 8).Value = item.Clean;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 9).Value = item.Clean+item.Treated;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 10).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 10).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 11).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 11).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 12).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 12).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }

    public string downLoadByVarietyByTotal(int? locationId, DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"SELECT        Location_ID, Location, Commodity, COUNT(*) AS Loads, SUM(Clean) AS Clean, SUM(Treated) AS Treated, Variety, VarietyID, CommodityDetails
                    FROM            SeedTicketInfo
                    WHERE        (Ticket_Date >= @StartDate) AND (Ticket_Date <= @EndDate) AND (Location_ID = ISNULL(@Location_ID, Location_ID))
                    GROUP BY Commodity, Location_ID, Location, Variety, VarietyID, CommodityDetails
                    ORDER BY Location_ID, Commodity";

            var data = context.Database.SqlQuery<VarietyDownloadByTotal>(query, sd, ed, lId).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Varieties");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Location";
                worksheet.Cell(currentRow, 2).Value = "Location Id";
                worksheet.Cell(currentRow, 3).Value = "Commodity";
                worksheet.Cell(currentRow, 4).Value = "Commodity Details";
                worksheet.Cell(currentRow, 5).Value = "Variety";
                worksheet.Cell(currentRow, 6).Value = "Variety ID";
                worksheet.Cell(currentRow, 7).Value = "# Loads";
                worksheet.Cell(currentRow, 8).Value = "Treated lbs";
                worksheet.Cell(currentRow, 9).Value = "Clean lbs";
                worksheet.Cell(currentRow, 10).Value = "Total lbs";
                worksheet.Cell(currentRow, 11).Value = "Treated Bu";
                worksheet.Cell(currentRow, 12).Value = "Clean Bu";
                worksheet.Cell(currentRow, 13).Value = "Total Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {

                    worksheet.Cell(currentRow, 1).Value = item.Location;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 2).Value = item.Location_ID;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 3).Value = item.Commodity;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 4).Value = item.CommodityDetails;
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 5).Value = item.Variety;
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 6).Value = item.VarietyID;
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 7).Value = item.Loads;
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 8).Value = item.Treated;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 9).Value = item.Clean;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 10).Value = item.Clean+item.Treated;
                    worksheet.Cell(currentRow, 10).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 11).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 11).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 12).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 12).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 12).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 13).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 13).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 13).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }

    public string downLoadByVarietyByTotal(DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);

            var query = @"SELECT      0 as  Location_ID, '' as Location, Commodity, COUNT(*) AS Loads, SUM(Clean) AS Clean, SUM(Treated) AS Treated, Variety, VarietyID, CommodityDetails
                    FROM            SeedTicketInfo
                    WHERE        (Ticket_Date >= @StartDate) AND (Ticket_Date <= @EndDate) 
                    GROUP BY Commodity, Variety, VarietyID, CommodityDetails
                    ORDER BY  Commodity";

            var data = context.Database.SqlQuery<VarietyDownloadByTotal>(query, sd, ed).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Varieties");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Commodity";
                worksheet.Cell(currentRow, 2).Value = "Commodity Details";
                worksheet.Cell(currentRow, 3).Value = "Variety";
                worksheet.Cell(currentRow, 4).Value = "Variety ID";
                worksheet.Cell(currentRow, 5).Value = "# Loads";
                worksheet.Cell(currentRow, 6).Value = "Treated lbs";
                worksheet.Cell(currentRow, 7).Value = "Clean lbs";
                worksheet.Cell(currentRow, 8).Value = "Total lbs";
                worksheet.Cell(currentRow, 9).Value = "Treated Bu";
                worksheet.Cell(currentRow, 10).Value = "Clean Bu";
                worksheet.Cell(currentRow, 11).Value = "Total Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {


                    worksheet.Cell(currentRow, 1).Value = item.Commodity;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 2).Value = item.CommodityDetails;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 3).Value = item.Variety;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 4).Value = item.VarietyID;
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 5).Value = item.Loads;
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 6).Value = item.Treated;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 7).Value = item.Clean;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 8).Value = item.Clean+item.Treated;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 9).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 10).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 10).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 11).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 11).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 11).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }

    #endregion

    #region Everything

    public string downLoadByEverythingTotal(DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);

            var query = @"SELECT 0 as  Location_Id, '' as Location,    COUNT(*) AS Loads, SUM(Clean) AS Clean, SUM(Treated) AS Treated
                    FROM            SeedTicketInfo
                    where        (Ticket_Date >= @StartDate) AND (Ticket_Date <= @EndDate) ";

            var data = context.Database.SqlQuery<CommodityDownloadTotal>(query, sd, ed).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Everything");
                var currentRow = 1;

                // Headers

                worksheet.Cell(currentRow, 1).Value = "# Loads";
                worksheet.Cell(currentRow, 2).Value = "Treated lbs";
                worksheet.Cell(currentRow, 3).Value = "Clean lbs";
                worksheet.Cell(currentRow, 4).Value = "Total lbs";
                worksheet.Cell(currentRow, 5).Value = "Treated Bu";
                worksheet.Cell(currentRow, 6).Value = "Clean Bu"; 
                worksheet.Cell(currentRow, 7).Value = "Total Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {


                    worksheet.Cell(currentRow, 1).Value = item.Loads;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 2).Value = item.Treated;
                    worksheet.Cell(currentRow, 2).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 3).Value = item.Clean;
                    worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 4).Value = item.Clean+item.Treated ;
                    worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);



                    worksheet.Cell(currentRow, 5).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 6).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 7).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }

    public string downLoadByEverythingByDay(int? locationId, DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"
;WITH DateSeries AS (
    SELECT @StartDate AS DateValue
    UNION ALL
    SELECT DATEADD(DAY, 1, DateValue)
    FROM DateSeries
    WHERE DateValue < @EndDate
),

FilteredLocations AS (
    SELECT ID, Description
    FROM Locations
    WHERE ID <> 0 AND ID = isnull(@Location_ID,ID) -- Exclude Location_ID 0
)
SELECT 
    CAST(DS.DateValue AS DATE) AS Ticket_Date, 
    FL.ID as Location_ID, 
    FL.Description as Location, 
    '' as Commodity, 
    COUNT(ST.Ticket_Date) AS Loads, 
    SUM(COALESCE(ST.Clean, 0)) AS Clean, 
    SUM(COALESCE(ST.Treated, 0)) AS Treated
FROM 
    DateSeries DS

CROSS JOIN 
    FilteredLocations FL
LEFT JOIN 
    SeedTicketInfo ST ON DS.DateValue = ST.Ticket_Date  and FL.ID = ST.Location_ID
GROUP BY 
    DS.DateValue,FL.ID, FL.Description
ORDER BY 
    DS.DateValue, FL.Description
OPTION (MAXRECURSION 0);";

            var data = context.Database.SqlQuery<CommodityDownloadByDay>(query, sd, ed, lId).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Commodities");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Location";
                worksheet.Cell(currentRow, 2).Value = "Location Id";
                worksheet.Cell(currentRow, 3).Value = "Date";
                worksheet.Cell(currentRow, 4).Value = "# Loads";
                worksheet.Cell(currentRow, 5).Value = "Treated lbs";
                worksheet.Cell(currentRow, 6).Value = "Clean lbs";
                worksheet.Cell(currentRow, 7).Value = "Total lbs";
                worksheet.Cell(currentRow, 8).Value = "Treated Bu";
                worksheet.Cell(currentRow, 9).Value = "Clean Bu";
                worksheet.Cell(currentRow, 10).Value = "Total Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {

                    worksheet.Cell(currentRow, 1).Value = item.Location;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 2).Value = item.Location_ID;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 3).Value = item.Ticket_Date;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 4).Value = item.Loads;
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 5).Value = item.Treated;
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);




                    worksheet.Cell(currentRow, 6).Value = item.Clean;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 7).Value =item.Clean+ item.Treated;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 8).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 9).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 10).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 10).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 10).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }




    public string downLoadByEverythingByDay(DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);

            var query = @"
;WITH DateSeries AS (
    SELECT @StartDate AS DateValue
    UNION ALL
    SELECT DATEADD(DAY, 1, DateValue)
    FROM DateSeries
    WHERE DateValue < @EndDate

)
SELECT 
    CAST(DS.DateValue AS DATE) AS Ticket_Date, 
    0 as Location_ID, 
    '' as Location, 
    '' as Commodity, 
    COUNT(ST.Ticket_Date) AS Loads, 
    SUM(COALESCE(ST.Clean, 0)) AS Clean, 
    SUM(COALESCE(ST.Treated, 0)) AS Treated
FROM 
    DateSeries DS

LEFT JOIN 
    SeedTicketInfo ST ON DS.DateValue = ST.Ticket_Date 
GROUP BY 
    DS.DateValue
ORDER BY 
    DS.DateValue
OPTION (MAXRECURSION 0); -- Removes limitation on recursion depth, use cautiously
";

            var data = context.Database.SqlQuery<CommodityDownloadByDay>(query, sd, ed).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Commodities");
                var currentRow = 1;

                // Headers

                worksheet.Cell(currentRow, 1).Value = "Date";
                worksheet.Cell(currentRow, 2).Value = "# Loads";
                worksheet.Cell(currentRow, 3).Value = "Treated lbs";
                worksheet.Cell(currentRow, 4).Value = "Clean lbs";
                worksheet.Cell(currentRow, 5).Value = "Total lbs";
                worksheet.Cell(currentRow, 6).Value = "Treated Bu";
                worksheet.Cell(currentRow, 7).Value = "Clean Bu";
                worksheet.Cell(currentRow, 8).Value = "Total Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {


                    worksheet.Cell(currentRow, 1).Value = item.Ticket_Date;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);




                    worksheet.Cell(currentRow, 2).Value = item.Loads;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 3).Value = item.Treated;
                    worksheet.Cell(currentRow, 3).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 4).Value = item.Clean;
                    worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 5).Value = item.Clean+item.Treated;
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 6).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 7).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 8).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }



    public string downLoadByEverythingTotal(int? locationId, DateTime startDate, DateTime endDate)
    {

        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"SELECT         Location_ID, Location,'' as Commodity,   COUNT(*) AS Loads, SUM(Clean) AS Clean, SUM(Treated) AS Treated
                    FROM            SeedTicketInfo
                    where        (Ticket_Date >= @StartDate) AND (Ticket_Date <= @EndDate) AND (Location_ID = ISNULL(@Location_ID, Location_ID))
                    GROUP BY    Location_ID, Location

                ORDER BY Location_ID";

            var data = context.Database.SqlQuery<CommodityDownloadTotal>(query, sd, ed, lId).ToList();





            // Convert data to Excel using ClosedXML
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Everything");
                var currentRow = 1;

                // Headers
                worksheet.Cell(currentRow, 1).Value = "Location";
                worksheet.Cell(currentRow, 2).Value = "Location Id";
                worksheet.Cell(currentRow, 3).Value = "# Loads";
                worksheet.Cell(currentRow, 4).Value = "Treated lbs";
                worksheet.Cell(currentRow, 5).Value = "Clean lbs";
                worksheet.Cell(currentRow, 6).Value = "Total lbs";
                worksheet.Cell(currentRow, 7).Value = "Treated Bu";
                worksheet.Cell(currentRow, 8).Value = "Clean Bu";
                worksheet.Cell(currentRow, 9).Value = "Total Bu";

                currentRow++;
                worksheet.SheetView.Freeze(1, 10);
                foreach (var item in data)
                {

                    worksheet.Cell(currentRow, 1).Value = item.Location;
                    worksheet.Cell(currentRow, 1).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 2).Value = item.Location_ID;
                    worksheet.Cell(currentRow, 2).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Left);

                    worksheet.Cell(currentRow, 3).Value = item.Loads;
                    worksheet.Cell(currentRow, 3).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 4).Value = item.Treated;
                    worksheet.Cell(currentRow, 4).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 4).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 5).Value = item.Clean;
                    worksheet.Cell(currentRow, 5).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 5).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 6).Value = item.Clean+item.Treated;
                    worksheet.Cell(currentRow, 6).Style.NumberFormat.Format = "0";
                    worksheet.Cell(currentRow, 6).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 7).Value = item.Treated / 60;
                    worksheet.Cell(currentRow, 7).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 7).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);


                    worksheet.Cell(currentRow, 8).Value = item.Clean / 60;
                    worksheet.Cell(currentRow, 8).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 8).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    worksheet.Cell(currentRow, 9).Value = (item.Clean+item.Treated) / 60;
                    worksheet.Cell(currentRow, 9).Style.NumberFormat.Format = "0.00";
                    worksheet.Cell(currentRow, 9).Style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);

                    currentRow++;
                }

                worksheet.Columns().AdjustToContents();
                // Send Excel file to client
                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] byteArray = stream.ToArray();
                    string base64String = Convert.ToBase64String(byteArray);
                    return base64String;
                }
            }
        }
    }
    #endregion




  

}
