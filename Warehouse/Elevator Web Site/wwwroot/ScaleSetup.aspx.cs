using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ScaleSetup : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (! this.IsPostBack )
        {
            Printing.UpdatePrinters();
            Update_Button();
        }
    
    }

   public void Update_Button()
    {
        using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
        {
            try
            {
                int NotFinished = (int)Q.CountOfScalesNotSet();
                lblNoAdd.Visible = NotFinished > 0;
                btnNewScale.Visible = NotFinished == 0;
            }
            catch
            {
                lblNoAdd.Visible = false;
                btnNewScale.Visible = true;
            }
        }

    }

    protected void btnNewScale_Click(object sender, EventArgs e)
    {
        Scales.AddNewScale();
        this.DataBind();
        Scales.LastScaleModification = DateTime.Now;
        Update_Button();
    }

    protected void txtIPAddress_TextChanged(object sender, EventArgs e)
    {
        TextBox txtIPAddress = (TextBox)sender;
        string Address = txtIPAddress.Text;
        System.Net.IPAddress IPAddress;
        if (!System.Net.IPAddress.TryParse(Address ,out IPAddress))
        {
            
            txtIPAddress.BackColor = System.Drawing.Color.Pink;
        }
        else
        {
            using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
            {
                txtIPAddress.Text = IPAddress.ToString();
                txtIPAddress.BackColor = System.Drawing.Color.White;
                GridViewRow row = (GridViewRow)txtIPAddress.NamingContainer;
                int index = row.RowIndex;
                Guid UID = (Guid)this.GridView1.DataKeys[index].Value;
                try
                {
                    Q.Update_Scale_IpAddress(txtIPAddress.Text, UID);
                    Scales.UpdateScales();
                    Scales.LastScaleModification = DateTime.Now;
                    Update_Button();
                }
                catch
                {

                }
                GridView1.DataBind();

            }


        }
        GridView1.DataBind();

    }

    protected void txtDescription_TextChanged(object sender, EventArgs e)
    {
        using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
        {
            TextBox txtDescription = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtDescription.NamingContainer;
            int index = row.RowIndex;
            Guid UID = (Guid)this.GridView1.DataKeys[index].Value;
            try
            {
                Q.Update_Scale_Description(txtDescription.Text, UID);
                Scales.UpdateScales();
                Update_Button();
                Scales.LastScaleModification = DateTime.Now;
            }
            catch
            {

            }
           GridView1.DataBind();
        }
    }



  
    protected void ddScaleType_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
        {
            DropDownList ddScaleType = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddScaleType.NamingContainer;
            int index = row.RowIndex;
            Guid UID = (Guid)this.GridView1.DataKeys[index].Value;
            try
            {
                Q.Update_Scale_Type(ddScaleType.SelectedValue, UID);
                Scales.UpdateScales();
                Scales.LastScaleModification = DateTime.Now;
                Update_Button();
            }
            catch
            {

            }
            GridView1.DataBind();
        }
    }

    protected void txtPort_TextChanged(object sender, EventArgs e)
    {
        using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
        {
            TextBox txtPort = (TextBox)sender;
            GridViewRow row = (GridViewRow)txtPort.NamingContainer;
            int index = row.RowIndex;
            Guid  UID = (Guid)this.GridView1.DataKeys[index].Value;
            try
            {
                int Port = 0;
                if (int.TryParse(txtPort.Text,out Port ))
                {
                    Q.Update_Scale_Port(Port, UID);
                    Scales.UpdateScales();
                    Update_Button();
                    Scales.LastScaleModification = DateTime.Now;

                }
                
            }
            catch
            {

            }
            GridView1.DataBind();
        }
    }

    protected void ddInboundPrinter_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
        {
            DropDownList ddInboundPrinter = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddInboundPrinter.NamingContainer;
            int index = row.RowIndex;
            Guid UID = (Guid)this.GridView1.DataKeys[index].Value;
            try
            {
                Q.Update_Scale_Printers(ddInboundPrinter.SelectedValue,null,null,null, UID);
                Scales.UpdateScales();
                Scales.LastScaleModification = DateTime.Now;
            }
            catch
            {

            }
            GridView1.DataBind();
        }
    }

    protected void ddOutboundPrinter_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
        {
            DropDownList ddOutboundPrinter = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddOutboundPrinter.NamingContainer;
            int index = row.RowIndex;
            Guid  UID = (Guid)this.GridView1.DataKeys[index].Value;
            try
            {
                Q.Update_Scale_Printers(null, ddOutboundPrinter.SelectedValue,null, null, UID);
                Scales.UpdateScales();
                Scales.LastScaleModification = DateTime.Now;
            }
            catch
            {

            }
            GridView1.DataBind();
        }
    }

   

    protected void ckPrintOut_CheckedChanged(object sender, EventArgs e)
    {
        using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
        {
            CheckBox ckPrintOut = (CheckBox)sender;
            GridViewRow row = (GridViewRow)ckPrintOut.NamingContainer;
            int index = row.RowIndex;
            Guid UID = (Guid)this.GridView1.DataKeys[index].Value;
            try
            {
                Q.Update_Scale_Printers(null,null, null, ckPrintOut.Checked , UID);
                Scales.UpdateScales();
                Scales.LastScaleModification = DateTime.Now;
            }
            catch
            {

            }
            GridView1.DataBind();
        }
    }


    //private  LocalDataSet.Weigh_ScalesRow SelectedScale(Guid UID)
    //{
    //    using (LocalDataSetTableAdapters.Weigh_ScalesTableAdapter weigh_ScalesTableAdapter = new LocalDataSetTableAdapters.Weigh_ScalesTableAdapter())
    //    {
    //        using (LocalDataSet.Weigh_ScalesDataTable weigh_ScalesDataTable = new LocalDataSet.Weigh_ScalesDataTable())
    //        {
    //        //    if (weigh_ScalesTableAdapter.Fill())
    //        }
    //    }
    //}

    protected void ckPrintIn_CheckedChanged(object sender, EventArgs e)
    {
        using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
        {
            CheckBox ckPrintIn = (CheckBox)sender;
            GridViewRow row = (GridViewRow)ckPrintIn.NamingContainer;
            int index = row.RowIndex;
            Guid  UID = (Guid)this.GridView1.DataKeys[index].Value;
            try
            {



                
                Q.Update_Scale_Printers(null, null, ckPrintIn.Checked,null, UID);
                Scales.UpdateScales();
                Scales.LastScaleModification = DateTime.Now;
            }
            catch
            {

            }
            GridView1.DataBind();
        }
    }

   



    protected void SqlDataSource1_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        Update_Button();
        Scales.LastScaleModification = DateTime.Now;
    }

    protected void ddLocation_SelectedIndexChanged(object sender, EventArgs e)
    {
        using (LocalDataSetTableAdapters.QueriesTableAdapter Q = new LocalDataSetTableAdapters.QueriesTableAdapter())
        {
            DropDownList ddLocation = (DropDownList)sender;
            GridViewRow row = (GridViewRow)ddLocation.NamingContainer;
            int index = row.RowIndex;
            Guid UID = (Guid)this.GridView1.DataKeys[index].Value;
            try
            {
                int Location_Id;
                if (int.TryParse(ddLocation.SelectedValue, out Location_Id))
                {
                    Q.Update_Scale_Location(Location_Id , UID);
                    Scales.UpdateScales();
                    Scales.LastScaleModification = DateTime.Now;
                }
            }
            catch
            {

            }
            GridView1.DataBind();
        }
    }
}