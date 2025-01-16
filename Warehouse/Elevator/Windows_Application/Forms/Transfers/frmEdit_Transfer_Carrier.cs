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
    public partial class frmEdit_Transfer_Carrier : Form
    {
        Guid Current_Weight_Sheet_UID = Guid.Empty;
        int Source_Id;
        int Location_Id;


        public frmEdit_Transfer_Carrier(Guid Weight_Sheet_UID ,int? CarrierId,int SourceId,int LocationId)
        {
            InitializeComponent();
            Source_Id = SourceId;
            Location_Id = LocationId;
            
            foreach (Control ctrl in this.pnlInput.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;
            }


          

            this.btnOk.GotFocus += new EventHandler(btnOk_GotFocus);
            this.btnOk.LostFocus += new EventHandler(btnOk_LostFocus);
            this.btnCancel.GotFocus += new EventHandler(btnCancel_GotFocus);
            this.btnCancel.LostFocus += new EventHandler(btnCancel_LostFocus);

            Load_Lists();
            Current_Weight_Sheet_UID = Weight_Sheet_UID;
            setRate();
            if (CarrierId != null)
            {
                this.cboHauler.SelectedValue = CarrierId;
            }
            else
            {
                this.cboHauler.SelectedIndex = -1;
                
            }
        }








        void btnCancel_LostFocus(object sender, EventArgs e)
        {
            this.btnCancel.BackColor = Color.Red;
            this.btnCancel.ForeColor = Color.White;

        }

        void btnCancel_GotFocus(object sender, EventArgs e)
        {
            this.btnCancel.BackColor = Color.Pink;
            this.btnCancel.ForeColor = Color.Black;

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



        void ctrl_LostFocus(object sender, EventArgs e)
        {
            var ctrl = sender as Control;
            if (ctrl.Tag is Color)
                ctrl.BackColor = Color.White;
        }

        void ctrl_GotFocus(object sender, EventArgs e)
        {
            var ctrl = sender as Control;
            ctrl.Tag = ctrl.BackColor;
            ctrl.BackColor = Color.Yellow;
        }

        private void Load_Lists()
        {
          

            this.carriersTableAdapter.FillByActive_cbo_List(this.nWDataset.Carriers);
            this.cboHauler.SelectedIndex = -1;
        }



        private void cboHauler_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void cboHauler_Validating(object sender, CancelEventArgs e)
        {
            Validate_cbo(ref cboHauler, "INVALID HAULER", "ERROR SETTING HAULER", true, false);
        }

        private void cboHauler_TextUpdate(object sender, EventArgs e)
        {

        }

        private void cboHauler_KeyPress(object sender, KeyPressEventArgs e)
        {


        }

        private void cboHauler_TextChanged(object sender, EventArgs e)
        {
        
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult DR = System.Windows.Forms.DialogResult.OK;





            if (DR == System.Windows.Forms.DialogResult.OK)
            {
                int? Carrier_Id = null;
                string BOL_Type = string.Empty;
                if (cboHauler.SelectedIndex > 0)
                {
                    Carrier_Id = (int)cboHauler.SelectedValue;
                }
                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    var Rate = WeightSheetHaulerRate.GetWeightSheetRate(Current_Weight_Sheet_UID);

                    Q.Update_Transfer_WS_Carrier(this.Current_Weight_Sheet_UID, Carrier_Id,Location_Id,Source_Id  );
                    if (Rate.RateType== WeightSheetHaulerRate.enumRateType.Custom && cboHauler.SelectedIndex > 0)
                    {
                        WeightSheetHaulerRate.UpdateWeightSheetRate(Rate);
                    }
                    

                }
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }



        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;

                this.SelectNextControl((Control)sender, true, true, true, true);
                SetTabStops();


            }
        }


        private void SetTabStops()
        {

            foreach (Control ctrl in this.pnlInput.Controls)
            {
                ctrl.TabStop = string.IsNullOrEmpty(ctrl.Text);

            }


           

        }


      




        private void Validate_cbo(ref ComboBox cbo, string Prompt, string Header, bool AllowEmptyString, bool SetFocus)
        {

            if (cbo.SelectedIndex == -1)
            {
                if ((string.IsNullOrEmpty(cbo.Text) && AllowEmptyString == false) || (!string.IsNullOrEmpty(cbo.Text)))
                {
                    Alert.Show(Prompt, Header);

                    if ((SetFocus) || (!string.IsNullOrEmpty(cbo.Text)))
                    {
                        cbo.Focus();
                    }
                    cbo.Text = "";
                }
            }
        }
        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void frmEdit_WS_Carrier_Load(object sender, EventArgs e)
        {

        }

        private void btnCancelRate_Click(object sender, EventArgs e)
        {
            this.pnlRate.Visible = false;
        }

        private void btnOkRate_Click(object sender, EventArgs e)
        {
            WeightSheetHaulerRate.UpdateWeightSheetRate(new WeightSheetHaulerRate.Rate() { UID = Current_Weight_Sheet_UID, RateAmount = numNewRate.Value, RateType = WeightSheetHaulerRate.enumRateType.Custom });
            setRate();
            
        }

        public void setRate()
        {
            this.pnlRate.Visible = false;
            var rate = WeightSheetHaulerRate.GetWeightSheetRate(Current_Weight_Sheet_UID);
            if (rate == null)
            {
                pnlCustomRate.Visible = false;
                numNewRate.Value = 0;

            }
            else
            {
                if (rate.RateType== WeightSheetHaulerRate.enumRateType.Custom)
                {
                    pnlCustomRate.Visible = true;
                    lblCustomRate.Text = string.Format("{0:N4}", rate.RateAmount);

                }
                else
                {
                    pnlCustomRate.Visible = false;
                    try
                    {
                        numNewRate.Value = rate.RateAmount;
                    }
                    catch
                    {
                        numNewRate.Value = 0;
                    }


                }
            }
        }

        private void btnCustomRate_Click(object sender, EventArgs e)
        {
            pnlRate.Left = 0;
            pnlRate.Top = 0;
            pnlRate.Width = this.Width;
            pnlRate.Height = this.Height;
            this.pnlRate.Visible = true;
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            WeightSheetHaulerRate.UpdateWeightSheetRate(new WeightSheetHaulerRate.Rate() { UID = Current_Weight_Sheet_UID, RateAmount =0, RateType = WeightSheetHaulerRate.enumRateType.NotSet });
            setRate();

        }
    }
}
