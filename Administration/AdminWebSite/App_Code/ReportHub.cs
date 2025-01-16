using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.Json.Serialization;
using System.Web;

public class ReportHub : Hub
{









    public List<TreatTotalsByDateRange> GetTreatments(int? locationId, DateTime startDate, DateTime endDate)
    {
        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };

            var query = @"
                SELECT        Treatment_ID, Description, SUM(TotalCwt) AS TotalCwt, SUM(TotalOz) AS TotalOz, SUM(TotalGals) AS TotalGals
                FROM dbo.TreatmentTotalsByDateRange(@StartDate, @EndDate) 
                WHERE (Location_ID = ISNULL(@Location_ID, Location_ID)) 
                GROUP BY Treatment_ID, Description 
                ORDER BY Description";

            return context.Database.SqlQuery<TreatTotalsByDateRange>(query, sd, ed, lId).ToList();

        }

    }




    public List<SeedTotalsByDateRange> GetSeedTotals(int? locationId, DateTime startDate, DateTime endDate)
    {
        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"SELECT        
                        Net,NetBu,Treated,TreatedBu,Clean, CleanBu
                        FROM            dbo.VarietyTotalsByDateRangeSum(@StartDate, @EndDate, @Location_Id) AS VarietyTotalsByDateRangeSum_1";

            return context.Database.SqlQuery<SeedTotalsByDateRange>(query, sd, ed, lId).ToList();

        }

    }
    public class SeedTicketLoad
    {
        public Guid UID { get; set; }
        public bool Treated { get; set; } // Corresponds to the "Treated" column (converted to bit in SQL)
        public string Condition { get; set; } // Corresponds to "Condition"
        public int Ticket { get; set; } // Corresponds to "Ticket"
        public DateTime TicketDate { get; set; } // Corresponds to "Ticket_Date"
        public double Net { get; set; } // Corresponds to "Net"
        public string Variety { get; set; } // Corresponds to "Variety"
        public int Dept { get; set; } // Corresponds to "Dept"
        public string CommodityDetails { get; set; } // Corresponds to "CommodityDetails"
        public string Commodity { get; set; } // Corresponds to "Commodity"
        public int GrowerId { get; set; } // Corresponds to "Grower_ID"
        public string Grower { get; set; } // Corresponds to "Grower"
        public bool Complete { get; set; }
        public string Location { get; set; }
    }

    public List<SeedTicketLoad> GetSeedTicketLoads(int? locationId, DateTime startDate, DateTime endDate)
    {
        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"SELECT 
                                SeedTicketInfo.UID, 
                                CASE 
                                    WHEN Treated = 0 THEN CONVERT(bit, 0) 
                                    ELSE CONVERT(bit, 1) 
                                END AS Treated, 
                                CASE 
                                    WHEN Treated = 0 THEN 'Clean' 
                                    ELSE 'Treated' 
                                END AS Condition, 
                                ISNULL(SeedTicketInfo.Ticket, 0) AS Ticket, 
                                CAST(SeedTicketInfo.Ticket_Date AS DATE) AS TicketDate, 
                                CASE 
                                    WHEN SeedTicketInfo.Ticket IS NULL THEN 0 
                                    ELSE SeedTicketInfo.Net / 60 
                                END AS Net, 
                                SeedTicketInfo.Variety, 
                                SeedTicketInfo.Dept, 
                                SeedTicketInfo.CommodityDetails, 
                                SeedTicketInfo.Commodity, 
                                SeedTicketInfo.Grower_ID, 
                                growers.Description AS Grower, 
                                CONVERT(bit, ISNULL(SeedTicketInfo.Ticket, 0)) AS Complete, 
                                Locations.Description AS Location
                            FROM 
                                SeedTicketInfo 
                            INNER JOIN 
                                (SELECT 
                                    Id, 
                                    Description 
                                 FROM 
                                    NW_Data.dbo.Producers
                                ) AS growers 
                            ON 
                                SeedTicketInfo.Grower_ID = growers.Id 
                            INNER JOIN 
                                Locations 
                            ON 
                                SeedTicketInfo.Location_ID = Locations.ID
                            WHERE 
                                (DATEDIFF(day, @StartDate, SeedTicketInfo.Ticket_Date) >= 0) 
                                AND (DATEDIFF(day, SeedTicketInfo.Ticket_Date, @EndDate) >= 0) 
                                AND (SeedTicketInfo.Location_ID = ISNULL(@Location_Id, SeedTicketInfo.Location_ID))
                            ORDER BY 
                                Location DESC, 
                                Ticket DESC;";
            var data = context.Database.SqlQuery<SeedTicketLoad>(query, sd, ed, lId).ToList();
            return data;
        }

    }

    

    public string GetLocation(int locationId)
    {
        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            SqlParameter lId = new SqlParameter("@Location_ID", locationId);
            var query = @"SELECT Description FROM Locations WHERE Id=@Location_ID";

            return context.Database.SqlQuery<string>(query, lId).FirstOrDefault() ?? string.Empty;
        }
    }


    public List<VarietyTotalsByDateRangeTreatStatus> GetVarietyTotals(int? locationId, DateTime startDate, DateTime endDate)
    {
        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            var sd = new SqlParameter("@StartDate", startDate);
            var ed = new SqlParameter("@EndDate", endDate);
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"SELECT        Description, Net, NetBu, Treated, TreatedBu, Clean, CleanBu
                          FROM            dbo.VarietyTotalsByDateRangeTreatStatus(@StartDate, @EndDate, @Location_ID) AS VarietyTotalsByDateRangeTreatStatus_1";


            return context.Database.SqlQuery<VarietyTotalsByDateRangeTreatStatus>(query, sd, ed, lId).ToList();

        }

    }


    public class Totals
    {


        public double Clean { get; set; }
        public double Treated { get; set; }
        public double Total
        {
            get
            {
                return Clean + Treated;
            }
        }


        public double CleanBu { get { return Clean / 60; } }
        public double TreatedBu { get{ return Treated / 60; } }
        public double TotalBu
        {
            get
            {
                return CleanBu + TreatedBu;
            }
        }


    }

    public class SeedTicketLoadData
    {
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public string Location { get; set; } = "All Locations";

        public List<SeedTicketLoad> SeedTicketLoads { get; set; }
        public Totals Totals { get; set; } = new Totals();
    }


    public void GetLoadData(int? locationId, DateTime startDate, DateTime endDate)
    {
        var seedTicketLoadData = new SeedTicketLoadData();
        seedTicketLoadData.SeedTicketLoads = GetSeedTicketLoads(locationId, startDate, endDate);

        seedTicketLoadData.Totals.Clean = seedTicketLoadData.SeedTicketLoads.Sum(x => x.Treated ? 0 : x.Net);
        seedTicketLoadData.Totals.Treated = seedTicketLoadData.SeedTicketLoads.Sum(x => x.Treated ? x.Net : 0);


        if (locationId != null)
        {

            seedTicketLoadData.Location = GetLocation((int)locationId);

        }


        
        
        Clients.Caller.updateSeedTicketData(seedTicketLoadData);


    }

    public class UsedLot
    {
        
        public int Variety_Id { get; set; } // Corresponds to Seed_Ticket_Varieties.Variety_ID

        public string Lot { get; set; } // Corresponds to Seed_Ticket_Varieties.Lot

        public string Description { get; set; } // Corresponds to Seed_Varieties.Description

        public int LocationId { get; set; } // Corresponds to Seed_Varieties.Location_ID

        public string Location { get; set; } // Corresponds to Locations.Description
    }

    public class UsedLotResults
    {
        public DateTime UpdateTime { get; set; } = DateTime.Now;
        public List<UsedLot> UsedLots { get; set; }
    }


    public void GetUsedLots(int? locationId)
    {
        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            SqlParameter lId = locationId.HasValue ? new SqlParameter("@Location_ID", locationId) : new SqlParameter("@Location_ID", SqlDbType.Int) { Value = DBNull.Value };
            var query = @"SELECT DISTINCT Seed_Ticket_Varieties.Variety_ID, Seed_Ticket_Varieties.Lot, Seed_Varieties.Description, Seed_Varieties.Location_ID, Locations.Description AS Location, Seed_Ticket_Varieties.Variety_ID
                    FROM            Seed_Ticket_Varieties INNER JOIN
                         Seed_Varieties ON Seed_Ticket_Varieties.Variety_ID = Seed_Varieties.Item_Id INNER JOIN
                         Locations ON Seed_Varieties.Location_ID = Locations.ID
                    WHERE        (Seed_Ticket_Varieties.Lot IS NOT NULL AND Seed_Varieties.Location_ID= isnull(@Location_Id,Seed_Varieties.Location_ID))
                    ORDER BY Location DESC, Seed_Ticket_Varieties.Lot DESC";
            var data = context.Database.SqlQuery<UsedLot>(query, lId).ToList();
            var results = new UsedLotResults();
            results.UsedLots = data;
            Clients.Caller.updateUsedLots(results);
        }
    }

public class SeedTicketLotDetail
{
 
    public string Lot { get; set; } // Corresponds to Seed_Ticket_Varieties.Lot

    public int Ticket { get; set; } // Corresponds to SeedTicketInfo.Ticket

    public string Variety { get; set; } // Corresponds to SeedTicketInfo.Variety

    public DateTime TicketDate { get; set; } // Corresponds to SeedTicketInfo.Ticket_Date

    public string Grower { get; set; } // Corresponds to growers.Description

    public string Location { get; set; } // Corresponds to SeedTicketInfo.Location

    public double Net { get; set; } // Corresponds to the calculated Net (converted to int in SQL)

    public bool Treated { get; set; } // Corresponds to SeedTicketInfo.Treated (converted to bit in SQL)
}



public void GetTicketsForLot(string Lot)
    {
        using (var context = new SeedModel())
        {
            context.Database.CommandTimeout = 240;
            SqlParameter lotParam = new SqlParameter("@Lot", Lot);
            var query = @" SELECT    Distinct   SeedTicketInfo.UID,  Seed_Ticket_Varieties.Lot, SeedTicketInfo.Ticket, SeedTicketInfo.Variety, CAST(SeedTicketInfo.Ticket_Date AS DATE) AS TicketDate, growers.Description AS Grower, SeedTicketInfo.Location, CONVERT(float, 
                         CASE WHEN clean > 0 THEN clean ELSE Treated END)/60 AS Net, CONVERT(bit, SeedTicketInfo.Treated) AS Treated
                FROM            SeedTicketInfo INNER JOIN
                         Seed_Ticket_Varieties ON SeedTicketInfo.UID = Seed_Ticket_Varieties.Seed_Ticket_UID INNER JOIN
                             (SELECT        Id, Description
                               FROM            NW_Data.dbo.Producers) AS growers ON SeedTicketInfo.Grower_ID = growers.Id
                    WHERE        (Seed_Ticket_Varieties.Lot = @Lot)
                    ORDER BY SeedTicketInfo.Ticket DESC ";
            var data = context.Database.SqlQuery<SeedTicketLoad>(query, lotParam).ToList();
            Clients.Caller.updateTicketsForLot(data);

        }
    }

    public void GetData(int? locationId, DateTime startDate, DateTime endDate)
    {
        var Treatments = GetTreatments(locationId, startDate, endDate);

        var seedTotals = GetSeedTotals(locationId, startDate, endDate);

        var varietyTotals = GetVarietyTotals(locationId, startDate, endDate);



        string varieties = JsonConvert.SerializeObject(varietyTotals);
        string treatments = JsonConvert.SerializeObject(Treatments);

        var clean = 0;
        var treated = 0;
        var total = 0;
        if (seedTotals.Count > 0)
        {
            clean = (int)seedTotals[0].Clean;
            treated = (int)seedTotals[0].Treated;
            total = clean + treated;


        }

        Clients.Caller.updateTables(varieties, treatments, clean, treated, total, DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());





    }
}
