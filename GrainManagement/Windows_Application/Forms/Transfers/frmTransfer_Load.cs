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
    public partial class frmTransfer_Load : Form
    {
        bool Startup = true;
        int TimesWeighedIn;
        public NWDataset.vwTransfer_Weight_SheetRow  vwTransfer_Weight_SheetRow = null;
        public NWDataset.vwTransfer_Weight_Sheet_InformationRow vwTransfer_Weight_Sheet_InformationRow = null;
        public int SourceBinCount;

        public Guid Weight_Sheet_UID = Guid.Empty;
        public Guid Load_UID = Guid.Empty;

        public VirtualDataset.SplitWeightDataTable splitWeights = new VirtualDataset.SplitWeightDataTable();

        public string SourceBinName
        {
            get
            {
                if (SourceBinCount > 1)
                {
                    if (btnSourceBin.Text == "Not Set")
                    {
                        return string.Empty;
                    }
                    else
                    {
                        return btnSourceBin.Text;
                    }

                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    if (SourceBinCount > 1)
                    {
                        btnSourceBin.Text = "Not Set";
                        btnSourceBin.BackColor = Color.Yellow;
                    }
                    else
                    {
                        btnSourceBin.Text = "";
                        btnSourceBin.Enabled = false;
                        //btnSourceBin.BackColor = Color.Lime;
                    }
                }
                else
                {
                    btnSourceBin.Text = value;
                    btnSourceBin.BackColor = Color.Lime;

                }
            }
        }



        public string CurrentBinName
        {
            get
            {
                if (btnBin.Text=="Not Set")
                {
                    return string.Empty;
                }
                else
                {
                    return btnBin.Text;
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value ))
                {
                    btnBin.Text = "Not Set";
                    btnBin.BackColor = Color.Yellow;    
                }
                else
                {
                    btnBin.Text = value;
                    btnBin.BackColor = Color.Lime;

                }
            }
        }



        string CurrentScale;

        public bool Cancel = false;

        //New Load
        public frmTransfer_Load(Guid Weight_Sheet_UID )
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            this.Weight_Sheet_UID = Weight_Sheet_UID;
            Load_Lists();
            CurrentBinName = string.Empty;
            SourceBinName = string.Empty;

            this.cboWeighMaster.Text = vwTransfer_Weight_Sheet_InformationRow.Weighmaster;
            pnlEdit.Visible = false;
            this.cboWeighMaster.Text = Globals.Weighmaster;
            this.pnlOutboundWeight.Visible = false;
            Scales.CurrentInboundScaleData = new Scales.ScaleData("InboundScale");
            Scales.CurrentOutboundScaleData = new NWGrain.Scales.ScaleData("OutboundScale");
            Scales.GetScaleByDescription(Scales.Manual).CurWeight = 0;
         
            this.cboScale.Visible = !vwTransfer_Weight_Sheet_InformationRow.Is_Loadout;

            btnTareLookup.Visible = Settings.AllowTareLookup;
            if (this.vwTransfer_Weight_Sheet_InformationRow.Is_Loadout)
            {
                
                this.lblPrompt.Text = "Rail Car Transfer Load";
                this.lblTruckId.Text = "Rail Car ID:";
                this.lblPrompt.BackColor = System.Drawing.Color.Red;
                this.lblPrompt.ForeColor = System.Drawing.Color.White;
 
                Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                Scales.CurrentInboundScaleData.CurWeight = 0;
            }
            else
            {
                Scales.CurrentInboundScaleData = Scales.GetScaleByDescription(cboScale.Text);
                if (cboScale.Text != "Manual")
                {
                    Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Connected;
                }
                else
                {
                    Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                }
            }
         
            UpdateWeight();
    
        }


        //Edit Load
        public frmTransfer_Load(Guid Weight_Sheet_UID,Guid Selected_Load_UID, bool ViewOnly)
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            GetLoadValues(Weight_Sheet_UID, Selected_Load_UID, ViewOnly);
        }


        public void GetLoadValues(Guid Weight_Sheet_UID, Guid Selected_Load_UID, bool ViewOnly)
        {
            btnTareLookup.Visible = false;

            this.Weight_Sheet_UID = Weight_Sheet_UID;
            Load_UID = Selected_Load_UID;
            this.vwTransfer_Weight_SheetTableAdapter.FillByLoad_UID(this.nWDataset.vwTransfer_Weight_Sheet, Selected_Load_UID);
            this.vwTransfer_Weight_SheetRow = this.nWDataset.vwTransfer_Weight_Sheet[0];
            if (this.vwTransfer_Weight_SheetRow.IsCommentNull()) vwTransfer_Weight_SheetRow.Comment = string.Empty;
            //Screwed up and grabbed the wrong comment. Too late in the season to mess with the database so had to do this.
            string Comment = "";
            if (this.loadsTableAdapter.FillByLoad_UID(this.nWDataset.Loads, vwTransfer_Weight_SheetRow.Load_UID) > 0)
            {
                if (!this.nWDataset.Loads[0].IsCommentNull()) { Comment = this.nWDataset.Loads[0].Comment; }
            }

            Load_Lists();
            CurrentBinName = string.Empty;
            SourceBinName = string.Empty;

            Scales.CurrentInboundScaleData = new Scales.ScaleData("InboundScale");
            Scales.CurrentOutboundScaleData = new NWGrain.Scales.ScaleData("OutboundScale");

            Scales.GetScaleByDescription(Scales.Manual).CurWeight = 0;
            pnlEdit.Visible = true;
            btnDelete.Visible = this.vwTransfer_Weight_SheetRow.IsTime_OutNull();

            btnWeighOut.Visible = this.vwTransfer_Weight_SheetRow.IsTime_OutNull();

            this.cboWeighMaster.Text = vwTransfer_Weight_Sheet_InformationRow.Weighmaster;
            this.cboScale.Visible = !vwTransfer_Weight_Sheet_InformationRow.Is_Loadout;

            this.txtTruckId.Text = this.vwTransfer_Weight_SheetRow.Truck_Id;

            this.rtbComments.Text = Comment;
            TimesWeighedIn = Misc.SplitWeight.Weighments(this.vwTransfer_Weight_SheetRow.Load_UID , true);
            
            if (!this.vwTransfer_Weight_SheetRow.IsBolNull()) this.txtBol.Text = this.vwTransfer_Weight_SheetRow.Bol;
            if (!this.vwTransfer_Weight_SheetRow.IsBinNull()) this.CurrentBinName = this.vwTransfer_Weight_SheetRow.Bin;
            if (!this.vwTransfer_Weight_SheetRow.IsSource_BinNull ()) this.SourceBinName  = this.vwTransfer_Weight_SheetRow.Source_Bin ;
            try
            {
                if (!this.vwTransfer_Weight_SheetRow.IsProteinNull()) this.txtProtein.Text = this.vwTransfer_Weight_SheetRow.Protein.ToString();
            }
            catch
            {

            }



            this.pnlOutboundWeight.Visible = false;


            if (this.vwTransfer_Weight_Sheet_InformationRow.Is_Loadout)
            {
                this.lblTruckId.Text = "Rail Car ID:";

                this.lblPrompt.Text = string.Format("Rail Car Transfer Load:{0}", vwTransfer_Weight_SheetRow.Load_Id);
                this.lblPrompt.BackColor = System.Drawing.Color.Red;
                this.lblPrompt.ForeColor = System.Drawing.Color.White;

                Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                Scales.CurrentInboundScaleData.CurWeight = 0;
            }
            else
            {
                this.lblPrompt.Text = string.Format("Transfer Load:{0}", vwTransfer_Weight_SheetRow.Load_Id);

            }



            if (this.vwTransfer_Weight_SheetRow.IsTime_OutNull())
            {

                if (!this.vwTransfer_Weight_Sheet_InformationRow.Is_Loadout) Scales.CurrentOutboundScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;
                Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                Scales.CurrentInboundScaleData.CurWeight = this.vwTransfer_Weight_SheetRow.Weight_In;
                Scales.CurrentOutboundScaleData = Scales.GetScaleByDescription(cboScale.Text);
                if (cboScale.Text != "Manual")
                {
                    Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Connected;
                }
                else
                {
                    Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                }
            }
            else
            {

                Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                Scales.CurrentInboundScaleData.CurWeight = this.vwTransfer_Weight_SheetRow.Weight_In;
                Scales.CurrentOutboundScaleData.CurWeight = this.vwTransfer_Weight_SheetRow.Weight_Out;
                pnlOutboundWeight.Visible = true;
            }

            UpdateWeight();
            if (ViewOnly)
            {
                cboScale.Visible = false;
                btnEditInbound.Visible = false;
                btnEditOutbound.Visible = false;
                pnlInput.Enabled = false;
                btnOk.Visible = false;
                btnMove.Visible = false;
                btnDelete.Visible = false;
                btnWeighOut.Visible = false;
                
            }
        }
        
        private void Load_Lists()
        {
            Scales.SetScale(ref this.cboScale);
            this.vwTransfer_Weight_Sheet_InformationTableAdapter.FillByWeight_SheetUID(this.nWDataset.vwTransfer_Weight_Sheet_Information, this.Weight_Sheet_UID);
            this.vwTransfer_Weight_Sheet_InformationRow = nWDataset.vwTransfer_Weight_Sheet_Information[0];
            this.weighMastersTableAdapter.Fill(this.nWDataset.WeighMasters);
           
   

            this.carriersTableAdapter.FillByActive_cbo_List(this.nWDataset.Carriers);
        
         
            this.cboCarrier.SelectedIndex = -1;
         
            this.cboWeighMaster.SelectedIndex = -1;

            this.lblCrop.Text = this.vwTransfer_Weight_Sheet_InformationRow.Crop_Description;
            this.lblSource.Text = this.vwTransfer_Weight_Sheet_InformationRow.Source;
            if (!this.vwTransfer_Weight_Sheet_InformationRow.IsVariety_DescriptionNull())
            {
                this.lblVariety.Text = this.vwTransfer_Weight_Sheet_InformationRow.Variety_Description;
            }
            else
            {
                   this.lblVariety.Text="";
            }
            if (!this.vwTransfer_Weight_Sheet_InformationRow.IsCarrier_DescriptionNull())
            {
                this.cboCarrier.Text = this.vwTransfer_Weight_Sheet_InformationRow.Carrier_Description;
                this.pnlEdit.Visible = !string.IsNullOrEmpty(this.cboCarrier.Text);

               



              
            }
            foreach (Control ctrl in this.pnlInput.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;
            }
            this.btnOk.GotFocus += new EventHandler(btnOk_GotFocus);
            this.btnOk.LostFocus += new EventHandler(btnOk_LostFocus);
            this.btnCancel.GotFocus += new EventHandler(btnCancel_GotFocus);
            this.btnCancel.LostFocus += new EventHandler(btnCancel_LostFocus);
            using (NWDatasetTableAdapters.BinListTableAdapter binListTableAdapter = new NWDatasetTableAdapters.BinListTableAdapter())
            {
                using (NWDataset.BinListDataTable binListDataTable = new NWDataset.BinListDataTable())
                {
                    SourceBinCount= binListTableAdapter.Fill(binListDataTable, vwTransfer_Weight_Sheet_InformationRow.Source_Id);

                }
            }
            tmrUpdate.Start();
        }

       

  

       
        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            UpdateWeight();
        }

        private void UpdateWeight()
        {
            CurrentScale = cboScale.Text;
            if (Scales.CurrentInboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off) Scales.CurrentInboundScaleData = Scales.GetScaleByDescription(CurrentScale);
            if (Scales.CurrentOutboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off) Scales.CurrentOutboundScaleData = Scales.GetScaleByDescription(CurrentScale);
            

            SetScaleDisplay(Scales.CurrentInboundScaleData, ref lblInboundWt, ref lblInStatus, ref btnEditInbound);
            SetScaleDisplay(Scales.CurrentOutboundScaleData, ref lblOutboundWt, ref lblOutStatus, ref btnEditOutbound);

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
                editButton.Visible = true;
            }
            else if (SelectedScaleData.Description == Scales.Manual)
            {
                editButton.Text = Scales.Manual;
                editButton.Visible = true;
            }
            else
            {
                editButton.Visible = false;
            }
            if (pnlOutboundWeight.Visible)
            {
                if (TimesWeighedIn > 1) editButton.Text = Scales.Manual;

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
                else if (Scales.CurrentOutboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Manual)
                {
                    Prompt = "Manual Outbound Weight";
                }
            }

            if (SelectedScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Off)
            {
                if (DefaultWeight < 0) DefaultWeight = 0;
                using (Manual_Weight frm = new Manual_Weight(Prompt, DefaultWeight, Load_Id, -1))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        
                        SelectedScaleData.CurWeight = frm.Weight;
                        SelectedScaleData.Motion = false;
                        SelectedScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                    }
                }
            }

            else if (SelectedScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Manual)
            {
             
                    if (Misc.SplitWeight.Weighments(Load_UID , true) > 1)
                    {
                        using (Manual_Weight frm = new Manual_Weight(Prompt, DefaultWeight, Load_Id, -1))
                        {
                            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {

                                SelectedScaleData.CurWeight = frm.Weight;
                                SelectedScaleData.Motion = false;
                                SelectedScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                                splitWeights = new VirtualDataset.SplitWeightDataTable();
                            }
                        }
                    }
                    else
                    {
                        {
                            SelectedScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Connected ;
                        }
                    }

             
            }

            else
            {

                using (Manual_Weight frm = new Manual_Weight(Prompt, DefaultWeight, Load_Id, -1))
                {
                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        SelectedScaleData.CurWeight = frm.Weight;
                        SelectedScaleData.Motion = false;
                        SelectedScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                        splitWeights=new NWGrain.VirtualDataset.SplitWeightDataTable();

                    }
                }
            }
         

            SetScaleDisplay(SelectedScaleData, ref WeightLabel, ref StatusLabel, ref editButton);

        }

        private void btnEditOutbound_Click(object sender, EventArgs e)
        {
            EditWeight(false);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            DialogResult DR=   SaveData();
            
            if (DR == DialogResult.OK || DR == DialogResult.Ignore)
            {
                Globals.Weighmaster = this.cboWeighMaster.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }

        }



    
        private System.Windows.Forms.DialogResult SaveData(bool PrintTicket=true)
        {
            DialogResult DR = System.Windows.Forms.DialogResult.OK;


            if (string.IsNullOrEmpty(SourceBinName) )
            {
               
                        if (SourceBinCount>1)
                        {
                            Alert.Show($"You Need To Select The Source Bin From {vwTransfer_Weight_Sheet_InformationRow.Source} ", $"Select The Source Bin ");
                            this.btnSourceBin.Focus();
                            DR = System.Windows.Forms.DialogResult.Cancel;

                        }
              

            }


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
                    if (this.vwTransfer_Weight_SheetRow == null || this.vwTransfer_Weight_SheetRow.Truck_Id.ToUpper() != this.txtTruckId.Text.ToUpper())
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





            string OriginalCarrier = string.Empty;
            string NewCarrier = this.cboCarrier.Text;
            if (this.vwTransfer_Weight_SheetRow != null )
            {
                if (!this.vwTransfer_Weight_SheetRow.IsCarrier_DescriptionNull())
                {
                    OriginalCarrier = this.vwTransfer_Weight_SheetRow.Carrier_Description;
                }
            }







            if (((vwTransfer_Weight_SheetRow != null && NewCarrier != OriginalCarrier) || ((!string.IsNullOrEmpty(OriginalCarrier)) && NewCarrier != OriginalCarrier)) && DR == System.Windows.Forms.DialogResult.OK)
            {
                {

                    string Message = "Do You Want To Change Weight Sheet From " + OriginalCarrier + " To " + NewCarrier;
                    if (String.IsNullOrEmpty(NewCarrier))
                    {
                        Message = "Do You Want To Reset Weight Sheet From " + OriginalCarrier + " To Default";
                    }
                    if (Alert.Show(Message, "Confirm Carrier", true) == System.Windows.Forms.DialogResult.No)
                    {
                        this.cboCarrier.Focus();
                        DR = System.Windows.Forms.DialogResult.Cancel;
                    }
                }
            }










            //Get the Weight
            if (DR == System.Windows.Forms.DialogResult.OK)
            {
                if (!this.vwTransfer_Weight_Sheet_InformationRow.Is_Loadout && Scales.CurrentInboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off && Scales.CurrentInboundScaleData.CurWeight<=0)
                {
                    Alert.Show("Inbound Weight 0", "Weight <= 0");
                    DR = DialogResult.Cancel; 
                }
                else if (!this.vwTransfer_Weight_Sheet_InformationRow.Is_Loadout &&  this.pnlOutboundWeight.Visible == true && Scales.CurrentOutboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off && Scales.CurrentOutboundScaleData.CurWeight <= 0)
                {
                    Alert.Show("Outbound Weight 0", "Weight <= 0");
                    DR = DialogResult.Cancel;
                }

                else if (Scales.CurrentInboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off && Scales.CurrentInboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Manual)
                {
                    if ((Scales.CurrentInboundScaleData.Motion && Scales.CurrentInboundScaleData.CurrentStatus == Scales.ScaleData.enumStatus.OK) || (Scales.CurrentInboundScaleData.CurrentStatus  != Scales.ScaleData.enumStatus.OK  ))
                    {
                        using (frmMotion frm = new frmMotion(Scales.CurrentInboundScaleData))
                        {
                            DR = frm.ShowDialog();
                        }
                    }


                }


                if (this.pnlOutboundWeight.Visible && Scales.CurrentOutboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off && Scales.CurrentOutboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Manual)
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
               
                string Carrier = null;
                if (!string.IsNullOrEmpty(this.cboCarrier.Text))
                {
                    Carrier = this.cboCarrier.Text;
                }

                
                string Comment = string.Empty;
                if (!string.IsNullOrEmpty(this.rtbComments.Text))
                {
                    Comment = rtbComments.Text;
                }


                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    DateTime TimeWeighed = DateTime.Now;
                    int? Carrier_ID = null;
                    if (string.IsNullOrEmpty(this.cboCarrier.Text))
                    {
                        Carrier_ID = null;
                    }
                    else if (this.cboCarrier.SelectedIndex > -1)
                    {
                        Carrier_ID = (int)this.cboCarrier.SelectedValue;
                    }

                    string Bin = null;
                    string BOL = null;
                    float? Protein = null;

                    if (!string.IsNullOrEmpty(txtBol.Text))
                    {
                        BOL = this.txtBol.Text; 
                    }

                    if (!string.IsNullOrEmpty(CurrentBinName))
                    {
                        Bin = this.CurrentBinName;
                    }

       

                    if (!string.IsNullOrEmpty(this.txtProtein.Text))
                    {
                        float P;
                        float.TryParse(this.txtProtein.Text,out P );
                        Protein = (float)P;
                    }

                    if (Load_UID == Guid.Empty)
                    {
                        bool Manual_WeightIn = Scales.CurrentInboundScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Manual;
                        int Weight = Scales.CurrentInboundScaleData.CurWeight;
                        Globals.Weighmaster = WeighMaster;
                        int Weight_In = Scales.CurrentInboundScaleData.CurWeight;
                        if (this.vwTransfer_Weight_Sheet_InformationRow.Is_Loadout)
                        {
                            Weight_In= 0;
                        }
                        bool Manual_Weight = Scales.CurrentInboundScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Manual;
                        ///Store In
                        try
                        {

                        
                        
                        Load_UID=(Guid)Q.Create_New_Inbound_Transfer_Load(location
                                                                ,this.vwTransfer_Weight_Sheet_InformationRow.Weight_Sheet_UID 
                                                                , TimeWeighed
                                                                , Weight_In
                                                                , Manual_Weight
                                                                , this.txtTruckId.Text
                                                                , this.rtbComments.Text
                                                                , Carrier_ID
                                                                , this.cboWeighMaster.Text
                                                                ,BOL
                                                                ,Bin
                                                                ,Protein );
                         Q.Update_Transfer_WS_Carrier(this.vwTransfer_Weight_Sheet_InformationRow.Weight_Sheet_UID, Carrier_ID, this.vwTransfer_Weight_Sheet_InformationRow.Location_Id, this.vwTransfer_Weight_Sheet_InformationRow.Source_Id);
                            SaveField(Load_UID);
                        }

                        catch (Exception ex)
                        {
                            Alert.Show(ex.Message, "Error");
                            DR = System.Windows.Forms.DialogResult.Cancel;
                        }
                        Misc.SplitWeight.SaveSplitWeights(splitWeights, Load_UID, true);
                        if (!string.IsNullOrEmpty(SourceBinName))
                        {
                            try
                            {
                                using (LoadFieldDataSet1TableAdapters.QueriesTableAdapter QSourceBin = new LoadFieldDataSet1TableAdapters.QueriesTableAdapter())
                                {
                                    QSourceBin.UpdateTransferSourceBin(SourceBinName, Load_UID);
                                }
                            }
                            catch
                            {

                            }
                        }



                        if (!this.vwTransfer_Weight_Sheet_InformationRow.Is_Loadout)
                        {

                            Logging.Add_System_Log("Print Transfer Load ", $"PrintTicket:{PrintTicket} Scales.CurrentInboundScaleData.Manual{Scales.CurrentInboundScaleData.Manual } Scales.CurrentOutboundScaleData.PrintOutbound{Scales.CurrentOutboundScaleData.PrintOutbound}  Scales.CurrentOutboundScaleData.OutboundPrinter{ Scales.CurrentInboundScaleData.PrintInbound}");
                            if (PrintTicket && !Scales.CurrentInboundScaleData.Manual && Scales.CurrentInboundScaleData.PrintInbound)
                            {
                                Printing.PrintTransfer_InyardTicket(Load_UID, "", Scales.CurrentInboundScaleData.InboundPrinter);
                                

                            }
                            
                        }
                        else
                        {
                            Load_Out_Scale.ShowFrm(Load_UID);
                        }

                    }
                    else 
                    {
                        
                        bool Manual_WeightIn = this.vwTransfer_Weight_SheetRow.Manual_Weight_In;
                        bool Manual_WeightOut = Scales.CurrentOutboundScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Manual;
                        int? WeightOut = null; 
                        int WeightIn = Scales.CurrentInboundScaleData.CurWeight;
                        DateTime? TimeOut = null;
                        PrintTicket = false;
                        bool ManualWeight = false;
                        if (this.pnlOutboundWeight.Visible)
                        {
                            ManualWeight = Manual_WeightOut;
                            if (vwTransfer_Weight_SheetRow.IsTime_OutNull())
                            {
                                WeightOut = Scales.CurrentOutboundScaleData.CurWeight;
                                TimeOut = DateTime.Now;
                                PrintTicket = true;     
                            }
                            else
                            {
                                WeightOut = Scales.CurrentOutboundScaleData.CurWeight;
                                TimeOut = vwTransfer_Weight_SheetRow.Time_Out;
                            }
                            Q.Weigh_Out(this.vwTransfer_Weight_SheetRow.Load_UID 
                                        , TimeOut
                                        , WeightOut);


                            if (!string.IsNullOrEmpty(SourceBinName))
                            {
                                try
                                {
                                    using (LoadFieldDataSet1TableAdapters.QueriesTableAdapter SourceBinQ = new LoadFieldDataSet1TableAdapters.QueriesTableAdapter())
                                    {
                                        SourceBinQ.UpdateTransferSourceBin(SourceBinName, Load_UID);
                                    }
                                }
                                catch
                                {

                                }
                            }

                            Q.UpdateManualWeightOut(Manual_WeightOut, this.vwTransfer_Weight_SheetRow.Load_UID);
                           
                        }

                        Q.Update_Transfer_Load(Load_UID
                                                , WeightIn
                                                , Manual_WeightIn
                                                , this.txtTruckId.Text
                                                , this.rtbComments.Text
                                                , Carrier_ID
                                                , WeighMaster
                                                , BOL
                                                , Bin
                                                , Protein);
                        Q.Update_Transfer_WS_Carrier(this.vwTransfer_Weight_Sheet_InformationRow.Weight_Sheet_UID, Carrier_ID, this.vwTransfer_Weight_Sheet_InformationRow.Location_Id, this.vwTransfer_Weight_Sheet_InformationRow.Source_Id);
                        Misc.SplitWeight.SaveSplitWeights(splitWeights, Load_UID, false);
                        if (PrintTicket)
                        {
                            if (!this.vwTransfer_Weight_Sheet_InformationRow.Is_Loadout)
                            {


                                //string Printer = "";
                                //Printer= string.Empty;
                                //using (frmSelect_Printer frm = new frmSelect_Printer("Where Do You Want To Print the Ticket"))
                                {
                                  //  if (frm.ShowDialog() == DialogResult.OK)
                                    {
                                    //    Printer = frm.PrinterName;
                                      //  if (string.IsNullOrEmpty(Printer))
                                        {
                                        //    using (PrintDialog pd = new PrintDialog())
                                            {
                                          //      if (pd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                                                {
                                            //        Printer = pd.PrinterSettings.PrinterName;
                                                }
                                            }
                                        }

                                        {
                                           
                                            {
                                                Logging.Add_System_Log("Print Transfer Load ", $"PrintTicket:{PrintTicket} Scales.CurrentOutboundScaleData.Manual{Scales.CurrentOutboundScaleData.Manual} Scales.CurrentOutboundScaleData.PrintOutbound{Scales.CurrentOutboundScaleData.PrintOutbound}  Scales.CurrentOutboundScaleData.OutboundPrinter{ Scales.CurrentOutboundScaleData.OutboundPrinter}");
                                                if (PrintTicket && !Scales.CurrentOutboundScaleData.Manual && Scales.CurrentOutboundScaleData.PrintOutbound)
                                                {
                                                    Printing.PrintTransfer_FinalTicket(this.Load_UID,"", Scales.CurrentOutboundScaleData.OutboundPrinter);


                                                }

                                            }
                                            
                                            DR = DialogResult.Ignore;
                                        }
                                    }
                                }
                           }
                        }
                    }
                    Q.Update_WeighMaster(WeighMaster);
                }
            }
            return DR;
        }


        public void SaveField(Guid Load_UID)
        {
            if ((Load_UID != null) && (Load_UID != Guid.Empty))
            {
                if (SiteOptions.GetPromptForTruckType())
                {
                    var result = Alert.Show("Is Truck An End Dump?", "Truck Type", true);
                    if (result == System.Windows.Forms.DialogResult.Yes)
                    {
                        try
                        {
                            using (LoadFieldDataSet1TableAdapters.QueriesTableAdapter LFQ = new LoadFieldDataSet1TableAdapters.QueriesTableAdapter())
                            {
                                LFQ.UpdateLoadField("ED", Load_UID);
                            }
                        }
                        catch
                        {
                          
                        }
                    }
                }

            }
        }
        private void btnReprint_Click(object sender, EventArgs e)
        {
            DialogResult DR = SaveData();
            if (DR == DialogResult.OK)
            {
                string Printer = string.Empty ;
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
                                }
                                else
                                {
                                    Printer = string.Empty;

                                }
                            }


                        }
                        if (Printer != string.Empty)
                        {
                            if (vwTransfer_Weight_SheetRow.IsTime_OutNull())
                            {
                                Printing.PrintTransfer_InyardTicket(this.Load_UID, "", Printer);

                            }
                            else
                            {
                                Printing.PrintTransfer_FinalTicket(this.Load_UID, "", Printer);
                            }
                        }
                    }
                }
            }

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Alert.Show(string.Format("Delete Load:{0} From Weight Sheet:{1} ", this.vwTransfer_Weight_SheetRow.Load_Id , this.vwTransfer_Weight_SheetRow.WS_Id), "Confirm Delete", true) == System.Windows.Forms.DialogResult.Yes)
            {
                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    Q.Delete_Transfer_Load(vwTransfer_Weight_SheetRow.Load_UID );
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void btnWeighOut_Click(object sender, EventArgs e)
        {
            WeighOut();


        }

        private void WeighOut()
        {
            TimesWeighedIn = Misc.SplitWeight.Weighments(Load_UID , true);
            if (TimesWeighedIn > 1)
            {
                using (frmSplit_Weigh frm = new frmSplit_Weigh(cboScale.Text, Load_UID))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        cboScale.Text = "Manual";
                        Scales.GetScaleByDescription(cboScale.Text).CurWeight = frm.GetTotalWeight();
                        Scales.CurrentOutboundScaleData.CurWeight = frm.GetTotalWeight();
                        Scales.CurrentOutboundScaleData.Motion = false;
                        Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                        splitWeights = frm.splitWeights;
                        Logging.Add_Audit_Trail("Split Weigh", "Truck ID:" + this.lblTruckId.Text, "Outbound Split Weigh", "", frm.GetWeightComment(), "");
                        this.pnlEdit.Visible = false;
                        this.pnlOutboundWeight.Visible = true;
                        

                    }
                }
            }
            else
            {

                if (!this.vwTransfer_Weight_Sheet_InformationRow.Is_Loadout)
                {
                    Scales.CurrentOutboundScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;
                }
                else
                {
                    Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                }

                this.pnlEdit.Visible = false;
                this.pnlOutboundWeight.Visible = true;
            }
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
            this.txtTruckId.Focus();
        }

        private void cboWeighMaster_Validating(object sender, CancelEventArgs e)
        {
     
        }

       
        private void button1_Click(object sender, EventArgs e)
        {
            using (frmSelect_Transfer_Weight_Sheet frm = new frmSelect_Transfer_Weight_Sheet(vwTransfer_Weight_Sheet_InformationRow.Weight_Sheet_UID,false  ))
            {
                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                    {
                        string UserResponse = User_Input.Get_User_Input(string.Format("Why Are You Moving Ticket {0}",vwTransfer_Weight_SheetRow.Load_Id));
                        if (!String.IsNullOrEmpty(UserResponse))
                        {
                            Q.Change_Transfer_Load_Weight_Sheet(this.vwTransfer_Weight_SheetRow.Load_UID,  frm.Weight_Sheet_UID, UserResponse);
                            using (NWDatasetTableAdapters.vwTransfer_Weight_SheetTableAdapter vwTransfer_Weight_SheetTableAdapter = new NWDatasetTableAdapters.vwTransfer_Weight_SheetTableAdapter())
                            using (NWDataset.vwTransfer_Weight_SheetDataTable tmpvwTransfer_Weight_Sheet = new NWDataset.vwTransfer_Weight_SheetDataTable())
                            {
                                if (vwTransfer_Weight_SheetTableAdapter.FillByLoad_UID(tmpvwTransfer_Weight_Sheet, vwTransfer_Weight_SheetRow.Load_UID) > 0)
                                {
                                    this.vwTransfer_Weight_SheetRow.Weight_Sheet_UID = frm.Weight_Sheet_UID;
                                }
                                else
                                {
                                    this.vwTransfer_Weight_SheetRow.Weight_Sheet_UID = Guid.Empty;
                                }
                            }
                                //using (NWDatasetTableAdapters.vwTransfer_Weight_SheetTableAdapter vwTransfer_Weight_SheetTableAdapter = new NWDatasetTableAdapters.vwTransfer_Weight_SheetTableAdapter())
                                //{
                                //    using (NWDataset.vwTransfer_Weight_SheetDataTable  tmpvwTransfer_Weight_Sheet = new NWDataset.vwTransfer_Weight_SheetDataTable())
                                //    {
                                //        if (vwTransfer_Weight_SheetTableAdapter.FillByLoad_UID(tmpvwTransfer_Weight_Sheet, vwTransfer_Weight_SheetRow.Load_UID) > 0)
                                //        {
                                //            NWDataset.vwTransfer_Weight_SheetRow NewRowValues = tmpvwTransfer_Weight_Sheet[0];
                                //            Logging.Add_Audit_Trail("Moved Ticket", vwTransfer_Weight_SheetRow.Load_Id.ToString(), string.Format("From Weight Sheet {0} To Weight Sheet{1}", vwTransfer_Weight_SheetRow.WS_Id.ToString(), NewRowValues.WS_Id.ToString()), vwTransfer_Weight_SheetRow.WS_Id.ToString(), NewRowValues.WS_Id.ToString(), UserResponse);

                                //        }
                                //        else
                                //        {

                                //            using (NWDatasetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new NWDatasetTableAdapters.vwWeigh_SheetTableAdapter())
                                //            {
                                //                using (NWDataset.vwWeigh_SheetDataTable tmpvwWeigh_SheetDataTable = new NWDataset.vwWeigh_SheetDataTable())
                                //                {
                                //                    vwWeigh_SheetTableAdapter.FillByLoad_UID(tmpvwWeigh_SheetDataTable, vwWeigh_SheetRow.Load_UID);
                                //                    NWDataset.vwWeigh_SheetRow NewRowValues = tmpvwWeigh_SheetDataTable[0];
                                //                    // Logging.Add_Audit_Trail("Moved Ticket", vwWeigh_SheetRow.Load_Id.ToString(), string.Format("From Weight Sheet {0} To Weight Sheet{1}", vwWeigh_SheetRow.WS_Id.ToString(), NewRowValues.WS_Id.ToString()), vwWeigh_SheetRow.WS_Id.ToString(), NewRowValues.WS_Id.ToString(), UserResponse);

                                //                }
                                //            }
                                //        }
                                //    }
                                //}

                                
                            this.DialogResult = System.Windows.Forms.DialogResult.Abort;
                            this.Close();
                        }
                    }
                }
            }
        }

        private void txtBol_Validating(object sender, CancelEventArgs e)
        {
            int BOL;

            if ((!string.IsNullOrEmpty(txtBol.Text)) && (!int.TryParse(this.txtBol.Text, out BOL)))
            {
                Alert.Show("BOL MUST BE A NUMBER", "ERROR SETTING BOL");
                this.txtBol.Text = "";
                this.txtBol.Focus();
            }
        }

      



        private void Validate_cbo(ref ComboBox cbo, string Prompt, string Header, bool AllowEmptyString, bool SetFocus)
        {

            if (cbo.SelectedIndex == -1)
            {
                if ((string.IsNullOrEmpty(cbo.Text) && AllowEmptyString == false) || (!string.IsNullOrEmpty(cbo.Text)))
                {
                    Alert.Show(Prompt, Header);

                    if ((SetFocus) || (!string.IsNullOrEmpty(cbo.Text)))
                    {
                        cbo.Focus();
                    }
                    cbo.Text = "";
                }
            }
        }

        private void cboCarrier_TextChanged(object sender, EventArgs e)
        {
        }

        private void cboCarrier_Validating(object sender, CancelEventArgs e)
        {
            if (this.cboCarrier.FindString(this.cboCarrier.Text) == -1)
            {
                this.cboCarrier.Text = "";
                this.cboCarrier.SelectedIndex = -1;
            }

        }

        private void lblInboundName_Click(object sender, EventArgs e)
        {

        }





        private void frmTransfer_Load_Load(object sender, EventArgs e)
        {
         
            Startup = false;

            this.Select();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboScale_Leave(object sender, EventArgs e)
        {
            if (this.cboScale.SelectedIndex == -1)
            {
                this.cboScale.Text = "";
            }

        }

        private void btnSplitWeigh_Click(object sender, EventArgs e)
        {

            using (frmSplit_Weigh frm = new frmSplit_Weigh(cboScale.Text ,( Load_UID!= null )?Load_UID:Guid.Empty  ))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    cboScale.Text = "Manual";
                    Scales.GetScaleByDescription(cboScale.Text).CurWeight = frm.GetTotalWeight();
                    splitWeights = frm.splitWeights;
                    
                    Logging.Add_Audit_Trail("Split Weigh", "Truck ID:" + this.lblTruckId.Text, "Transfer Split Weigh", "", frm.GetWeightComment(), "");
                }

            }
        }

        private void bwUpdateWeight_DoWork(object sender, DoWorkEventArgs e)
        {
       
        }

        private void frmTransfer_Load_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cancel = true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void frmTransfer_Load_FormClosed(object sender, FormClosedEventArgs e)
        {
         
        }


        private void cboScale_TextChanged(object sender, EventArgs e)
        {
            if (!Startup)UpdateWeight();

        }

        private void btnBin_Click(object sender, EventArgs e)
        {
            using (SelectBin frm = new NWGrain.SelectBin("",""))
            {
                if (frm.ShowDialog()== DialogResult.OK )
                {
                    CurrentBinName   = frm.SelectedBin;
                }
            }
        }

        private void btnTareLookup_Click(object sender, EventArgs e)
        {

            // cboScale.Text = "Manual";

            using (TareLookup frm = new NWGrain.TareLookup())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {

                    txtTruckId.Text = frm.VehicleID;

                    //Scales.GetScaleByDescription(cboScale.Text).CurWeight = frm.Tare;

                    DialogResult DR = SaveData(false);
                    if (DR == DialogResult.OK || DR == DialogResult.Ignore)
                    {
                        this.GetLoadValues(this.Weight_Sheet_UID , this.Load_UID, false);
                        WeighOut();
                        cboScale.Text = "Manual";
                        Scales.GetScaleByDescription(cboScale.Text).CurWeight = frm.Tare;
                        Application.DoEvents();
                        DR = SaveData();
                        //  this.DialogResult = System.Windows.Forms.DialogResult.OK;
                        Globals.Weighmaster = this.cboWeighMaster.Text;
                        this.Close();
                    }
                }

            }
        }

        #region Protein 
        string OriginalProteinValue = string.Empty;

        private void txtProtein_TextChanged(object sender, EventArgs e)
        {
            float Valid = 0;
            if (!float.TryParse(txtProtein.Text,out Valid))
            {
                txtProtein.Text = OriginalProteinValue;
            }
            else
            {
                if (Valid<0 || Valid>50)
                {
                    Alert.Show("Invalid Protein Value" , "Error Setting Protein");
                    txtProtein.Text = OriginalProteinValue;
                    txtProtein.Focus();
                }
            }
        }

        private void txtProtein_Enter(object sender, EventArgs e)
        {
            OriginalProteinValue = txtProtein.Text; 
        }
        #endregion

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void btnSourceBin_Click(object sender, EventArgs e)
        {

            try
            {
                int SourceId = this.vwTransfer_Weight_Sheet_InformationRow.Source_Id;


                using (SelectBin frm = new NWGrain.SelectBin("", this.vwTransfer_Weight_Sheet_InformationRow.Source, SourceId, ""))
                {
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        SourceBinName = frm.SelectedBin;
                    }

                }


            }
            catch
            {

            }
           
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }
    }

}
