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
    public partial class frmSupervisor_Selection : Form
    {
        public frmSupervisor_Selection()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (frmSetup frm = new frmSetup())
            {
                //frm.Opacity = 1;
                //frm.TopMost = true;
                frm.ShowDialog();

            }
        
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (frmAudit_Trail frm = new frmAudit_Trail())
            {
                frm.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (frmSiteLog frm = new frmSiteLog())
            {

                frm.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
         
        }

        private void button6_Click(object sender, EventArgs e)
        {
            using (frmProducers frm = new frmProducers())
            {
                frm.ShowDialog();
            }
        }

        private void frmSupervisor_Selection_Load(object sender, EventArgs e)
        {

        }
    }
}
