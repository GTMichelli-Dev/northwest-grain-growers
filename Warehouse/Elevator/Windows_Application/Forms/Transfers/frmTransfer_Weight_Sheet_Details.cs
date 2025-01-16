using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using  NWGrain.Code;

namespace NWGrain
{
    public partial class frmTransfer_Weight_Sheet_Details : Form
    {
  
        public NWDataset.vwTransfer_Weight_Sheet_InformationRow  vwTransfer_Weight_Sheet_InformationRow = null;
        public Guid Selected_Weight_Sheet_UID;
        bool Startup = true;

        

        bool AllowSettingVariety = false;

        bool frmClosing = false;

        public frmTransfer_Weight_Sheet_Details()
        {
            InitializeComponent();
            Load_Lists();
            this.cbo_Crop.SelectedIndex = -1;
            this.cbo_Crop.Text = "";
            this.lblPrompt.Text = "New Transfer Weight Sheet";
            this.lblWs_ID.Visible = false;
            AllowSettingVariety = true;
            SetVariety();
        }


      


        public frmTransfer_Weight_Sheet_Details(Guid Transfer_Weight_Sheet_UID )
        {
            InitializeComponent();
            Load_Lists();
            this.UpdateData(Transfer_Weight_Sheet_UID);
            this.lblPrompt.Text = "Transfer Weight Sheet";
            this.lblWs_ID.Visible = true;
            this.lblWs_ID.Text = this.vwTransfer_Weight_Sheet_InformationRow.WS_Id.ToString();
            this.lblPrompt.Text = "Transfer #:";
            this.cboSource.Text = vwTransfer_Weight_Sheet_InformationRow.Source;
            this.ck_LoadOut.Checked = vwTransfer_Weight_Sheet_InformationRow.Is_Loadout;
            
            


            int Position = this.cropListBindingSource.Find("Crop_Id", this.vwTransfer_Weight_Sheet_InformationRow.Crop_Id);
            if (Position < 0) { Position = 0; }
            this.cropListBindingSource.Position = Position;
            this.cbo_Crop.SelectedIndex = Position;





            SetVariety();
            
            //this.cbo_Crop.Text = this.vwTransfer_Weight_Sheet_InformationRow.Crop_Description;
            this.cboWeighmaster.Text = this.vwTransfer_Weight_Sheet_InformationRow.Weighmaster;



            

            if (!this.vwTransfer_Weight_Sheet_InformationRow.IsVariety_DescriptionNull()) this.cboVariety.Text = this.vwTransfer_Weight_Sheet_InformationRow.Variety_Description;
            if (!this.vwTransfer_Weight_Sheet_InformationRow.IsCommentNull() )this.rtbComment.Text = this.vwTransfer_Weight_Sheet_InformationRow.Comment;

            AllowSettingVariety = true;
            SetVariety();
        }

        private void UpdateData(Guid Transfer_Weight_Sheet_UID)
        {
            using (NWDatasetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter vwTransfer_Weight_Sheet_InformationTableAdapter = new NWDatasetTableAdapters.vwTransfer_Weight_Sheet_InformationTableAdapter())
            {
                vwTransfer_Weight_Sheet_InformationTableAdapter.FillByWeight_SheetUID(nWDataset.vwTransfer_Weight_Sheet_Information, Transfer_Weight_Sheet_UID);
                vwTransfer_Weight_Sheet_InformationRow = nWDataset.vwTransfer_Weight_Sheet_Information[0];
            }
        }

        private void Load_Lists()
        {
            foreach (Control ctrl in this.pnlInput.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;
            }

            using (NWDatasetTableAdapters.Site_SetupTableAdapter Site_SetupTableAdapter = new NWDatasetTableAdapters.Site_SetupTableAdapter())
            {
                Site_SetupTableAdapter.Fill(nWDataset.Site_Setup, Settings.Location_Id);
            }
            this.ck_LoadOut.Visible = (nWDataset.Site_Setup[0].Has_Loadout);
            this.weighMastersTableAdapter.Fill(this.nWDataset.WeighMasters);
            this.cropVarietyTableAdapter.FillByDistinctCrops(this.cropsDataSet.CropVarietyList);
            //this.cropsTableAdapter.Fill(this.nWDataset.Crops);
            this.locationsTableAdapter.Fill(this.nWDataset.Locations);
            this.cbo_Crop.SelectedIndex = -1;
            this.cboSource.SelectedIndex = -1;
            this.cboVariety.SelectedIndex = -1;
            this.cboWeighmaster.SelectedIndex = -1;
            //if (Settings.SiteSetup.Is_Seed_Mill == false) this.cropsBindingSource.Filter = "Use_At_Elevator=true";
        }



        private void frmEdit_Lot_Load(object sender, EventArgs e)
        {
            



        }

        private string AddMessageItem(string CurrentMessage, string Item)
        {
            if (CurrentMessage.Trim() == "")
            {
                return Item;
            }
            else
            {
                return CurrentMessage+ ", " + Item;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.SaveData();
        }

        private void SaveData()
        {
  
            try
            {
                string Message = "";
                System.Windows.Forms.DialogResult DR = System.Windows.Forms.DialogResult.OK;
                this.Validate();

                if (this.cboWeighmaster.SelectedIndex == -1 && string.IsNullOrEmpty(this.cboWeighmaster.Text))
                {

                    Alert.Show("You Need To Select The WeighMaster", "Select A Weighmaster");
                    this.cboWeighmaster.Focus();
                    DR = System.Windows.Forms.DialogResult.Cancel;

                }
                if (DR == System.Windows.Forms.DialogResult.OK && (this.cboWeighmaster.SelectedIndex == -1) && (!string.IsNullOrEmpty(this.cboWeighmaster.Text)))
                {
                    if (Alert.Show(this.cboWeighmaster.Text + " Does Not Exists" + System.Environment.NewLine + "Create It?", "New Weighmaster", true) == System.Windows.Forms.DialogResult.No)
                    {
                        this.cboWeighmaster.Text = "";
                        this.cboWeighmaster.Focus();
                        DR = System.Windows.Forms.DialogResult.Cancel;


                    }
                    else
                    {
                        string WeighMaster = this.cboWeighmaster.Text;
                        this.nWDataset.WeighMasters.AddWeighMastersRow(Guid.NewGuid(), WeighMaster);
                        this.cboWeighmaster.Text = WeighMaster;
                    }

                }






                if (DR == System.Windows.Forms.DialogResult.OK &&  string.IsNullOrEmpty(this.cboSource.Text))
                {
                    Alert.Show("You Need To Select A Source", "Select A Source", false);
                    this.cboSource.Focus();
                    DR = System.Windows.Forms.DialogResult.Cancel;
                }
                else if (string.IsNullOrEmpty(this.cbo_Crop.Text))
                {
                    Alert.Show("You Need To Select A Crop", "Select A Crop", false);
                    DR = System.Windows.Forms.DialogResult.Cancel;
                    this.cbo_Crop.Focus();
                }

                if (DR == System.Windows.Forms.DialogResult.OK)
                {

                    if (this.vwTransfer_Weight_Sheet_InformationRow != null)
                    {
                        {
                            nWDataset.Audit_Trail.Clear();
                            {
                                if ((this.vwTransfer_Weight_Sheet_InformationRow.Source!=this.cboSource.Text ) )
                                {
                                    Message = "Source";

                                }


                                if ((this.vwTransfer_Weight_Sheet_InformationRow.Crop_Id != (int)this.cbo_Crop.SelectedValue  ))
                                {
                                    Message += "Crop";

                                }




                                if (!String.IsNullOrEmpty(Message.Trim()))
                                {
                                    string Response = User_Input.Get_User_Input("Why Did You Change " + Message);
                                    if (string.IsNullOrEmpty(Response))
                                    {
                                        DR = System.Windows.Forms.DialogResult.Cancel;
                                    }
                                    else
                                    {
                                       
                                        this.rtbComment.Text += Response;
                                    }
                                }



                            }

                        }
                    }
                    if (DR == System.Windows.Forms.DialogResult.OK)
                    {

                        if (vwTransfer_Weight_Sheet_InformationRow != null)
                        {
                            if (Message.Contains("Source"))
                            {
                                Logging.Add_Audit_Trail("Changed Transfer Sheet ", vwTransfer_Weight_Sheet_InformationRow.WS_Id.ToString(), " Source", vwTransfer_Weight_Sheet_InformationRow.Source, this.cboSource.Text, this.rtbComment.Text);
                            }
                            if (Message.Contains("Crop"))
                            {
                                Logging.Add_Audit_Trail("Changed Transfer Sheet ", vwTransfer_Weight_Sheet_InformationRow.WS_Id.ToString(), " Crop", vwTransfer_Weight_Sheet_InformationRow.Crop_Description, this.cbo_Crop.Text, this.rtbComment.Text);
                            }

                            if (this.vwTransfer_Weight_Sheet_InformationRow != null)
                            {
                                int Source_Id = (int)this.cboSource.SelectedValue;
                                int Crop_Id = (int)this.cbo_Crop.SelectedValue;
                                int? Variety_Id = null;
                              

                                if (this.cboVariety.SelectedIndex > 0) Variety_Id = int.Parse(this.cboVariety.SelectedValue.ToString());
                                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                                {
                                    
                                    Q.Update_Weight_Sheet_Transfer_Load(vwTransfer_Weight_Sheet_InformationRow.Weight_Sheet_UID, Source_Id, Crop_Id, Variety_Id);
                                    Q.Update_WS_WeighMaster(this.vwTransfer_Weight_Sheet_InformationRow.Weight_Sheet_UID, this.cboWeighmaster.Text);
                                    Q.Update_WS_Comment(vwTransfer_Weight_Sheet_InformationRow.Weight_Sheet_UID, this.rtbComment.Text);
                                    Q.Update_Weight_Sheet_Rail_Car(vwTransfer_Weight_Sheet_InformationRow.Weight_Sheet_UID, this.ck_LoadOut.Checked);
                                }
                                this.DialogResult = DialogResult.OK;
                                this.Close();

                            }

                        }


                        else
                        {
                            using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                            {
                                using (NWDatasetTableAdapters.Site_SetupTableAdapter Site_SetupTableAdapter = new NWDatasetTableAdapters.Site_SetupTableAdapter())
                                {
                                    int? Variety_Id = null;
                                    if (!string.IsNullOrEmpty(cboVariety.Text)) Variety_Id = int.Parse(this.cboVariety.SelectedValue.ToString());
                                    Site_SetupTableAdapter.Fill(this.nWDataset.Site_Setup, Settings.Location_Id);

                                    bool UseLoadOut = false;
                                    if (this.ck_LoadOut.Visible) UseLoadOut = this.ck_LoadOut.Checked;
                                    Selected_Weight_Sheet_UID = (Guid)Q.Create_Transfer_WS(this.nWDataset.Site_Setup[0].Location_Id, (int)cbo_Crop.SelectedValue, Variety_Id, (int?)this.cboSource.SelectedValue, this.cboWeighmaster.Text, this.rtbComment.Text, UseLoadOut);
                                    this.UpdateData(Selected_Weight_Sheet_UID);
                                }
                            }
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Add_System_Log("frmTransfer_Weight_Sheet_Details.SaveData", ex.Message.ToString());
                Alert.Show("Something Went Wrong Saving Transfer Details", "Error", false);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
          
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
            ctrl.BackColor = Color.LightYellow;
        }

        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;

                this.SelectNextControl((Control)sender, true, true, true, true);
            


            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void cbo_Crop_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (!frmClosing) if (AllowSettingVariety) SetVariety();

        }




        public void SetVariety()
        {
            if (!frmClosing)
            {


                if (this.cbo_Crop.FindString(this.cbo_Crop.Text) == -1)
                {
                    this.cbo_Crop.Text = "";
                }
                try
                {
                    if (this.cropListBindingSource.Current != null)
                    {
                        var Row = vwTransfer_Weight_Sheet_InformationRow;
                        VarietiesDataSet.CropVarietyListRow CropRow = (VarietiesDataSet.CropVarietyListRow)(DataRow)((DataRowView)this.cropListBindingSource.Current).Row;
                        this.listsDataSet.Variety_Drop_Down_List.Clear();
                        if (cbo_Crop.SelectedIndex > -1)
                        {
                            this.cropVarietyTableAdapter.FillByCrop_Id(this.varietiesDataSet.CropVarietyList, (int)this.cbo_Crop.SelectedValue);
                        }

                        if (this.varietiesDataSet.CropVarietyList.Count > 0)
                        {


                            if (Row != null && !Row.IsVariety_DescriptionNull ())
                            {
                                int Position = this.VarietyListBindingSource.Find("Variety", Row.Variety_Description );
                                if (Position < 0) { Position = 0; }
                                this.VarietyListBindingSource.Position = Position;
                                this.cboVariety.SelectedIndex = Position;

                            }
                            else
                            {
                                this.VarietyListBindingSource.Position = 0;
                            }
                        }
                        else
                        {
                            this.VarietyListBindingSource.Position = 0;
                        }

                    }

                }
                catch
                {
                }

            
            }
        }

        private void frmTransfer_Lot_Details_Activated(object sender, EventArgs e)
        {
            if (Startup)
            {
                if (this.vwTransfer_Weight_Sheet_InformationRow == null)
                {

                    this.cboWeighmaster.Focus();
                }
                else
                {
                    this.btnOk.Focus();
                }
                Startup = false;
            }
        }

        private void cboSource_Validated(object sender, EventArgs e)
        {
            if (this.cboSource.FindString(this.cboSource.Text) == -1)
            {
                this.cboSource.Text = "";
            }
        }


        private void cbo_Crop_Validating(object sender, CancelEventArgs e)
        {
         

        }

     

        private void cboVariety_Validating(object sender, CancelEventArgs e)
        {
            if (this.cboVariety.FindString(this.cboVariety.Text) == -1)
            {
                this.cboVariety.Text = "";
            }

        }

        private void cboVariety_SelectedIndexChanged(object sender, EventArgs e)
        {
           // if (!frmClosing) if (AllowSettingVariety) SetVariety();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Loading.Close();
            timer1.Stop();
        }

        private void frmTransfer_Weight_Sheet_Details_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.frmClosing= true;
        }
    }
}
