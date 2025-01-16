using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ElevatorCamera
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
          
            axVLCPlugin23.Toolbar = false;

            axVLCPlugin23.playlist.stop();
            axVLCPlugin23.playlist.items.clear();
            axVLCPlugin23.playlist.add("rtsp://admin:Scales123@10.0.60.240/h264/ch1/main/av_stream");
            axVLCPlugin23.playlist.play();
       
        }

       
       
        private void VideoResize(object sender, EventArgs e)
        {
            AxAXVLC.AxVLCPlugin2 axVLCPlugin = (AxAXVLC.AxVLCPlugin2)sender;
            
            string Ratio = $"{axVLCPlugin.Width}:{axVLCPlugin.Height}";
            axVLCPlugin.video.aspectRatio = Ratio;
           


        }

        private void ckTopMost_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = ckTopMost.Checked; 
        }
    }
}
