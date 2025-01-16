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
namespace KioskIFace
{
    public partial class frmMain : Form
    {
        public KioskWebService.InboundScan lastScanInfo = null;
        public bool cancel = false;
        public bool CheckingTicketData;
        public string ScaleName = "";
        public KioskWebService.LocalDataSet.Weigh_ScalesRow CurrentScaleData;
        public KioskWebService.LocalDataSet.Weigh_ScalesRow LastCurrentScaleData;
        public string LastStatus;
        string ScannerData = string.Empty;
        int RestartCountDown = 30;
        DateTime LastReset;
        bool ConnectionError;
        string ConnectionMessage;
        DateTime LastScaleUpdate;

        bool Restarted = false;
        bool OkToDisplay = false;

        private string StatusLbl;
        private string Message1;
        private string Message2;
        private string Message3;

        KioskWebService.Kiosk KioskProxy = new KioskWebService.Kiosk();
        public frmMain()
        {
            InitializeComponent();
            this.Text = Application.ProductVersion.ToString();
            KioskProxy.Timeout = 2000;
            bwUpdateScale.RunWorkerAsync();
        }









        public void Initialize()
        {
            this.timer1.Stop();
            try
            {
                // LastCurrentScaleData = null;


                KioskWebService.LocalDataSet.Weigh_ScalesDataTable WeighScales = KioskProxy.GetScales(DateTime.Now);
                var scaleData = WeighScales.FindByUID(Scale_UID);
                if (scaleData != null) ScaleName = scaleData.Description;

                this.timer1.Stop();
                if (this.serialPort1.IsOpen) this.serialPort1.Close();

                this.lblInfo.Text = "Scanner Com Port: " + ScannerComPort +
                        System.Environment.NewLine + "Printer " + PrinterName +
                        System.Environment.NewLine + "Scale " + ScaleName;

                if (!string.IsNullOrEmpty(ScannerComPort.Trim())) this.serialPort1.PortName = ScannerComPort;
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

                this.SendMessageToDisplay(MatrixOrbital.clear_screen);
                this.SendMessageToDisplay(MatrixOrbital.ResetScreen(0, "Connecting", "", ""));
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

        public Guid Scale_UID
        {
            get
            {
                try
                {

                    System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    return Guid.Parse(config.AppSettings.Settings["Scale_UID"].Value);
                }

                catch
                {
                    return Guid.Empty;
                }
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
                if (!ConnectionError)
                {


                    if (CurrentScaleData != null)
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
                        SendMessageToDisplay(MatrixOrbital.ResetScreen(0, "ERROR", "CONNECTING", "TO SCALE"));
                        return false;
                    }
                }
                else
                {
                    SendMessageToDisplay(MatrixOrbital.ResetScreen(0, "ERROR", "NO CONNECTION", "TO SERVER"));
                    DisplayErrorState();
                    this.lblError.Text = "Scale Error :" + ConnectionMessage + System.Environment.NewLine;
                    return false;

                }

            }
            catch (Exception ex)
            {

                SendMessageToDisplay(MatrixOrbital.ResetScreen(0, "ERROR", "CONNECTION TO", "SERVER DOWN"));
                DisplayErrorState();
                this.lblError.Text = "Scale Error :" + ex.Message + System.Environment.NewLine;
                return false;
            }
        }


        public void UpdateWeightFromServer()
        {

            try
            {
                ConnectionError = false;
                ConnectionMessage = "";

                KioskWebService.LocalDataSet.Weigh_ScalesDataTable WeighScales = KioskProxy.GetScales(DateTime.Now);
                this.CurrentScaleData = WeighScales.FindByUID(Scale_UID);
                LastScaleUpdate = DateTime.Now;
            }


            catch (Exception ex)
            {
                ConnectionError = true;
                ConnectionMessage = ex.Message;
            }



        }


       

        private void setScreenMsg(string Status,string message1, string message2, string message3)
        {
            StatusLbl = Status;
            Message1 = message1;
            Message2 = message2;
            Message3 = message3;
        }
       

        
        private void setScreenStatus(string Status)
        {
            StatusLbl = Status;
        }

        private void setScreenMsg( string message1, string message2, string message3)
        {
           
            Message1 = message1;
            Message2 = message2;
            Message3 = message3;
        }



        private void timer1_Tick(object sender, EventArgs e)
        {

            if (CheckingTicketData )
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
                                    || LastCurrentScaleData.Status != CurrentScaleData.Status
                                    || (LastCurrentScaleData.Motion != CurrentScaleData.Motion && LastCurrentScaleData.Weight != CurrentScaleData.Weight))
                                {
                                    if (lastScanInfo != null)
                                    {


                                        if (CurrentScaleData.OK)
                                        {

                                            setScreenMsg(lblStatus.Text, "TO WEIGH OUT", "SCAN BAR CODE", "");
                                            SendMessageToDisplay(MatrixOrbital.ResetScreen(CurrentScaleData.Weight, lblStatus.Text, "TO WEIGH OUT", "SCAN BAR CODE"));
                                        }
                                        else
                                        {
                                            setScreenMsg(lblStatus.Text, "", "", "");
                                            SendMessageToDisplay(MatrixOrbital.ResetScreen(CurrentScaleData.Weight, lblStatus.Text, "", ""));
                                        }
                                    }
                                    LastReset = DateTime.Now;
                                }
                                else if (LastCurrentScaleData.Weight != CurrentScaleData.Weight)
                                {
                                    SendMessageToDisplay(MatrixOrbital.SetWeight(CurrentScaleData.Weight));
                                }
                                else if (LastStatus != this.lblStatus.Text)
                                {
                                    setScreenStatus(lblStatus.Text);
                                    SendMessageToDisplay(MatrixOrbital.SetWeightStatus(CurrentScaleData.Weight, lblStatus.Text));
                                }
                                LastStatus = this.lblStatus.Text;
                                LastCurrentScaleData = CurrentScaleData;



                            }
                            catch (Exception ex)
                            {
                                setScreenMsg("ERROR", "NO CONNECTION", "TO SERVER", "");
                                SendMessageToDisplay(MatrixOrbital.ResetScreen(0, "ERROR", "NO CONNECTION", "TO SERVER"));
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



        public void RestartSerialPort()
        {
            LastScaleUpdate = DateTime.Now;
            this.tmrCheckToRestart.Start();
            Initialize();
            this.lblRestart.Text = "System Initialized";
            lblError.Text = "";
            OkToDisplay = true;
            Restarted = true;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            do
            {
                UpdateWeightFromServer();
                

            } while (!cancel);
        }

        private void bwUpdateScale_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }

        private void DisplayErrorState()
        {
            try
            {
                if (CurrentScaleData != null)
                {
                    this.CurrentScaleData.Status = "Fail";
                    this.CurrentScaleData.Motion = false;
                    this.CurrentScaleData.Weight = 0;
                    this.lblScaleWeight.Text = "----";
                    this.lblStatus.Text = "Error";
                }
            }
            catch { }

        }


        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {

        }



        private void Form1_Load(object sender, EventArgs e)
        {

        }








        private void CheckTicket(string Ticket)
        {
            try
            {

                if (Ticket.Contains("INBOUND"))
                {
                    CheckInboundReceivingTicket(Ticket);
                }
                else
                {

                    SendMessageToDisplay(MatrixOrbital.clear_screen);
                    setScreenMsg("Checking Ticket", Ticket, "");
                    SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "Checking Ticket", Ticket, ""));
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
                            this.lblResponse.Text = "Invalid Ticket";
                            SendMessageToDisplay(MatrixOrbital.clear_screen);
                            SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "Ticket", Ticket, "Invalid"));
                            System.Threading.Thread.Sleep(3000);
                        }
                        else if (TicketStatus == KioskWebService.TicketStatus.TruckNotUnloaded)
                        {
                            setScreenMsg("UNLOAD TRUCK FIRST", "OPERATION", "CANCELLED");
                            SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "UNLOAD TRUCK FIRST", "OPERATION", "CANCELLED"));
                            this.lblResponse.Text = "Truck Not Unloaded";
                            System.Threading.Thread.Sleep(3000);

                        }
                        else if (TicketStatus == KioskWebService.TicketStatus.WeightToLow)
                        {
                            setScreenMsg("SCALE WEIGHT TOO LOW", "OPERATION", "CANCELLED");
                            SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "SCALE WEIGHT TOO LOW", "OPERATION", "CANCELLED"));
                            this.lblResponse.Text = "Truck Not Unloaded";
                            System.Threading.Thread.Sleep(3000);


                        }
                        else if (TicketStatus == KioskWebService.TicketStatus.OldTicket)
                        {
                            setScreenMsg("OLD TICKET", "OPERATION", "CANCELLED");
                            SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "OLD TICKET", "OPERATION", "CANCELLED"));
                            this.lblResponse.Text = "Old Ticket";
                            System.Threading.Thread.Sleep(3000);
                        }
                        else if (TicketStatus == KioskWebService.TicketStatus.Complete)
                        {
                            setScreenMsg("REPRINTING", "TICKET", Ticket);
                            SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "REPRINTING", "TICKET", Ticket));
                            this.lblResponse.Text = "Reprinting Ticket";
                            Kiosk.ProcessTicket(Ticket, CurrentScaleData.Weight, ScaleName, PrinterName);
                            System.Threading.Thread.Sleep(3000);
                        }
                        else if (TicketStatus == KioskWebService.TicketStatus.ReadyToComplete)
                        {
                            if (CurrentScaleData == null || !CurrentScaleData.OK)
                            {
                                setScreenMsg("**Scale Error**", "OPERATION", "CANCELLED");
                                SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "**Scale Error**", "OPERATION", "CANCELLED"));

                                this.lblResponse.Text = "Cannot Finish Ticket Scale Error";
                                System.Threading.Thread.Sleep(3000);
                            }
                            else
                            {
                                if (MotionOk() == true)
                                {
                                    this.lblResponse.Text = "Finishing Ticket";
                                    setScreenMsg("PRINTING", "TICKET", Ticket);
                                    SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "PRINTING", "TICKET", Ticket));
                                    Kiosk.ProcessTicket(Ticket, CurrentScaleData.Weight, ScaleName, PrinterName);
                                    System.Threading.Thread.Sleep(3000);
                                }
                                else
                                {
                                    setScreenMsg("MOTION TIMOUT", "OPERATION", "CANCELLED");
                                    SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "MOTION TIMOUT", "OPERATION", "CANCELLED"));
                                    System.Threading.Thread.Sleep(2000);
                                }
                            }
                        }
                        else
                        {
                            this.lblResponse.Text = "Unknown Error";
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "Error Checking Ticket " + ex.Message;
            }
        }




        private void CheckInboundReceivingTicket(string Ticket)
        {
            if (CurrentScaleData.Weight < 1000)
            {
                setScreenMsg("SCALE WEIGHT TOO LOW", "OPERATION", "CANCELLED");
                SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "SCALE WEIGHT TOO LOW", "OPERATION", "CANCELLED"));
                
                System.Threading.Thread.Sleep(3000);
            }
            else
            {
                SendMessageToDisplay(MatrixOrbital.clear_screen);
                setScreenMsg("Checking Ticket", "", "");
                SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "Checking Ticket", "", ""));
                System.Threading.Thread.Sleep(500);
                LastReset = DateTime.Now.AddSeconds(-100);
                this.lblScanner.Text = Ticket;
                using (KioskWebService.Kiosk Kiosk = new KioskWebService.Kiosk())
                {


                    KioskWebService.TicketStatus TicketStatus = Kiosk.CheckInboundTicket(Ticket, Scale_UID);
                    if (TicketStatus == KioskWebService.TicketStatus.Invalid)
                    {
                        this.lblResponse.Text = "Invalid Ticket";
                        setScreenMsg("Ticket", "", "Invalid");
                        SendMessageToDisplay(MatrixOrbital.clear_screen);
                        SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "Ticket", "", "Invalid"));
                        System.Threading.Thread.Sleep(3000);
                    }
                    else if (TicketStatus == KioskWebService.TicketStatus.OldTicket)
                    {
                        setScreenMsg("Ticket", "Expired", "Invalid");
                        SendMessageToDisplay(MatrixOrbital.clear_screen);
                        SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "Ticket", "Expired", "Invalid"));
                        System.Threading.Thread.Sleep(3000);

                    }
                    else if (TicketStatus == KioskWebService.TicketStatus.InvalidLocation)
                    {
                        setScreenMsg("Ticket", "Invalid", "Location");
                        SendMessageToDisplay(MatrixOrbital.clear_screen);
                        SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "Ticket", "Invalid", "Location"));
                        System.Threading.Thread.Sleep(3000);

                    }
                    else
                    {
                        setScreenMsg("Waiting For", "Scale House", "To Validate");
                        SendMessageToDisplay(MatrixOrbital.clear_screen);
                        SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "Waiting For", "Scale House", "To Validate"));
                        bwCheckInbound.RunWorkerAsync();
                    }

                }
            }

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
                    setScreenMsg(">>> MOTION <<<", "ON SCALE", "PLEASE WAIT");
                    SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, ">>> MOTION <<<", "ON SCALE", "PLEASE WAIT"));

                    do
                    {
                        int CurWt = CurrentScaleData.Weight;
                        if (GetWeight() == false)
                        {
                            return false;
                        }
                        if (CurWt != CurrentScaleData.Weight)
                        {
                            SendMessageToDisplay(MatrixOrbital.SetWeight(CurrentScaleData.Weight));
                        }
                    } while (CurrentScaleData.Motion == true && (Math.Abs((DateTime.Now - MotionTimeOut).TotalSeconds) < 10));
                    return !CurrentScaleData.Motion;
                }
                else return true;

            }

        }


        public void SendMessageToDisplay(byte[] Message)
        {
            /// Had to delay the display on initial startup because on reboot the serial port would freeze up
            if (OkToDisplay)
            {
                try
                {
                    if (this.serialPort1.IsOpen)
                    {
                        this.serialPort1.Write(Message, 0, Message.Length);
                    }
                }
                catch (Exception ex)
                {
                    this.lblError.Text = "Error Sending Data To Display " + ex.Message;
                }
            }
        }







        private void tmrReset_Tick(object sender, EventArgs e)
        {

        }




        private void frmMain_Load(object sender, EventArgs e)
        {
            this.serialPort1.NewLine = Convert.ToString((char)10);
            Initialize();

            //            this.tmrReset.Start();
        }



        private void tmrCheckToRestart_Tick(object sender, EventArgs e)
        {
            if (Math.Abs((LastScaleUpdate - DateTime.Now).TotalSeconds) > 15)
            {
                Application.Restart();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            lblError.Text = "";
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            cancel = true;
        }

        private void bwCheckInbound_DoWork(object sender, DoWorkEventArgs e)
        {
           
            do
            {
                using (KioskWebService.Kiosk Kiosk = new KioskWebService.Kiosk())
                {
                    lastScanInfo = Kiosk.GetLastScannedInboundQR();
                    if (lastScanInfo.PrintingTicket)
                    {
                        setScreenMsg("Printing", "Inbound", "Ticket");
                        SendMessageToDisplay(MatrixOrbital.clear_screen);
                        SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "Printing", "Inbound", "Ticket"));
                        System.Threading.Thread.Sleep(3000);
                        lastScanInfo = null;
                        ResetText();
                    }
                    else if (lastScanInfo.Acknowledged)
                    {
                        setScreenMsg("Saving", "Inbound", "Ticket");
                        SendMessageToDisplay(MatrixOrbital.clear_screen);
                        SendMessageToDisplay(MatrixOrbital.SetAll(CurrentScaleData.Weight, "Saving", "Inbound", "Ticket"));
                        System.Threading.Thread.Sleep(3000);
                        

                    }
                    else if (CurrentScaleData.Weight < 1000)
                    {
                        ResetText();
                        lastScanInfo = null;
                    }
                    

                }
                System.Threading.Thread.Sleep(500);
            }
            while (!cancel);

        }
    }
}
