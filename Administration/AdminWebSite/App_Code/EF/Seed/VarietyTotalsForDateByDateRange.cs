using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

public class VarietyTotalsForDateByDateRange
{

    public int Location_Id { get; set; }
    public string Department { get; set; }
    public string Description { get; set; }
    public DateTime Date_Shipped { get; set; }
    public double Total_Lbs { get; set; }
    public double Total_Bushels { get; set; }
    public int Total_Loads { get; set; }
    public double Clean_Lbs { get; set; }
    public double Clean_Bushels { get; set; }
    public int Clean_Loads { get; set; }
    public double Treated_Lbs { get; set; }
    public double TreatedBushels { get; set; }
    public int Treated_Loads { get; set; }




    public static List<VarietyTotalsForDateByDateRange> GetVarietyTotalsForDatesByDate(int? locationId, DateTime startDate, DateTime endDate)
    {
        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"SELECT Location_ID, Location, Department, Description, Date_Shipped, Total_Lbs, Total_Bushels, Total_Loads, Clean_Lbs, Clean_Bushels, Clean_Loads, Treated_Lbs, TreatedBushels, Treated_Loads
                        FROM dbo.VarietyTotalsForDateByDateRange(@StartDate, @EndDate, @Location_ID) AS VarietyTotalsForDateByDateRange_1
                        ORDER BY Location_ID, Date_Shipped, Description, Department";
            try
            {
                var lst = context.Database.SqlQuery<VarietyTotalsForDateByDateRange>(query, sd, ed, lId).ToList();
                return lst;
            }
            catch(Exception ex)
            {

                return null;
            }


        }

    }

   




}