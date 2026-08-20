using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NWGrain.Transfer_Lot

{
    public partial class frmNew_Transfer_Weight_Sheet : Form
    {
        public Guid New_WS_ID = Guid.Empty ;
        
        public frmNew_Transfer_Weight_Sheet()
        {
            InitializeComponent();


        }


        private void NewLot_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'nWDataset.Carriers' table. You can move, or remove it, as needed.
            this.carriersTableAdapter.FillByActive_cbo_List (this.nWDataset.Carriers);
            // TODO: This line of code loads data into the 'nWDataset.WeighMasters' table. You can move, or remove it, as needed.
            this.weighMastersTableAdapter.Fill(this.nWDataset.WeighMasters);
            
            this.locationsTableAdapter.Fill(this.nWDataset.Locations);
          

            foreach (Control ctrl in this.pnlInput.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;
            }

            this.cropsTableAdapter.Fill(this.nWDataset.Crops);


            this.cboCrop.SelectedIndex = -1;
          
            this.cboLocations.SelectedIndex = -1;
            this.lblErrorCrop.Visible = false;
            this.lblErrorProducer.Visible = false;
            using (NWDatasetTableAdapters.Site_SetupTableAdapter Site_SetupTableAdapter = new NWDatasetTableAdapters.Site_SetupTableAdapter())
            {
                Site_SetupTableAdapter.Fill(this.nWDataset.Site_Setup);
            }
            
            this.cboCrop.Focus();
            ctrl_GotFocus(this.cboCrop, new EventArgs());
            
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

  

        private void crop_IdLabel_Click(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //if (this.Opacity < 1)
            //{
            //    Opacity += .2;

            //}
            //else
            //{
            //    this.timer1.Enabled = false;
            //}
        }

      

        private void cboCrop_Validating(object sender, CancelEventArgs e)
        {
            ValidateComboBox(ref cboCrop,ref lblErrorCrop , ref e);
        }

        private void ValidateComboBox(ref ComboBox cbo,ref Label ErrorLabel, ref CancelEventArgs e)
        {
            if (cbo.SelectedIndex == -1)
            {
                ErrorLabel.Visible = true;
               // cbo.BackColor = Color.Pink;
                cbo.Text = "";
                ErrorLabel.Visible = true;
             //   e.Cancel = true;
            }
            else
            {
                ErrorLabel.Visible = false;
            }

            Check_OK_Button();
        }


        private void Check_OK_Button()
        {
            this.BtnOk.Visible = (this.cboCrop.SelectedIndex != -1 && this.cboLocations.SelectedIndex != -1);
        }


        private void cboProducer_Validating(object sender, CancelEventArgs e)
        {
            ValidateComboBox(ref cboLocations,ref lblErrorProducer , ref e);
        }

        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;

                this.SelectNextControl((Control)sender, true, true, true, true);

            }
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            this.Validate();
            bool Valid = true;
            if (this.cboCrop.SelectedIndex == -1)
            {
                Valid = false;
                
                this.cboCrop.Focus();
                
            }
            else if (this.cboLocations.SelectedIndex == -1)
            {
                Valid = false;
                this.cboLocations.Focus();

            }
            if (Valid)
            {
                using (NWDatasetTableAdapters.Site_SetupTableAdapter Site_SetupTableAdapter=new NWDatasetTableAdapters.Site_SetupTableAdapter())
                {
                    Site_SetupTableAdapter.Fill(nWDataset.Site_Setup);

                }
               // this.NewLot = new Lot();
               // NewLot.LotsRow.Crop_Id = (int)this.cboCrop.SelectedValue;
               
                

                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                // New_Lot_UID=  (Guid) Q.Create_New_Lot(nWDataset.Site_Setup[0].Location_Id, (int)this.cboCrop.SelectedValue, null, (int)this.cboProducer.SelectedValue, this.cboProducer.Text, this.cboState.Text, Show_Protien, Split, FSA_Number,Landlord, Comment);
                }

               
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }




        void ctrl_LostFocus(object sender, EventArgs e)
        {
            var ctrl = sender as Control;
            if (ctrl.Tag is Color)
                ctrl.BackColor = Color.White ;
        }

        void ctrl_GotFocus(object sender, EventArgs e)
        {
            var ctrl = sender as Control;
            ctrl.Tag = ctrl.BackColor;
            ctrl.BackColor = Color.Yellow   ;
        }

        private void cboProducer_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void cboWeighMaster_Validating(object sender, CancelEventArgs e)
        {
            this.lblErrorWeighMaster.Visible = string.IsNullOrEmpty(this.cboWeighMaster.Text);
        }

        private void cboCarrier_Validating(object sender, CancelEventArgs e)
        {
            if (this.cboCarrier.SelectedIndex==-1)this.cboCarrier.Text = "";
        }

       
     


    

        

    

       

    }
}
