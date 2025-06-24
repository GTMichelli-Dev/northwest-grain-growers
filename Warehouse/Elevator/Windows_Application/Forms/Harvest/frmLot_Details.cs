using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NWGrain
{
    public partial class frmLot_Details : Form
    {
        public Guid New_Lot_UID = Guid.Empty;     
     

        bool AllowSettingVariety = false;

        bool frmClosing = false;


        private NWDataset.vw_LotsRow vw_LotsRow = null;


        public Guid Original_Lot_UID = Guid.Empty ;

        public frmLot_Details(Guid Lot_UID )
        {
          //  Loading.Show("Loading Lot Details", Form.ActiveForm);
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            Original_Lot_UID = Lot_UID;
         

            Loading.Close();
        }

        public frmLot_Details()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            EnableLotDetails( );  
        }


        private void EnableLotDetails( )
        {
            var enabled = cbo_Crop.SelectedIndex > -1;
            cboProducer.Enabled = enabled;
            
            cboVariety.Enabled = enabled;
            cboLandlord.Enabled = enabled;
            rtbComment.Enabled = enabled;
            if (!enabled)
            {
                cboLandlord.DataSource=null;
                cboVariety.DataSource=null;

            }
            else
            {
               cboLandlord.DataSource = landlordsBindingSource;
                cboLandlord.DisplayMember = "Landlord";
                cboLandlord.ValueMember = "Landlord";
                using (CropProducerFilterDataSetTableAdapters.CropProducerFilterDetailsTableAdapter cropProducerFilterDetailsTableAdapter = new CropProducerFilterDataSetTableAdapters.CropProducerFilterDetailsTableAdapter())
                {
                    using (CropProducerFilterDataSet cropProducerFilterDataSet = new CropProducerFilterDataSet())
                    {
                        if (cropProducerFilterDetailsTableAdapter.FillByCropDescription(cropProducerFilterDataSet.CropProducerFilterDetails, cbo_Crop.Text) > 0)
                        {
                            DataTable producersTable = cropProducerFilterDataSet.Tables["CropProducerFilterDetails"];
                            DataView sortedView = new DataView(producersTable);
                            sortedView.Sort = "Producer ASC"; // Sort by Producer in ascending order
                            cboProducer.DataSource = sortedView;
                            cboProducer.DisplayMember = "Producer";
                            cboProducer.ValueMember = "Producer_Id";
                       
                        }
                        else
                        {

                            cboProducer.DataSource = producerDropDownListBindingSource;
                            cboProducer.DisplayMember = "Description";
                            cboProducer.ValueMember = "Id";
  
                        }
                        cboVariety.DataSource = VarietyListBindingSource;
                        cboVariety.DisplayMember = "Variety";
                        cboVariety.ValueMember = "Item_Id";
                        
                        if (AllowSettingVariety) SetVariety();


                        cboVariety.SelectedIndex = -1;
                        cboProducer.SelectedIndex = -1;
                        cboLandlord.SelectedIndex = -1;




                    }
                }
            }
        }

        private void Load_Lists()
        {
            foreach (Control ctrl in this.pnlInput.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;
            }
            lblLoading.Text = "Getting Producers";
            Application.DoEvents();



    
            this.producer_Drop_Down_ListTableAdapter.Fill(this.listsDataSet.Producer_Drop_Down_List);
            this.cboProducer.DataSource = this.producerDropDownListBindingSource;
            lblLoading.Text = "Getting Crops";
            Application.DoEvents();
            this.cropVarietyTableAdapter.Fill(this.cropsDataSet.CropVarietyList);
            System.Threading.Thread.Sleep(150);

            this.cropVarietyTableAdapter.FillByDistinctCrops(this.cropsDataSet.CropVarietyList);
            lblLoading.Text = "Getting Landlords";
            Application.DoEvents();
            System.Threading.Thread.Sleep(150);

            this.landlordsTableAdapter.Fill(this.nWDataset.Landlords);
            //if (!Settings.SiteSetup.Is_Seed_Mill )
            //{
            //    this.cropsBindingSource.Filter = " Use_At_Elevator=true";
            //}
            //this.cropsBindingSource.Filter = "Use_At_Elevator=1";
        }



        void ctrl_LostFocus(object sender, EventArgs e)
        {
            if (!frmClosing)
            {
                var ctrl = sender as Control;
                if (ctrl.Tag is Color)
                    ctrl.BackColor = Color.White;

            }
        }

        void ctrl_GotFocus(object sender, EventArgs e)
        {
            if (!frmClosing)
            {
                var ctrl = sender as Control;
                ctrl.Tag = ctrl.BackColor;
                ctrl.BackColor = Color.LightYellow;
            }
        }


        private void frmEdit_Lot_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'varietiesDataSet.CropVarietyList' table. You can move, or remove it, as needed.
         

            tmrCloseLoading.Enabled = true;
            pnlLoading.BringToFront();
           // Loading.Show("Loading Lot Details", Form.ActiveForm);
          
          
          //  Loading.Close();

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
                if (ValidateCropHauler())
                {
                    string Producer = this.cboProducer.Text;
                    string Message = "";
                    System.Windows.Forms.DialogResult DR = System.Windows.Forms.DialogResult.OK;
                    this.Validate();

                    if (string.IsNullOrEmpty(this.cboProducer.Text))
                    {
                        Alert.Show("You Need To Select A Producer", "Select A Producer", false);
                        this.cboProducer.Focus();
                        DR = System.Windows.Forms.DialogResult.Cancel;
                    }
                    else if (string.IsNullOrEmpty(this.cbo_Crop.Text))
                    {
                        Alert.Show("You Need To Select A Crop", "Select A Crop", false);
                        DR = System.Windows.Forms.DialogResult.Cancel;
                        this.cbo_Crop.Focus();
                    }
                    else if (ddState.SelectedIndex < 1)
                    {
                        Alert.Show("You Need To Select A State", "Select A State", false);
                        DR = System.Windows.Forms.DialogResult.Cancel;
                        this.cbo_Crop.Focus();
                    }



                    if (DR == System.Windows.Forms.DialogResult.OK)
                    {

                        this.lotsBindingSource.EndEdit();
                        NWDataset.LotsRow Row;
                        NWDataset.LotsRow OriginalRow = null;
                        Row = (NWDataset.LotsRow)(DataRow)((DataRowView)this.lotsBindingSource.Current).Row;
                        if (Row != null)
                        {
                            if (cboVariety.SelectedIndex < 0 || cboVariety.Text == "")
                            {
                                Row.SetVariety_IdNull();
                            }
                            else
                            {
                                Row.Variety_Id = (int)cboVariety.SelectedValue;
                            }
                        }

                        if (this.vw_LotsRow != null)
                        {
                            using (NWDatasetTableAdapters.LotsTableAdapter LotsTableAdapter = new NWDatasetTableAdapters.LotsTableAdapter())
                            {
                                nWDataset.Audit_Trail.Clear();
                                using (NWDataset.LotsDataTable OriginalTable = new NWDataset.LotsDataTable())
                                {
                                    LotsTableAdapter.FillByUID(OriginalTable, Row.UID);
                                    OriginalRow = OriginalTable[0];

                                    if ((OriginalRow.Producer_Description != this.cboProducer.Text))
                                    {
                                        Message = "Producer";

                                    }


                                    if ((OriginalRow.Crop_Id != Row.Crop_Id))
                                    {
                                        Message = "Crop";

                                    }



                                    if (OriginalRow.IsLandlordNull() && !Row.IsLandlordNull())
                                    {
                                        Message = AddMessageItem(Message, "Landlord");
                                    }
                                    else if (Row.IsLandlordNull() && !OriginalRow.IsLandlordNull())
                                    {
                                        Message = AddMessageItem(Message, "Landlord");
                                    }
                                    else if (!Row.IsLandlordNull() && !OriginalRow.IsLandlordNull())
                                    {
                                        if (Row.Landlord != OriginalRow.Landlord)
                                        {
                                            Message = AddMessageItem(Message, "Landlord");
                                        }
                                    }


                                    if (OriginalRow.IsFSA_NumberNull() && !Row.IsFSA_NumberNull())
                                    {
                                        Message = AddMessageItem(Message, "Farm #");
                                    }
                                    else if (Row.IsFSA_NumberNull() && !OriginalRow.IsFSA_NumberNull())
                                    {
                                        Message = AddMessageItem(Message, "Farm #");
                                    }
                                    if (!Row.IsFSA_NumberNull() && !OriginalRow.IsFSA_NumberNull())
                                    {
                                        if (Row.FSA_Number != OriginalRow.FSA_Number)
                                        {
                                            Message = AddMessageItem(Message, "Farm #");
                                        }
                                    }


                                    if (OriginalRow.IsSplit_NumberNull() && !Row.IsSplit_NumberNull())
                                    {
                                        Message = AddMessageItem(Message, "Split #");
                                    }
                                    else if (Row.IsSplit_NumberNull() && !OriginalRow.IsSplit_NumberNull())
                                    {
                                        Message = AddMessageItem(Message, "Split #");
                                    }
                                    else if (!Row.IsSplit_NumberNull() && !OriginalRow.IsSplit_NumberNull())
                                    {
                                        if (Row.Split_Number != OriginalRow.Split_Number)
                                        {
                                            Message = AddMessageItem(Message, "Split #");
                                        }
                                    }


                                    if (OriginalRow.IsVariety_IdNull() && !Row.IsVariety_IdNull())
                                    {
                                        Message = AddMessageItem(Message, "Variety");
                                    }
                                    else if (Row.IsVariety_IdNull() && !OriginalRow.IsVariety_IdNull())
                                    {
                                        Message = AddMessageItem(Message, "Variety");
                                    }
                                    else if (!OriginalRow.IsVariety_IdNull() && !Row.IsVariety_IdNull())
                                    {
                                        if (Row.Variety_Id != OriginalRow.Variety_Id)
                                        {
                                            Message = AddMessageItem(Message, "Variety");
                                        }
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
                                            if (Row.IsCommentNull()) { Row.Comment = ""; }
                                            Row.Comment += Response;
                                        }
                                    }



                                }

                            }
                        }
                        if (DR == System.Windows.Forms.DialogResult.OK)
                        {
                            if (string.IsNullOrEmpty(Producer))
                            {
                                Alert.Show("Either Select Or Type In a Producer", "Enter A Producer", false);
                                this.cboProducer.Focus();
                                DR = System.Windows.Forms.DialogResult.Cancel;
                            }
                            else
                            {
                                Row.Producer_Description = Producer;
                                if (this.cboProducer.SelectedIndex == -1)
                                {
                                    Row.Producer_Id = 0;

                                }
                                else
                                {
                                    Row.Producer_Id = (int)this.cboProducer.SelectedValue;
                                }

                            }


                            using (ListsDataSet.Producer_Drop_Down_ListDataTable TestTable = new ListsDataSet.Producer_Drop_Down_ListDataTable())
                            {
                                using (ListsDataSetTableAdapters.Producer_Drop_Down_ListTableAdapter ProducersTableAdapter = new ListsDataSetTableAdapters.Producer_Drop_Down_ListTableAdapter())
                                {
                                    if (ProducersTableAdapter.FillByDescription(TestTable, Row.Producer_Description) == 0 && Row.Producer_Id == 0)
                                    {
                                        Alert.Show("Producer Is Not In System", "Verify Producer", false);
                                        {
                                            DR = System.Windows.Forms.DialogResult.Cancel;
                                        }
                                    }

                                }
                            }

                        }
                        if (DR == System.Windows.Forms.DialogResult.OK)
                        {

                            if (vw_LotsRow != null)
                            {
                                if (Message.Contains("Producer"))
                                {
                                    Logging.Add_Audit_Trail("Changed Lot ", Row.Lot_Number.ToString(), " Producer", OriginalRow.Producer_Description, Row.Producer_Description, Row.Comment);
                                }
                                if (Message.Contains("Landlord"))
                                {
                                    string OriginalValue = string.Empty;
                                    string NewValue = string.Empty;
                                    if (!Row.IsLandlordNull()) { NewValue = Row.Landlord; }
                                    if (!OriginalRow.IsLandlordNull()) { OriginalValue = OriginalRow.Landlord; }
                                    Logging.Add_Audit_Trail("Changed Lot ", Row.Lot_Number.ToString(), " Landlord", OriginalValue, NewValue, Row.Comment);
                                }
                                if (Message.Contains("Crop"))
                                {
                                    string OriginalValue = string.Empty;
                                    string NewValue = string.Empty;
                                    NewValue = Row.Crop_Id.ToString();
                                    OriginalValue = OriginalRow.Crop_Id.ToString();
                                    Logging.Add_Audit_Trail("Changed Lot ", Row.Lot_Number.ToString(), " Crop #", OriginalValue, NewValue, Row.Comment);
                                }
                                if (Message.Contains("Farm #"))
                                {
                                    string OriginalValue = string.Empty;
                                    string NewValue = string.Empty;
                                    if (!Row.IsFSA_NumberNull()) { NewValue = Row.FSA_Number; }
                                    if (!OriginalRow.IsFSA_NumberNull()) { OriginalValue = OriginalRow.FSA_Number; }
                                    Logging.Add_Audit_Trail("Changed Lot ", Row.Lot_Number.ToString(), " Farm", OriginalValue, NewValue, Row.Comment);
                                }

                                if (Message.Contains("Split #"))
                                {
                                    string OriginalValue = string.Empty;
                                    string NewValue = string.Empty;
                                    if (!Row.IsSplit_NumberNull()) { NewValue = Row.Split_Number.ToString(); }
                                    if (!OriginalRow.IsSplit_NumberNull()) { OriginalValue = OriginalRow.Split_Number.ToString(); }
                                    Logging.Add_Audit_Trail("Changed Lot ", Row.Lot_Number.ToString(), " Split #", OriginalValue, NewValue, Row.Comment);
                                }

                                if (Message.Contains("Variety"))
                                {
                                    string OriginalValue = string.Empty;
                                    string NewValue = string.Empty;
                                    if (!Row.IsVariety_IdNull()) { NewValue = Row.Variety_Id.ToString(); }
                                    if (!OriginalRow.IsVariety_IdNull()) { OriginalValue = OriginalRow.Variety_Id.ToString(); }
                                    Logging.Add_Audit_Trail("Changed Lot ", Row.Lot_Number.ToString(), " Variety", OriginalValue, NewValue, Row.Comment);
                                }
                                Row.State_Abv = ddState.Text;
                                this.lotsTableAdapter.Update(this.nWDataset);

                            }
                            else
                            {

                                string FSA_Number = null;

                                if (!Row.IsFSA_NumberNull())
                                {
                                    try
                                    {
                                        FSA_Number = Row.FSA_Number;
                                    }
                                    catch
                                    {

                                    }
                                }

                                int? Split = null;

                                if (!Row.IsSplit_NumberNull())
                                {
                                    try
                                    {
                                        Split = Row.Split_Number;
                                    }
                                    catch
                                    {

                                    }
                                }

                                int? Variety = null;
                                if (!Row.IsVariety_IdNull())
                                {
                                    Variety = Row.Variety_Id;
                                }

                                string Landlord = null;
                                if (!Row.IsLandlordNull())
                                {
                                    Landlord = Row.Landlord;
                                }




                                string Comment = null;
                                if (!Row.IsCommentNull())
                                { Comment = Row.Comment; }


                                Row.State_Abv = ddState.Text;


                                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                                {

                                    New_Lot_UID = (Guid)Q.Create_New_Lot(Settings.Location_Id,
                                                                         Row.Crop_Id,
                                                                         Variety,
                                                                         Row.Producer_Id,
                                                                         Row.Producer_Description,
                                                                         Row.Show_Protien_On_WS,
                                                                         Split,
                                                                         FSA_Number,
                                                                         Landlord,
                                                                         Comment);

                                    try
                                    {
                                        Q.UpdateLotState(ddState.Text, New_Lot_UID);
                                    }
                                    catch
                                    {

                                    }
                                }

                            }
                            if (! string.IsNullOrEmpty(this.cboLandlord.Text))
                            {
                                addLandlord(this.cboLandlord.Text);
                            }
                           
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Add_System_Log("frmLot_Details.SaveData", ex.Message.ToString());
                Alert.Show("Something Went Wrong Saving Lot", "Error", false);
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }




     
        private void addLandlord(string Landlord)
        {
            
            Landlord = Landlord.Trim().ToUpper();
            if (!string.IsNullOrEmpty(Landlord))
            {
                using (var context = new NWDataModel())
                {
                    context.Database.CommandTimeout = 240;
                    SqlParameter landlordParam = new SqlParameter("@Landlord", SqlDbType.NVarChar) { Value = Landlord };
                    var query = @"if not exists (select * From Landlords Where Description=@Landlord)
                      begin
                          INSERT INTO Landlords (UID, Description)
                          VALUES (NEWID(), @Landlord)
                      end";
                    context.Database.ExecuteSqlCommand(query, landlordParam);
                }
            }
        }


        private void producer_IdComboBox_Validating(object sender, CancelEventArgs e)
        {
            
         
        }

        private void button2_Click(object sender, EventArgs e)
        {
            NWDataset.LotsRow Row;
            Row = (NWDataset.LotsRow)(DataRow)((DataRowView)this.lotsBindingSource.Current).Row;
            int Id=this.producerDropDownListBindingSource.Find("Id", 0);
            this.producerDropDownListBindingSource.Position = Id;
            Row.Producer_Description = "";
            Set_Producer_Description();
            
        }


        private void Set_Producer_Description()
        {
            if (!frmClosing)
            {
                NWDataset.LotsRow Row;
                Row = (NWDataset.LotsRow)(DataRow)((DataRowView)this.lotsBindingSource.Current).Row;
                if (this.cboProducer.SelectedIndex == -1) { Row.Producer_Id = 0; }
                if (Row.Producer_Id != 0)
                {
                    this.cboProducer.Text = "";
                }
                else
                {
                    this.cboProducer.Text = Row.Producer_Description;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void frmEdit_Lot_Activated(object sender, EventArgs e)
        {
            //if (Startup)
            //{
            //    NWDataset.LotsRow Row;
            //    Row = (NWDataset.LotsRow)(DataRow)((DataRowView)this.lotsBindingSource.Current).Row;
            //    if ((this.vw_LotsRow  != null) && (Row.Producer_Id == 0))
            //    {
            //        this.cboProducer.Text = Row.Producer_Description;
            //    }
            //    Startup = false;
            //    Loading.Close();
            //}
            //this.cboProducer.Focus();
        }

        private void producer_IdComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!frmClosing) UpdateProducerName();
        }


        public void UpdateProducerName()
        {
            if (!frmClosing)
            {
                try
                {
                    int Id = this.producerDropDownListBindingSource.Find("Id", 0);
                    if (cboProducer.SelectedIndex == -1 || cboProducer.SelectedIndex == Id)
                    {
                        this.producerDropDownListBindingSource.Position = Id;
                        this.tmrUpdateProducerName.Start();


                    }
                }
                catch
                {

                }
            }
        }

        private void tmrUpdateProducerName_Tick(object sender, EventArgs e)
        {
            if (!frmClosing )
            {
                if (this.lotsBindingSource.Current != null && this.vw_LotsRow != null)
                {

                    NWDataset.LotsRow Row;
                    Row = (NWDataset.LotsRow)(DataRow)((DataRowView)this.lotsBindingSource.Current).Row;

                    this.cboProducer.Text = Row.Producer_Description;
                }
                else
                {
                    this.cboProducer.Text = "";
                }
            }
                this.tmrUpdateProducerName.Stop();
            
        }

        private void split_NumberTextBox_KeyDown(object sender, KeyEventArgs e)
        {

           
        }

        private void split_NumberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnWeighOut_Click(object sender, EventArgs e)
        {
            DialogResult DR = DialogResult.OK;
            if (vw_LotsRow.IsClose_DateNull())
            {
                if (Alert.Show(string.Format("Close Lot {0} ?", this.vw_LotsRow.Lot_Number), "Close Lot", true) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (this.vw_LotsRow.NotComplete > 0)
                    {
                        using (frmConfirmCloseLot frm = new frmConfirmCloseLot(this.vw_LotsRow.Lot_Number, this.vw_LotsRow.UID))
                        {
                            DR = frm.ShowDialog();
                        }
                    }
                    if (DR == DialogResult.OK)
                    {
                        using (NWDatasetTableAdapters.vw_LotsTableAdapter vw_LotsTableAdapter = new NWDatasetTableAdapters.vw_LotsTableAdapter())
                        {
                            vw_LotsTableAdapter.FillByUID(nWDataset.vw_Lots, this.Original_Lot_UID);
                            this.vw_LotsRow = nWDataset.vw_Lots[0];
                            if (this.vw_LotsRow.NotComplete == 0)
                            {

                                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                                {
                                    Q.Close_Lot(this.Original_Lot_UID);

                                    if (Alert.Show("Print Sample Label?", "Sample Label", true) == DialogResult.Yes)
                                    {

                                        Q.Set_Lot_Sampled(this.Original_Lot_UID);
                                        
                                        Printing.PrintSampleLabel(vw_LotsRow.UID);


                                    }
                                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                                    this.Close();

                                }
                            }
                        }

                    }
                }

            }
            else
            {
                // this.vw_LotsRow = nWDataset.vw_Lots[0];
                //if (NWGrain.Lot.LotHasClosedWeightSheets(this.vw_LotsRow.Lot_Number))
                //{
                //    Alert.Show("You Cannot Re Open Lot Because Weight Sheets Belonging To This Lot Have Had Originals Printed", "Cannot Re Open Lot", false);
                //}
                //else
                {
                    if (Alert.Show(string.Format("Re Open Lot {0} ?", this.vw_LotsRow.Lot_Number), "Re Open Lot", true) == System.Windows.Forms.DialogResult.Yes)
                    {
                        using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                        {
                            Q.ReOpenLot(this.Original_Lot_UID);
                            Q.ClearLotSampled(this.Original_Lot_UID);
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            this.Close();

                        }

                    }
                }
            }
        }


        private void ShowWeightSheets()
        {
            
            if (this.vw_LotsRow.Weight_Sheet_Count == 0)
            {

                Alert.Show(string.Format("No Weight Sheets For Lot {0}", vw_LotsRow.Lot_Number), "No Weight Sheets", false) ;
            }
            else
            {

                NWDataset.LotsRow Row;
                Row = (NWDataset.LotsRow)(DataRow)((DataRowView)this.lotsBindingSource.Current).Row;
                using (frmLot_Weight_Sheets frm = new frmLot_Weight_Sheets(Row.UID))
                {
                    frm.ShowDialog();
                }
            }
        }

        private void btnWeight_Sheets_Click(object sender, EventArgs e)
        {
            ShowWeightSheets(); 
        }

        private void btnSample_Click(object sender, EventArgs e)
        {
           
            Printing.PrintSampleLabel(vw_LotsRow.UID);
            if (Alert.Show("Did Sample Lable Print Ok", "Confirm Print", true) == System.Windows.Forms.DialogResult.Yes)
            {
                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    Q.Set_Lot_Sampled(this.Original_Lot_UID);
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    this.Close();

                }

            }
            

        }

        private void cboVariety_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (this.cboVariety.FindString(this.cboVariety.Text) == -1)
            //{
            //    this.cboVariety.Text = "";
            //}
        }

        private void cbo_Crop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!frmClosing)
            {
              
                    EnableLotDetails();
                   
              
            }
       
        }




        public void SetVariety()
        {
            if (!frmClosing)
            {

                if (this.cropListBindingSource.Current != null && this.lotsBindingSource.Current != null)
                {
                    NWDataset.LotsRow Row = (NWDataset.LotsRow)(DataRow)((DataRowView)this.lotsBindingSource.Current).Row;
                    VarietiesDataSet.CropVarietyListRow     CropRow = (VarietiesDataSet.CropVarietyListRow)(DataRow)((DataRowView)this.cropListBindingSource.Current).Row;
                    this.listsDataSet.Variety_Drop_Down_List.Clear();
                    if (cbo_Crop.SelectedIndex>-1)
                    {
                        this.cropVarietyTableAdapter.FillByCrop_Id (this.varietiesDataSet.CropVarietyList ,(int) this.cbo_Crop.SelectedValue );
                    }
                    
                    if (this.varietiesDataSet.CropVarietyList.Count > 0)
                    {


                        if (!Row.IsVariety_IdNull())
                        {
                            int Position = this.VarietyListBindingSource.Find("Item_Id", Row.Variety_Id);
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
        }



        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;

                this.SelectNextControl((Control)sender, true, true, true, true);
               


            }
        }

        private void frmLot_Details_FormClosing(object sender, FormClosingEventArgs e)
        {
            frmClosing = true;
        }

        private void landlordsBindingSource_CurrentChanged(object sender, EventArgs e)
        {

        }




        public void LoadOriginalLotDetails()
        {
            Load_Lists();
            this.lotsTableAdapter.FillByUID(this.nWDataset.Lots, Original_Lot_UID );
            this.lotsBindingSource.MoveFirst();

            using (NWDatasetTableAdapters.vw_LotsTableAdapter vw_LotsTableAdapter = new NWDatasetTableAdapters.vw_LotsTableAdapter())
            {
                vw_LotsTableAdapter.FillByUID(nWDataset.vw_Lots, Original_Lot_UID);
            }





            NWDataset.LotsRow Row = (NWDataset.LotsRow)(DataRow)((DataRowView)this.lotsBindingSource.Current).Row;


            this.vw_LotsRow = nWDataset.vw_Lots[0];
            bool WeightSheetsOriginalPrinted = Lot.LotHasClosedWeightSheets(vw_LotsRow.Lot_Number);

            if (vw_LotsRow.IsClose_DateNull())
            {
                this.btnClose.Text = "Close Lot";
            }
            else
            {
                this.btnClose.Text = "Open Lot";
                this.btnClose.Visible = (vw_LotsRow.Close_Date - DateTime.Now).Days == 0;
            }
            this.btnSample.Visible = (!vw_LotsRow.Lot_Sampled) && (!vw_LotsRow.IsClose_DateNull());

            if (Row.IsState_AbvNull()) Row.State_Abv = "WA";
            ddState.SelectedIndex = ddState.FindString(Row.State_Abv);

            if (!Settings.CurrentWorkStationLocationRow.IsDefaultStateNull())
            {
                ddState.Enabled = false;
            }




            lblMessage.Visible = WeightSheetsOriginalPrinted;

            bool CanEdit = (vw_LotsRow.IsClose_DateNull()) || (!vw_LotsRow.Lot_Sampled) && (!vw_LotsRow.IsClose_DateNull() && (!WeightSheetsOriginalPrinted));
            this.btnOk.Visible = CanEdit;
            btnSample.Location = btnClose.Location;
            this.cboProducer.Text = Row.Producer_Description;

            this.pnlInput.Enabled = CanEdit;
            AllowSettingVariety = true;
            SetVariety();
        }

        public void LoadNewLotDetails()
        {
            Load_Lists();

            NWDataset.LotsRow Row = nWDataset.Lots.NewLotsRow();//= (NWDataset.LotsRow)(DataRow)((DataRowView)this.lotsBindingSource.Current).Row;
            Row.UID = Guid.NewGuid();
            using (NWDatasetTableAdapters.Site_SetupTableAdapter Site_SetupTableAdapter = new NWDatasetTableAdapters.Site_SetupTableAdapter())
            {
                Site_SetupTableAdapter.Fill(this.nWDataset.Site_Setup, Settings.Location_Id);
                Row.Void = false;
                Row.Lot_Number = 0;
                Row.Location_Id = this.nWDataset.Site_Setup[0].Location_Id;
                if (Settings.CurrentWorkStationLocationRow.IsDefaultStateNull())
                {
                    ddState.SelectedIndex = 0;
                }
                else
                {

                    Row.State_Abv = Settings.CurrentWorkStationLocationRow.DefaultState;
                    ddState.SelectedIndex = ddState.FindString(Row.State_Abv);
                    ddState.Enabled = false;


                }

                Row.Server_Name = Settings.ServerName;
                Row.Sequence_ID = Settings.Sequence.LotSequence;
                Row.Crop_Id = -1;

                Row.Producer_Id = 0;
                Row.Producer_Description = "";
                Row.Show_Protien_On_WS = true;
                Row.Start_Date = DateTime.Now;
                Row.Lot_Sampled = false;

                //this.cboState.SelectedIndex = this.cboState.FindString(Row.State);
                this.lot_NumberLabel1.Visible = false;
                this.lot_NumberLabel.Text = "New Lot";
            }
            this.nWDataset.Lots.AddLotsRow(Row);

            this.lotsBindingSource.MoveFirst();
            this.vw_LotsRow = null;
            this.btnClose.Visible = false;
            this.btnSample.Visible = false;
            this.btnWeight_Sheets.Visible = false;
            this.cboProducer.SelectedIndex = -1;
            AllowSettingVariety = true;
            SetVariety();



            //Taken from Form Activate
            NWDataset.LotsRow lRow;
            lRow = (NWDataset.LotsRow)(DataRow)((DataRowView)this.lotsBindingSource.Current).Row;
            if ((this.vw_LotsRow != null) && lRow != null) //&& (lRow.Producer_Id == 0))
            {
                this.cboProducer.Text = lRow.Producer_Description;
            }
           

        }


        private void tmrCloseLoading_Tick(object sender, EventArgs e)
        {
            tmrCloseLoading.Enabled = false;
            if (Original_Lot_UID == Guid.Empty )
            {
                LoadNewLotDetails();
                
            }
            else
            {
                LoadOriginalLotDetails();
            }
         


         

            pnlLoading.Visible = false;
           
        }

        private void cbo_Crop_Leave(object sender, EventArgs e)
        {
            if (cbo_Crop.SelectedIndex < 0)
            {
                cbo_Crop.Text = "";
            }
            ValidateCropHauler();
        }

        private void cboProducer_Leave(object sender, EventArgs e)
        {
            ValidateCropHauler();
        }

        private bool ValidateCropHauler() {
            if (!string.IsNullOrWhiteSpace(cbo_Crop.Text) && !string.IsNullOrWhiteSpace(cboProducer.Text))
            {


                using (CropProducerFilterDataSetTableAdapters.CropProducerFilterDetailsTableAdapter cropProducerFilterDetailsTableAdapter = new CropProducerFilterDataSetTableAdapters.CropProducerFilterDetailsTableAdapter())
                {
                    using (CropProducerFilterDataSet cropProducerFilterDataSet = new CropProducerFilterDataSet())
                    {
                        if (cropProducerFilterDetailsTableAdapter.FillByCropDescription(cropProducerFilterDataSet.CropProducerFilterDetails, cbo_Crop.Text) > 0)
                        {
                            if (cropProducerFilterDataSet.CropProducerFilterDetails.FirstOrDefault(x => x.Producer.ToUpper() == cboProducer.Text.ToUpper()) == null)
                            {
                                Alert.Show($"{cboProducer.Text} Cannot Supply {cbo_Crop.Text}", "Invalid Grower For Crop", false);
                                return false;
                            }
                            else
                            {
                                return true;
                            }

                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                return true;
            }
         } 
    }
}
