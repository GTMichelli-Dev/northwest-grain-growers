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
    public partial class frmSelect_Outbound_Load : Form
    {

        private bool Wait = true;
        private bool DateChanged = false;

        private DateTime FirstDay;
        private DateTime LastDay;

  
        public NWDataset.vw_Outbound_LoadRow SelectedRow;
        public Guid Selected_Weight_Sheet_UID;

        public frmSelect_Outbound_Load()
        {
            InitializeComponent();
            Reset_Filter(true);
            this.vwOutboundLoadBindingSource.Position = -1;
            
        }
        
        public void Reset_Filter(bool UpdateLists=true )
        {
            Wait = true;
            if (UpdateLists)
            {
                this.vw_Outbound_LoadTableAdapter.Fill(this.nWDataset.vw_Outbound_Load);
                this.outbound_Load_CarriersTableAdapter.Fill(this.nWDataset.Outbound_Load_Carriers);
                this.outbound_Load_CropsTableAdapter.Fill(this.nWDataset.Outbound_Load_Crops);
                this.outbound_Load_DestinationsTableAdapter.Fill(this.nWDataset.Outbound_Load_Destinations);
           
            }

            this.FirstDay = new DateTime(DateTime.Now.Year, 1, 1);
            this.LastDay = DateTime.Now;
            this.dtDateStart.Value = FirstDay;
            this.dtDateEnd.Value = LastDay;
            DateChanged = false;
            this.cboCrop.SelectedIndex = 0;
            this.cboCarrier.SelectedIndex = 0;
            this.cboDestination.SelectedIndex = 0;
            this.cboLoadStatus.SelectedIndex = 0;
            this.btnReset.Visible = false;
            Wait = false;
            UpdateData();
        }
        
        private void Forms_Load(object sender, EventArgs e)
        {
        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex == this.Outbound_Load_Id.Index)
            {
                SelectRow();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnNewLoad_Click(object sender, EventArgs e)
        {
            Loading.Show("Creating New Outbound Load");
            using (frmOutbound_Load frm = new frmOutbound_Load())
            {

                DialogResult Dr = frm.ShowDialog();
                if (Dr == System.Windows.Forms.DialogResult.OK)
                {
                    Reset_Filter();
                    this.vwOutboundLoadBindingSource.Position = this.vwOutboundLoadBindingSource.Find("UID", frm.Outbound_Load_UID);

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
                
                
                    SelectRow();
                
               
            }
        }
        
        private void SelectRow()
        {
            SelectedRow = (NWDataset.vw_Outbound_LoadRow )(DataRow)((DataRowView)this.vwOutboundLoadBindingSource.Current).Row;
            using (NWDatasetTableAdapters.Site_SetupTableAdapter Site_SetupTableAdapter = new NWDatasetTableAdapters.Site_SetupTableAdapter())
            {
                Site_SetupTableAdapter.Fill(nWDataset.Site_Setup);

            }


            using (frmOutbound_Load frm = new frmOutbound_Load(SelectedRow.UID ))
            {
                //this.Opacity = 0;
                DialogResult Dr = frm.ShowDialog();
                //this.Opacity = 1;
                if (Dr == System.Windows.Forms.DialogResult.OK)
                {
                    UpdateData();
                    this.vwOutboundLoadBindingSource.Position = this.vwOutboundLoadBindingSource.Find("UID", frm.Outbound_Load_UID);

                }

            }

         

         


        }

        private void button4_Click(object sender, EventArgs e)
        {
            Reset_Filter();
        }

        public void UpdateData()
        {
            if (!Wait)
            {
                string Crop = null;
                
                string Carrier = null;
                string Destination = null;
                

                DateTime StartDate = DateTime.Now.AddYears(-10);
                DateTime EndDate = DateTime.Now;

                if (this.pnlDate.Visible == true)
                {
                    StartDate = this.dtDateStart.Value;
                    EndDate = this.dtDateEnd.Value;
                }
               



                bool? Closed= null;
                if (cboLoadStatus.SelectedIndex == 0)
                {
                    Closed = false;
                }
                else if (cboLoadStatus.SelectedIndex == 1)
                {
                    Closed = true;
                }

                bool Void = cboLoadStatus.SelectedIndex == 2;
                if (cboCrop.SelectedIndex > 0) { Crop = cboCrop.Text; }
                if (cboCarrier.SelectedIndex > 0) { Carrier = cboCarrier.SelectedValue.ToString(); }
                if (cboDestination.SelectedIndex > 0) { Destination = cboDestination.SelectedValue.ToString(); }
                this.vw_Outbound_LoadTableAdapter.FillByFilter(nWDataset.vw_Outbound_Load,Closed,  StartDate, EndDate,Destination,Carrier,Void,Crop  );
                int DateSelectedIndex = 0;
                if (pnlDate.Visible && DateChanged) { DateSelectedIndex = 1; }
                this.btnReset.Visible = this.cboLoadStatus.SelectedIndex + this.cboDestination.SelectedIndex + this.cboCrop.SelectedIndex + this.cboCarrier.SelectedIndex  + DateSelectedIndex>1;
            }
        }

        private void cboCrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void cboLoadStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pnlDate.Visible = this.cboLoadStatus.SelectedIndex != 0;
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

    

      
       
    }
}
