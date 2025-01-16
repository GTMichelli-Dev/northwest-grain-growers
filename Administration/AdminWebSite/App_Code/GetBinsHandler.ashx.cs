using System;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using BinData;

public class GetBinsHandler : IHttpHandler
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "application/json";
        using (var dbContext = new BinDBContext())
        {
            var query = from location in dbContext.Locations
                        join bin in dbContext.Bins on location.Id equals bin.LocationId
                        join adjustmentGroup in
                            (from ba in dbContext.BinAdjustments
                             group ba by ba.BinUid into g
                             select new
                             {
                                 UID = g.Key,
                                 AdjustedDate = g.Max(ba => ba.AdjustedDate)
                             })
                        on bin.Uid equals adjustmentGroup.UID into adjustmentGroups
                        from adjGroup in adjustmentGroups.DefaultIfEmpty()
                        join adjustment in dbContext.BinAdjustments
                        on new { BinUid = bin.Uid, AdjustedDate = adjGroup.AdjustedDate } equals new { adjustment.BinUid, adjustment.AdjustedDate } into adjustments
                        from adj in adjustments.DefaultIfEmpty()
                        orderby location.Description, bin.Bin1
                        select new BinDTO
                        {
                            AdjustedDate = adjGroup.AdjustedDate,
                            LocationId = bin.LocationId,
                            Bin = bin.Bin1,
                            Location = location.Description,
                            BinUID = bin.Uid,
                            District = location.District,
                            Bushels = adj != null ? adj.Bushels : 0,
                            Protein = adj != null ? (float)adj.Protein : 0,
                            Comment = adj != null ? adj.Comment : string.Empty,
                            AdjustmentUID = adj != null ? adj.Uid : Guid.Empty,
                        };

            var bins = query.ToList();
            var json = JsonConvert.SerializeObject(bins);
            context.Response.Write(json);
        }
    }

    public bool IsReusable
    {
        get { return false; }
    }
}
