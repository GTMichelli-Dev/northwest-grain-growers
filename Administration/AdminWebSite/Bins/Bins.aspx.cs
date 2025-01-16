using System.Web.Services;
using System.Web.Script.Serialization;
using BinData;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Mvc;
using System.Threading.Tasks;

public partial class Bins_Bins : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            hfBins.Value = GetBins();
        }
    }





    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    [HttpGet]
    public static string GetBins()
    {
        using (var context = new BinDBContext())
        {
            //DateTime startDate = DateTime.Now.AddMonths(-24);
            //DateTime endDate = DateTime.Now;

            //var binAdjustmentsList = context.BinAdjustments
            //    .OrderBy(x => x.Uid)
            //    .ThenBy(x => x.AdjustedDate)
            //    .Where(x => x.AdjustedDate >= startDate && x.AdjustedDate <= endDate)
            //    .Select(x => new BinAdjustmentDTO
            //    {
            //        Uid = x.Uid,
            //        BinUid = x.BinUid,
            //        AdjustedDate = x.AdjustedDate,
            //        Bushels = x.Bushels,
            //        Protein = x.Protein,
            //        Comment = x.Comment
            //    }).ToList();

            var query = from location in context.Locations
                        join bin in context.Bins on location.Id equals bin.LocationId
                        join adjustmentGroup in
                            (from ba in context.BinAdjustments
                             group ba by ba.BinUid into g
                             select new
                             {
                                 UID = g.Key,
                                 AdjustedDate = g.Max(ba => ba.AdjustedDate)
                             })
                        on bin.Uid equals adjustmentGroup.UID into adjustmentGroups
                        from adjGroup in adjustmentGroups.DefaultIfEmpty()
                        join adjustment in context.BinAdjustments
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
            //foreach (var bin in bins)
            //{
            //    bin.BinAdjustmentDTOs = binAdjustmentsList.Where(x => x.BinUid == bin.BinUID).ToList();
            //}

            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(bins);
        }
    }

}
