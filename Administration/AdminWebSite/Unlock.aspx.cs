using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Unlock : System.Web.UI.Page
{
    static string prevPage = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            prevPage = (Request.UrlReferrer == null) ? "Default.aspx" : Request.UrlReferrer.ToString();
            txtPass.Focus();

            Security.enumSecurityLevel CurLevel = Security.GetUsersSecurityLevel(Session);
            if (CurLevel != Security.enumSecurityLevel.View)
            {
                Response.Redirect(prevPage);
            }
            else
            {
                ddUserLevel.SelectedIndex = 0;
            }
        }
  


    }

    protected void txtPass_TextChanged(object sender, EventArgs e)
    {
        CheckPass();
    }

    protected void lnkOk_Click(object sender, EventArgs e)
    {
        CheckPass();
    }

    public void CheckPass()
    {
        Security.enumSecurityLevel NewLevel = Security.enumSecurityLevel.View;
        string Password = "";
        if (ddUserLevel.SelectedIndex == 0)
        {
            NewLevel = Security.enumSecurityLevel.Supervisor;
            Password = txtPass.Text;
        }
        else if (ddUserLevel.SelectedIndex == 1)
        {
            NewLevel = Security.enumSecurityLevel.Administrator;
            Password = txtPass.Text;
        }

        if (Security.ChangeUserSecurityLevel(Session, NewLevel, Password))
        {

            Response.Redirect(prevPage);
        }
        else
        {
            lblError.Text = "Invalid Password";
            txtPass.Focus();
        }
    }
}