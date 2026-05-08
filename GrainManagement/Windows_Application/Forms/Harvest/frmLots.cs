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



    public partial class frmLots : Form
    {



        public class PastDateItems
        {
            public string text { get; set; }
            public int Value { get; set; }
        }


        private bool Wait = true;

        private bool ClosingForm = false;
        private Guid Weight_Sheet_UID = Guid.Empty;
        private long Weight_Sheet_Id;
        private long Lot_Id;
        private bool ReturnToWeightSheet = false;
        public List<PastDateItems> pastDateItems = new List<PastDateItems>();

        public frmLots()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            addPastDateItems();


        }

        public frmLots(Guid Weight_Sheet_To_Move, long Weight_Sheet, long LotNumber)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            addPastDateItems();
            Weight_Sheet_UID = Weight_Sheet_To_Move;
            //this.label3.BackColor = System.Drawing.Color.Red;
            //this.label3.ForeColor = System.Drawing.Color.White;
            this.Weight_Sheet_Id = Weight_Sheet;
            this.Lot_Id = LotNumber;
            ReturnToWeightSheet = true;


            this.label3.Text = string.Format("Select New Lot For Weight Sheet {0} Current Lot:{1}", Weight_Sheet, LotNumber);
            this.btnDone.BackColor = System.Drawing.Color.Red;
            this.btnDone.ForeColor = System.Drawing.Color.White;
            this.btnDone.Text = "Cancel";



        }

        public void addPastDateItems()
        {
            pastDateItems.Add(new PastDateItems { text = "Past 7 Days", Value = 7 });
            pastDateItems.Add(new PastDateItems { text = "Past 30 Days", Value = 30 });
            pastDateItems.Add(new PastDateItems { text = "Past 180 Days", Value = 180 });
            pastDateItems.Add(new PastDateItems { text = "Past 365 Days", Value = 365 });
            pastDateItems.Add(new PastDateItems { text = "Past 10 Years", Value = 3650 });
        }

        private void Lots_Load(object sender, EventArgs e)
        {

            this.lot_Farms_Drop_Down_ListTableAdapter.FillByOpenLots(this.listsDataSet.Lot_Farms_Drop_Down_List, Settings.Location_Id);
            this.lot_Landlords_Drop_Down_ListTableAdapter.FillByOpenLots(this.listsDataSet.Lot_Landlords_Drop_Down_List, Settings.Location_Id);
            this.lot_Crops_Drop_Down_ListTableAdapter.FillByOpenLots(this.nWDataset.Lot_Crops_Drop_Down_List, Settings.Location_Id);
            this.lot_Producer_Drop_Down_ListTableAdapter.FillByOpenLots(this.listsDataSet.Lot_Producer_Drop_Down_List, Settings.Location_Id);

            lotCropsDropDownListBindingSource.ResetBindings(false);
            lotFarmsDropDownListBindingSource.ResetBindings(false);
            lotProducerDropDownListBindingSource.ResetBindings(false);
            lotLandlordsDropDownListBindingSource.ResetBindings(false);
            Reset_Filter();
            
        }
        private void bwLoadLists_DoWork(object sender, DoWorkEventArgs e)
        {
            LoadFilterValues();
        }

        private void bwLoadLists_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void LoadFilterValues()
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            if (ReturnToWeightSheet)
            {

               
                frmHarvest_WS frmHWS = new frmHarvest_WS(Weight_Sheet_UID);

                Display.ShowForm(frmHWS);
                
                

            }
                this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Reset_Filter();
        }


        public void Reset_Filter()
        {
            Wait = true;
            txtLot.Text = "";
            if (this.cboLotStatus.SelectedIndex < 0) { this.cboLotStatus.SelectedIndex = 1; }
            if (cboCrop.Items.Count > 0) this.cboCrop.SelectedIndex = 0;
            if (cboFarm.Items.Count > 0) this.cboFarm.SelectedIndex = 0;
            if (cboLandlord.Items.Count > 0) this.cboLandlord.SelectedIndex = 0;

            if (cboProducer.Items.Count > 0) this.cboProducer.SelectedIndex = 0;
            ddPastDate.Items.Clear();
            foreach (var item in pastDateItems)
            {
                ddPastDate.Items.Add(item.text);
            }
            
            cboLotStatus.SelectedIndex = 1;
            ddPastDate.SelectedIndex = 0;
          
            Wait = false;
            this.ddPastDate.Visible = false;
            UpdateData();
        }

        public void UpdateData()
        {
            if (!Wait && !ClosingForm)
            {
                int Position = this.vwLotsBindingSource1.Position;
                string Crop = null;
                string Producer = null;
                string Farm = null;
                string Landlord = null;
                bool? Closed = null;
                bool? Lot_Sampled = null;
                int? Lot = null;
                int? pastDate = null;

                if (!string.IsNullOrEmpty(txtLot.Text))
                {
                    int ILot;
                    int.TryParse(txtLot.Text, out ILot);
                    Lot = ILot;
                }
                else
                {
                  

                    if (this.ddPastDate.Visible == true)
                    {
                        var text = ddPastDate.SelectedItem.ToString();
                        pastDate = pastDateItems.FirstOrDefault(x => x.text ==text).Value;
                       
                    }

                    if (cboCrop.SelectedIndex > 0) { Crop = cboCrop.SelectedValue.ToString(); }
                    if (cboProducer.SelectedIndex > 0) { Producer = cboProducer.SelectedValue.ToString(); }
                    if (cboFarm.SelectedIndex > 0) { Farm = cboFarm.SelectedValue.ToString(); }
                    if (cboLandlord.SelectedIndex > 0) { Landlord = cboLandlord.SelectedValue.ToString(); }
                    if (cboLotStatus.SelectedIndex == 1)// Open
                    {
                        Closed = false;
                    }
                    else if (cboLotStatus.SelectedIndex == 2)//closed
                    {
                        Lot_Sampled = false;
                        Closed = true;
                    }
                    else if (cboLotStatus.SelectedIndex == 3)//Finished
                    {
                        Lot_Sampled = true;
                        Closed = true;
                    }
                }

                this.vw_LotsTableAdapter1.FillByFilter(this.lotsDataSet.vw_Lots, Crop, Producer, Lot_Sampled, Farm, Landlord, Settings.Location_Id, Closed, pastDate, Lot);
                    
                    
               
                if (Position < this.vwLotsBindingSource1.Count)

                {
                    this.vwLotsBindingSource1.Position = Position;
                }
                //int DateSelectedIndex = 0;
                //if (pnlDate.Visible && DateChanged) { DateSelectedIndex = 1; }
                // this.btnReset.Visible = this.cboProducer.SelectedIndex + this.cboLandlord.SelectedIndex + this.cboCrop.SelectedIndex + this.cboFarm.SelectedIndex + DateSelectedIndex > 0;
            }
        }

        private void cboLotStatus_SelectedIndexChanged(object sender, EventArgs e)
        {


            this.ddPastDate.Visible = this.cboLotStatus.SelectedIndex != 1;
            UpdateData();
            this.dataGridView1.Focus();
        }


        private void filterChanged(object sender, EventArgs e)
        {
           
            UpdateData();
            this.dataGridView1.Focus();
        }

    

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex != this.btnReprintLotLable.Index)
            {

                {
                    SelectLot();

                }

                UpdateData();
            }
        }


        private void SelectLot()
        {
            LotsDataSet.vw_LotsRow SelectedRow;
            SelectedRow = (LotsDataSet.vw_LotsRow)(DataRow)((DataRowView)this.vwLotsBindingSource1.Current).Row;
            if (this.Weight_Sheet_UID == Guid.Empty)
            {
                using (frmLot_Details frm = new frmLot_Details(SelectedRow.UID))
                {
                    frm.ShowDialog();
                    UpdateData();
                    this.dataGridView1.Focus();
                }
            }
            else
            {
                using (frmEdit_Screen frm = new frmEdit_Screen("Why Are You Changing the Lot For Weight Sheet " + this.Weight_Sheet_Id.ToString()))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        Logging.Add_Audit_Trail("Moving Weight Sheet Lot", Weight_Sheet_UID.ToString(), string.Format("Moving Weight Sheet {0}", this.Weight_Sheet_Id), this.Lot_Id.ToString(), SelectedRow.Lot_Number.ToString(), frm.UserInput);
                        using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                        {
                            Q.UpdateWeightSheetLot(SelectedRow.UID, this.Weight_Sheet_UID);
                            this.Close();
                            if (ReturnToWeightSheet)
                            {


                                frmHarvest_WS frmHWS = new frmHarvest_WS(Weight_Sheet_UID);

                                Display.ShowForm(frmHWS);



                            }
                           
                        }
                    }
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex > -1 && e.ColumnIndex == this.btnSelect.Index)
            {

                SelectLot();

            }
            else if (e.RowIndex > -1 && e.ColumnIndex == this.btnReprintLotLable.Index)
            {
                LotsDataSet.vw_LotsRow SelectedRow;
                SelectedRow = (LotsDataSet.vw_LotsRow)(DataRow)((DataRowView)this.vwLotsBindingSource1.Current).Row;
                Printing.PrintSampleLabel(SelectedRow.UID);

            }

            //else if (e.RowIndex > -1 && e.ColumnIndex == this.btnReopenLot.Index)
            //{
            //    LotsDataSet.vw_LotsRow SelectedRow;
            //    SelectedRow = (LotsDataSet.vw_LotsRow)(DataRow)((DataRowView)this.vwLotsBindingSource1.Current).Row;
            //    using (LotsDataSetTableAdapters.QueriesTableAdapter Q= new LotsDataSetTableAdapters.QueriesTableAdapter())
            //    {
            //        Q.ReOpenLot(SelectedRow.UID);
            //        UpdateData();
            //    }

            //}

        }

        private void dataGridView1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            this.dataGridView1.Rows[e.RowIndex].Cells[this.btnReprintLotLable.Index].Value = "Reprint Lot Label";

        }

        private void txtLot_TextChanged(object sender, EventArgs e)
        {


        }

        private void txtLot_Leave(object sender, EventArgs e)
        {
            int Lot = 0;
            if (!int.TryParse(txtLot.Text, out Lot)) txtLot.Text = "";


        }

        private void button2_Click(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            using (frmLot_Details frm = new frmLot_Details())
            {
                Wait = true;
                this.Enabled=false;
                // this.Opacity = 0;
                DialogResult Dr = frm.ShowDialog();
                // this.Opacity = 1;
                this.Enabled = true;
                if (Dr == System.Windows.Forms.DialogResult.OK)
                {
                    Reset_Filter();
                }

            }

        }

        private void frmLots_FormClosing(object sender, FormClosingEventArgs e)
        {
            ClosingForm = true;
        }
    }
}
