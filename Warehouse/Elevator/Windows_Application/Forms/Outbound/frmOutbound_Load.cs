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
    public partial class frmOutbound_Load : Form
    {
        public NWDataset.vw_Outbound_LoadRow vw_Outbound_LoadRow = null;

        public Guid Outbound_Load_UID = Guid.Empty;
        public Guid Load_UID = Guid.Empty;

        string CurrentScale;

        public bool Cancel = false;

        public frmOutbound_Load()
        {
            InitializeComponent();
            Load_Lists();
            pnlEdit.Visible = false;
            this.cboWeighMaster.Text = Globals.Weighmaster;
            this.pnlOutboundWeight.Visible = false;
            Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
            Scales.CurrentInboundScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;
            if (Scales.ManualBydefault)
            {
                tmrUpdate.Stop();
                Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                Scales.CurrentInboundScaleData.CurWeight = 0;

            }
            
            UpdateWeight();

        }
        
        public frmOutbound_Load(Guid Outbound_UID)
        {
            InitializeComponent();
            this.Outbound_Load_UID = Outbound_UID;
            this.vw_Outbound_LoadTableAdapter.FillByUID(this.nWDataset.vw_Outbound_Load, Outbound_UID);
            this.vw_Outbound_LoadRow = this.nWDataset.vw_Outbound_Load[0];
            Load_Lists();
            pnlEdit.Visible = true;
            btnDelete.Visible = this.vw_Outbound_LoadRow.IsTime_OutNull();

            btnWeighOut.Visible = this.vw_Outbound_LoadRow.IsTime_OutNull();

            this.cboWeighMaster.Text = this.vw_Outbound_LoadRow.Weighmaster;
            this.txtTruckId.Text = this.vw_Outbound_LoadRow.Truck_Id;
            this.cboDeliveredTo.Text = this.vw_Outbound_LoadRow.Destination;
            if (! this.vw_Outbound_LoadRow.IsFor_Account_OfNull()) this.txtAccountOf.Text = this.vw_Outbound_LoadRow.For_Account_Of;
            this.cboCrop.Text = this.vw_Outbound_LoadRow.Crop;
            if (!this.vw_Outbound_LoadRow.IsCarrierNull()) this.cboCarrier.Text = this.vw_Outbound_LoadRow.Carrier;
            if (!this.vw_Outbound_LoadRow.IsCarrier_AddressNull()) this.txtAddress.Text = this.vw_Outbound_LoadRow.Carrier_Address;
            if (!this.vw_Outbound_LoadRow.IsCommentNull()) this.rtbComments.Text = this.vw_Outbound_LoadRow.Comment;
            this.lblPrompt.Text = string.Format("Outbound Load:{0}", vw_Outbound_LoadRow.Out_Load_Id );
            if (this.vw_Outbound_LoadRow.Void)
            {
                this.pnlInput.Enabled = false;
                this.lblPrompt.Text += "(VOID)";
                this.lblPrompt.ForeColor = System.Drawing.Color.Red;
                this.btnDelete.Text = "Restore";
                this.btnOk.Visible = false;
                this.btnWeighOut.Visible = false;
                this.btnEditInbound.Visible = false;
                this.btnEditOutbound.Visible = false;
                this.btnReprint.Visible = false;
                this.btnCancel.Text = "OK";
            }
            else
            {
                this.btnDelete.Text = "Void";
            }
            this.ckBarge.Checked = this.vw_Outbound_LoadRow.Barge;
            this.pnlOutboundWeight.Visible = false;
            Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
            if (this.vw_Outbound_LoadRow.IsTime_OutNull())
            {
                Scales.CurrentOutboundScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK ;
                Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                Scales.CurrentInboundScaleData.CurWeight = this.vw_Outbound_LoadRow.Weight_In; 
            }
            else 
            {
                Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off ;
                Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                Scales.CurrentInboundScaleData.CurWeight = this.vw_Outbound_LoadRow.Weight_In;
                Scales.CurrentOutboundScaleData.CurWeight = this.vw_Outbound_LoadRow.Weight_Out;
                pnlOutboundWeight.Visible = true;
            }
            if (Scales.ManualBydefault)
            {
                tmrUpdate.Stop();
                Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                Scales.CurrentOutboundScaleData.CurWeight = 0;

            }
            
            UpdateWeight();

        }




        private void Load_Lists()
        {
            Scales.SetScale(ref this.cboScale);
            this.outbound_CarriersTableAdapter.Fill(this.nWDataset.Outbound_Carriers);
            this.outbound_DestinationsTableAdapter.Fill(this.nWDataset.Outbound_Destinations);
            this.weighMastersTableAdapter.Fill(this.nWDataset.WeighMasters);
            this.cropsTableAdapter.Fill(this.nWDataset.Crops);
            this.weigh_ScalesTableAdapter.Fill(this.nWDataset.Weigh_Scales, Settings.Location_Id);
            this.cboCarrier.SelectedIndex = -1;
            this.cboCrop.SelectedIndex = -1;
            this.cboDeliveredTo.SelectedIndex = -1;
            this.cboWeighMaster.SelectedIndex = -1;
            foreach (Control ctrl in this.pnlInput.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;
            }
            this.btnOk.GotFocus += new EventHandler(btnOk_GotFocus);
            this.btnOk.LostFocus += new EventHandler(btnOk_LostFocus);
            this.btnCancel.GotFocus += new EventHandler(btnCancel_GotFocus);
            this.btnCancel.LostFocus += new EventHandler(btnCancel_LostFocus);

        }

        private void frmOutbound_Load_Load(object sender, EventArgs e)
        {
            Program.FrmMain.Hide();
            

       
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void rtbComments_TextChanged(object sender, EventArgs e)
        {

        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            UpdateWeight();
        }

        private void UpdateWeight()
        {
            if (cboScale.SelectedIndex > -1)
            {
                CurrentScale = cboScale.Text;
                SetScaleDisplay(Scales.CurrentInboundScaleData, ref lblInboundWt, ref lblInStatus, ref btnEditInbound);
                SetScaleDisplay(Scales.CurrentOutboundScaleData, ref lblOutboundWt, ref  lblOutStatus, ref btnEditOutbound);

            }


        }


        private void SetScaleDisplay(Scales.ScaleData SelectedScaleData, ref Label lblWeight, ref Label lblStatus, ref Button editButton)
        {
            string StatusString = "";
            System.Drawing.Color BackColor = System.Drawing.Color.White;
            if (SelectedScaleData.CurrentStatus == Scales.ScaleData.enumStatus.OK || SelectedScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Off)
            {
                if (SelectedScaleData.Motion)
                {
                    StatusString = "Motion";
                    BackColor = System.Drawing.Color.Yellow;
                }
            }
            else
            {
                BackColor = System.Drawing.Color.Pink;
                StatusString = SelectedScaleData.CurrentStatus.ToString();
            }

            lblWeight.Text = string.Format("{0:N0} lbs", SelectedScaleData.CurWeight);
            lblStatus.Text = StatusString;
            lblWeight.BackColor = BackColor;

            if (SelectedScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Off)
            {
                editButton.Text = "Edit";

            }
            else if ((!Scales.ManualBydefault) && (SelectedScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Manual && this.nWDataset.Weigh_Scales.Count > 0))
            {
                editButton.Text = "Auto";
            }
            else
            {
                editButton.Text = Scales.Manual;
            }
            if (pnlOutboundWeight.Visible)
            {
                int G;
                int T;
                int N;
                if (Scales.CurrentInboundScaleData.CurWeight < Scales.CurrentOutboundScaleData.CurWeight)
                {
                    G = Scales.CurrentOutboundScaleData.CurWeight;
                    T = Scales.CurrentInboundScaleData.CurWeight;

                }
                else
                {
                    G = Scales.CurrentInboundScaleData.CurWeight;
                    T = Scales.CurrentOutboundScaleData.CurWeight;
                }
                N = Math.Abs(G - T);
                this.lblGross.Text = string.Format("{0:N0} lbs", G);
                this.lblTare.Text = string.Format("{0:N0} lbs", T);
                this.lblNet.Text = string.Format("{0:N0} lbs", N);
            }
        }


        void btnCancel_LostFocus(object sender, EventArgs e)
        {
            this.btnCancel.BackColor = Color.Red;
            this.btnCancel.ForeColor = Color.White;

        }

        void btnCancel_GotFocus(object sender, EventArgs e)
        {
            //this.btnCancel.BackColor = Color.Pink;
            //this.btnCancel.ForeColor = Color.Black;

        }

        void btnOk_LostFocus(object sender, EventArgs e)
        {

            this.btnOk.BackColor = Color.SeaGreen;
            this.btnOk.ForeColor = Color.White;
        }

        void btnOk_GotFocus(object sender, EventArgs e)
        {
            this.btnOk.BackColor = Color.Lime;
            this.btnOk.ForeColor = Color.Black;
        }



        void ctrl_LostFocus(object sender, EventArgs e)
        {
            var ctrl = sender as Control;
            if (ctrl.Tag is Color)
                ctrl.BackColor = Color.White;
        }

        void ctrl_GotFocus(object sender, EventArgs e)
        {
            var ctrl = sender as Control;
            ctrl.Tag = ctrl.BackColor;
            ctrl.BackColor = Color.LightYellow;
        }




        private void btnEditInbound_Click_1(object sender, EventArgs e)
        {
            EditWeight(true);
        }


        private void Control_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                e.SuppressKeyPress = true;

                this.SelectNextControl((Control)sender, true, true, true, true);
                SetTabStops();


            }
        }


        private void SetTabStops()
        {

            foreach (Control ctrl in this.pnlInput.Controls)
            {
                ctrl.TabStop = string.IsNullOrEmpty(ctrl.Text);

            }



        }

        private void EditWeight(bool InboundScale)
        {
            Scales.ScaleData SelectedScaleData;
            Button editButton;
            Label WeightLabel;
            Label StatusLabel;
            string Prompt = "";
            int DefaultWeight = 0;
            long? Load_Id = null;
           
            if (InboundScale)
            {
                SelectedScaleData = Scales.CurrentInboundScaleData;
                DefaultWeight = Scales.CurrentInboundScaleData.CurWeight;
                editButton = this.btnEditInbound;
                StatusLabel = this.lblInStatus;
                WeightLabel = this.lblInboundWt;
                if (Scales.CurrentInboundScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Off)
                {
                    Prompt = "Edit Inbound Weight";

                }
                else if (Scales.CurrentInboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Manual)
                {
                    Prompt = "Manual Inbound Weight";
                }
            }
            else
            {
                SelectedScaleData = Scales.CurrentOutboundScaleData;
                DefaultWeight = Scales.CurrentOutboundScaleData.CurWeight;
                editButton = this.btnEditOutbound;
                StatusLabel = this.lblOutStatus;
                WeightLabel = this.lblOutboundWt;
                if (Scales.CurrentOutboundScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Off)
                {
                    Prompt = "Edit Outbound Weight";

                }
                else if (Scales.CurrentInboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Manual)
                {
                    Prompt = "Manual Outbound Weight";
                }
            }

            if (SelectedScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Off)
            {

                using (Manual_Weight frm = new Manual_Weight(Prompt, DefaultWeight, Load_Id, -1,this.ckBarge.Checked ))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SelectedScaleData.CurWeight = frm.Weight;
                        SelectedScaleData.Motion = false;
                        SelectedScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                    }
                }
            }

            else if (SelectedScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Manual)
            {
                SelectedScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;
            }

            else
            {


                using (Manual_Weight frm = new Manual_Weight(Prompt, DefaultWeight, Load_Id, -1, this.ckBarge.Checked))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SelectedScaleData.CurWeight = frm.Weight;
                        SelectedScaleData.Motion = false;
                        SelectedScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;

                    }
                }
            }

            if (InboundScale)
            {
                Scales.CurrentInboundScaleData = SelectedScaleData;
            }
            else
            {
                Scales.CurrentOutboundScaleData = SelectedScaleData;
            }

            SetScaleDisplay(SelectedScaleData, ref WeightLabel, ref StatusLabel, ref editButton);

        }

        private void btnEditOutbound_Click(object sender, EventArgs e)
        {
            EditWeight(false);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult DR = SaveData();
            if (DR == DialogResult.OK || DR == DialogResult.Ignore)
            {
                Globals.Weighmaster = this.cboWeighMaster.Text;    
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }

        }

        private System.Windows.Forms.DialogResult SaveData()
        {
            DialogResult DR = System.Windows.Forms.DialogResult.OK;

            if (DR == System.Windows.Forms.DialogResult.OK && this.cboWeighMaster.SelectedIndex == -1 && string.IsNullOrEmpty(this.cboWeighMaster.Text))
            {

                Alert.Show("You Need To Select The WeighMaster", "Select A Weighmaster");
                this.cboWeighMaster.Focus();
                DR = System.Windows.Forms.DialogResult.Cancel;

            }
            if (DR == System.Windows.Forms.DialogResult.OK && (this.cboWeighMaster.SelectedIndex == -1) && (!string.IsNullOrEmpty(this.cboWeighMaster.Text)))
            {
                if (Alert.Show(this.cboWeighMaster.Text + " Does Not Exists" + System.Environment.NewLine + "Create It?", "New Weighmaster", true) == System.Windows.Forms.DialogResult.No)
                {
                    this.cboWeighMaster.Text = "";
                    this.cboWeighMaster.Focus();
                    DR = System.Windows.Forms.DialogResult.Cancel;


                }
                else
                {
                    string WeighMaster= this.cboWeighMaster.Text;
                    this.nWDataset.WeighMasters.AddWeighMastersRow(Guid.NewGuid(), WeighMaster);
                    this.cboWeighMaster.Text = WeighMaster;
                }

            }

            if (DR == System.Windows.Forms.DialogResult.OK && string.IsNullOrEmpty(this.txtTruckId.Text))
            {
                Alert.Show("You Need A Truck Id #", "TRUCK ID");
                this.txtTruckId.Focus();
                DR = System.Windows.Forms.DialogResult.Cancel;
            }

            else
            {

                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    if (this.vw_Outbound_LoadRow == null || this.vw_Outbound_LoadRow.Truck_Id.ToUpper() != this.txtTruckId.Text.ToUpper())
                    {
                        bool TruckIdUsed = (bool)Q.Truck_Id_In_Yard(this.txtTruckId.Text, Settings.Location_Id);
                        if (TruckIdUsed)
                        {
                            Alert.Show("TRUCK ID ALREADY IN USE", "CHANGE TRUCK ID");
                            this.txtTruckId.Focus();
                            DR = System.Windows.Forms.DialogResult.Cancel;

                        }
                    }
                }

            }

            if (DR == System.Windows.Forms.DialogResult.OK &&  string.IsNullOrEmpty(this.cboDeliveredTo.Text))
            {

                Alert.Show("Where Is Load Going", "Select Delivered To");
                this.cboDeliveredTo.Focus();
                DR = System.Windows.Forms.DialogResult.Cancel;

            }





            if (DR == System.Windows.Forms.DialogResult.OK && string.IsNullOrEmpty(this.cboCrop.Text ))
            {

                Alert.Show("What Is The Commodity", "Select Commodity");
                this.cboCrop.Focus();
                DR = System.Windows.Forms.DialogResult.Cancel;

            }









            //Get the Weight
            if (DR == System.Windows.Forms.DialogResult.OK)
            {

                if (Scales.CurrentInboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off && Scales.CurrentInboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Manual)
                {
                    if ((Scales.CurrentInboundScaleData.Motion && Scales.CurrentInboundScaleData.CurrentStatus == Scales.ScaleData.enumStatus.OK) || (Scales.CurrentInboundScaleData.CurrentStatus != Scales.ScaleData.enumStatus.OK))
                    {
                        using (frmMotion frm = new frmMotion(Scales.CurrentInboundScaleData))
                        {
                            DR = frm.ShowDialog();
                        }
                    }


                }


                if (this.pnlOutboundWeight.Visible && Scales.CurrentOutboundScaleData.ConnectionStatus  != Scales.ScaleData.enumConnectionStatus.Off && Scales.CurrentOutboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Manual)
                {
                    if ((Scales.CurrentOutboundScaleData.Motion && Scales.CurrentOutboundScaleData.CurrentStatus == Scales.ScaleData.enumStatus.OK) || (Scales.CurrentOutboundScaleData.CurrentStatus != Scales.ScaleData.enumStatus.OK))
                    {
                        using (frmMotion frm = new frmMotion(Scales.CurrentOutboundScaleData))
                        {
                            DR = frm.ShowDialog();
                        }
                    }


                }

            }








            if (DR == System.Windows.Forms.DialogResult.OK)
            {
                using (NWDatasetTableAdapters.Site_SetupTableAdapter Site_SetupTableAdapter = new NWDatasetTableAdapters.Site_SetupTableAdapter())
                {
                    Site_SetupTableAdapter.Fill(nWDataset.Site_Setup, Settings.Location_Id);
                }
                int location = nWDataset.Site_Setup[0].Location_Id;
  

                string WeighMaster = this.cboWeighMaster.Text;
                string Truck_Id = this.txtTruckId.Text;
                string Destination = this.cboDeliveredTo.Text;

                NWDataset.CropsRow  SelectedRow;
                SelectedRow = (NWDataset.CropsRow)(DataRow)((DataRowView)this.cropsBindingSource.Current).Row;
                int Crop_Id = SelectedRow.Id;
                string ForAccountOf = null;
                if (!string.IsNullOrEmpty(this.txtAccountOf.Text))
                {
                    ForAccountOf = this.txtAccountOf.Text;
                }
                string Carrier = null;
                if (!string.IsNullOrEmpty(this.cboCarrier.Text))
                {
                    Carrier = this.cboCarrier.Text;
                }
                string Address = null;
                if (!String.IsNullOrEmpty(this.txtAddress.Text))
                {
                    Address = this.txtAddress.Text;
                }

                
                string Comment = string.Empty;
                if (!string.IsNullOrEmpty(this.rtbComments.Text))
                {
                    Comment = rtbComments.Text;
                }


                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    DateTime TimeWeighed = DateTime.Now;


                    if (Outbound_Load_UID == Guid.Empty)
                    {
                        bool Manual_WeightIn = Scales.CurrentInboundScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Manual;
                        int Weight = Scales.CurrentInboundScaleData.CurWeight;
                        
                        this.Load_UID =(Guid) Q.Create_Outbound_Load(Destination,
                                                                       Crop_Id,
                                                                       Carrier,
                                                                       Address,
                                                                       ForAccountOf,
                                                                       WeighMaster,
                                                                       Weight,
                                                                       Manual_WeightIn,
                                                                       location,
                                                                       Truck_Id,
                                                                       Comment,
                                                                       this.ckBarge.Checked );
                        Globals.Weighmaster = WeighMaster;
                        if (!this.ckBarge.Checked)
                        {
                           Printing.PrintOutbound_InyardTicket( Load_UID , Settings.workStation_SetupRow.Weigh_Scale);
                        }
                    }
                    else 
                    {
                        bool Manual_WeightIn = Scales.CurrentInboundScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Manual;
                        bool Manual_WeightOut = Scales.CurrentOutboundScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Manual;
                        int? WeightOut = null; 
                        int WeightIn = Scales.CurrentInboundScaleData.CurWeight;
                        DateTime? TimeOut = null;
                        bool PrintTicket = false;
                        if (this.pnlOutboundWeight.Visible)
                        {
                            if (vw_Outbound_LoadRow.IsTime_OutNull())
                            {
                                WeightOut = Scales.CurrentOutboundScaleData.CurWeight;
                                TimeOut = DateTime.Now;
                                PrintTicket = true;     
                            }
                            else
                            {
                                WeightOut = Scales.CurrentOutboundScaleData.CurWeight;
                                TimeOut = vw_Outbound_LoadRow.Time_Out;
                            }
                            

                        }
                        Q.Update_Outbound_Load(this.Outbound_Load_UID,
                                                    Destination,
                                                    Crop_Id,
                                                    Carrier,
                                                    Address,
                                                    ForAccountOf,
                                                    WeighMaster,
                                                    WeightIn,
                                                    Manual_WeightIn,
                                                    location,
                                                    Truck_Id,
                                                    Comment,
                                                    TimeOut,
                                                    WeightOut,
                                                    Manual_WeightOut,
                                                    this.ckBarge.Checked);
                        if (PrintTicket)
                        {
                            if (!this.ckBarge.Checked)
                            {
                                Printing.PrintOutbound_Final_Kiosk_Ticket(this.vw_Outbound_LoadRow.Load_UID, Settings.workStation_SetupRow.Weigh_Scale);
                            }
                            else
                            {
                                Printing.PrintOutbound_Final_Office_Ticket(this.vw_Outbound_LoadRow.Load_UID); 
                            }
                        }
                    }
                    Q.Add_Outbound_Carrier(Carrier,Address);
                    Q.Add_Outbound_Destination(Destination);
                    Q.Update_WeighMaster(WeighMaster);
                }
            }
            return DR;
        }

        private void cboCrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboCrop.FindString(this.cboCrop.Text) == -1)
            {
                this.cboCrop.Text = "";
                this.cboCrop.SelectedIndex = -1;
            }
        }

        private void cboCrop_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void cboCrop_Validating(object sender, CancelEventArgs e)
        {
            if (this.cboCrop.FindString(this.cboCrop.Text) == -1)
            {
                this.cboCrop.Text = "";
                this.cboCrop.SelectedIndex = -1;
            }
        }

        private void cboCarrier_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void btnReprint_Click(object sender, EventArgs e)
        {
            DialogResult DR = SaveData();
            if (DR == DialogResult.OK)
            {
                string Printer = string.Empty ;
                bool PrintToKiosk = true;
                using (frmSelect_Printer frm = new frmSelect_Printer("Where Do You Want To Print the Ticket"))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        Printer = frm.PrinterName;
                        if (string.IsNullOrEmpty(Printer))
                        {
                            using (PrintDialog pd = new PrintDialog())
                            {
                                if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                {
                                    Printer = pd.PrinterSettings.PrinterName;
                                    PrintToKiosk = false;
                                }
                                else
                                {
                                    Printer = string.Empty;

                                }
                            }


                        }
                        if (Printer != string.Empty)
                        {
                            if (vw_Outbound_LoadRow.IsTime_OutNull())
                            {
                                Printing.PrintOutbound_InyardTicket(this.vw_Outbound_LoadRow.Load_UID ,"",true,Printer);

                            }
                            else
                            {
                                if (PrintToKiosk == true)
                                {
                                    Printing.PrintOutbound_Final_Kiosk_Ticket(this.vw_Outbound_LoadRow.Load_UID,"",Printer);
                                }
                                else
                                {
                                    Printing.PrintOutbound_Final_Office_Ticket(this.vw_Outbound_LoadRow.Load_UID, Printer);
                                }
                            }
                        }
                    }
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string Message = string.Format("Void Outbound Load:{0} ", this.vw_Outbound_LoadRow.Load_Id); 
            if (vw_Outbound_LoadRow.Void)
            {
                Message = string.Format("Restore Outbound Load:{0} ", this.vw_Outbound_LoadRow.Load_Id); 
            }
            if (Alert.Show(Message , "Confirm ", true) == System.Windows.Forms.DialogResult.Yes)
            {
                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    Q.Change_Load_Void_State(vw_Outbound_LoadRow.Load_UID, !vw_Outbound_LoadRow.Void );
                    
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void btnWeighOut_Click(object sender, EventArgs e)
        {
            Scales.CurrentOutboundScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;
            this.pnlEdit.Visible = false;
            this.pnlOutboundWeight.Visible = true;
            UpdateWeight();
        }

        private void btnEditInbound_Click(object sender, EventArgs e)
        {
            EditWeight(true);
        }

        private void btnEditOutbound_Click_1(object sender, EventArgs e)
        {
            EditWeight(false);
        }

        private void frmOutbound_Load_Activated(object sender, EventArgs e)
        {
            Loading.Close();
            this.cboWeighMaster.Focus();
        }

        private void cboWeighMaster_Validating(object sender, CancelEventArgs e)
        {
     
        }

        private void cboCarrier_Validating(object sender, CancelEventArgs e)
        {
            if (this.cboCarrier.SelectedIndex > -1)
            {
                NWDataset.Outbound_CarriersRow SelectedRow;
                SelectedRow = (NWDataset.Outbound_CarriersRow)(DataRow)((DataRowView)this.outboundCarriersBindingSource.Current).Row;
                if (!SelectedRow.IsCarrier_AddressNull()) this.txtAddress.Text = SelectedRow.Carrier_Address;
            }
        }

        private void ckBarge_CheckedChanged(object sender, EventArgs e)
        {
            if (!this.pnlOutboundWeight.Visible)
            {
                if (this.ckBarge.Checked)
                {
                    Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                    Scales.CurrentInboundScaleData.CurWeight = 0;
                    this.pnlInbound.Visible = false;

                }
                else
                {

                    Scales.CurrentInboundScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK ;
                    Scales.CurrentInboundScaleData.CurWeight = 0;
                    this.pnlInbound.Visible = true ;

                }
                this.UpdateWeight();
            }

        }

        private void bwUpdateWeight_DoWork(object sender, DoWorkEventArgs e)
        {
        
        }

        private void frmOutbound_Load_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cancel = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void frmOutbound_Load_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.FrmMain.Show();
        }
    }
}
