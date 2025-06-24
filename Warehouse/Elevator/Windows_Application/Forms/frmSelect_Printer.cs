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
    public partial class frmSelect_Printer : Form
    {

        public string PrinterName = string.Empty;
        
        public frmSelect_Printer(string Prompt)
        {
            
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            this.label1.Text = Prompt;
        
            try
            {
                this.weigh_ScalesTableAdapter.Fill(this.nWDataset1.Weigh_Scales,Settings.Location_Id );
                this.cboScale.SelectedIndex = this.cboScale.FindStringExact(Settings.workStation_SetupRow.Weigh_Scale );
                try
                {
                    if (cboScale.SelectedIndex == -1) this.cboScale.SelectedIndex = 0;
                }
                catch
                {

                }

                using (WS.WSSoapClient proxy = new WS.WSSoapClient())
                {
                    WS.LocalDataSet.Site_PrintersDataTable PrinterList = new WS.LocalDataSet.Site_PrintersDataTable();
                    PrinterList = proxy.GetPrinterList();
                    foreach (WS.LocalDataSet.Site_PrintersRow row in PrinterList)
                    {
                      this.cboPrinter.Items.Add(row.Printer_Name);
                    }
                    this.cboPrinter.SelectedIndex = -1;
                }


            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }


        }

        private void btnPtrOut_Click(object sender, EventArgs e)
        {
           
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel ;
            this.Close();

        }

     

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(this.cboPrinter.SelectedIndex>-1)
            {
                PrinterName = this.cboPrinter.Text;  
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();

            }
            else
            {
                Alert.Show("Select A Printer", "Opps");
            }
        }

        private void cboPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboPrinter.SelectedIndex == -1) this.cboPrinter.Text = "";
        }
    }
}
