using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Agvantage_Transfer
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
        }

       AgvantageTransfer  AgvTransfer = new AgvantageTransfer(null); 

        private void frmTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            AgvTransfer.Dispose();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {

            AgvTransfer.StartTransfer(@"C:\Program Files\IBM\Client Solutions\Start_Programs\Windows_x86-64\Download.bat", @"C:\Agvantage\DataTransfer", 300,1);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
