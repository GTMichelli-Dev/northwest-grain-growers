using BinData;
using System;
using System.Web.UI;
using System.Linq;
using System.Web.Script.Serialization;

public partial class Bins_AddBin : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            using (var context = new BinDBContext())
            {
                var locations = context.Locations.ToList().OrderBy(x => x.Description).Select(x=> new LocationDTO { 
                Active = x.Active,
                Description = x.Description,
                Id = x.Id

                }).ToList();
                locations.Insert(0, new LocationDTO { Id = 0, Description = "Select Location" });
                //this.hfLocations.Value = new JavaScriptSerializer().Serialize(locations);

                cboLocations.DataSource = locations;
                cboLocations.DataTextField = "Description";
                cboLocations.DataValueField = "Id";
                cboLocations.DataBind();
            }
        }
    }

  


    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        string binName = txtBinName.Text.Trim();
        int locationId;
        if (!int.TryParse(cboLocations.SelectedValue, out locationId))
        {
            // Show SweetAlert for invalid location selection
            string script = "Swal.fire({ title: 'Error', text: 'Please select a location.', icon: 'error' });";
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", script, true);
            return;
        }
        if (locationId == 0)
        {
            // Show SweetAlert
            string script = "Swal.fire({ title: 'Error', text: 'Please select a location.', icon: 'error' });";
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", script, true);
            return;
        }
        else if(string.IsNullOrEmpty(binName))
        {
            // Show SweetAlert
            string script = "Swal.fire({ title: 'Error', text: 'Please enter a bin name.', icon: 'error' });";
            ScriptManager.RegisterStartupScript(this, GetType(), "showalert", script, true);
            return;
        }
        using (var context = new BinDBContext())
        {
            var existingBin = context.Bins.FirstOrDefault(b => b.Bin1 == binName && b.LocationId ==locationId)   ;

            if (existingBin != null)
            {
                // Bin already exists, show SweetAlert
                string script = "Swal.fire({ title: 'Error', text: 'The bin already exists.', icon: 'error' });";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", script, true);
            }
            else
            {
                // Bin does not exist, save the new bin
                var newBin = new Bin
                {
                    Bin1 = binName,
                    LocationId = locationId,
                };

                context.Bins.Add(newBin);
                context.SaveChanges();
                context.BinAdjustments.Add(new BinAdjustment
                {
                    BinUid = newBin.Uid,
                    AdjustedDate = DateTime.Now,
                    Bushels = 0,
                    Protein = 0,
                    Comment = "Initial bin setup"
                });
                context.SaveChanges();
                // Redirect or show success message
                string script = "Swal.fire({ title: 'Success', text: 'The bin has been added successfully.', icon: 'success' }).then(function() { window.location = 'Bins.aspx'; });";
                ScriptManager.RegisterStartupScript(this, GetType(), "showalert", script, true);
            }
        }
    }
}
