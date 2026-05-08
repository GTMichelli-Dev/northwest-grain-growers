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
    public partial class Select_Weight_Sheet : Form
    {
        public Guid Weight_Sheet_UID;

        public  enum enumWeightSheetType { Harvest, Transfer }
        private enumWeightSheetType Weight_Sheet_Type;

        public Select_Weight_Sheet(Guid Current_Weight_Sheet_UID,enumWeightSheetType WeightSheetType)
        {
            InitializeComponent();
            this.Weight_Sheet_Type = WeightSheetType;
            if (this.Weight_Sheet_Type == enumWeightSheetType.Harvest)
            {
                this.vw_Open_Weight_SheetsTableAdapter.FillByValid_Weight_Sheet_Moves(this.nWDataset.vw_Open_Weight_Sheets, Current_Weight_Sheet_UID);
            }
            dataGridView1.ClearSelection();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }



        private void button1_Click(object sender, EventArgs e)
        {
                    if (this.Weight_Sheet_Type== enumWeightSheetType.Harvest )
                    {
                        using (Harvest_Lot.frmSelect_Weight_Sheet_Lot frm = new Harvest_Lot.frmSelect_Weight_Sheet_Lot())
                        {
                            this.Enabled = false;
                            DialogResult DR = frm.ShowDialog();
                            if (DR == System.Windows.Forms.DialogResult.OK)
                            {
                                this.Weight_Sheet_UID  = frm.Selected_Weight_Sheet_UID;
                                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                                this.Close();
                            }

                        }
                    }
                    else
                    {
                        //using (Transfer_Lot.frmNew_Transfer_Weight_Sheet frm = new Transfer_Lot.frmNew_Transfer_Weight_Sheet())
                        //{
                        //    this.Enabled = false;
                        //    DialogResult DR = frm.ShowDialog();
                        //    if (DR == System.Windows.Forms.DialogResult.OK)
                        //    {

                        //    }

                        //    this.Enabled = true;
                        //}
                    }
                }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
             
             
                {
                    SelectRow();
      
                }
         
            }
        }


        private void SelectRow()
        {
            NWDataset.vw_Open_Weight_SheetsRow SelectedRow;
            SelectedRow = (NWDataset.vw_Open_Weight_SheetsRow)(DataRow)((DataRowView)this.vwOpenWeightSheetsBindingSource.Current).Row;
            this.Weight_Sheet_UID = SelectedRow.Weight_Sheet_UID;
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if ((e.RowIndex>-1) && (e.ColumnIndex==this.btnSelect.Index))
            {
                SelectRow();


            }
        }

    }
}
