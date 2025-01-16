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
    public partial class TodaysLoads : Form
    {
        public TodaysLoads()
        {
            InitializeComponent();
            todaysLoadsTableAdapter.Fill(this.nWDataset.TodaysLoads, Settings.Location_Id);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {

                if (e.ColumnIndex == WS_Id.Index)
                {


                    NWDataset.TodaysLoadsRow SelectedRow;
                    SelectedRow = (NWDataset.TodaysLoadsRow)(DataRow)((DataRowView)this.todaysLoadsBindingSource.Current).Row;


                    Loading.Show("Loading Weight Sheet", Form.ActiveForm);
                    if (SelectedRow.LoadType == "Inbound")
                    {
                        frmHarvest_WS frmHWS = new frmHarvest_WS(SelectedRow.Weight_Sheet_UID);
                        
                            Display.ShowForm(frmHWS);
                        
                    }
                    else
                    {
                        frmTransfer_WS frmTWS = new frmTransfer_WS(SelectedRow.Weight_Sheet_UID);
                       Display.ShowForm(frmTWS);
                        

                    }
                  

                }
            }
            Loading.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmLots frm = new frmLots();
            Display.ShowForm(frm);
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.timer1.Stop();
            this.WindowState = FormWindowState.Maximized;
        }

        private void TodaysLoads_Activated(object sender, EventArgs e)
        {
            this.timer1.Start();
        }
    }
}
