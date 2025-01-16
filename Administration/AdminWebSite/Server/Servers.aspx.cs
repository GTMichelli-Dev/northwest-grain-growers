using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Server_Servers : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    private void CheckError(SqlDataSourceStatusEventArgs e)
    {
        if (e.Exception != null)
        {
            lblError.Text = e.Exception.Message;
            lblError.Visible = true;
            e.ExceptionHandled = true;
            hfError_ModalPopupExtender.Show();
        }
    }

    protected void SqlServers_Deleted(object sender, SqlDataSourceStatusEventArgs e)
    {
        CheckError( e);
    }

    protected void SqlServers_Inserted(object sender, SqlDataSourceStatusEventArgs e)
    {
        CheckError(e);
    }

    protected void SqlServers_Updated(object sender, SqlDataSourceStatusEventArgs e)
    {
        CheckError(e);
    }

    protected void SqlServers_Disposed(object sender, EventArgs e)
    {

    }

    protected void SqlServers_Deleting(object sender, SqlDataSourceCommandEventArgs e)
    {

    }

    protected void SqlServers_Updating(object sender, SqlDataSourceCommandEventArgs e)
    {

    }

    protected void SqlServers_Inserting(object sender, SqlDataSourceCommandEventArgs e)
    {

    }

  

    protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void lnkAdd_Click(object sender, EventArgs e)
    {
        InsertNewServer( ((TextBox)GridView1.HeaderRow.Cells[1].FindControl("txtNewServer")).Text) ;
        
    }

    public void InsertNewServer(string ServerName)
    {
        try
        {
            this.SqlServers.InsertParameters[0].DefaultValue = ServerName;
            this.SqlServers.Insert();

            GridView1.DataBind();
            for (int count = 0; count < GridView1.Rows.Count; count++)
            {
                if (((Label)GridView1.Rows[count].FindControl("lblServerName")).Text == ServerName)
                {
                    GridView1.Rows[count].BackColor = System.Drawing.Color.LightGray;


                }
            }
        }
        catch (Exception ex)
        {

            lblError.Text = ex.Message;
            lblError.Visible = true;
            hfError_ModalPopupExtender.Show();

        }
    }




    protected void txtNewServer_TextChanged(object sender, EventArgs e)
    {
        InsertNewServer(((TextBox)GridView1.HeaderRow.Cells[1].FindControl("txtNewServer")).Text);
    }
}