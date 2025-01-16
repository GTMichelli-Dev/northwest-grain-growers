using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Threading;
using System.Windows.Forms.VisualStyles;
namespace KioskIFace
{
    public partial class frmMain : Form
    {
        public bool CheckingTicketData;

        public KioskWebService.LocalDataSet.Weigh_ScalesRow CurrentScaleData;
        public KioskWebService.LocalDataSet.Weigh_ScalesRow LastCurrentScaleData;
        public string LastStatus;
        string ScannerData = string.Empty;
        int RestartCountDown = 30;        
        DateTime LastReset;
        bool ConnectionError;
        string ConnectionMessage;
        DateTime LastScaleUpdate;

        bool Restarted=false;
     //   bool OkToDisplay = false;

       
        KioskWebService.Kiosk KioskProxy = new KioskWebService.Kiosk();
        public frmMain()
        {
            InitializeComponent();
            this.label4.Text = Application.ProductVersion.ToString();
            KioskProxy.Timeout = 2000;
            bwUpdateScale.RunWorkerAsync();
        }


       






        public void Initialize()
        {
            this.timer1.Stop();
            try
            {
                // LastCurrentScaleData = null;
             


                this.timer1.Stop();
                if (this.serialPort1.IsOpen) this.serialPort1.Close();
                this.lblInfo.Text = "Scanner Com Port: " + ScannerComPort +
                        System.Environment.NewLine + "Printer " + PrinterName +
                        System.Environment.NewLine + "Scale " + ScaleName;

                if (! string.IsNullOrEmpty(ScannerComPort.Trim())) this.serialPort1.PortName = ScannerComPort;
                if (!this.serialPort1.IsOpen && !string.IsNullOrEmpty(ScannerComPort.Trim()))
                {
                    this.serialPort1.Open();
                    this.lblScanner.Visible = true;
                    this.lblScannerHeader.Text = "Last Scanned";

                }
                else
                {
                    this.lblScanner.Visible = false;
                    this.lblScannerHeader.Text = "USB Serial Port Not Set";
                }

                //this.SendMessageToDisplay(MatrixOrbital.clear_screen);
                //this.SendMessageToDisplay(MatrixOrbital.ResetScreen(0, "Connecting", "", ""));
            }
            catch (Exception ex)
            {
                DisplayErrorState();
                this.lblError.Text = "Error Initializing " + ex.Message;
            }
            this.timer1.Start();

        }






        public string ScannerComPort
        {
            get
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                
                return config.AppSettings.Settings["ComPort"].Value;
            }
        }

        public string ScaleName
        {
            get
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                return config.AppSettings.Settings["Scale"].Value;
            }
        }


        public string PrinterName
        {
            get
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                return config.AppSettings.Settings["Printer"].Value;
            }
        }



        public bool GetWeight()
        {
            try
            {
                if (CurrentScaleData != null)
                {
                    if (!ConnectionError)
                    {




                        this.lblScaleWeight.Text = string.Format("{0:N0} lbs.", CurrentScaleData.Weight);
                        this.lblStatuschr.Text = LastScaleUpdate.ToLongTimeString();


                        if (CurrentScaleData.OK)
                        {
                            if (CurrentScaleData.Motion == true)
                            {
                                this.lblStatus.Text = "Motion";
                            }
                            else
                            {
                                this.lblStatus.Text = "";
                            }
                            return true;
                        }
                        else
                        {
                            this.lblStatus.Text = this.CurrentScaleData.Status;
                            return false;

                        }
                    }
                    else
                    {
                        //SendMessageToDisplay(MatrixOrbital.ResetScreen(0, "ERROR", "NO CONNECTION", "TO SERVER"));
                        DisplayErrorState();
                        this.lblError.Text = "Scale Error :" + ConnectionMessage + System.Environment.NewLine;
                        return false;

                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

               // SendMessageToDisplay(MatrixOrbital.ResetScreen(0, "ERROR", "CONNECTION TO", "SERVER DOWN"));
                DisplayErrorState();
                this.lblError.Text = "Scale Error :" + ex.Message + System.Environment.NewLine;
                return false;
            }
        }


        public void UpdateWeightFromServer()
        {
            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            try
            {
                ConnectionError = false;
                ConnectionMessage = "";
                Guid ScaleUID = Guid.Parse(config.AppSettings.Settings["Scale_UID"].Value);
                KioskWebService.LocalDataSet.Weigh_ScalesDataTable WeighScales= KioskProxy.GetScales( DateTime.Now);
                this.CurrentScaleData = WeighScales.FindByUID(ScaleUID);
               
                LastScaleUpdate = DateTime.Now;
            }
               
            
            catch (Exception ex)
            {
                ConnectionError = true;
                ConnectionMessage = ex.Message ;
            }



        }




        private void timer1_Tick(object sender, EventArgs e)
        {
            if (this.CurrentScaleData != null) lblScaleWeight.Text= this.CurrentScaleData.Weight.ToString()+" lbs.";
            if (CheckingTicketData)
            { }
            else
            {
                CheckingTicketData = true;
                this.timer1.Stop();
                this.label2.Text = "Current Time:" + DateTime.Now.ToLongTimeString() + "   Last Scale Update:" + LastScaleUpdate.ToLongTimeString();
                string ErrorMessage = "";
                if (!this.serialPort1.IsOpen && !string.IsNullOrEmpty(ScannerComPort.Trim()))
                {
                    Initialize();
                }
                else
                {

                    try
                    {
                        if (!string.IsNullOrEmpty(ScannerComPort.Trim()) && this.serialPort1.IsOpen)
                        {
                            this.label3.Text = string.Format("Bytes To Read {0}", serialPort1.BytesToRead);
                            try
                            {
                                if (serialPort1.BytesToRead > 0)
                                {
                                    serialPort1.ReadTimeout = 500;

                                    ScannerData = serialPort1.ReadLine();
                                    /// Recieved a Scanned Ticket 
                                    if (!string.IsNullOrEmpty(this.ScannerData))
                                    {
                                        CheckTicket(this.ScannerData);
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                ErrorMessage += "Error Reading Scanned Ticket " + ex.Message + System.Environment.NewLine;

                            }
                        }
                        // Get the Scale Weight check for Scanned Ticket
                        if (GetWeight())
                        {

                            try
                            {
                                if ((DateTime.Now - LastReset).TotalSeconds > 5
                                    || LastCurrentScaleData == null
                                    || LastCurrentScaleData.Status  != CurrentScaleData.Status
                                    || (LastCurrentScaleData.Motion != CurrentScaleData.Motion && LastCurrentScaleData.Weight != CurrentScaleData.Weight))
                                {
                                    if (CurrentScaleData.OK)
                                    {
                                        //SendMessageToDisplay(MatrixOrbital.ResetScreen(CurrentScaleData.Weight, lblStatus.Text, "TO WEIGH OUT", "SCAN BAR CODE"));
                                    }
                                    else
                                    {
                                        //SendMessageToDisplay(MatrixOrbital.ResetScreen(CurrentScaleData.Weight, lblStatus.Text, "", ""));
                                    }
                                    LastReset = DateTime.Now;
                                }
                                else if (LastCurrentScaleData.Weight != CurrentScaleData.Weight)
                                {
                                   // SendMessageToDisplay(MatrixOrbital.SetWeight(CurrentScaleData.Weight));
                                }
                                else if (LastStatus != this.lblStatus.Text)
                                {
                                  //  SendMessageToDisplay(MatrixOrbital.SetWeightStatus(CurrentScaleData.Weight, lblStatus.Text));
                                }
                                LastStatus = this.lblStatus.Text;
                                LastCurrentScaleData = CurrentScaleData;



                            }
                            catch (Exception ex)
                            {
//SendMessageToDisplay(MatrixOrbital.ResetScreen(0, "ERROR", "NO CONNECTION", "TO SERVER"));
                                DisplayErrorState();
                                ErrorMessage += "Scale Error :" + ex.Message + System.Environment.NewLine;
                                Application.DoEvents();
                                System.Threading.Thread.Sleep(1000);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorMessage += ex.Message + System.Environment.NewLine;

                    }
                    this.ScannerData = "";
                }
                if (this.lblError.Text.Length > 10000) this.lblError.Text = "";
                if (!string.IsNullOrEmpty(ErrorMessage)) this.lblError.Text = ErrorMessage;
                if (RestartCountDown > 0) { RestartCountDown -= 1; }

                if (RestartCountDown > 0)
                {
                    this.lblRestart.Text = string.Format("Initializing {0}", RestartCountDown);
                }
                else if (RestartCountDown == 0 && Restarted == false)
                {

                    RestartSerialPort();
                }
                this.timer1.Start();
                CheckingTicketData = false;
            }
        }


        private void DisplayErrorState()
        {
            try
            {
                if (CurrentScaleData != null)
                {
                    this.CurrentScaleData.Status="Fail";
                    this.CurrentScaleData.Motion = false;
                    this.CurrentScaleData.Weight = 0;
                    //this.lblScaleWeight.Text = "----";
                    this.lblStatus.Text = "Error";
                }
            }
            catch { }

        }


        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

        }

        private void setupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.tmrCheckToRestart.Stop();
                timer1.Stop();
                using (Form2 frm = new Form2())
                {
                    frm.ShowDialog();
                    Initialize();
                }
            }
            catch (Exception ex)
            {
                this.lblResponse.Text = "Error " + ex.Message;
            }
            LastScaleUpdate = DateTime.Now;
            this.tmrCheckToRestart.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void testTicketToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Test_Ticket frm = new Test_Ticket())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    CheckTicket(frm.Ticket);

                }
            }
        }

        private void CheckTicket(string Ticket)
        {
            try
            {
                this.lblResponse.Text = "Checking Ticket: " + Ticket;
                this.BackColor = Color.Yellow;
                Application.DoEvents();
                //SendMessageToDisplay(MatrixOrbital.clear_screen);
                //SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "Checking Ticket", Ticket, ""));
                System.Threading.Thread.Sleep(500);
                LastReset = DateTime.Now.AddSeconds(-100);
                this.lblScanner.Text = Ticket;
                using (KioskWebService.Kiosk Kiosk = new KioskWebService.Kiosk())
                {
                    int Weight = 0;
                    if (CurrentScaleData != null) { Weight = CurrentScaleData.Weight; }
                    KioskWebService.TicketStatus TicketStatus = Kiosk.CheckTicket(Ticket, Weight);
                    if (TicketStatus == KioskWebService.TicketStatus.Invalid)
                    {
                        this.BackColor = Color.Pink;
                        this.lblResponse.Text = "Invalid Ticket";
                        //Application.DoEvents();
                        //SendMessageToDisplay(MatrixOrbital.clear_screen);
                        //SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "Ticket", Ticket, "Invalid"));
                        
                    }
                    else if (TicketStatus == KioskWebService.TicketStatus.TruckNotUnloaded)
                    {
                        //SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "UNLOAD TRUCK FIRST", "OPERATION", "CANCELLED"));
                        this.lblResponse.Text = "Truck Not Unloaded";
                        this.BackColor = Color.Pink;
                        //Application.DoEvents();
                        
                    }
                    else if (TicketStatus == KioskWebService.TicketStatus.WeightToLow)
                    {
                        //SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "SCALE WEIGHT TOO LOW", "OPERATION", "CANCELLED"));
                        this.BackColor = Color.Pink;
                        this.lblResponse.Text = "Truck Not Unloaded";
                        //Application.DoEvents();
                  

                    }
                    else if (TicketStatus == KioskWebService.TicketStatus.OldTicket)
                    {
                        //SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "OLD TICKET", "OPERATION", "CANCELLED"));
                        this.BackColor = Color.Pink;
                        this.lblResponse.Text = "Old Ticket";
                    }
                    else if (TicketStatus == KioskWebService.TicketStatus.Complete)
                    {
                        //SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "REPRINTING", "TICKET", Ticket));
                        this.BackColor = Color.Lime;

                        this.lblResponse.Text = "Reprinting Ticket";
                        Application.DoEvents();
                        try
                        {
                            Kiosk.ProcessTicket(Ticket, CurrentScaleData.Weight, ScaleName, PrinterName);
                        }
                        catch
                        {

                        }
                        
                  
                    }
                    else if (TicketStatus == KioskWebService.TicketStatus.ReadyToComplete)
                    {
                        if (CurrentScaleData == null || !CurrentScaleData.OK)
                        {
                            this.BackColor = Color.Pink;
                            this.lblResponse.Text = "Cannot Finish Ticket Scale Error";
                           // Application.DoEvents();
                        //    SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "**Scale Error**", "OPERATION", "CANCELLED"));

                        }
                        else
                        {
                            if (MotionOk() == true)
                            {
                                this.BackColor = Color.Lime;
                                this.lblResponse.Text = "Finishing Ticket";
                                Application.DoEvents();
                                //     SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "PRINTING", "TICKET", Ticket));
                                try
                                {


                                    Kiosk.ProcessTicket(Ticket, CurrentScaleData.Weight, ScaleName, PrinterName);
                                }
                                catch
                                {

                                }

                            }
                            else
                            {
                                this.BackColor = Color.Pink;
                                this.lblResponse.Text = "MOTION TIMOUT";

                               // Application.DoEvents();
                          //      SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "MOTION TIMOUT", "OPERATION", "CANCELLED"));
                              
                            }
                        }
                    }
                    else
                    {
                        this.BackColor = Color.Pink;
                        this.lblResponse.Text = "Unknown Error";

                        //Application.DoEvents();


                    }

                }
            }
            catch (Exception ex)
            {
                this.BackColor = Color.Pink;
                lblError.Text = "Error Checking Ticket " + ex.Message;
            }
            tmrReset.Stop();
            tmrReset.Start();
        }






        public bool MotionOk()
        {

            DateTime MotionTimeOut = DateTime.Now;
            if (CurrentScaleData == null)
            {
                return false;
            }
            else
            {
                if (CurrentScaleData.Motion)
                {
                    //SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, ">>> MOTION <<<", "ON SCALE", "PLEASE WAIT"));
                    
                    do
                    {
                        int CurWt = CurrentScaleData.Weight;
                        if (GetWeight() == false)
                        {
                            return false;
                        }
                        if (CurWt != CurrentScaleData.Weight)
                        {
                            //SendMessageToDisplay(MatrixOrbital.SetWeight(CurrentScaleData.Weight));
                        }
                    } while (CurrentScaleData.Motion == true && (Math.Abs((DateTime.Now - MotionTimeOut).TotalSeconds) < 10));
                    return !CurrentScaleData.Motion;
                }
                else return true;

            }

        }


        //public void SendMessageToDisplay(byte[] Message)
        //{
        //    /// Had to delay the display on initial startup because on reboot the serial port would freeze up
        //    if (OkToDisplay)
        //    {
        //        try
        //        {
        //            if (this.serialPort1.IsOpen)
        //            {
        //                this.serialPort1.Write(Message, 0, Message.Length);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            this.lblError.Text = "Error Sending Data To Display " + ex.Message;
        //        }
        //    }
        //}






        private void testDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //this.SendMessageToDisplay(MatrixOrbital.clear_screen);
            //this.SendMessageToDisplay(MatrixOrbital.SetLine1("Line 1"));

            //this.SendMessageToDisplay(MatrixOrbital.SetLine3("Line 3"));
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void test2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.SendMessageToDisplay(MatrixOrbital.SetLine2_3("2", "3"));
        }

        private void test2ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
          //  this.SendMessageToDisplay(MatrixOrbital.SetLine2_3("", ""));
        }

        private void restartDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
           // SendMessageToDisplay(MatrixOrbital.RestartScreen());
        }

        private void tmrReset_Tick(object sender, EventArgs e)
        {
            tmrReset.Stop();
            this.BackColor = Color.White;
            lblScanner.Text = "";
            this.lblResponse.Text = "";
            this.lblError.Text = "";
        }


        public void RestartSerialPort()
        {
            LastScaleUpdate = DateTime.Now;
            this.tmrCheckToRestart.Start();
            Initialize();
            this.lblRestart.Text = "System Initialized";
            lblError.Text = "";
            //OkToDisplay = true;
            Restarted = true;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.serialPort1.NewLine = Convert.ToString((char)10);
            Initialize();

//            this.tmrReset.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
             UpdateWeightFromServer();
            
            } while (true);
        }

        private void bwUpdateScale_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void tmrCheckToRestart_Tick(object sender, EventArgs e)
        {
            if (Math.Abs( (LastScaleUpdate-DateTime.Now).TotalSeconds )>15)
            {
                Application.Restart();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CheckTicket("1234567890");
        }
    }
}
