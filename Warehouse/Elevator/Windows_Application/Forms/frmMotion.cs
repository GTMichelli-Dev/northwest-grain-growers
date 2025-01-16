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
    public partial class frmMotion : Form
    {
        
        
        
        Scales.ScaleData SelectedScale;

        public frmMotion(Scales.ScaleData ScaleToCheck)
        {
            InitializeComponent();
          
            SelectedScale = ScaleToCheck;    
 
        }


        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            UpdateScale();
        }

        private void UpdateScale()
        {


            if (SelectedScale != null)
            {
                string StatusString = "";
                System.Drawing.Color BackColor = System.Drawing.Color.White;
                if (SelectedScale.CurrentStatus == Scales.ScaleData.enumStatus.OK)
                {
                    if (SelectedScale.Motion)
                    {
                        StatusString = "Motion";
                        //BackColor = System.Drawing.Color.Yellow;
                        this.label1.Text = "Wait For Scale To Settle";
                    }
                    else
                    {

                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        this.Close();
                    }
                }
                else
                {
                    this.label1.Text = "Scale Error";
                    BackColor = System.Drawing.Color.Pink;
                    StatusString = SelectedScale.CurrentStatus.ToString();
                }

                lblWeight.Text = string.Format("{0:N0} lbs", SelectedScale.CurWeight);
                lblStatus.Text = StatusString;
                lblWeight.BackColor = BackColor;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
