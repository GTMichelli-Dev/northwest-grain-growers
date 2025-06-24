using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NWGrain.Harvest_Lot

{
    public partial class frmSelect_Weight_Sheet_Lot : Form
    {

        bool Wait = true;
    //    public NWDataset.Open_Lots_ListRow CurrentRow = null;
        public NWDataset.Open_Lots_ListRow SelectedRow;
        public Guid Selected_Weight_Sheet_UID;
        public frmSelect_Weight_Sheet_Lot(bool ShowAllWeightSheets=false)
        {
            InitializeComponent();

            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;

            Wait = true;
   
            this.ck_LoadOut.Visible = (Settings.SiteSetup.Has_Loadout);
            this.btnNewLot.Visible = !ShowAllWeightSheets;
            this.openLotsListBindingSource.Position = -1;
            this.lot_Farms_Drop_Down_ListTableAdapter.FillByOpenLots(this.listsDataSet1.Lot_Farms_Drop_Down_List, Settings.Location_Id);
            this.lot_Landlords_Drop_Down_ListTableAdapter.FillByOpenLots(this.listsDataSet1.Lot_Landlords_Drop_Down_List,Settings.Location_Id );
            this.lot_Crops_Drop_Down_ListTableAdapter.FillByOpenLots(this.nWDataset.Lot_Crops_Drop_Down_List, Settings.Location_Id);
            this.lot_Producer_Drop_Down_ListTableAdapter.FillByOpenLots(this.listsDataSet1.Lot_Producer_Drop_Down_List, Settings.Location_Id);
            //this.cboCrop.Visible = true;
            //this.cboFarm.Visible = true;
            //this.cboLandlord.Visible = true;
            //this.cboProducer.Visible = true;
            //lotCropsDropDownListBindingSource.ResetBindings(false);
            //lotFarmsDropDownListBindingSource.ResetBindings(false);
            //lotProducerDropDownListBindingSource.ResetBindings(false);
            //lotLandlordsDropDownListBindingSource.ResetBindings(false);
            Reset_Filter();

        }


        public void Reset_Filter(bool UpdateLists=true )
        {
            Wait = true;
            
            if (cboCrop.Items.Count>0) this.cboCrop.SelectedIndex = 0;
            if (cboFarm.Items.Count > 0) this.cboFarm.SelectedIndex = 0;
            if (cboLandlord.Items.Count > 0) this.cboLandlord.SelectedIndex = 0;
            if (cboProducer.Items.Count > 0) this.cboProducer.SelectedIndex = 0;
            this.btnReset.Visible = false;
            Wait = false;
            
            UpdateData();
        }


        private void Forms_Load(object sender, EventArgs e)
        {

          




        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == this.btnLot.Index)
                {
                  //  SelectRow();
                }
                else if (e.ColumnIndex == this.btnEdit.Index)
                {
                     SelectedRow = (NWDataset.Open_Lots_ListRow)(DataRow)((DataRowView)this.openLotsListBindingSource.Current).Row;
                     using (NWDatasetTableAdapters.Site_SetupTableAdapter Site_SetupTableAdapter = new NWDatasetTableAdapters.Site_SetupTableAdapter())
                     {
                         using (frmLot_Details frm = new frmLot_Details(SelectedRow.UID))
                         {
                             if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                             {
                                 Guid Current_UID = SelectedRow.UID;
                                 Reset_Filter();
                                 this.openLotsListBindingSource.Position = this.openLotsListBindingSource.Find("UID", Current_UID);
                             }
                         }
    

                     }

                }

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Wait = true;
            this.Close();
        }

        private void btnNewLot_Click(object sender, EventArgs e)
        {
            using (frmLot_Details  frm = new frmLot_Details())
            {
                Wait = true;
                this.Hide();
               // this.Opacity = 0;
                DialogResult Dr = frm.ShowDialog();
                // this.Opacity = 1;
                this.Show();
                if (Dr == System.Windows.Forms.DialogResult.OK)
                {
                    Reset_Filter();
                    //if (this.ck_LoadOut.Visible == true && this.ck_LoadOut.Checked==false)
                    //{
                    //    this.ck_LoadOut.Checked= (Alert.Show("Will The Next Weight Sheet Be For A Train Load?","Train Load",true)== System.Windows.Forms.DialogResult.Yes);
                    //}

                    this.open_Lots_ListTableAdapter.Fill(this.nWDataset.Open_Lots_List,null,null, null, null, Settings.Location_Id);
                    //this.openLotsListBindingSource.Position = this.openLotsListBindingSource.Find("UID", frm.New_Lot_UID);

                    var position= this.openLotsListBindingSource.Find("UID", frm.New_Lot_UID);
                    
                    if (position > -1)
                    {
                        this.openLotsListBindingSource.Position = position;
                        // this.openLotsListBindingSource.Position = this.openLotsListBindingSource.Find("UID", Guid.NewGuid());
                        NWDataset.Open_Lots_ListRow row = this.nWDataset.Open_Lots_List.FindByUID(frm.New_Lot_UID);

                        SelectRow();
                    }
                    else
                    {
                        Dr = System.Windows.Forms.DialogResult.Cancel;
                    }
                }

            }
        }

        private void dataGridView1_SizeChanged(object sender, EventArgs e)
        {

        }

        private void cboFullDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void cboFullDescription_TextChanged(object sender, EventArgs e)
        {
          //  btnOk.Visible = this.cboFullDescription.FindString(this.cboFullDescription.Text) > -1 && !string.IsNullOrEmpty(this.cboFullDescription.Text);
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
           
         
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex != this.btnEdit.Index)
                {
                    SelectRow();
                }
               
            }
        }



        private void SelectRow()
        {
        

                SelectedRow = (NWDataset.Open_Lots_ListRow)(DataRow)((DataRowView)this.openLotsListBindingSource.Current).Row;
                using (NWDatasetTableAdapters.Site_SetupTableAdapter Site_SetupTableAdapter = new NWDatasetTableAdapters.Site_SetupTableAdapter())
                {
                    Site_SetupTableAdapter.Fill(nWDataset.Site_Setup, Settings.Location_Id);

                }
                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    bool UseLoadOut = false;
                    if (this.ck_LoadOut.Visible) UseLoadOut = this.ck_LoadOut.Checked;
                    Selected_Weight_Sheet_UID = (Guid)Q.Create_WS(nWDataset.Site_Setup[0].Location_Id, SelectedRow.UID, null, "", null, UseLoadOut);
                    //Added During Harvest so I didnt want to mess with the database 7/21/2006 Sean
                    //They wanted the load comment Information on the Weight Sheet By Default
                    try
                    {
                        string LotComment = (string)Q.Get_Lot_Comment(Selected_Weight_Sheet_UID);
                        Q.Update_Weight_Sheet_Comment(LotComment, Selected_Weight_Sheet_UID);
                    }
                    catch
                    { }
                }
                Wait = true;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();

         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Reset_Filter();
        }

        public void UpdateData()
        {
            if (!Wait )
            {
                string Crop = null;
                string Producer = null;
                string Farm = null;
                string Landlord = null;
              
            
                if (cboCrop.SelectedIndex > 0) { Crop = cboCrop.SelectedValue.ToString(); }
                if (cboProducer.SelectedIndex > 0) { Producer = cboProducer.SelectedValue.ToString(); }
                if (cboFarm.SelectedIndex > 0) { Farm = cboFarm.SelectedValue.ToString(); }
                if (cboLandlord.SelectedIndex > 0) { Landlord = cboLandlord.SelectedValue.ToString(); }
                this.open_Lots_ListTableAdapter.Fill(this.nWDataset.Open_Lots_List,Producer,Crop,Farm,Landlord, Settings.Location_Id);
                this.btnReset.Visible= this.cboProducer.SelectedIndex+this.cboLandlord.SelectedIndex+this.cboCrop.SelectedIndex +this.cboFarm.SelectedIndex>1;

            }
        }

     

        private void cboCrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void cboProducer_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void cboLandlord_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void cboFarm_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void dtDate_CloseUp(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Reset_Filter(false);
        }

        private void frmSelect_Weight_Sheet_Lot_Activated(object sender, EventArgs e)
        {
            Loading.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex>-1 && e.ColumnIndex==btnLot.Index ) SelectRow();
        }
    }
}
