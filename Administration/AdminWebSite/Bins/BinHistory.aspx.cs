using System.Web.Services;
using System.Web.Script.Serialization;
using BinData;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Script.Services;
using System.Web.Mvc;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.InkML;

public partial class Bins_BinHistory : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string binUidParam = Request.QueryString["BinUID"];
            Guid binUid;
            if (string.IsNullOrWhiteSpace(binUidParam) || !Guid.TryParse(binUidParam, out binUid))
            {
                Response.Redirect("Bins.aspx");
            }
            else
            {
                hfBins.Value = GetBinHistory(binUid);
            }
        }
    }

    public string GetBinHistory(Guid BinUid)
    {
        using (var context = new BinDBContext())
        {
            var bin = context.Bins.FirstOrDefault(x => x.Uid == BinUid);
            if (bin == null)
            {
                Response.Redirect("Bins.aspx");
                return null;
            }
            this.header.InnerText = $"History (Past 24 Months) For {bin.Location.Description} Bin:{ bin.Bin1}" ;

            DateTime startDate = DateTime.Now.AddMonths(-24);
            DateTime endDate = DateTime.Now;

            var binAdjustmentsList = context.BinAdjustments
                .OrderBy(x => x.Uid)
                .ThenBy(x => x.AdjustedDate)
                .Where(x=> x.BinUid == BinUid)
                .Where(x => x.AdjustedDate >= startDate && x.AdjustedDate <= endDate)
                .Select(x => new BinAdjustmentDTO
                {
                    Uid = x.Uid,
                    BinUid = x.BinUid,
                    AdjustedDate = x.AdjustedDate,
                    Bushels = x.Bushels,
                    Protein = x.Protein,
                    Comment = x.Comment
                }).
                OrderByDescending(x=> x.AdjustedDate)
                .ToList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return js.Serialize(binAdjustmentsList);
        }
    }




}
