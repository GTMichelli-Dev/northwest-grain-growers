using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace ScaleSimulatorWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
          

                Task.Run(() => StartServer(numericUpDown1, 10001,ckM1,ckE1));
                Task.Run(() => StartServer(numericUpDown2, 10002,ckM2,ckE2));
                Task.Run(() => StartServer(numericUpDown3, 10003,ckM3,ckE3));
                Task.Run(() => StartServer(numericUpDown4, 10004,ckM4,ckE4));
                Task.Run(() => StartServer(numericUpDown5, 10005,ckM5,ckE5));
                Task.Run(() => StartServer(numericUpDown6, 10006,ckM6,ckE6));
                Task.Run(() => StartServer(numericUpDown7, 10007,ckM7,ckE7));
                Task.Run(() => StartServer(numericUpDown8, 10008,ckM8,ckE8));
                Task.Run(() => StartServer(numericUpDown9, 10009,ckM9,ckE9));
                Task.Run(() => StartServer(numericUpDown10, 10010,ckM10,ckE10));    
        }

        private static void StartServer(NumericUpDown numUpDown, int port,CheckBox motionCheckBox,CheckBox errorCheckbox)
        {
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Console.WriteLine($"Server started on port {port}");
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                Task.Run(() => HandleClient(client,numUpDown, motionCheckBox,errorCheckbox));
            }
        }

        private static void HandleClient(TcpClient client,NumericUpDown numericUpDown, CheckBox motionCheckBox, CheckBox errorCheckbox)
        {
            try
            {
                byte[] buffer = new byte[1024];
                NetworkStream stream = client.GetStream();
                string request = string.Empty;
                while (true)
                {
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        break; // The client has disconnected
                    }

                    request += Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    if (request.Contains("\r") && request.Contains("\n"))
                    {
                        if (request.Contains("W") )
                        {

                            int weight = (int)numericUpDown.Value;
                            string weightString = weight.ToString().PadLeft(9);
                            string sCode = " ";
                            string mCode = " ";


                            if (errorCheckbox.Checked)
                            {
                                sCode = "O";
                            }
                            else if (weight == 0)
                            {
                                sCode = "Z";
                            }
                            if (motionCheckBox.Checked)
                            {
                                mCode = "M";
                            }
                            string response = $"\n {sCode}1G{mCode} {weightString}lb \r";

                            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                            stream.Write(responseBytes, 0, responseBytes.Length);
                        }
                        else
                        {

                            string response = $"\n?\r";

                            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                            stream.Write(responseBytes, 0, responseBytes.Length);
                        }

                        request = string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
            
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = checkBox2.Checked;
        }



        private void updatenumericUpDown(HScrollBar hScrollBar, NumericUpDown numericUpDown)
        {
            int roundedValue = (int)Math.Round((double)hScrollBar.Value / 20) * 20;
            numericUpDown.Value = roundedValue;
        }

        private void UpdateHscrollBar(NumericUpDown numericUpDown, HScrollBar hScrollBar)
        {
            int roundedValue = (int)Math.Round((double)numericUpDown.Value / 20) * 20;
            hScrollBar.Value = roundedValue;
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            updatenumericUpDown(hScrollBar1, numericUpDown1);

        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            UpdateHscrollBar(numericUpDown1, hScrollBar1);
        }

        private void hScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            updatenumericUpDown(hScrollBar2, numericUpDown2);
        }

        private void hScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            updatenumericUpDown(hScrollBar3, numericUpDown3);
        }

        private void hScrollBar4_Scroll(object sender, ScrollEventArgs e)
        {
            updatenumericUpDown(hScrollBar4, numericUpDown4);
        }

        private void hScrollBar5_Scroll(object sender, ScrollEventArgs e)
        {
            updatenumericUpDown(hScrollBar5, numericUpDown5);
        }

        private void hScrollBar6_Scroll(object sender, ScrollEventArgs e)
        {
            updatenumericUpDown(hScrollBar6, numericUpDown6);
        }

        private void hScrollBar7_Scroll(object sender, ScrollEventArgs e)
        {
            updatenumericUpDown(hScrollBar7, numericUpDown7);
        }

        private void hScrollBar8_Scroll(object sender, ScrollEventArgs e)
        {
            updatenumericUpDown(hScrollBar8, numericUpDown8);
        }

        private void hScrollBar9_Scroll(object sender, ScrollEventArgs e)
        {
            updatenumericUpDown(hScrollBar9, numericUpDown9);
        }

        private void hScrollBar10_Scroll(object sender, ScrollEventArgs e)
        {
            updatenumericUpDown(hScrollBar10, numericUpDown10);
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            UpdateHscrollBar(numericUpDown2, hScrollBar2);
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            UpdateHscrollBar(numericUpDown3, hScrollBar3);
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            UpdateHscrollBar(numericUpDown4, hScrollBar4);
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            UpdateHscrollBar(numericUpDown5, hScrollBar5);
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            UpdateHscrollBar(numericUpDown6, hScrollBar6);
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            UpdateHscrollBar(numericUpDown7, hScrollBar7);
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            UpdateHscrollBar(numericUpDown8, hScrollBar8);
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            UpdateHscrollBar(numericUpDown9, hScrollBar9);
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            UpdateHscrollBar(numericUpDown10, hScrollBar10);
        }
    }
}
