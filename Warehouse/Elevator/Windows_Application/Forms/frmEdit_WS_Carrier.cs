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
    public partial class frmEdit_WS_Carrier : Form
    {

        public bool Startup;

        Guid Current_Weight_Sheet_UID = Guid.Empty;

        /// <summary>
        /// Used for and inbound load
        /// </summary>
        /// <param name="_Open_Weigh_SheetsRow"></param>
        public frmEdit_WS_Carrier(Guid Weight_Sheet_UID,int? CarrierId , string Bol_Type,int? Miles )
        {
            Current_Weight_Sheet_UID = Weight_Sheet_UID;
            Startup = true;
            InitializeComponent();
            foreach (Control ctrl in this.pnlInput.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;
            }
            var rate = WeightSheetHaulerRate.GetWeightSheetRate(Current_Weight_Sheet_UID);

            foreach (Control ctrl in this.pnlMilage.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;
            }

            this.btnOk.GotFocus += new EventHandler(btnOk_GotFocus);
            this.btnOk.LostFocus += new EventHandler(btnOk_LostFocus);
            this.btnCancel.GotFocus += new EventHandler(btnCancel_GotFocus);
            this.btnCancel.LostFocus += new EventHandler(btnCancel_LostFocus);
         
            Load_Lists();

            rate = WeightSheetHaulerRate.GetWeightSheetRate(Current_Weight_Sheet_UID);
            if (CarrierId != null)
            {
                this.cboHauler.SelectedValue = CarrierId;
            }
            else
            {
                this.cboHauler.SelectedIndex = -1;
                //this.pnlMilage.Visible = false;
            }
            //if (! string.IsNullOrEmpty( Bol_Type))
            //{
            //    this.cboBOLtype.SelectedValue = Bol_Type;
            //}
            //else
            //{
            //    this.cboBOLtype.SelectedIndex = -1;

            //}
            if (Miles != null)
            {
                this.cboMiles.SelectedIndex = this.cboMiles.FindString(Miles.ToString());
            }
            else
            {
                this.cboMiles.SelectedIndex = -1;
            }

            setRate();
            Startup = false;
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
            this.bol_TypeTableAdapter.Fill(this.nWDataset.Bol_Type);

            this.carriersTableAdapter.FillByActive_cbo_List(this.nWDataset.Carriers);
            for (int i = 1; i < 91; i++)
            {
                this.cboMiles.Items.Add(i.ToString());
            }
            this.cboMiles.SelectedIndex = -1;
            this.cboHauler.SelectedIndex = -1;
        }



        private void cboHauler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
            {


                bool WasVisible = this.pnlMilage.Visible;
                this.pnlMilage.Visible = this.cboHauler.SelectedIndex > 0;
                if (!WasVisible || this.pnlMilage.Visible == false)
                {
                    this.cboMiles.SelectedIndex = -1;
                    this.cboBOLtype.SelectedIndex = -1;
                }
                this.btnCustomRate.Visible = this.cboHauler.SelectedIndex > -1;
                if (!Startup) UpdateRate();
            }
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
            this.pnlMilage.Visible = this.cboHauler.FindStringExact(this.cboHauler.Text) > -1;
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
            var Rate = WeightSheetHaulerRate.GetWeightSheetRate(Current_Weight_Sheet_UID);

            if ((this.cboHauler.SelectedIndex >0) && DR == System.Windows.Forms.DialogResult.OK &&   Rate.RateType != WeightSheetHaulerRate.enumRateType.Custom)
            {
                if (cboBOLtype.SelectedIndex == -1)
                {
                    Alert.Show("You Need To Select The BOL Type", "Select A BOL Type");
                    this.cboBOLtype.Focus();
                    DR = System.Windows.Forms.DialogResult.Cancel;
                }
                else if (cboMiles.SelectedIndex == -1)
                {
                    Alert.Show("You Need To Select The Hauler Miles", "Select Miles");
                    this.cboMiles.Focus();
                    DR = System.Windows.Forms.DialogResult.Cancel;
                }
            }

      

         
            if (DR == System.Windows.Forms.DialogResult.OK)
            {
        
                
        
                int? Carrier_Id = null;
                string BOL_Type = string.Empty;

                int? Miles = null;
                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    if (cboHauler.SelectedIndex > 0)
                    {
                        Carrier_Id = (int)cboHauler.SelectedValue;
                        if (Rate.RateType != WeightSheetHaulerRate.enumRateType.Custom)
                        {
                            BOL_Type = this.cboBOLtype.SelectedValue.ToString();
                            Miles = Convert.ToInt32(this.cboMiles.Text);
                            Q.Update_WS_Carrier(this.Current_Weight_Sheet_UID, Carrier_Id, BOL_Type, Miles, Settings.Location_Id);
                        }
                        else
                        {
                            Q.Update_WS_Carrier(this.Current_Weight_Sheet_UID, Carrier_Id, "A", 0, Settings.Location_Id);
                            WeightSheetHaulerRate.UpdateWeightSheetRate(Rate);

                        }


                    }
                    else
                    {
                        Q.Update_WS_Carrier(this.Current_Weight_Sheet_UID, null, null, null, Settings.Location_Id);
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


            foreach (Control ctrl in this.pnlMilage.Controls)
            {
                ctrl.TabStop = string.IsNullOrEmpty(ctrl.Text);

            }

        }


        private void cboBOLtype_Validating(object sender, CancelEventArgs e)
        {
            Validate_cbo(ref cboBOLtype, "INVALID BOL TYPE", "ERROR SETTING BOL TYPE", true, false);
           
        }

        private void cboMiles_Validating(object sender, CancelEventArgs e)
        {
            Validate_cbo(ref cboMiles, "INVALID HAULER MILES", "ERROR SETTING HAULER MILES", true, false);
           
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

        private void btnCustomRate_Click(object sender, EventArgs e)
        {
            pnlRate.Left = 0;
            pnlRate.Top = 0;
            pnlRate.Width = this.Width;
            pnlRate.Height = this.Height;
            this.pnlRate.Visible = true;
        }

        private void btnOkRate_Click(object sender, EventArgs e)
        {
            WeightSheetHaulerRate.UpdateWeightSheetRate(new WeightSheetHaulerRate.Rate() { UID = Current_Weight_Sheet_UID, RateAmount = numNewRate.Value, RateType = WeightSheetHaulerRate.enumRateType.Custom });
            setRate();
        }


        public void setRate()
        {
            if (cboHauler.SelectedIndex == -1)
            {
                btnCustomRate.Visible = false;
                pnlCustomRate.Visible = false;
                cboBOLtype.SelectedIndex = 0;
                numNewRate.Value = 0;

            }
            else
            {
                btnCustomRate.Visible = true;

                this.pnlRate.Visible = false;
                var rate = WeightSheetHaulerRate.GetWeightSheetRate(Current_Weight_Sheet_UID);
                if (rate == null)
                {
                    pnlCustomRate.Visible = false;
                    cboBOLtype.SelectedIndex = 0;
                    numNewRate.Value = 0;

                }
                else
                {
                    if (rate.RateType == WeightSheetHaulerRate.enumRateType.Custom)
                    {

                        pnlCustomRate.Visible = true;
                        pnlCustomRate.Left = pnlMilage.Left;
                        pnlCustomRate.Top = pnlMilage.Top;
                        pnlCustomRate.Width = pnlMilage.Width;
                        pnlCustomRate.Height = pnlMilage.Height;
                        lblCustomRate.Text = string.Format("{0:N4}", rate.RateAmount);

                    }
                    else
                    {
                        pnlCustomRate.Visible = false;
                        var BolType = WeightSheetHaulerRate.GetBolType(rate.RateType);
                        if (BolType != null) cboBOLtype.SelectedValue = BolType;
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
        }

        private void btnCancelRate_Click(object sender, EventArgs e)
        {
            this.pnlRate.Visible = false;
        }

        private void Reset_Click(object sender, EventArgs e)
        {
            WeightSheetHaulerRate.UpdateWeightSheetRate(new WeightSheetHaulerRate.Rate() { UID = Current_Weight_Sheet_UID, RateAmount = 0, RateType = WeightSheetHaulerRate.enumRateType.NotSet });
           UpdateRate();
            
        }

        private void numNewRate_ValueChanged(object sender, EventArgs e)
        {

        }


        public void UpdateRate()
        {
            if (!Startup)
            {
                if (cboHauler.SelectedIndex < 1)
                {
                    WeightSheetHaulerRate.UpdateWeightSheetRate(new WeightSheetHaulerRate.Rate() { UID = Current_Weight_Sheet_UID, RateAmount = 0, RateType = WeightSheetHaulerRate.enumRateType.NotSet });
                }
                //else
                //{
                //    var rate = WeightSheetHaulerRate.GetWeightSheetRate(Current_Weight_Sheet_UID);
                //    if (rate.RateType==)
                //}

                setRate();
            }
        }

        private void cboBOLtype_Click(object sender, EventArgs e)
        {
            UpdateRate();
        }

        private void cboMiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRate();
        }
    }
}
