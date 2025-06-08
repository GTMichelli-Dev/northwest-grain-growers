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
    public partial class frmHarvest_Load : Form
    {
        public VirtualDataset.SplitWeightDataTable splitWeights = new VirtualDataset.SplitWeightDataTable();
        int TimesWeighedIn;

        private Boolean FirstPass = true;
        Boolean settingRate = false;

        public NWDataset.vw_All_Weight_SheetsRow _All_Open_Weight_Sheets_Row = null;
        public NWDataset.vwWeigh_SheetRow vwWeigh_SheetRow = null;

        string CurrentScale;

        public bool Cancel = false;

        public NWDataset.Weight_SheetsRow  CurrentWeightSheetRow;

        public void SetCurrentWeightSheetRow(Guid WeightSheetUID)
        {

            using (NWDatasetTableAdapters.Weight_SheetsTableAdapter Weigh_SheetTableAdapter = new NWDatasetTableAdapters.Weight_SheetsTableAdapter())
            {
                using (NWDataset.Weight_SheetsDataTable Weigh_Sheets = new NWDataset.Weight_SheetsDataTable())
                {
                    if (Weigh_SheetTableAdapter.FillByUID(Weigh_Sheets, WeightSheetUID) > 0)
                    {
                        CurrentWeightSheetRow = Weigh_Sheets[0];
                    }
                }
            }
        }


        public string CurrentBinName
        {
            get
            {
                if (btnBin.Text == "Not Set")
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
                if (string.IsNullOrEmpty(value))
                {
                    btnBin.Text = "Not Set";
                    btnBin.BackColor = Color.Yellow;
                    btnBin.ForeColor = Color.Black;
                }
                else
                {
                    btnBin.Text = value;
                    btnBin.BackColor = Color.Lime;
                    btnBin.ForeColor = Color.Black;
                }
            }
        }



        Guid Current_Load_UID = Guid.Empty;
        Guid Current_Weight_Sheet_UID = Guid.Empty;

        /// <summary>
        /// Used for and inbound load
        /// </summary>
        /// <param name="_Open_Weigh_SheetsRow"></param>
        public frmHarvest_Load(NWDataset.vw_Open_Weight_SheetsRow Open_Weigh_SheetsRow)
        {
            InitializeComponent();






            CurrentBinName = string.Empty;
            SetCurrentWeightSheetRow(Open_Weigh_SheetsRow.Weight_Sheet_UID);
            using (NWDatasetTableAdapters.vw_All_Weight_SheetsTableAdapter vw_All_Weight_SheetsTableAdapter = new NWDatasetTableAdapters.vw_All_Weight_SheetsTableAdapter())
            {
                vw_All_Weight_SheetsTableAdapter.FillByWeight_Sheet_UID(nWDataset.vw_All_Weight_Sheets, Open_Weigh_SheetsRow.Weight_Sheet_UID);
            }
            this._All_Open_Weight_Sheets_Row = nWDataset.vw_All_Weight_Sheets[0];
            cboHauler.Enabled = this._All_Open_Weight_Sheets_Row.Total_Loads == 0;
            pnlMilage.Enabled = this._All_Open_Weight_Sheets_Row.Total_Loads == 0;
            lblHauler.Enabled = this._All_Open_Weight_Sheets_Row.Total_Loads == 0;
            foreach (Control ctrl in this.pnlInput.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;
            }


            foreach (Control ctrl in this.pnlMilage.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;
            }
            this.pnlOutboundWeight.Visible = false;
            this.btnOk.GotFocus += new EventHandler(btnOk_GotFocus);
            this.btnOk.LostFocus += new EventHandler(btnOk_LostFocus);
            this.btnCancel.GotFocus += new EventHandler(btnCancel_GotFocus);
            this.btnCancel.LostFocus += new EventHandler(btnCancel_LostFocus);

            this.lblLot.Text = String.Format("{0} ", _All_Open_Weight_Sheets_Row.Lot_Number);
            if (!this._All_Open_Weight_Sheets_Row.IsFSA_NumberNull()) this.lblFSA.Text = _All_Open_Weight_Sheets_Row.FSA_Number;
            if (!_All_Open_Weight_Sheets_Row.IsFSA_NumberNull()) this.lblFSA.Text = String.Format("{0} ", _All_Open_Weight_Sheets_Row.FSA_Number);
            this.lblProducer.Text = _All_Open_Weight_Sheets_Row.Producer_Description;
            this.lblCrop.Text = _All_Open_Weight_Sheets_Row.Crop;
            if (!this._All_Open_Weight_Sheets_Row.IsLandlordNull()) this.lblLandlord.Text = _All_Open_Weight_Sheets_Row.Landlord;




            //}

            //public void TEst(NWDataset.vw_Open_Weight_SheetsRow Open_Weigh_SheetsRow)
            //{
            Load_Lists();
            if (!this._All_Open_Weight_Sheets_Row.IsCarrierNull()) this.cboHauler.Text = this._All_Open_Weight_Sheets_Row.Carrier;
            if (!this._All_Open_Weight_Sheets_Row.IsMilesNull()) this.cboMiles.Text = this._All_Open_Weight_Sheets_Row.Miles.ToString();

            if (!this._All_Open_Weight_Sheets_Row.IsBOL_TypeNull())
            {
                string BolType = this._All_Open_Weight_Sheets_Row.BOL_Type;
                if (BolType == "A")
                {
                    this.cboBOLtype.SelectedIndex = 0;
                }
                else if (BolType == "F")
                {
                    this.cboBOLtype.SelectedIndex = 1;
                }
            }

            if (!string.IsNullOrEmpty(this._All_Open_Weight_Sheets_Row.Weighmaster))
            {
                this.cboWeighMaster.Text = Globals.Weighmaster;
            }
            else
            {

                this.cboWeighMaster.Text = this._All_Open_Weight_Sheets_Row.Weighmaster;
            }

            this.cboWeighMaster.TabStop = string.IsNullOrEmpty(this.cboWeighMaster.Text);

            Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
            if (_All_Open_Weight_Sheets_Row.Is_Loadout)
            {
                lblLotInfo.ForeColor = System.Drawing.Color.Red;
                this.lblLotInfo.Text = string.Format("New Inbound Rail Load For Weight Sheet {0}", _All_Open_Weight_Sheets_Row.WS_Id);
                this.lblLotInfo.BackColor = System.Drawing.Color.Red;
                this.lblLotInfo.ForeColor = System.Drawing.Color.White;

                Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                Scales.CurrentInboundScaleData.CurWeight = 0;
                this.pnlInbound.Visible = false;
                this.tmrUpdate.Stop();
                btnTareLookup.Visible = false;
            }
            else
            {


                btnTareLookup.Visible = Settings.AllowTareLookup;
                this.lblLotInfo.Text = string.Format("New Inbound Load For Weight Sheet {0}", _All_Open_Weight_Sheets_Row.WS_Id);
                Scales.CurrentInboundScaleData = Scales.GetScaleByDescription(cboScale.Text);
                if (cboScale.Text != "Manual")
                {
                    Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Connected;
                }
                else
                {
                    Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual;
                }
                GetLoadField(_All_Open_Weight_Sheets_Row.Lot_Number, Guid.Empty);
            }

            SetRate();



            //UpdateWeight();


        }


        public void SetRate()
        {
            if (!FirstPass && DialogResult== DialogResult.None)
            {
                settingRate = true;

                if (cboHauler.SelectedIndex == -1)
                {
                    pnlMilage.Visible = false;
                    using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                    {
                        Q.Update_WS_Carrier(this.Current_Weight_Sheet_UID, null, null, null, Settings.Location_Id);
                    }
                }
                else
                {


                    //try
                    //{
                    //    if (!CurrentWeightSheetRow.IsCarrier_IdNull())
                    //    {
                    //        cboHauler.SelectedValue = CurrentWeightSheetRow.Carrier_Id;
                    //        if (!CurrentWeightSheetRow.IsMilesNull())
                    //        {
                    //            cboMiles.Text = CurrentWeightSheetRow.Miles.ToString();
                    //        }
                    //    }
                    //}
                    //catch
                    //{

                    //}



                    var Rate = WeightSheetHaulerRate.GetWeightSheetRate(CurrentWeightSheetRow.UID);
                    if (Rate != null)
                    {
                        if (Rate.RateType == WeightSheetHaulerRate.enumRateType.Custom)
                        {
                            pnlMilage.Visible = true;
                            labelMiles.Text = "Rate";
                            numNewRate.Left = cboMiles.Left;
                            numNewRate.Value = Rate.RateAmount;
                            cboMiles.Visible = false;
                            cboBOLtype.SelectedIndex = cboBOLtype.FindString("C");
                            numNewRate.Visible = true;
                        }
                        else
                        {
                            pnlMilage.Visible = true;
                            labelMiles.Text = "Miles";
                            numNewRate.Left = cboMiles.Left;
                            cboMiles.Visible = true;
                            numNewRate.Visible = false;

                            var BolType = "";
                            if (Rate.RateType == WeightSheetHaulerRate.enumRateType.FarmStorage)
                            {
                                BolType = "F";
                            }
                            else if (Rate.RateType == WeightSheetHaulerRate.enumRateType.FarmStorage)
                            {
                                BolType = "A";
                            }

                            cboBOLtype.SelectedIndex = cboBOLtype.FindString(BolType);




                        }
                    }
                }
                settingRate = false;
            }
        }
   


        /// <summary>
        /// Used for Editing Finishing Load
        /// </summary>
        /// <param name="Load_UID">Load UID TO Edit</param>
        public frmHarvest_Load(Guid Load_UID,bool ViewOnly)
        {

            InitializeComponent();
            GetLoadValues(Load_UID, ViewOnly);
           
        }

        public void GetLoadValues(Guid Load_UID, bool ViewOnly)
        {
            CurrentBinName = string.Empty;

            using (NWDatasetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new NWDatasetTableAdapters.vwWeigh_SheetTableAdapter())
            {
                if (vwWeigh_SheetTableAdapter.FillByLoad_UID(nWDataset.vwWeigh_Sheet, Load_UID) > 0)
                {
                    this.vwWeigh_SheetRow = nWDataset.vwWeigh_Sheet[0];
                    using (NWDatasetTableAdapters.vw_All_Weight_SheetsTableAdapter vw_All_Weight_SheetsTableAdapter = new NWDatasetTableAdapters.vw_All_Weight_SheetsTableAdapter())

                    {
                        vw_All_Weight_SheetsTableAdapter.FillByWeight_Sheet_UID(nWDataset.vw_All_Weight_Sheets, vwWeigh_SheetRow.Weight_Sheet_UID);
                        this._All_Open_Weight_Sheets_Row = nWDataset.vw_All_Weight_Sheets[0];
                    }
                    GetLoadField(vwWeigh_SheetRow.Lot_Number ,Load_UID   );
                    SetCurrentWeightSheetRow(vwWeigh_SheetRow.Weight_Sheet_UID);
                }
            }
            if (vwWeigh_SheetRow == null)
            {
                Alert.Show("Cannot Find Information For Load");
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            else
            {

                foreach (Control ctrl in this.pnlInput.Controls)
                {
                    ctrl.GotFocus += ctrl_GotFocus;
                    ctrl.LostFocus += ctrl_LostFocus;
                }


                foreach (Control ctrl in this.pnlMilage.Controls)
                {
                    ctrl.GotFocus += ctrl_GotFocus;
                    ctrl.LostFocus += ctrl_LostFocus;
                }

                this.btnOk.GotFocus += new EventHandler(btnOk_GotFocus);
                this.btnOk.LostFocus += new EventHandler(btnOk_LostFocus);
                this.btnCancel.GotFocus += new EventHandler(btnCancel_GotFocus);
                this.btnCancel.LostFocus += new EventHandler(btnCancel_LostFocus);
                if (_All_Open_Weight_Sheets_Row.Is_Loadout)
                {
                    this.lblLotInfo.Text = string.Format("Rail Car Weight Sheet: {0}     Ticket: {1}", vwWeigh_SheetRow.WS_Id, vwWeigh_SheetRow.Load_Id);
                    this.lblLotInfo.BackColor = System.Drawing.Color.Red;
                    this.lblLotInfo.ForeColor = System.Drawing.Color.White;

                }
                else
                {
                    this.lblLotInfo.Text = string.Format("Weight Sheet: {0}     Ticket: {1}", vwWeigh_SheetRow.WS_Id, vwWeigh_SheetRow.Load_Id);
                }


                this.lblLot.Text = String.Format("{0} ", _All_Open_Weight_Sheets_Row.Lot_Number);
                if (!this._All_Open_Weight_Sheets_Row.IsFSA_NumberNull()) this.lblFSA.Text = _All_Open_Weight_Sheets_Row.FSA_Number;
                if (!_All_Open_Weight_Sheets_Row.IsFSA_NumberNull()) this.lblFSA.Text = String.Format("{0} ", _All_Open_Weight_Sheets_Row.FSA_Number);
                this.lblProducer.Text = _All_Open_Weight_Sheets_Row.Producer_Description;
                this.lblCrop.Text = _All_Open_Weight_Sheets_Row.Crop;
                if (!this.vwWeigh_SheetRow.IsLandlordNull()) this.lblLandlord.Text = vwWeigh_SheetRow.Landlord;
                Load_Lists();
                GetLoadField(vwWeigh_SheetRow.Lot_Number, Load_UID);

                this.cboWeighMaster.Select(0, 0);
                this.pnlEdit.Visible = true;
                Scales.CurrentInboundScaleData = new Scales.ScaleData("InboundScale");
                Scales.CurrentInboundScaleData.CurWeight = vwWeigh_SheetRow.Weight_In;
                Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;


                Scales.CurrentInboundScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;

                btnTareLookup.Visible = false;

                if (vwWeigh_SheetRow.IsTime_OutNull())
                {
                    this.pnlOutboundWeight.Visible = false;
                    this.btnWeighOut.Visible = true;
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
                    this.btnWeighOut.Visible = false;
                    this.btnDelete.Visible = false;
                    this.pnlOutboundWeight.Visible = true;
                   
                    Scales.CurrentOutboundScaleData = new Scales.ScaleData("OutboundScale");
                    Scales.CurrentOutboundScaleData.CurWeight = vwWeigh_SheetRow.Weight_Out;
                    Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                    Scales.CurrentOutboundScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;

                }

              
                this.cboWeighMaster.Text = this.vwWeigh_SheetRow.Weighmaster;
                this.txtTruckId.Text = this.vwWeigh_SheetRow.Truck_Id;
                if (!this.vwWeigh_SheetRow.IsBolNull()) this.txtBol.Text = this.vwWeigh_SheetRow.Bol;
                if (!this.vwWeigh_SheetRow.IsBinNull()) CurrentBinName = this.vwWeigh_SheetRow.Bin;
                if (!this.vwWeigh_SheetRow.IsProtienNull()) this.cboProtien.Text = this.vwWeigh_SheetRow.Protien.ToString();
                if (!this.vwWeigh_SheetRow.IsCarrier_DescriptionNull()) this.cboHauler.Text = this.vwWeigh_SheetRow.Carrier_Description;
                if (!this.vwWeigh_SheetRow.IsMilesNull()) this.cboMiles.Text = this.vwWeigh_SheetRow.Miles.ToString();
                if (!this.vwWeigh_SheetRow.IsLoad_CommentNull())
                {
                    this.rtbComments.Text = this.vwWeigh_SheetRow.Load_Comment;
                    TimesWeighedIn = Misc.SplitWeight.Weighments(this.vwWeigh_SheetRow.Load_UID , true);
                }

                if (!this.vwWeigh_SheetRow.IsBOL_TypeNull())
                {
                    string BolType = this.vwWeigh_SheetRow.BOL_Type;
                    if (BolType == "A")
                    {
                        this.cboBOLtype.SelectedIndex = 0;
                    }
                    else if (BolType == "F")
                    {
                        this.cboBOLtype.SelectedIndex = 1;
                    }
                }
                //this.btnEditInbound.Visible = !this.vwWeigh_SheetRow.Original_Printed;
                //this.btnEditOutbound.Visible = !this.vwWeigh_SheetRow.Original_Printed;
                //this.btnMove.Visible = !this.vwWeigh_SheetRow.Original_Printed;
                //this.btnOk.Visible =  !this.vwWeigh_SheetRow.Original_Printed;
                //this.pnlInput.Enabled = !this.vwWeigh_SheetRow.Original_Printed;
                //this.pnlMilage.Enabled = !this.vwWeigh_SheetRow.Original_Printed;
                //this.pnlComment.Enabled = !this.vwWeigh_SheetRow.Original_Printed;

                this.btnCancel.Focus();

                if (ViewOnly)
                {
                    cboScale.Visible = false;
                    btnEditInbound.Visible = false;
                    btnEditOutbound.Visible = false;
                  
                    pnlInput.Enabled = false;
                    pnlMilage.Enabled = false;
                    btnMove.Visible = false;
                    btnDelete.Visible = false;
                    btnOk.Visible = false;
                    btnWeighOut.Visible = false;
                    btnCancel.BackColor = btnWeighOut.BackColor;
                    btnCancel.Visible = true;
                    pnlField.Enabled = false;
                }


            }
        }

        

        public void GetLoadField(long LotId, Guid Load_UID )
        {
            try
            {
                using (LoadFieldDataSet1 loadsFieldDataset = new LoadFieldDataSet1())
                {
                    using (LoadFieldDataSet1TableAdapters.ProducerVarietyFieldTableAdapter producerVarietyFieldTableAdapter = new LoadFieldDataSet1TableAdapters.ProducerVarietyFieldTableAdapter())
                    {
                        producerVarietyFieldTableAdapter.FillByLot_Number(loadsFieldDataset.ProducerVarietyField, LotId);
                        pnlField.Visible = (loadsFieldDataset.ProducerVarietyField.Count) > 0;
                        if (loadsFieldDataset.ProducerVarietyField.Count>0 )
                        {
                            cboField.Items.Clear();
                            cboField.DataSource = loadsFieldDataset.ProducerVarietyField;
                            cboField.DisplayMember = "Field";
                            cboField.ValueMember = "Variety_Id";
                            cboField.SelectedIndex = (loadsFieldDataset.ProducerVarietyField.Count>1)? - 1:0;
                            cboField.Text = "";
                            //foreach (var item in loadsFieldDataset.ProducerVarietyField)
                            //{
                            //    ComboBox.item
                            //    cboField.Items.Add(item.Variety_Id );
                            //}
                            if (Load_UID != Guid.Empty)
                            {
                                using (LoadFieldDataSet1TableAdapters.LoadFieldVarietyTableAdapter loadFieldVarietyTableAdapter = new LoadFieldDataSet1TableAdapters.LoadFieldVarietyTableAdapter())
                                {
                                    using (LoadFieldDataSet1.LoadFieldVarietyDataTable loadFieldVarietyDataTable = new LoadFieldDataSet1.LoadFieldVarietyDataTable())
                                    {
                                        if (loadFieldVarietyTableAdapter.Fill(loadFieldVarietyDataTable, Load_UID) >0)
                                        {
                                           cboField.SelectedIndex=  cboField.FindString(loadFieldVarietyDataTable[0].Field);
                                        }
                                        
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch {
            }
           
        }


        /// <summary>
        /// Create the Harvest_Load Sheet
        /// </summary>
        /// <param name="Load_UID">Set To GUID.Empty on An Inbound Load</param>
        /// <param name="Weight_Sheet_UID">Set To GUID.Empty on An Outbound Load</param>
        public frmHarvest_Load(Guid Load_UID, Guid Weight_Sheet_UID)
        {

            InitializeComponent();
            SetCurrentWeightSheetRow(Weight_Sheet_UID);
            foreach (Control ctrl in this.pnlInput.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;
            }


            foreach (Control ctrl in this.pnlMilage.Controls)
            {
                ctrl.GotFocus += ctrl_GotFocus;
                ctrl.LostFocus += ctrl_LostFocus;
            }

            this.btnOk.GotFocus += new EventHandler(btnOk_GotFocus);
            this.btnOk.LostFocus += new EventHandler(btnOk_LostFocus);
            this.btnCancel.GotFocus += new EventHandler(btnCancel_GotFocus);
            this.btnCancel.LostFocus += new EventHandler(btnCancel_LostFocus);

            if (Load_UID == Guid.Empty && Weight_Sheet_UID == Guid.Empty)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
                this.Close();
            }
            else
            {
                Load_Lists();
                if (Load_UID == Guid.Empty)
                {
                    Scales.CurrentOutboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                    Scales.CurrentInboundScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;
                }
                else
                {
                    Scales.CurrentInboundScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                    Scales.CurrentOutboundScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;
                }
                SetRate();
                UpdateWeight();
             
            }

        }



        void btnCancel_LostFocus(object sender, EventArgs e)
        {
            this.btnCancel.BackColor = Color.Red;
            this.btnCancel.ForeColor = Color.White;

        }

        void btnCancel_GotFocus(object sender, EventArgs e)
        {
            this.btnCancel.BackColor = Color.Pink;
            this.btnCancel.ForeColor = Color.Black;
            
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

        private void Load_Lists()
        {
            Scales.SetScale(ref this.cboScale );

            this.weighMastersTableAdapter.Fill(this.nWDataset.WeighMasters);
            this.bol_TypeTableAdapter.FillWithCustom(this.nWDataset.Bol_Type);
            this.carriersTableAdapter.FillByActive_cbo_List(this.nWDataset.Carriers);

            this.cboProtien.Items.Clear();




            //for (int i = 1; i < 5000; i++)
            //{
            //    this.cboProtien.Items.Add((i * 0.01).ToString());
            //}
            //for (int i = 1; i < 200; i++)
            //{
            //    this.cboMiles.Items.Add(i.ToString());
            //}
            if (this._All_Open_Weight_Sheets_Row.Is_Loadout)
            {
                this.lblTruckId.Text = "Rail Car ID";
            }
            this.cboWeighMaster.SelectedIndex = -1;
            this.cboMiles.SelectedIndex = -1;
            this.cboProtien.SelectedIndex = -1;
            this.cboProtien.Text = "";
            this.cboHauler.SelectedIndex = -1;
            tmrUpdate.Start();
        }


  
        private void Harvest_Load_Load(object sender, EventArgs e)
        {

            this.Select();
            if (string.IsNullOrEmpty(cboWeighMaster.Text) )
            {
                cboWeighMaster.Focus();

            }
            else
            {
                txtTruckId.Focus();
            }
            //Program.frmMain


            //            else if (this.nWDataset.Weigh_Scales.Count > 1)
            //            {

            //                using (frmSelectScale frm = new frmSelectScale())
            //                {
            //                    if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //                    {
            //                        this.cboScale.Text = frm.Scale_Description;
            //                        this.cboScale.Visible = true;
            //                    }
            //                    else
            //                    {
            //                        this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            //                        this.Close();
            //                    }

            //                }

            //            }
            //            else if (this.nWDataset.Weigh_Scales.Count==0)
            //            {
            //               // CurrentInboundScaleData.CurrentStatus = ScaleData.enumStatus.Manual;
            //               // CurrentInboundScaleData.CurWeight=0;
            ////               if (this.CurrentOutboundScaleData.CurrentStatus== ScaleData.enumStatus.Off )
            //  //             EditWeight(true);

            //               //
            //             //   this.cboScale.Visible = true;
            //            }
            //            else
            //            {
            //                this.cboScale.SelectedIndex = 0;
            //                this.cboScale.Visible = true;
            //            }




        }

        private void cboHauler_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!settingRate && DialogResult== DialogResult.None) { 
            bool WasVisible = this.pnlMilage.Visible;
            this.pnlMilage.Visible = this.cboHauler.SelectedIndex > 0;
                if (!WasVisible || this.pnlMilage.Visible == false)
                {
                    this.cboMiles.SelectedIndex = -1;
                    this.cboBOLtype.SelectedIndex = -1;
                }
                else
                {
                    if (!this._All_Open_Weight_Sheets_Row.IsCarrierNull() && this._All_Open_Weight_Sheets_Row.Carrier == this.cboHauler.Text)
                    {
                        if (!this._All_Open_Weight_Sheets_Row.IsBOL_TypeNull())
                        {

                            this.cboBOLtype.SelectedValue = this._All_Open_Weight_Sheets_Row.BOL_Type;
                        }
                        else
                        {
                            this.cboBOLtype.SelectedIndex = -1;
                        }
                        if (!this._All_Open_Weight_Sheets_Row.IsMilesNull())
                        {
                            this.cboMiles.SelectedIndex = this.cboMiles.FindString(this._All_Open_Weight_Sheets_Row.Miles.ToString());
                        }
                        else
                        {
                            this.cboMiles.SelectedIndex = -1;
                        }


                    }
                }
                SetRate();
            }

          //    
        }

        private void cboHauler_Validating(object sender, CancelEventArgs e)
        {
            Validate_cbo(ref cboHauler, "INVALID HAULER", "ERROR SETTING HAULER", true,false);
           
        }

        private void cboHauler_TextUpdate(object sender, EventArgs e)
        {
            
        }

        private void cboHauler_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        
        }

        private void cboHauler_TextChanged(object sender, EventArgs e)
        {
           if (!settingRate) this.pnlMilage.Visible = this.cboHauler.FindStringExact(this.cboHauler.Text) > -1;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {

            DialogResult DR = SaveData();
            if (DR==DialogResult.OK || DR== DialogResult.Ignore)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                Globals.Weighmaster = this.cboWeighMaster.Text;
                this.Close();
            }
        }


        private System.Windows.Forms.DialogResult SaveData(bool PrintTicket=true)
        {
            DialogResult DR = System.Windows.Forms.DialogResult.OK;
           
            if (string.IsNullOrEmpty(this.txtTruckId.Text))
            {
                if (_All_Open_Weight_Sheets_Row.Is_Loadout)
                {
                    Alert.Show("You Need A Rail Car Id #", "RAIL CAR ID");
                }
                else
                {
                    Alert.Show("You Need A Truck Id #", "TRUCK ID");
                }
                
                this.txtTruckId.Focus();
                DR = System.Windows.Forms.DialogResult.Cancel;
            }
            else if ((pnlField.Visible) && (cboField.SelectedIndex < 0))
            {
                Alert.Show("You Need To Enter The Grower Field", "FIELD REQUIRED");
                DR = System.Windows.Forms.DialogResult.Cancel;
            }
            else if (this.vwWeigh_SheetRow == null || (this.vwWeigh_SheetRow.Truck_Id.ToUpper() != this.txtTruckId.Text.ToUpper()))
            {
                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    bool TruckIdUsed = (bool)Q.Truck_Id_In_Yard(this.txtTruckId.Text, Settings.Location_Id);
                    if (TruckIdUsed)
                    {
                        if (_All_Open_Weight_Sheets_Row.Is_Loadout)
                        {
                            Alert.Show("RAIL CAR ID ALREADY IN USE", "CHANGE TRUCK ID");
                        }
                        else
                        {
                            Alert.Show("TRUCK ID ALREADY IN USE", "CHANGE TRUCK ID");
                        }
                        
                        this.txtTruckId.Focus();
                        DR = System.Windows.Forms.DialogResult.Cancel;

                    }
                }

            }


            //if ((this.cboBin.SelectedIndex == -1) && DR== System.Windows.Forms.DialogResult.OK )
            //{
            //    Alert.Show("You Need To Select A Bin", "Select A Bin");
            //    this.cboBin.Focus();
            //    DR = System.Windows.Forms.DialogResult.Cancel;
            //}


            if ((this.cboHauler.SelectedIndex > 0) && DR == System.Windows.Forms.DialogResult.OK)
            {
                if (cboBOLtype.SelectedIndex == -1)
                {
                    Alert.Show("You Need To Select The BOL Type", "Select A BOL Type");
                    this.cboBOLtype.Focus();
                    DR = System.Windows.Forms.DialogResult.Cancel;
                }
                else if (string.IsNullOrEmpty (cboMiles.Text))
                {
                    Alert.Show("You Need To Select The Hauler Miles", "Select Miles");
                    this.cboMiles.Focus();
                    DR = System.Windows.Forms.DialogResult.Cancel;
                }
            }





            if (DR == System.Windows.Forms.DialogResult.OK && this.cboWeighMaster.SelectedIndex == -1 && string.IsNullOrEmpty(this.cboWeighMaster.Text))
            {

                Alert.Show("You Need To Select The WeighMaster", "Select A Weighmaster");
                this.cboWeighMaster.Focus();
                DR = System.Windows.Forms.DialogResult.Cancel;

            }





            string OriginalCarrier = string.Empty;
            string NewCarrier = this.cboHauler.Text;
            if (!this._All_Open_Weight_Sheets_Row.IsCarrierNull())
            {
                OriginalCarrier = this._All_Open_Weight_Sheets_Row.Carrier;
            }





            

            if (((vwWeigh_SheetRow !=null &&  NewCarrier != OriginalCarrier)|| ((!string.IsNullOrEmpty(OriginalCarrier)) && NewCarrier != OriginalCarrier)) && DR == System.Windows.Forms.DialogResult.OK)
            {
                {

                    string Message = "Do You Want To Change Weight Sheet From " + OriginalCarrier + " To " + NewCarrier;
                    if (String.IsNullOrEmpty(NewCarrier))
                    {
                        Message = "Do You Want To Reset Weight Sheet From " + OriginalCarrier + " To Default";
                    }
                    if (Alert.Show(Message, "Confirm Carrier", true) == System.Windows.Forms.DialogResult.No)
                    {
                        this.cboHauler.Focus();
                        DR = System.Windows.Forms.DialogResult.Cancel;
                    }
                }
            }





            //Get the Weight
            if (DR == System.Windows.Forms.DialogResult.OK)
            {

                if (!this._All_Open_Weight_Sheets_Row.Is_Loadout)
                {
                    ////1st Check For 0 weight
                    if (!this.pnlOutboundWeight.Visible && Scales.CurrentInboundScaleData.CurWeight <= 0)
                    {

                        Alert.Show("Inbound Weight 0", "Weight <= 0");
                        DR = DialogResult.Cancel;

                    }
                    else if (this.pnlOutboundWeight.Visible && Scales.CurrentOutboundScaleData.CurWeight <= 0)
                    {
                        Alert.Show("Outbound Weight 0", "Weight <= 0");
                        DR = DialogResult.Cancel;


                    }



                    else if (Scales.CurrentInboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off && Scales.CurrentInboundScaleData.CurWeight <= 0)
                    {

                        Alert.Show("Inbound Weight 0", "Weight <= 0");
                        DR = DialogResult.Cancel;
                    }
                    else if (pnlOutboundWeight.Visible == true && Scales.CurrentOutboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off && Scales.CurrentOutboundScaleData.CurWeight <= 0)
                    {
                        Alert.Show("Outbound Weight 0", "Weight <= 0");
                        DR = DialogResult.Cancel;
                    }

                    else if (Scales.CurrentInboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off && Scales.CurrentInboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Manual)
                    {
                        if ((Scales.CurrentInboundScaleData.CurrentStatus != Scales.ScaleData.enumStatus.OK))
                        {
                            Alert.Show("Scale Error", "Error");
                            DR = DialogResult.Cancel;
                        }
                        else if ((Scales.CurrentInboundScaleData.Motion && Scales.CurrentInboundScaleData.CurrentStatus == Scales.ScaleData.enumStatus.OK))
                        {
                            using (frmMotion frm = new frmMotion(Scales.CurrentInboundScaleData))
                            {
                                DR = frm.ShowDialog();
                            }
                        }


                    }


                    else if (Scales.CurrentOutboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Off && Scales.CurrentOutboundScaleData.ConnectionStatus != Scales.ScaleData.enumConnectionStatus.Manual)
                    {
                        if (Scales.CurrentOutboundScaleData.CurrentStatus != Scales.ScaleData.enumStatus.OK)
                        {
                            Alert.Show("Scale Error", "Error");
                            DR = DialogResult.Cancel;
                        }

                        else if ((Scales.CurrentOutboundScaleData.Motion && Scales.CurrentOutboundScaleData.CurrentStatus == Scales.ScaleData.enumStatus.OK))
                        {
                            using (frmMotion frm = new frmMotion(Scales.CurrentOutboundScaleData))
                            {
                                DR = frm.ShowDialog();
                            }
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

                Guid Weight_Sheet_UID = this._All_Open_Weight_Sheets_Row.Weight_Sheet_UID;
                Guid Lot_UID = this._All_Open_Weight_Sheets_Row.Lot_UID;
                DateTime TimeIn = DateTime.Now;
                int Weight_In = Scales.CurrentInboundScaleData.CurWeight;
                bool Manual_Weight = Scales.CurrentInboundScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Manual;
                bool ManualOut = Scales.CurrentOutboundScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Manual;

                string BOL = string.Empty;
                if (!string.IsNullOrEmpty(this.txtBol.Text)) BOL = this.txtBol.Text;
                string Bin = CurrentBinName ;
                string Truck_Id = this.txtTruckId.Text;
                Single? Protien = null;
                if (!String.IsNullOrEmpty(this.cboProtien.Text)) Protien = Convert.ToSingle(this.cboProtien.Text);
                string Comment = string.Empty;
                if (!string.IsNullOrEmpty(this.rtbComments.Text)) Comment = rtbComments.Text;
                int? Carrier_Id = null;
                string BOL_Type = string.Empty;
                



                int? Miles = null;
                string WeighMaster = this.cboWeighMaster.Text;
                if (cboHauler.SelectedIndex > 0)
                {
                    Carrier_Id = (int)cboHauler.SelectedValue;
                    BOL_Type = this.cboBOLtype.SelectedValue.ToString();
                    Miles = Convert.ToInt32(this.cboMiles.Text);
                }





                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {


                    if (this.vwWeigh_SheetRow == null)
                    {

                        if (this._All_Open_Weight_Sheets_Row.Is_Loadout && Load_Out_Scale.IsRunning)
                        {

                            Alert.Show("Load Out Is Already Running", "Load Canceled");
                        }
                        else
                        {
                            bool PrintSampleLabel = false;

                            PrintSampleLabel = (bool)Q.Is_First_Lot_Load(this._All_Open_Weight_Sheets_Row.Lot_UID);



                            Guid Load_UID = (Guid)Q.Create_New_Inbound_Harvest_Load(location, Weight_Sheet_UID, TimeIn, Weight_In, Manual_Weight, BOL, Bin, Truck_Id, Protien, Comment, Carrier_Id, BOL_Type, Miles, WeighMaster);
                            Misc.SplitWeight.SaveSplitWeights(splitWeights, Load_UID, true);
                            Current_Load_UID = Load_UID;
                            SaveField(Load_UID);
                            Guid WeighmasterUID = (Guid)Q.Update_WeighMaster(WeighMaster);
                            Globals.Weighmaster = WeighMaster;
                            if (!this._All_Open_Weight_Sheets_Row.Is_Loadout )
                            {
                                Logging.Add_System_Log("Print In Harvest Load ", $"PrintTicket:{PrintTicket} Scales.CurrentInboundScaleData.Manual{Scales.CurrentInboundScaleData.Manual } Scales.CurrentInboundScaleData.PrintInbound{Scales.CurrentInboundScaleData.PrintInbound}  Scales.CurrentInboundScaleData.InboundPrinter{Scales.CurrentInboundScaleData.InboundPrinter}");

                                if (PrintTicket && Scales.CurrentInboundScaleData.PrintInbound && !Scales.CurrentInboundScaleData.Manual)
                                {
                                    Printing.PrintInbound_InyardTicket(Load_UID, "", Scales.CurrentInboundScaleData.InboundPrinter);
                                }
                                
                            }
                            else if (this._All_Open_Weight_Sheets_Row.Is_Loadout)
                            {
                               Load_Out_Scale.Show(Load_UID);
                            }
                            if (PrintSampleLabel && PrintTicket)
                            {
                                Printing.PrintSampleLabel(this._All_Open_Weight_Sheets_Row.Lot_UID);
                            }
                        }
                    }
                    else
                    {
                     //   SaveField(vwWeigh_SheetRow.Load_UID);
                        Q.Update_WS_Carrier(vwWeigh_SheetRow.Weight_Sheet_UID, Carrier_Id, BOL_Type, Miles,Settings.Location_Id );
                        Q.Update_WS_WeighMaster(vwWeigh_SheetRow.Weight_Sheet_UID, WeighMaster);
                        Q.Update_Harvest_Load(vwWeigh_SheetRow.Load_UID, this.txtTruckId.Text, BOL, Bin, Protien, Comment);
                        Misc.SplitWeight.SaveSplitWeights(splitWeights, vwWeigh_SheetRow.Load_UID, true);
                        if (vwWeigh_SheetRow.Weight_In != Scales.CurrentInboundScaleData.CurWeight)
                        {
                            Q.Update_Harvest_Weight_In(vwWeigh_SheetRow.Load_UID, Scales.CurrentInboundScaleData.CurWeight);
                            
                        }
                        if (!vwWeigh_SheetRow.IsTime_OutNull() && this.pnlOutboundWeight.Visible)
                        {
                            if (this.vwWeigh_SheetRow.Weight_Out != Scales.CurrentOutboundScaleData.CurWeight)
                            {
                                Q.Update_Harvest_Weight_Out(this.vwWeigh_SheetRow.Load_UID, Scales.CurrentOutboundScaleData.CurWeight);
                               
                                {
                                    if (vwWeigh_SheetRow.IsManual_Weight_OutNull()) vwWeigh_SheetRow.Manual_Weight_Out = false;
                                        {
                                        if (ManualOut!= vwWeigh_SheetRow.Manual_Weight_Out)
                                        {
                                            Q.UpdateManualWeightOut(ManualOut, this.vwWeigh_SheetRow.Load_UID);
                                        }
                                    }
                                }
                            }
                        }
                        else if (vwWeigh_SheetRow.IsTime_OutNull() && this.pnlOutboundWeight.Visible)
                        {
                            Q.Weigh_Out(vwWeigh_SheetRow.Load_UID , DateTime.Now, Scales.CurrentOutboundScaleData.CurWeight);
                            if (ManualOut) Q.UpdateManualWeightOut(ManualOut, this.vwWeigh_SheetRow.Load_UID);
                            if (!this._All_Open_Weight_Sheets_Row.Is_Loadout)
                            {
                                Logging.Add_System_Log("Print Out Harvest Load ", $"Scales.CurrentOutboundScaleData.Manua:{Scales.CurrentOutboundScaleData.Manual} Scales.CurrentOutboundScaleData.PrintOutbound{Scales.CurrentOutboundScaleData.PrintOutbound } Scales.CurrentOutboundScaleData.OutboundPrinter{Scales.CurrentOutboundScaleData.OutboundPrinter}");

                                if (!Scales.CurrentOutboundScaleData.Manual && Scales.CurrentOutboundScaleData.PrintOutbound )
                                            {

                                                Printing.PrintInbound_FinalTicket(vwWeigh_SheetRow.Load_UID, "", Scales.CurrentOutboundScaleData.OutboundPrinter);
                                                
                                            }

                                            
                                            DR = DialogResult.Ignore;
                                        }
                        }

                    }
                }
            }
            return DR;
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            //tmrUpdate.Stop();
            tmrUpdate.Interval = 1000;
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
            if (SelectedScaleData.CurrentStatus == Scales.ScaleData.enumStatus.OK || SelectedScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Off )
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
            else if (SelectedScaleData.Description==Scales.Manual )
            {
                editButton.Text = Scales.Manual;
                editButton.Visible = true;
            }
            else
            {
                editButton.Visible = false ;
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

        private void btnEditInbound_Click(object sender, EventArgs e)
        {
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


            foreach (Control ctrl in this.pnlMilage.Controls)
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
            if (vwWeigh_SheetRow != null)
            {
                Load_Id = vwWeigh_SheetRow.Load_Id;
            }
            if (InboundScale)
            {
                SelectedScaleData = Scales.CurrentInboundScaleData;
                DefaultWeight = Scales.CurrentInboundScaleData.CurWeight;
                editButton = this.btnEditInbound; 
                StatusLabel=this.lblInStatus;
                WeightLabel=this.lblInboundWt; 
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
                StatusLabel=this.lblOutStatus;
                WeightLabel=this.lblOutboundWt; 
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
                if (this.vwWeigh_SheetRow == null)
                {
                    if (DefaultWeight < 0) DefaultWeight = 0;
                    using (Manual_Weight frm = new Manual_Weight(Prompt, DefaultWeight,false))
                    {
                        if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            SelectedScaleData.CurWeight = frm.Weight;
                            SelectedScaleData.Motion = false;
                            SelectedScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                        }
                    }
                }
                else
                {
                    using (Manual_Weight frm = new Manual_Weight(Prompt, DefaultWeight, Load_Id, this.vwWeigh_SheetRow.WS_Id))
                    {
                        if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            SelectedScaleData.CurWeight = frm.Weight;
                            SelectedScaleData.Motion = false;
                            SelectedScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Off;
                        }
                    }
                }
            }

            else if (SelectedScaleData.ConnectionStatus == Scales.ScaleData.enumConnectionStatus.Manual && this.nWDataset.Weigh_Scales.Count>0)
            {
                if (InboundScale)
                {
                    SelectedScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;
                    splitWeights = new VirtualDataset.SplitWeightDataTable();
                }
                else
                {
                    if (Misc.SplitWeight.Weighments(vwWeigh_SheetRow.Load_UID,false )>0)
                    {
                        using (Manual_Weight frm = new Manual_Weight(Prompt, DefaultWeight, Load_Id, this.vwWeigh_SheetRow.WS_Id))
                        {
                            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                            {
                                
                                SelectedScaleData.CurWeight = frm.Weight;
                                SelectedScaleData.Motion = false;
                                SelectedScaleData.ConnectionStatus = Scales.ScaleData.enumConnectionStatus.Manual ;
                                splitWeights = new VirtualDataset.SplitWeightDataTable();
                            }
                        }
                    }
                    else
                    {
                        {
                            SelectedScaleData.CurrentStatus = Scales.ScaleData.enumStatus.OK;
                        }
                    }

                }

            }

            else
            {


                using (Manual_Weight frm = new Manual_Weight(Prompt, DefaultWeight, Load_Id, this._All_Open_Weight_Sheets_Row.WS_Id))
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

            if (InboundScale)
            {
                Scales.CurrentInboundScaleData = SelectedScaleData;
            }
            else
            {
                Scales.CurrentOutboundScaleData = SelectedScaleData;
            }

            SetScaleDisplay(SelectedScaleData, ref WeightLabel, ref StatusLabel,ref editButton);

        }

        private void btnEditOutbound_Click(object sender, EventArgs e)
        {
            EditWeight(false);
        }

        private void Validate_cbo(ref ComboBox cbo,string Prompt,string Header,bool AllowEmptyString,bool SetFocus)
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

        private void txtBol_Validating(object sender, CancelEventArgs e)
        {
            int BOL;

            if ((!string.IsNullOrEmpty(txtBol.Text))&& (!int .TryParse(this.txtBol.Text,out BOL ) ))
            {
                Alert.Show("BOL MUST BE A NUMBER", "ERROR SETTING BOL");
                this.txtBol.Text = "";
                this.txtBol.Focus();
            }
        }

        private void Harvest_Load_Activated(object sender, EventArgs e)
        {
            if (FirstPass)
            {
                FirstPass = false;


                UpdateWeight();
                if (vwWeigh_SheetRow == null)
                {
                    if (this.cboWeighMaster.TabStop)
                    {
                        this.cboWeighMaster.Focus();
                    }
                    else
                    {
                        this.txtTruckId.Focus();
                    }
                }
                else
                {
                    this.btnCancel.Focus();
                }
                this.cboWeighMaster.Select(0, 0);

                this.cboBOLtype.Select(0, 0);
                this.cboHauler.Select(0, 0);
                this.cboMiles.Select(0, 0);
                this.cboProtien.Select(0, 0);
                Loading.Close();
                SetRate();
            }

        }

        private void cboProtien_Validating(object sender, CancelEventArgs e)
        {
            Validate_cbo(ref cboProtien, "INVALID PROTEIN", "ERROR SETTING PROTEIN", true,false);
        }

      

        private void cboBOLtype_Validating(object sender, CancelEventArgs e)
        {
            Validate_cbo(ref cboBOLtype, "INVALID BOL TYPE", "ERROR SETTING BOL TYPE", true,false );
           
        }

        private void cboMiles_Validating(object sender, CancelEventArgs e)
        {
            this.cboMiles.SelectedIndex = this.cboMiles.FindString(this.cboMiles.Text);
            Validate_cbo(ref cboMiles, "INVALID HAULER MILES", "ERROR SETTING HAULER MILES", true, false);
        }

        private void cboWeighMaster_Validating(object sender, CancelEventArgs e)
        {
            if ((this.cboWeighMaster.SelectedIndex==-1) && (!string.IsNullOrEmpty(this.cboWeighMaster.Text )))
            {
                if (Alert.Show(this.cboWeighMaster.Text +" Does Not Exists"+System.Environment.NewLine+"Create It?","New Weighmaster",true)== System.Windows.Forms.DialogResult.No)
                {
                    this.cboWeighMaster.Text = "";
                }

            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void btnWeighOut_Click(object sender, EventArgs e)
        {
            WeighOut();
        }


        private void WeighOut()
        {
            TimesWeighedIn = Misc.SplitWeight.Weighments(Current_Load_UID, true);
            if (TimesWeighedIn > 1)
            {
                using (frmSplit_Weigh frm = new frmSplit_Weigh(cboScale.Text,Current_Load_UID ))
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
                if (!this.vwWeigh_SheetRow.Is_Loadout)
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




        public void SaveField(Guid Load_UID)
        {
            if ((Load_UID != null) && (Load_UID != Guid.Empty))
            {
                if (SiteOptions.GetPromptForTruckType())
                {
                    if (Alert.Show("Is Truck An End Dump?", "Truck Type", true) == System.Windows.Forms.DialogResult.No)
                    {
                        try
                        {
                            using (LoadFieldDataSet1TableAdapters.QueriesTableAdapter LFQ = new LoadFieldDataSet1TableAdapters.QueriesTableAdapter())
                            {
                                LFQ.UpdateLoadField("End Dump", Load_UID);
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
                 string Printer=string.Empty ;
                 using (frmSelect_Printer frm = new frmSelect_Printer("Where Do You Want To Print the Ticket"))
                 {
                     if (frm.ShowDialog() == DialogResult.OK)
                     {
                         Printer = frm.PrinterName;
                         if (string.IsNullOrEmpty(Printer))
                         {
                             using (PrintDialog pd = new PrintDialog())
                             {
                                 DR = pd.ShowDialog(); 
                                 if (DR == System.Windows.Forms.DialogResult.OK)
                                 {
                                     Printer = pd.PrinterSettings.PrinterName;
                                 }
                             }


                         }
                         if (DR == System.Windows.Forms.DialogResult.OK)
                         {
                             if (vwWeigh_SheetRow.IsTime_OutNull())
                             {
                                 Printing.PrintInbound_InyardTicket(vwWeigh_SheetRow.Load_UID, "", Printer);
                             }
                             else
                             {
                                 Printing.PrintInbound_FinalTicket(vwWeigh_SheetRow.Load_UID, "", Printer);
                             }
                         }
                         //this.DialogResult = DialogResult.OK;
                         //this.Close();
                     }
                 }
             }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            
            if (Alert.Show(string.Format("Delete Load:{0} From Weight Sheet:{1} " , vwWeigh_SheetRow.Load_Id, vwWeigh_SheetRow.WS_Id) , "Confirm Delete", true) == System.Windows.Forms.DialogResult.Yes)
            {
                using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                {
                    Q.Delete_Load(vwWeigh_SheetRow.Load_UID);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            using (Select_Weight_Sheet frm = new Select_Weight_Sheet(vwWeigh_SheetRow.Weight_Sheet_UID,false))
            {


                if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                    {
                        string UserResponse=User_Input.Get_User_Input(string.Format("Why Are You Moving Ticket {0}",vwWeigh_SheetRow.Load_Id));
                        if (!String.IsNullOrEmpty(UserResponse))
                        {
                        Q.Change_Harvest_Load_Weight_Sheet(vwWeigh_SheetRow.Load_UID, frm.Weight_Sheet_UID,UserResponse );

                            using (LoadFieldDataSet1TableAdapters.QueriesTableAdapter LFQ = new LoadFieldDataSet1TableAdapters.QueriesTableAdapter())
                            {
                                LFQ.SetLoadFieldNull(vwWeigh_SheetRow.Load_UID);
                            }
                            using (NWDatasetTableAdapters.vwWeigh_SheetTableAdapter vwWeigh_SheetTableAdapter = new NWDatasetTableAdapters.vwWeigh_SheetTableAdapter())
                            {
                                using (NWDataset.vwWeigh_SheetDataTable tmpvwWeigh_SheetDataTable = new NWDataset.vwWeigh_SheetDataTable())
                                {
                                    if (vwWeigh_SheetTableAdapter.FillByLoad_UID(tmpvwWeigh_SheetDataTable, vwWeigh_SheetRow.Load_UID) > 0)
                                    {
                                        NWDataset.vwWeigh_SheetRow NewRowValues = tmpvwWeigh_SheetDataTable[0];
                                        vwWeigh_SheetRow.Weight_Sheet_UID = frm.Weight_Sheet_UID;
                                        // Logging.Add_Audit_Trail("Moved Ticket", vwWeigh_SheetRow.Load_Id.ToString(), string.Format("From Weight Sheet {0} To Weight Sheet{1}", vwWeigh_SheetRow.WS_Id.ToString(), NewRowValues.WS_Id.ToString()), vwWeigh_SheetRow.WS_Id.ToString(), NewRowValues.WS_Id.ToString(), UserResponse);
                                    }
                                    else
                                    {
                                        vwWeigh_SheetRow.Weight_Sheet_UID = Guid.Empty;
                                    }
                                }
                            }

                            
                        this.DialogResult = System.Windows.Forms.DialogResult.Abort;
                        this.Close();
                        }
                    }
                }
            }
        }

        private void cboMiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void pnlInput_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboScale_TextChanged(object sender, EventArgs e)
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
         
            using (frmSplit_Weigh frm = new frmSplit_Weigh(cboScale.Text,(Current_Load_UID == null)?Guid.Empty :Current_Load_UID  ))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    cboScale.Text = "Manual";
                    Scales.GetScaleByDescription(cboScale.Text).CurWeight= frm.GetTotalWeight();
                    splitWeights = frm.splitWeights;
                    //this.rtbComments.Text = Misc.SplitWeight.Add_Split_Weights(this.rtbComments.Text, frm.GetWeightComment(), true);
                    Logging.Add_Audit_Trail("Split Weigh", "Truck ID:" + this.lblTruckId.Text, "Split Weigh", "", frm.GetTotalWeight().ToString() , "");
                }

            }
        }

        private void pnlOutboundWeight_VisibleChanged(object sender, EventArgs e)
        {
   
        }

        private void pnlEdit_VisibleChanged(object sender, EventArgs e)
        {
     
        }


        

        private void bwUpdateWeight_DoWork(object sender, DoWorkEventArgs e)
        {

           

        }

        private void frmHarvest_Load_FormClosing(object sender, FormClosingEventArgs e)
        {
            Cancel = true;
          
        }

        private void frmHarvest_Load_FormClosed(object sender, FormClosedEventArgs e)
        {
        
        }

        private void cboProtien_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(cboProtien.Text))
            {
                decimal P;
                if (!decimal.TryParse(cboProtien.Text, out P)) cboProtien.Text = "";
                if (P<0) cboProtien.Text = "";
            }
        }

        private void cboScale_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnBin_Click(object sender, EventArgs e)
        {
            using (SelectBin frm = new NWGrain.SelectBin("", ""))
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    CurrentBinName = frm.SelectedBin;
                }
            }
        }

        private void cboProtien_Validating_1(object sender, CancelEventArgs e)
        {
            bool Valid = true;
            if ((!string.IsNullOrEmpty(cboProtien.Text)))
            {
                decimal Protein = 0;
                if (!decimal.TryParse(cboProtien.Text, out Protein))
                {
                    Valid = false;
                }
                else
                {
                    if (Protein < 0 || Protein > 50)
                    {
                        Valid = false;
                    }
                }
            }
            if (!Valid)
            {

                Alert.Show("Invalid Protein", "Error Setting Protein");
                cboProtien.Focus();
                cboProtien.Text = "";
            }
        }

        private void cboMiles_Validating_1(object sender, CancelEventArgs e)
        {
            bool Valid = true;
            if ((!string.IsNullOrEmpty(cboMiles.Text)))
            {
                int Miles = 0;
                if (!int.TryParse(cboMiles.Text, out Miles))
                {
                    Valid = false;
                }
                else
                {
                    if (Miles < 0 || Miles > 200)
                    {
                        Valid = false;
                    }
                }
            }
            if (!Valid)
            {

                Alert.Show("Invalid Miles", "Error Setting Miles");
                cboMiles.Focus();
                cboMiles.Text = "";
            }
        }

        private void btnTareLookup_Click(object sender, EventArgs e)
        {

           // cboScale.Text = "Manual";
            
            using (TareLookup frm = new NWGrain.TareLookup())
            {
                if (frm.ShowDialog()== DialogResult.OK )
                {
                    
                    txtTruckId.Text= frm.VehicleID;
                   
                    //Scales.GetScaleByDescription(cboScale.Text).CurWeight = frm.Tare;

                    DialogResult DR = SaveData(false );
                    if (DR == DialogResult.OK || DR == DialogResult.Ignore)
                    {
                        this.GetLoadValues(this.Current_Load_UID, false);
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

        private void cboField_Leave(object sender, EventArgs e)
        {
            if (cboField.SelectedIndex == -1) cboField.Text = "";
        }

        private void cboBOLtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRate();
        }


        private void UpdateRate()
        {
            if (!settingRate && this.DialogResult== DialogResult.None )
            {

                if (cboBOLtype.SelectedIndex > -1)
                {
                    var Rate = new WeightSheetHaulerRate.Rate() { UID = CurrentWeightSheetRow.UID };
                    Rate.RateType = WeightSheetHaulerRate.GetRateType(cboBOLtype.SelectedValue.ToString());
                    if (Rate.RateType == WeightSheetHaulerRate.enumRateType.Custom)
                    {
                        Rate.RateAmount = numNewRate.Value;
                    }
                    else
                    {
                        Rate.RateAmount = 0;
                    }
                    WeightSheetHaulerRate.UpdateWeightSheetRate(Rate);






                }

                SetRate();

            }
        }

        private void numNewRate_Validating(object sender, CancelEventArgs e)
        {
            UpdateRate();
        }
    }
}
