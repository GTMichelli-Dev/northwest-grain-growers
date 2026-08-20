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
    public partial class SelectBin : Form
    {
        public string SelectedBin;

        public SelectBin(string CurrentBin,string Ticket,bool ShowTicket=false )
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            SelectedBin = CurrentBin;
            this.binListTableAdapter.Fill(this.nWDataset.BinList,Settings.Location_Id );
            
            label1.Text = "Select Bin For Load" + System.Environment.NewLine + Ticket;
            int Index = listBox1.FindString(CurrentBin);
            listBox1.SelectedIndex = Index;
            btnOk.Enabled = Index > -1; 
            
        }




        public SelectBin(string CurrentBin,string SourceDescription,int SourceLocation, string Ticket, bool ShowTicket = false)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            SelectedBin = CurrentBin;
            this.binListTableAdapter.Fill(this.nWDataset.BinList, SourceLocation);
            label1.BackColor = Color.LightYellow;
            label1.ForeColor = Color.Black;  
            label1.Text = "Select Source Bin" + System.Environment.NewLine + Ticket;
            int Index = listBox1.FindString(CurrentBin);
            listBox1.SelectedIndex = Index;
            btnOk.Enabled = Index > -1;

        }

        public void AcceptBin()
        {
            if (listBox1.SelectedIndex == -1)
            {
                SelectedBin = string.Empty;
                this.DialogResult = DialogResult.Cancel;
            }
            else
            {
                SelectedBin = listBox1.Text;
                this.DialogResult = DialogResult.OK;
            }

            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AcceptBin();
         
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnOk.Enabled = listBox1.SelectedIndex>-1;
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            AcceptBin();
        }
    }
}
