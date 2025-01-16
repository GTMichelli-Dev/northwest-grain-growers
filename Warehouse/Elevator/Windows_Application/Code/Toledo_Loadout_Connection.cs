using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Diagnostics;
namespace NWGrain
{
    public class Toledo_Loadout_Connection
    {




        
        
        private int _Port;
        private string _IpAddress;

        public string CurrentData = "";
        public int Subtotal;
        public int Total;
        public bool Finished=false;
        public int TareWt;
        

        private TcpClient client;
        private NetworkStream stream;

        byte[] RecieveBuffer = new byte[1];


        

        private Toledo_Loadout_Connection(){}

        /// <summary>
        /// Creates a new instance of the keypad
        /// </summary>
        /// <param name="IpAddress">Ipaddress of the keypad</param>
        /// <param name="port">port of the keypad</param>
        public Toledo_Loadout_Connection(string IpAddress, int port)
        {
            try
            {
                _IpAddress = IpAddress;
                _Port = port;
                client = new TcpClient(_IpAddress, _Port);
                client.SendTimeout = 250;
                client.ReceiveTimeout = 4750;

            }
            catch 
            {
                RaiseConnectionError();
                
            }
        }

        public void Connect()
        {
            try
            {
                stream = client.GetStream();
                Finished = false;
                ReadInputBuffer();
            }
            catch
            {
                RaiseConnectionError();
            }
        }

        public void Reset()
        {
            CurrentData = "";
            Subtotal = 0;
            Total = 0;
        }


        /// <summary>
        /// Sets the User Input to an asynchronously read the input buffer
        /// </summary>
        private void ReadInputBuffer()
        {
            // Trigger the initial read.

            stream.BeginRead(RecieveBuffer, 0, 1, this.RecieveCallback, null);
            //stream.BeginRead(RecieveBuffer, 0, RecieveBuffer.Length, this.RecieveCallback, null);
        }


        /// <summary>
        /// The Delegate that is fired when the User Input has some Data to share
        /// </summary>
        /// <param name="ar"></param>
        private void RecieveCallback(System.IAsyncResult ar)
        {

            try
            {

                // End pending asynchronous request.
                if (ar.IsCompleted)
                {
                 
                    this.stream.EndRead(ar);


                    String responseData = String.Empty;// String to store the response ASCII representation.
                    responseData = System.Text.Encoding.ASCII.GetString(RecieveBuffer, 0, RecieveBuffer.Length);
                    const string ORDERWEIGHT = "ORDER WEIGHT";
                    const string GROSS ="GROSS";
                    const string TARE = "TARE";
                    const string TOTALWEIGHT= "TOTAL WEIGHT";
                    const string LBS = "LB";
                    int GrossLocation;
                    int TareLocation;
                    int TotalLocation;
                    int GrossLbs;
                    int TareLbs;
                    int TotalLbs;
                    bool  Changed = false;
                    



                    CurrentData += responseData.ToUpper();


                    if (CurrentData.IndexOf(ORDERWEIGHT) > -1)
                    {
                        
                        TareWt = 0;
                        Subtotal = 0;
                        Total = 0;
                    }


                    GrossLocation=CurrentData.IndexOf(GROSS);
                    if (GrossLocation > -1)
                    {
                        GrossLocation += GROSS.Length;
                        GrossLbs = CurrentData.IndexOf(LBS,GrossLocation+GROSS.Length );
                        if (GrossLbs > -1)
                        {
                            
                            string Wt = CurrentData.Substring(GrossLocation, GrossLbs - GrossLocation);
                            int TempWt;
                            if (int.TryParse(Wt,out  TempWt))
                            {
                                Finished = false;
                                Total = 0;
                                Subtotal = TempWt-TareWt ;
                                Changed = true;
                            }
                        }

                    }

                    TareLocation = CurrentData.IndexOf(TARE);
                    if (TareLocation > -1)
                    {
                        TareLocation += TARE.Length;
                        TareLbs = CurrentData.IndexOf(LBS, TareLocation + TARE.Length);
                        if (TareLbs > -1)
                        {

                            string Wt = CurrentData.Substring(TareLocation, TareLbs - TareLocation);
                            int TempWt;
                            if (int.TryParse(Wt, out  TempWt))
                            {
                                Finished = false;
                                
                                TareWt= TempWt;
                                Changed = true;
                            }
                        }

                    }

                    TotalLocation = CurrentData.IndexOf(TOTALWEIGHT);
                    if (TotalLocation > -1)
                    {
                        TotalLocation += TOTALWEIGHT.Length;
                        TotalLbs = CurrentData.IndexOf(LBS, TotalLocation + TOTALWEIGHT.Length);
                        if (TotalLbs > -1)
                        {

                            string Wt = CurrentData.Substring(TotalLocation, TotalLbs - TotalLocation);
                            int TempWt;
                            if (int.TryParse(Wt, out  TempWt))
                            {
                                Total  = TempWt;
                                Changed = true;
                                Finished = true;
                            }
                        }

                    }
                    if (Changed)
                    {
                        CurrentData = "";
                        OnDataRecieved(responseData);
                    }
                    
                }
            }
            catch
            {

            }
            try
            {
                stream.BeginRead(RecieveBuffer, 0, 1, this.RecieveCallback, null);
            }
            catch
            {

            }
        }



        /// <summary>
        /// The event Arguments that are raised by the DataRecievedEvent
        /// </summary>
        public class DataRecievedEventArgs : EventArgs
        {
            public string RecievedData { get; set; }
            public int Subtotal { get; set; }
            public int Total { get; set; }
            public bool Finished { get; set; }
        }




        /// <summary>
        /// Called when Keyboard Data is recieved
        /// </summary>
        public event EventHandler<DataRecievedEventArgs> EventDataRecieved;



        public event EventHandler ConnectionError;

        protected virtual void RaiseConnectionError()
        {
            EventHandler handler = ConnectionError;
            if (handler != null)
            {
                handler(this, null);
            }

        }


        public void SendHeartbeat()
        {
            try
            {
                Byte[] sendBytes;
                sendBytes = Encoding.UTF8.GetBytes("1");
                stream.Write(sendBytes, 0, sendBytes.Length);
            }
            catch
            {

            }
        }

        protected virtual void OnDataRecieved(string Data)
        {
            DataRecievedEventArgs e = new DataRecievedEventArgs();
            e.RecievedData = Data;
            e.Subtotal = Subtotal;
            e.Total = Total;
            e.Finished = Finished;

            EventHandler<DataRecievedEventArgs> handler = EventDataRecieved;
            if (handler != null)
            {
                handler(this, e);
            }
        }


        public void Disconnect()
        {
            try
            {
                stream.Close();
                client.Close();
                stream.Dispose();

            }
            catch
            {

            }
        }
    }
}
