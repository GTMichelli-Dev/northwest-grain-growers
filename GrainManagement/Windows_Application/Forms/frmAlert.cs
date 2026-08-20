using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NWGrain
{
    public partial class frmAlert : Form
    {
        public frmAlert(string Prompt,string Header="",bool ShowYesNo_Buttons = false)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            this.lblHeader.Text = Header;
            this.lblPrompt.Text = Prompt;
            this.btnOk.Visible = !ShowYesNo_Buttons;
            this.btnYes.Visible = ShowYesNo_Buttons;
            this.btnNo.Visible = ShowYesNo_Buttons;
            //this.btnOk.GotFocus += new EventHandler(btnOk_GotFocus);
            //this.btnOk.LostFocus += new EventHandler(btnOk_LostFocus);
            //this.btnNo.GotFocus += new EventHandler(btnCancel_GotFocus);
            //this.btnNo.LostFocus += new EventHandler(btnCancel_LostFocus);
            //this.btnYes.LostFocus += new EventHandler(btnYes_LostFocus);
            //this.btnYes.GotFocus += new EventHandler(btnYes_GotFocus);
            if (btnYes.Visible)
            {
                this.AcceptButton = btnYes;
                this.CancelButton = btnNo;

            }
        }


        public static Control FindFocusedControl(Control control)
        {
            var container = control as IContainerControl;
            while (container != null)
            {
                control = container.ActiveControl;
                container = control as IContainerControl;
            }
            return control;
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Left || keyData == Keys.Right )
            {
                SelectNextControl(FindFocusedControl(this),true,true,true,true);
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }


        void btnYes_LostFocus(object sender, EventArgs e)
        {

            this.btnYes.BackColor = Color.SeaGreen;
            this.btnYes.ForeColor = Color.White;
        }

        void btnYes_GotFocus(object sender, EventArgs e)
        {
            this.btnYes.BackColor = Color.Lime;
            this.btnYes.ForeColor = Color.Black;
        }

  
        void btnCancel_LostFocus(object sender, EventArgs e)
        {
            this.btnNo.BackColor = Color.Red;
            this.btnNo.ForeColor = Color.White;

        }

        void btnCancel_GotFocus(object sender, EventArgs e)
        {
            this.btnNo.BackColor = Color.Pink;
            this.btnNo.ForeColor = Color.Black;

        }



        private void OK_Enter(object sender, EventArgs e)
        {
            this.pnlOk.BorderStyle = BorderStyle.FixedSingle;
            ((Button)sender).BackColor = System.Drawing.Color.Lime;
            ((Button)sender).ForeColor = System.Drawing.Color.Black;
        }

        private void OK_Leave(object sender, EventArgs e)
        {
            this.pnlOk.BorderStyle = BorderStyle.None;
            ((Button)sender).BackColor = System.Drawing.Color.SeaGreen;
            ((Button)sender).ForeColor = System.Drawing.Color.White;
        }


        private void Yes_Enter(object sender, EventArgs e)
        {
            this.pnlYes.BorderStyle = BorderStyle.FixedSingle  ;
            
            ((Button)sender).BackColor = System.Drawing.Color.Lime;
            ((Button)sender).ForeColor = System.Drawing.Color.Black;
        }

        private void Yes_Leave(object sender, EventArgs e)
        {
            this.pnlYes.BorderStyle = BorderStyle.None;
            ((Button)sender).BackColor = System.Drawing.Color.SeaGreen;
            ((Button)sender).ForeColor = System.Drawing.Color.White;
            
        }


        private void No_Enter(object sender, EventArgs e)
        {
            this.pnlNo.BorderStyle = BorderStyle.FixedSingle;
        }

        private void No_Leave(object sender, EventArgs e)
        {
            this.pnlNo.BorderStyle = BorderStyle.None ;
        }


        void btnOk_LostFocus(object sender, EventArgs e)
        {

            this.btnOk.BackColor = Color.SeaGreen;
            this.btnOk.ForeColor = Color.White;
        }

        void btnOk_GotFocus(object sender, EventArgs e)
        {
            this.btnOk.BackColor = Color.Lime;
            this.btnOk.ForeColor = Color.Black;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No ;
            this.Close();
        }

        private void SelectNext_KeyDown(object sender, KeyEventArgs e)
        {
       
        }

        private void btnNo_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

     
    }
}
