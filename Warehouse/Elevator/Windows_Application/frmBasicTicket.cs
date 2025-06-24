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
    public partial class frmBasicTicket : Form
    {

        int CurrentWeight = 0;

        public frmBasicTicket()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            Scales.SetScaleNoManual(ref this.cboScale);

        }

        string CurrentScale;

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            tmrUpdate.Interval = 1000;
            UpdateWeight();
        }


        private void SetScaleDisplay(Scales.ScaleData SelectedScaleData, ref Label lblWeight, ref Label lblStatus)
        {
            string StatusString = "";
            System.Drawing.Color BackColor = System.Drawing.Color.White;
            if (SelectedScaleData.CurrentStatus == Scales.ScaleData.enumStatus.OK || SelectedScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Off)
            {
                if (SelectedScaleData.Motion)
                {
                    StatusString = "Motion";
                    BackColor = System.Drawing.Color.Yellow;
                }
            }
            else
            {
                BackColor = System.Drawing.Color.Pink;
                StatusString = SelectedScaleData.CurrentStatus.ToString();
            }
            CurrentWeight = SelectedScaleData.CurWeight;
            lblWeight.Text = string.Format("{0:N0} lbs", SelectedScaleData.CurWeight);
            lblStatus.Text = StatusString;
            lblWeight.BackColor = BackColor;
        }


        private void UpdateWeight()
        {
            CurrentScale = cboScale.Text;

            if (Scales.CurrentInboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off) Scales.CurrentInboundScaleData = Scales.GetScaleByDescription(CurrentScale);
            
         
            SetScaleDisplay(Scales.CurrentInboundScaleData, ref lblInboundWt, ref lblInStatus);
         
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            using (PrintingTicket frm = new PrintingTicket())
            {
                try
                {
                    frm.Show();
                    System.Windows.Forms.Application.DoEvents();
                    Printing.Print_Basic_Ticket(CurrentWeight, txtDescription.Text);
                    Logging.Add_Audit_Trail("Certified Ticket Printed","",  txtDescription.Text, "", CurrentWeight.ToString(), "");
                }
                catch
                {

                }
            }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void frmBasicTicket_Load(object sender, EventArgs e)
        {
            try
            {
                if (cboScale.Items.Count ==0)
                {
                    Alert.Show("You Dont Have A Scale To Connect To", "No Scale", false);
                    this.Close();
                }
                else
                {
                    
                    if (cboScale.Items.Count > 0)
                    {
                        this.cboScale.SelectedIndex = 0;
                        Scales.CurrentInboundScaleData = Scales.GetScaleByDescription(cboScale.Text);
                        tmrUpdate.Start();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            catch
            {
                this.Close();
            }
          
        }
    }



}
