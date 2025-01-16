using BinData;
using System;
using System.Web.UI;
using System.Linq;
public partial class Bins_UpdateBushels : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Set the current date and time to the DateTimeEntered TextBox
            DateTimeEntered.Text = DateTime.Now.ToString("yyyy-MM-ddTHH:mm");

            string binUidParam = Request.QueryString["BinUID"];
            Guid binUid;
            if (string.IsNullOrWhiteSpace(binUidParam) || !Guid.TryParse(binUidParam, out binUid))
            {
                Response.Redirect("Bins.aspx");
            }
            else
            {
                using (var context = new BinDBContext())
                {
                    var bin = context.Bins.FirstOrDefault(x => x.Uid == binUid);
                    if (bin == null)
                    {
                        Response.Redirect("Bins.aspx");

                    }
                    this.header.InnerText = $"Update Values For {bin.Location.Description} Bin:{bin.Bin1}";

                }

            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        int bushels;
        int protein;
        DateTime dateTimeEntered;

        if (int.TryParse(Bushels.Text, out bushels) && int.TryParse(Protein.Text, out protein) && DateTime.TryParse(DateTimeEntered.Text, out dateTimeEntered))
        {
            if (bushels >= 0 && protein >= 0 && protein <= 20)
            {
                // Add the record to the binAdjustment
                // Assuming you have a method to add the record to the database
                AddBinAdjustment(bushels, protein, dateTimeEntered);

                // Redirect to another page or show a success message
                Response.Redirect("Bins.aspx");
            }
            else
            {
                // Show validation error
                // This should not happen due to client-side validation
            }
        }
    }

    private void AddBinAdjustment(int bushels, int protein, DateTime dateTimeEntered)
    {
        string binUidParam = Request.QueryString["BinUID"];
        Guid binUid;
        if (string.IsNullOrWhiteSpace(binUidParam) || !Guid.TryParse(binUidParam, out binUid))
        {
            Response.Redirect("Bins.aspx");
        }
        else
        {
            using (var context = new BinDBContext())
            {
                var bin = context.Bins.FirstOrDefault(x => x.Uid == binUid);
                if (bin == null)
                {
                    Response.Redirect("Bins.aspx");

                }
                context.BinAdjustments.Add(new BinAdjustment
                {
                    Uid = Guid.NewGuid(),
                    BinUid = binUid,
                    AdjustedDate = dateTimeEntered,
                    Bushels = bushels,
                    Protein = protein,
                    Comment = ""
                });
                context.SaveChanges();
                Response.Redirect("Bins.aspx");
            }

        }
    }
}
