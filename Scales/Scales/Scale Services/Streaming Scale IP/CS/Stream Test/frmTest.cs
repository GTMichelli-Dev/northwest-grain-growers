using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using StreamScaleIP;
namespace Stream_Test
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
            
        }

        delegate void DelegateShowWeightCallback();

   


        public void ShowWeight()
        {

            if (this.InvokeRequired)
            {
                //Wrong Thread
                DelegateShowWeightCallback d = new DelegateShowWeightCallback(ShowWeight);
                try
                {
                    this.Invoke(d, new object[] { });
                }
                catch { }
            }
            else
            {
                this.lblWeight.Text = StreamingScale.CurrentScaleData.CurWeight.ToString();
                this.lblStatus.Text = StreamingScale.CurrentScaleData.CurrentStatus.ToString();  
            }
        }




        private void StreamingScale_ScaleDataRecieved(object sender, EventArgs e)
        {
            ShowWeight();
        }

        private StreamScaleIP.StreamingScale StreamingScale;

       


        private void button1_Click(object sender, EventArgs e)
        {
            if (StreamingScale!= null )
            {
                StreamingScale.Disconnect();
                StreamingScale.Dispose();
                StreamingScale = null;
            }
            try
            {
              StreamingScale = new StreamingScale("Truckscale", txtAddress.Text, (int)numPort.Value );
                StreamingScale.ScaleDataRecieved += StreamingScale_ScaleDataRecieved;
                StreamingScale.Connect(200);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Setting Scale " + ex.Message);
            }


                
        }

        private void frmTest_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void frmTest_FormClosing(object sender, FormClosingEventArgs e)
        {
           try
            {
                if (StreamingScale != null)
                {
                    StreamingScale.Disconnect();
                    StreamingScale.Dispose(); 
                }
            }
            catch
            {

            }
        }
    }
}
