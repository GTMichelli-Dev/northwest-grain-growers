using System.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.Entity;


public class LoadBinProteinService
{
    private readonly NWDataModel _context;

    public LoadBinProteinService(NWDataModel context)
    {
        _context = context;
    }

    public List<BinProteins> GetLoadBinProteinReport(DateTime startDate, DateTime endDate, int? locationId = null)
    {
        var startDateParam = new SqlParameter("@StartDate", startDate);
        var endDateParam = new SqlParameter("@EndDate", endDate);
        var locationIdParam = new SqlParameter("@Location_Id", locationId.HasValue ? (object)locationId.Value : DBNull.Value);

        return _context.Database.SqlQuery<BinProteins>(
            "SELECT * FROM dbo.GetBinProteinAverages(@StartDate, @EndDate, @Location_Id)",
            startDateParam,
            endDateParam,
            locationIdParam
        ).ToList();
    }


    public class BinProteins
    {
        public int Net { get; set; }
        public string Bin { get; set; }
        public int Location_Id { get; set; }
        public string Location { get; set; }=string.Empty;
        public decimal Protein { get; set; }
        public int Crop_Id { get; set; }
        public string Crop { get; set; }
        public int Factor { get; set; }
        public decimal UnitNet { get; set; }
        public string StrUnits { get; set; }
    }
}
