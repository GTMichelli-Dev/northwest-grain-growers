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
    public partial class frmPassword : Form
    {
        string Pass = string.Empty;

        public frmPassword()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            Pass = Properties.Settings.Default.SetupPassword.ToLower();
        }


        public frmPassword(string Password)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            this.panel1.Enabled = false;
            Pass = Password.ToLower();
            Point p = new Point(Program.FrmMain .Location.X + (Program.FrmMain.Width / 2 - this.Width / 2), Program.FrmMain.Location.Y + (Program.FrmMain.Height / 2 - this.Height / 2));

           
            this.Location = p;
        }




        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (Pass.ToLower()== this.textBox1.Text.ToLower())
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                this.lblError.Visible = true;
            }
        }

        private void Password_Load(object sender, EventArgs e)
        {
            this.textBox1.Focus();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.Opacity < 1)
            {
                Opacity += .2;

            }
            else
            {
                this.timer1.Enabled = false;
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Click(object sender, EventArgs e)
        {

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
