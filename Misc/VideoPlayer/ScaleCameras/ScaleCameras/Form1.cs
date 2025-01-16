using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScaleCameras
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            axVLCPlugin22.Toolbar = false;
            axVLCPlugin23.Toolbar = false;
       
        }

       
       
        private void VideoResize(object sender, EventArgs e)
        {
            AxAXVLC.AxVLCPlugin2 axVLCPlugin = (AxAXVLC.AxVLCPlugin2)sender;
            
            string Ratio = $"{axVLCPlugin.Width}:{axVLCPlugin.Height}";
            axVLCPlugin.video.aspectRatio = Ratio;
            ResetCrop(axVLCPlugin);


        }


        public void ResetCrop(AxAXVLC.AxVLCPlugin2 axVLCPlugin)
        {
            string Value = string.Format("{0}x{1}+{2}+{3}", (int)numericUpDown1.Value, (int)numericUpDown2.Value,(int)numericUpDown3.Value,(int)numericUpDown4.Value );
            axVLCPlugin.video.crop = Value;
            
        }

        private void axVLCPlugin22_ClientSizeChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            axVLCPlugin22.Visible = false;
            tableLayoutPanel1.ColumnStyles[1].Width=0;
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
        }

        private void btnBoth_Click(object sender, EventArgs e)
        {
            Reset();
        }


        public void Reset()
        {
            axVLCPlugin22.Visible = true;
            axVLCPlugin23.Visible = true;
            tableLayoutPanel1.ColumnStyles[0].Width = 50;
            tableLayoutPanel1.ColumnStyles[1].Width = 50;
        }

        private void btnScale_Click(object sender, EventArgs e)
        {
            Reset();
            axVLCPlugin22.Visible = false;
            tableLayoutPanel1.ColumnStyles[1].Width = 0;
            //  axVLCPlugin22.Visible = true;
            //axVLCPlugin23.Visible = false;
            ////tableLayoutPanel1.ColumnStyles[0].Width = 100;
            //tableLayoutPanel1.ColumnStyles[1].Width = 0;
        }

        private void btnOperations_Click(object sender, EventArgs e)
        {
            Reset();
            axVLCPlugin23.Visible = false;
            tableLayoutPanel1.ColumnStyles[0].Width = 0;

            //axVLCPlugin22.Visible = false;
            //axVLCPlugin23.Visible = true;
            //tableLayoutPanel1.ColumnStyles[0].Width = 0;
            //tableLayoutPanel1.ColumnStyles[1].Width = 50;
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            ResetCrop(this.axVLCPlugin22);
            ResetCrop(this.axVLCPlugin23);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.TopMost = ckStayOnTop.Checked;
        }

        private void ckStayOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = ckStayOnTop.Checked;
        }
    }
}
