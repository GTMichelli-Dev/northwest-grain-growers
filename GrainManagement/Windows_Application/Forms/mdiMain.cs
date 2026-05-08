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
    public partial class mdiMain : Form
    {
        private bool UpdateChecked = false;

        public mdiMain()
        {
            InitializeComponent();
            this.Text = string.Format("{0}    {1}", Application.ProductName, Application.ProductVersion);
        }

        private void mdiMain_Load(object sender, EventArgs e)
        {
           Program.FrmMain = new NWGrain.frmMain();

            Program.FrmMain.ShowIcon = false;
            Program.FrmMain.ControlBox = false;
            Program.FrmMain.MaximizeBox = true;
            Program.FrmMain.MinimizeBox = false;
            Program.FrmMain.FormBorderStyle = FormBorderStyle.None ;
            Program.FrmMain.ShowInTaskbar = false;
            Program.FrmMain.Text = "";
            Program.FrmMain.MdiParent = Program.frmMdiMain;
           

            Program.FrmMain.Show();
            Program.FrmMain.WindowState = FormWindowState.Maximized;

        }

        private void mdiMain_Activated(object sender, EventArgs e)
        {
          
            if (!UpdateChecked )
            {

                if (Settings.CheckUpdateAtStartup && (AppUpdater.UpdateInfo.CheckUpdates)) CheckForUpdate();
                UpdateChecked = true;
            }
            
            //this.WindowState = FormWindowState.Normal;
            //this.Show();
            this.BringToFront();
            this.TopMost=false;
        }

        private void CheckForUpdate()
        {
           
        }

       
    }
}
