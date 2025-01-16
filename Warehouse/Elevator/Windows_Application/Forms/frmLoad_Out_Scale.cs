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
    public partial class frmLoad_Out_Scale : Form
    {
        public Toledo_Loadout_Connection Load_Out_Connection;
        private bool mouseIsDown = false;
        private Point firstPoint;
        delegate void SetTextCallback(int Subtotal,int Total , bool finished); 


        public frmLoad_Out_Scale(Guid Load_UID)
        {
            InitializeComponent();
            using (NWDatasetTableAdapters.LoadsTableAdapter LoadsTableAdapter = new NWDatasetTableAdapters.LoadsTableAdapter())
            {

                LoadsTableAdapter.FillByLoad_UID(nwDataset.Loads, Load_UID);
                this.label1.Text = "Unloading Car " + nwDataset.Loads[0].Truck_Id;

            }
            frmMain frm = (frmMain)System.Windows.Forms.Application.OpenForms["frmMain"];
            if (frm != null)
            {
                Point Location = frm.Location;
                Location.X += 20;
                Location.Y += 20;
                this.Location = Location;
            }
            using (NWDatasetTableAdapters.Site_SetupTableAdapter Site_SetupTableAdapter = new NWDatasetTableAdapters.Site_SetupTableAdapter())
            {
                Site_SetupTableAdapter.Fill(this.nwDataset.Site_Setup,Settings.Location_Id );
            }
           
            this.Load_Out_Connection = new Toledo_Loadout_Connection(this.nwDataset.Site_Setup[0].Load_Out_Ip, this.nwDataset.Site_Setup[0].Load_Out_Port );
            this.Load_Out_Connection.EventDataRecieved += new EventHandler<Toledo_Loadout_Connection.DataRecievedEventArgs>(Load_Out_Connection_EventDataRecieved);
            this.Load_Out_Connection.ConnectionError += new EventHandler(Load_Out_Connection_ConnectionError);
            this.Load_Out_Connection.Connect();
        }

        void Load_Out_Connection_ConnectionError(object sender, EventArgs e)
        {
            Alert.Show("Error Connecting To Loadout", "Load Out Connection Failed");
            this.Close();
        }

        void Load_Out_Connection_EventDataRecieved(object sender, Toledo_Loadout_Connection.DataRecievedEventArgs e)
        {
            SetText(e.Subtotal, e.Total, e.Finished);
        }

        public void Shutdown()
        {
            this.Close();
        }

        private void frmLoad_Out_Scale_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Load_Out_Connection.Disconnect();
           
        }

        private void frmLoad_Out_Scale_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouseIsDown)
            {
                // Get the difference between the two points
                int xDiff = firstPoint.X - e.Location.X;
                int yDiff = firstPoint.Y - e.Location.Y;

                // Set the new point
                int x = this.Location.X - xDiff;
                int y = this.Location.Y - yDiff;
                this.Location = new Point(x, y);
            }
        }

        private void frmLoad_Out_Scale_MouseDown(object sender, MouseEventArgs e)
        {
            firstPoint = e.Location;
            mouseIsDown = true;
        }

        private void frmLoad_Out_Scale_MouseUp(object sender, MouseEventArgs e)
        {
            mouseIsDown = false;
        }

        private void frmLoad_Out_Scale_Load(object sender, EventArgs e)
        {

        }



        private void SetText(int SubTotal , int Total, bool Finished)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.label1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { SubTotal,Total,Finished  });
            }
            else
            {
               if (Finished==false)
                {
                    this.lblSubtotal.Text = string.Format("Subtotal :{0}", SubTotal);
                }
                else
                {
                    using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                    {
                        Q.Weigh_Out(this.nwDataset.Loads[0].UID, DateTime.Now, Total);

                        using (NWDataset.LoadsDataTable loads= new NWDataset.LoadsDataTable())
                        {
                            using (NWDatasetTableAdapters.LoadsTableAdapter loadsTableAdapter= new NWDatasetTableAdapters.LoadsTableAdapter())
                            {
                                if (loadsTableAdapter.FillByLoad_UID(loads, this.nwDataset.Loads[0].UID)>0){
                                    loads[0].Weight_In = loads[0].Weight_Out ;
                                    loads[0].Weight_Out = 0;
                                    loadsTableAdapter.Update(loads);
                                }

                            }
                        }
                        Alert.Show("Rail Car Complete","Done");
                        this.Close();
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Load_Out_Connection.SendHeartbeat();
        }

        private void frmLoad_Out_Scale_DoubleClick(object sender, EventArgs e)
        {
            if (Alert.Show("Cancel Loading?", "Cancel Load", true) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
        }

    }
}
