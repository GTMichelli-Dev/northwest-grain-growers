using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;


namespace WeightSimulator
{
    public partial class Form1 : Form
    {
        private int DefaultX;


        private int CurrentWeight = 0;
        private bool Motion = false;

        private bool TruckEmpty=false;

        private Point MouseDownLocation;

        private WebService.LocalDataSet.Weigh_ScalesDataTable weigh_ScalesDataTable;

        private string SelectedScale = "";


        private bool Cancel = false;

        public Form1()
        {
            InitializeComponent();
            DefaultX = Truck.Left;
            this.Text += $" vers {Application.ProductVersion}";
            this.Truck.Image = Properties.Resources.FullTruck ;
            SetWeight();


            using (WebService.LocalWebService proxy = new WebService.LocalWebService())
            {

                 weigh_ScalesDataTable = proxy.GetScales();
                foreach(WebService.LocalDataSet.Weigh_ScalesRow  scale in weigh_ScalesDataTable )
                {
                    
                    ddScales.Items.Add(scale.Description);
                }
            
            }
            if (ddScales.Items.Count >0)
            {
                
                ddScales.SelectedIndex=0;
                SelectedScale = ddScales.Text;
                bwScaleUpdate.RunWorkerAsync();
            }
            

        }


        private void Truck_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                Truck.Left = e.X + Truck.Left - MouseDownLocation.X;
                int truckRight = Truck.Left + Truck.Width;
                int scaleLeft = pnlScale.Left;
                int scaleRight = pnlScale.Left + pnlScale.Width;
                int truckFront = Truck.Left + 26;
                int truckFront2 = Truck.Left + 40;
                int truckBack = Truck.Left + 190;
                int truckBack2 = Truck.Left + 171;
                int pOnFrontend =pnlScale.Left - truckFront2;
                int pOnFrontstart = pnlScale.Left - truckFront;
                bool truckFrontOn = (truckFront >= scaleLeft && truckFront <= scaleRight);
                bool truckFrontOn2 = (truckFront2 >= scaleLeft && truckFront2 <= scaleRight);
              
                bool truckBackOn = (truckBack >= scaleLeft && truckBack <= scaleRight);
                bool truckBackOn2 = (truckBack2 >= scaleLeft && truckBack2 <= scaleRight);
                int scaleStart = pnlScale.Left;
                int scaleEnd = pnlScale.Left + pnlScale.Width;
                
                var frontHalfOn = (truckFrontOn ^ truckFrontOn2);
                var backHalfOn = (truckBackOn ^ truckBackOn2);
                var onScale= (truckFrontOn && truckFrontOn2 && truckBackOn && truckBackOn2);
                decimal frontWheelEnd = (truckFront2<scaleStart)?scaleStart :truckFront2 ;
                decimal frontWheelStart = (truckFront > scaleEnd) ? scaleEnd : truckFront;
                decimal frontPercent = (frontHalfOn) ? ( frontWheelStart/ frontWheelEnd) : 1;


                decimal backWheelEnd = (truckBack2 < scaleStart) ? scaleStart : truckBack2;
                decimal backWheelStart = (truckBack > scaleEnd) ? scaleEnd : truckBack;
                decimal backPercent = (backHalfOn) ? ( backWheelEnd/ backWheelStart) : 1;

                decimal Weight = (TruckEmpty) ? numEmptyWt.Value :numFullWt.Value;
                if (truckFrontOn || truckFrontOn2 || truckBackOn || truckBackOn2)
                {
                    if (frontHalfOn)
                    {
                        Weight = ((Weight / frontPercent) /2 );
                    }
                    else if (backHalfOn)
                    {
                        Weight = ((Weight / backPercent) /2 );
                    }
                 
                    else if (!truckFrontOn && !truckFrontOn2)
                    {
                        Weight = (!TruckEmpty ) ? (Weight / 2) : (Weight / 2) * (decimal)0.82;
                    }
                    else if (!truckBackOn && !truckBackOn2)
                    {
                        Weight = (!TruckEmpty) ? (Weight / 2) * (decimal)0.82 : (Weight / 2);
                    }
                }
                else if (!onScale)
                {
                    Weight = 0;
                }

                Weight = Weight / 10;
                

                CurrentWeight = (int)Weight*10;
                if (CurrentWeight > 0)
                {
                   

                    Motion = true;
                    timer1.Stop();
                    timer1.Start();
                }

               // SetWeight();



 
                
            }
        }

        private void Truck_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                MouseDownLocation = e.Location;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Truck.Left = DefaultX;
            this.Truck.Image = Properties.Resources.EmptyTruck ;
            TruckEmpty = true;
            CurrentWeight = 0;
            Motion = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Truck.Left = DefaultX;
            this.Truck.Image = Properties.Resources.FullTruck;
            TruckEmpty = false;
            CurrentWeight = 0;
            Motion = false;
        }

        private void Truck_MouseHover(object sender, EventArgs e)
        {
            //Point P = PointToScreen(new Point(pictureBox1.Bounds.Left, pictureBox1.Bounds.Top));
            //Int32 X = Cursor.Position.X - P.X;
            //Int32 Y = Cursor.Position.Y - P.Y;
            //if (X < 0 || Y < 0 || X > pictureBox1.Bounds.Width || Y > pictureBox1.Bounds.Height)
            //    this.Text = "--, --";
            //else
            //    this.Text = String.Format("{0}, {1}", X, Y);
        }

        private void Truck_Click(object sender, EventArgs e)
        {
            
        }

        private void numFullWt_ValueChanged(object sender, EventArgs e)
        {
            int val = (int)numFullWt.Value;
            val = val / 10;
            val = val * 10;
            numFullWt.Value = val;
            CheckWeight();
        }


        public void CheckWeight()
        {
          
                if (numEmptyWt.Value > numFullWt.Value)
                {
                    var emptyWeight = numFullWt.Value;
                    numFullWt.Value = numEmptyWt.Value;
                    numEmptyWt.Value = emptyWeight;
                }
          
        }


        private void numEmptyWt_ValueChanged(object sender, EventArgs e)
        {
            int val = (int)numEmptyWt.Value;
            val = val / 10;
            val = val * 10;
            numEmptyWt.Value = val;
            CheckWeight();
        }



        public void SetWeight()
        {
            lblWeight.Text = CurrentWeight.ToString();
            lblMotion.Visible = Motion;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            Motion = false;
            SetWeight();

        }

        private void tmrSimMotion_Tick(object sender, EventArgs e)
        {
            if (Motion)
            {
                lblMotion.Visible = !lblMotion.Visible;
            }
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
          
            if (Motion)
            {
                lblMotion.Visible = !lblMotion.Visible;

                int TempWeight = CurrentWeight;
                var r = new Random();
                TempWeight = r.Next(TempWeight - 100, TempWeight + 100);
                TempWeight = TempWeight / 10;
                TempWeight = TempWeight * 10;
                lblWeight.Text = TempWeight.ToString();

            }
            else
            {
                SetWeight();
            }
        }

        private void ckStayOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = ckStayOnTop.Checked;
        }

        private void bwScaleUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!Cancel )
            {
                    try
                    {
                    using (WebService.LocalWebService proxy = new WebService.LocalWebService())
                    {
                        while (!Cancel )
                        {

                       
                           foreach (WebService.LocalDataSet.Weigh_ScalesRow row in weigh_ScalesDataTable)
                            {
                                if (row.Description==SelectedScale )
                                {
                                    row.Last_Update = DateTime.Now;
                                    row.Motion = Motion;
                                    row.OK = true;
                                    row.Weight = int.Parse(lblWeight.Text);
                                    
                                }
                            }
                            proxy.UpdateScales(weigh_ScalesDataTable);

                            System.Threading.Thread.Sleep(500);
                        }

                    }

                }
                catch
                    {

                    }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cancel = true;
        }

        private void ddScales_SelectedIndexChanged(object sender, EventArgs e)
        {
            SelectedScale = ddScales.Text;
        }
    }
}
