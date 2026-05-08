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
    public partial class PrintingTicket : Form
    {
        public PrintingTicket( string Prompt = "")
        {


          
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            if (!string.IsNullOrEmpty(Prompt))
            {
                this.label1.Text = Prompt;
            }



         

            Point p = new Point(Globals.MainPoint.X + (Program.FrmMain.Width / 2 - this.Width / 2), Globals.MainPoint.Y + (Program.FrmMain.Height / 2 - this.Height / 2));
            

            this.Location = p;
        }

        public void SetPrompt(string Prompt)
        {
            if (!string.IsNullOrEmpty(Prompt))
            {
                this.label1.Text = Prompt;
            }

        }
    }
}
