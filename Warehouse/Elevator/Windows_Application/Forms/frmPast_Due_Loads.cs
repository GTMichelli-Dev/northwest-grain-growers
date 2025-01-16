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
    public partial class frmPast_Due_Loads : Form
    {
        private bool IncludeToday = false;
        public DialogResult DR = DialogResult.Cancel;
        public frmPast_Due_Loads(bool ShowToday)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            InitializeComponent();
            IncludeToday = ShowToday; 
            UpdateData();    

        }


        private void UpdateData()
        {
            int Weight_Sheet_ID = -1;
            if (this.openWeightSheetLoadsBindingSource.Position > -1)
            {
                NWDataset.Open_Weight_Sheet_LoadsRow SelectedRow;
                SelectedRow = (NWDataset.Open_Weight_Sheet_LoadsRow)(DataRow)((DataRowView)this.openWeightSheetLoadsBindingSource.Current).Row;
                Weight_Sheet_ID = SelectedRow.Weight_Sheet_Id; 
            }
            if (this.open_Weight_Sheet_LoadsTableAdapter.Fill(this.nWDataset.Open_Weight_Sheet_Loads, IncludeToday, Settings.Location_Id) == 0)
            {
                this.DR  = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                this.openWeightSheetLoadsBindingSource.Position=this.openWeightSheetLoadsBindingSource.Find("Weight_Sheet_Id", Weight_Sheet_ID);
                this.DR = System.Windows.Forms.DialogResult.Cancel;

            }
            
        }


        private void froPast_Due_Loads_Load(object sender, EventArgs e)
        {
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex== this.btnSelectWeightSheet.Index )
            {

                {
                    NWDataset.Open_Weight_Sheet_LoadsRow SelectedRow;
                    SelectedRow = (NWDataset.Open_Weight_Sheet_LoadsRow)(DataRow)((DataRowView)this.openWeightSheetLoadsBindingSource.Current).Row;
                    if (SelectedRow.Load_Type == "I")
                    {
                        using (frmHarvest_WS frmHWS = new frmHarvest_WS(SelectedRow.Weight_Sheet_UID))
                        {
                            if (frmHWS.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                UpdateData();
                            }
                        }
                    }
                    else if (SelectedRow.Load_Type == "T")
                    {
                        using (frmTransfer_WS FTWS = new frmTransfer_WS(SelectedRow.Weight_Sheet_UID))
                        {
                            if (FTWS.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {

                                UpdateData();
                            }
                        }
                    }

                }
            }
        }



        private void frmPast_Due_Loads_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = this.DR;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                NWDataset.Open_Weight_Sheet_LoadsRow SelectedRow;
                SelectedRow = (NWDataset.Open_Weight_Sheet_LoadsRow)(DataRow)((DataRowView)this.openWeightSheetLoadsBindingSource.Current).Row;
                if (SelectedRow.Load_Type == "I")
                {

                    using (frmHarvest_Load frm = new frmHarvest_Load(SelectedRow.Load_UID,false ))
                    {
                        frm.ShowDialog();
                        UpdateData();
                    }
                }
                else if (SelectedRow.Load_Type == "T")
                {

                    using (frmTransfer_Load frm = new frmTransfer_Load(SelectedRow.Weight_Sheet_UID,SelectedRow.Load_UID,false))
                    {
                        frm.ShowDialog();
                        UpdateData();
                    }
                }

            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (this.open_Weight_Sheet_LoadsTableAdapter.Fill(this.nWDataset.Open_Weight_Sheet_Loads, IncludeToday, Settings.Location_Id) == 0)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            }
            this.Close();
        }
    }
}
