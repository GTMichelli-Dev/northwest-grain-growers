
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace StreamingScale_Lib
{
    /// <summary>
    /// Class for Creating a Network connection to a SMA enabled scale indicator
    /// </summary>
    public class StreamingScale :IDisposable 

    {
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);     
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    this.Disconnect();

                }

                disposed = true;
            }
        }

        ~StreamingScale()
        {
            Dispose(false);
        }


        public static List<string> ScaleDescription = new List<string>();

        private StreamingScale()
        {

        }

        /// <summary>
        /// Creates a new connection to the scale using a condec continious output
        /// </summary>
        /// <param name="Address">The IP address of the scale</param>
        /// <param name="Port"> The Port Number of the Address</param>
        public StreamingScale(string Scale_Description,string Address, int Port, int Site_Location_Id)
        {
            bool Exists = false;
            for (int i = 0; i < ScaleDescription.Count; i++)
            {
                if (ScaleDescription[i].Equals(Scale_Description))
                {
                    Exists = true;
                    break;
                }
            }
            if (!Exists)
            {
                Location_Id = Site_Location_Id;
                Description = Scale_Description;
                _Address = Address;
                _Port = Port;
                Connect(1000); 
            }
            else
            {
                _Address = "";
                _Port = 0;
                Description = "";
                this.Dispose();
            }
        }





        /// <summary>
        /// Called when Scale Data is recieved
        /// </summary>
        public event EventHandler ScaleDataRecieved;

        /// <summary>
        /// The event handeler that is called when Data from the scale Is Recieved
        /// call get the scale data by checking the CurrentScaleData
        /// </summary>
        /// <param name="e">The Keypress that occured</param>
        protected virtual void OnScaleDataRecieved()
        {
            EventArgs e= new EventArgs();
            EventHandler handler = ScaleDataRecieved;
            if (handler != null)
            {
                handler(this,e);
            }
        }


        /// <summary>
        /// Called when Scale is Connecting
        /// </summary>
        public event EventHandler ScaleConnecting;

        /// <summary>
        /// The event handeler that is called when Data from the scale Is Recieved
        /// call get the scale data by checking the CurrentScaleData
        /// </summary>
        /// <param name="e">The Keypress that occured</param>
        protected virtual void OnScaleConnecting()
        {
            EventArgs e = new EventArgs();
            EventHandler handler = ScaleConnecting;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        /// <summary>
        /// Called when Scale is Connected
        /// </summary>
        public event EventHandler ScaleConnected;

        /// <summary>
        /// The event handeler that is called when Data from the scale Is Recieved
        /// call get the scale data by checking the CurrentScaleData
        /// </summary>
        /// <param name="e">The Keypress that occured</param>
        protected virtual void OnScaleConnected()
        {
            EventArgs e = new EventArgs();
            EventHandler handler = ScaleConnected;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public string Description;
        public int Location_Id;
        private string _Address;
        private int _Port;
   
        private bool _ZeroScale;

        /// <summary>
        /// The INterval in which the scale is updated
        /// </summary>
        public int Interval;


        //private const string WeightCommand = "\nW\r";
        //private const string ZeroCommand = "%z";
        //
        private const string ZeroCommand = "F130\r";

        private Thread ScaleWeightThread = null;

        private ScaleData  _CurrentScaleData= new ScaleData();


        /// <summary>
        /// The Network Connection Status of the scale
        /// </summary>
        public enum enumConnectionStatus {
            /// <summary>
            /// Scale Is Connected
            /// </summary>
            Ok,
            /// <summary>
            /// Scale Is Not Connected But is trying to Connect
            /// </summary>
            Connecting,
            /// <summary>
            /// Scale is Not Connected
            /// </summary>
            NotConnected
        }

        private enumConnectionStatus _ConnectionStatus;
        private string _ConnectionStatusString;


        /// <summary>
        /// Gets the current Scale Data Recieved from the indicator
        /// </summary>
        public ScaleData CurrentScaleData
        {
            get
            {
                return _CurrentScaleData;
            }
        }


        /// <summary>
        /// True if the connection is running 
        /// </summary>
        public bool IsRunning
        {

            get 
            {
                bool running = ScaleWeightThread != null;
                return running;
            }
        }

        /// <summary>
        /// Gets the status of the Scale Network Connection
        /// </summary>
        public enumConnectionStatus ConnectionStatus
        {
            get { return _ConnectionStatus; }
        }

        /// <summary>
        /// Gets a Text status of the connection
        /// </summary>
        public string ConnectionStatusString
        {
            get { return _ConnectionStatusString; }
        }


        /// <summary>
        /// Class containg all the current Data Comming from the scale
        /// </summary>
        public class ScaleData
        {

            public string StreamError = string.Empty; 

            /// <summary>
            /// return true if the scale is in motion ( Unstable)
            /// </summary>
            public bool Motion;

            public enum enumStatus 
            { 
                /// <summary>
                /// Everything is Ok
                /// </summary>
                OK,
                /// <summary>
                /// Scale is Underrange
                /// </summary>
                Under,
                /// <summary>
                /// Scale is OverRange
                /// </summary>
                Over,
                /// <summary>
                /// Scale is in a faulted State
                /// </summary>
                Fail,
                /// <summary>
                /// Scale is Trying to connect
                /// </summary>
                Connecting,
                /// <summary>
                /// Manual Weight Entry
                /// </summary>
                Manual,
                /// <summary>
                /// Streaming is Off
                /// </summary>
                Off};

            /// <summary>
            ///  The current status of the scale
            /// </summary>
            public enumStatus CurrentStatus;

            /// <summary>
            ///  The Current Weight of the scale
            /// </summary>
            public int CurWeight;

            private string[] StatusCharArray={" ","|","/","--","\\","|","/","--","\\"};
            private int _CurStatusInt=0;
            private string _RawWeightString;
            private int ErrorCount = 0;

            /// <summary>
            /// The Status Character to be sent to the application. If the character is blank then the connection is not present
            /// </summary>
            public string CurrentStatusChar
            {
                get
                {
                    // If the system is not Ok then set the status bit to -1 which will then incremant to 0 (a blank)
                    if (CurrentStatus != enumStatus.OK) _CurStatusInt = -1;
                    //incremant the status int by 1
                    _CurStatusInt += 1;
                    if (_CurStatusInt > 8) _CurStatusInt = 1;
                    return StatusCharArray[_CurStatusInt];
                }
            }

            
            /// <summary>
            /// Class for holding the current scale infoemation
            /// </summary>
            public ScaleData()
            {
                CurWeight = 0;
                CurrentStatus = enumStatus.Connecting ;
                StreamError = string.Empty;
                Motion = false;
                _RawWeightString = "";
                _CurStatusInt = 0;
            }


            /// <summary>
            /// Resets the scale Data to a default values  
            /// CurWeight = 0;
            /// CurrentStatus = enumStatus.Connecting;
            /// Motion = false;
            /// RawWeightString = "";
            /// </summary>
            public void ResetScaleData()
            {
                CurWeight = 0;
                CurrentStatus = enumStatus.Connecting;
                Motion = false;
                _RawWeightString = "";
                _CurStatusInt = 0;

            }


            /// <summary>
            /// The Last String Of Information comming from the scale just as it was recieved
            /// </summary>
            public string RawWeightString
            {
                get { return _RawWeightString; }

                set 
                {
                    if (_RawWeightString.Length > 50) _RawWeightString = "";
                    _RawWeightString += value;
                    _RawWeightString= _RawWeightString.Replace("\0", string.Empty);

                    _RawWeightString = _RawWeightString.Replace("\x02", string.Empty);
                    string[] WtArray;
                    WtArray=RawWeightString.Split('\n');


                    bool Success = false;
                    ErrorCount += 1;
                    for (int  I= WtArray.Length-1;  I> 0; I--)
                    {
                       if (WtArray[I].Contains("\r") && (WtArray[I].Length > 8))
                        {
                            int Wt;
                            if (int.TryParse(WtArray[I].Substring(1, 7), out Wt))
                            {
                                if (WtArray[I].Contains('-')) Wt = Wt * -1;
                                
                                
                                Motion = WtArray[I].Contains('M');
                                if (WtArray[I].Contains('O'))
                                {
                                    Success = false;
                                    ErrorCount = 10;
                                    CurWeight = 0;
                                    CurrentStatus = enumStatus.Over; 
                                }
                                else
                                {
                                    CurrentStatus = enumStatus.OK;
                                    CurWeight = Wt;
                                    Success = true;
                                    ErrorCount = 0;
                                }
                                this._RawWeightString = "";
                                break;
                            }
                        }
                        
                    }

                    if (Success==false && ErrorCount>10)
                    {
                            CurWeight = 0;
                            CurrentStatus = enumStatus.Fail ;
                            Motion = false;

                   }
                }
            }

        }



  


        /// <summary>
        /// Zeros the scale
        /// </summary>
        public void ZeroScale()
        {
            this._ZeroScale = true;
        }


        /// <summary>
        /// Stops and closes the connection
        /// </summary>
        public void Stop()
        {
            Disconnect();

        }

        /// <summary>
        /// Starts a connection to the indicator
        /// </summary>
        /// <param name="UpdateRate">The Rate in milliseconds that the scale is poled for the weight</param>
        public void Start(int UpdateRate)
        {
            Connect(UpdateRate);
        }


        /// <summary>
        /// Starts a connection to the indicator
        /// </summary>
        /// <param name="UpdateRate">The Rate in milliseconds that the scale is poled for the weight</param>
        public void Connect(int UpdateRate)
        {
            if (this.ScaleWeightThread == null)
            {
                Interval = UpdateRate;
                this.ScaleWeightThread = new Thread(new ThreadStart(StreamWeight));
                this.ScaleWeightThread.Start();
            }
        }


        /// <summary>
        /// Stops and closes the connection
        /// </summary>
        public void Disconnect()
        {
            if (this.ScaleWeightThread != null)
            {
                this.ScaleWeightThread.Abort();
                this.ScaleWeightThread = null;
            }

        }


        private void StreamWeight()
        {
            do
            {
                using (TcpClient tcpClient = new TcpClient())
                {
                    try
                    {
                        this._ConnectionStatus =enumConnectionStatus.Connecting;
                        this._CurrentScaleData.CurrentStatus = ScaleData.enumStatus.Connecting;
                        OnScaleConnecting();
                        this._ConnectionStatusString = String.Format("Connecting To Address {0} Port{1}", _Address, _Port);
                        tcpClient.Connect(_Address, _Port);
                        tcpClient.SendTimeout = Interval+3000;
                        tcpClient.ReceiveTimeout = Interval+3000;
                        OnScaleConnected(); 
                        this._ConnectionStatus = enumConnectionStatus.Ok ;
                        this._ConnectionStatusString = String.Format("Connected To Scale @ Address {0} Port{1}", _Address, _Port);
                        NetworkStream netStream = tcpClient.GetStream();
                     
                        do
                        {
                            //send the zero command if necessary
                            if (_ZeroScale) 
                            {
                                _ZeroScale = false;
                                SendBytes(netStream, ZeroCommand);
                                System.Threading.Thread.Sleep(150);
                                SendBytes(netStream, ZeroCommand);
                                System.Threading.Thread.Sleep(150);
                                SendBytes(netStream, ZeroCommand);
                                System.Threading.Thread.Sleep(150);
                                SendBytes(netStream, ZeroCommand); 

                            }
                            System.Threading.Thread.Sleep(Interval);
                            //Get the Weight
            //                SendBytes(netStream, WeightCommand);
                            byte[] bytes = new byte[tcpClient.ReceiveBufferSize];
                            
                            // Read can return anything from 0 to numBytesToRead.  
                            // This method blocks until at least one byte is read.
                            netStream.Read(bytes, 0, (int)tcpClient.ReceiveBufferSize);

                            // Returns the data received from the scale. 
                            string returndata = Encoding.UTF8.GetString(bytes);
                            //Dont let the Raw String Get To Long
                            //System.Diagnostics.Debug.Print("<Return Data" + returndata + ">");
                            this._CurrentScaleData.RawWeightString = returndata;
                            this._CurrentScaleData.StreamError = string.Empty;
                            //System.Diagnostics.Debug.Print("<"+this._CurrentScaleData.RawWeightString+">" );
                            OnScaleDataRecieved();
                            
                        }
                        while (true);
                    }
                    catch (Exception streamException)
                    {
                        this._CurrentScaleData.StreamError = streamException.Message;
                        this._CurrentScaleData.ResetScaleData();
                        this._CurrentScaleData.CurrentStatus = ScaleData.enumStatus.Fail;
                        OnScaleDataRecieved();

                    }
                    if (tcpClient.Connected)
                    {
                        tcpClient.Close();

                    }
                    this._CurrentScaleData.ResetScaleData();


                }
            }
            while (true);
        }

        private void SendBytes(NetworkStream netStream, string StringToSend)
        {
            Byte[] sendBytes;
            sendBytes = Encoding.UTF8.GetBytes(StringToSend);
            netStream.Write(sendBytes, 0, sendBytes.Length);
        }

    }

}
