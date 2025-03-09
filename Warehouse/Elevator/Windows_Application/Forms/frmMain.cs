using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AppUpdater;
using System.Threading.Tasks;


namespace NWGrain
{
    public partial class frmMain : Form
    {
     bool DoUpdate;

        bool CancelUpdate = false;
        DateTime LastUpdate = DateTime.Now.AddDays(-1);
        
        private bool m_bLayoutCalled = false;
        private DateTime m_dt;

        bool InitialUpdate = false;
   

        bool Initializing;

        public frmMain()
        {
            InitializeComponent();
            LblSiteSettings.Text = "";
       
    
        }


        private void StopGridUpdate()
        {
        
            
         tmrUpdate.Stop();
        }

        private void StartGridUpdate()
        {
            
            if (!tmrUpdate.Enabled)
            {
                UpdateData();       
                tmrUpdate.Start();
            }
        }

        private void Initialize_Location()
        {
            if (Settings.Location_Id == -1)
            {
                this.Close();
                Application.Exit();

            }
            else
            {
                Initializing = true;
                this.vwSiteLocationsTableAdapter.Fill(this.settingsDataSet.vwSiteLocations);
                bool Allow_Multi_Locations = Settings.workStation_SetupRow.Allow_Multi_Locations;

                foreach (SettingsDataSet.vwSiteLocationsRow row in this.settingsDataSet.vwSiteLocations)
                {
                    if (row.Location_Id == Settings.Location_Id)
                    {
                        lblLocation.Text = row.ID_Description;
                        cboLocations.SelectedIndex = cboLocations.FindString(row.ID_Description);
                        break;
                    }

                }
                cboLocations.Visible = Allow_Multi_Locations;
                lblLocation.Visible = !Allow_Multi_Locations;

               
                


                this.LblSiteSettings.Text= string.Format("In Seq:{0}   Out Seq:{1}   Lot Seq:{2}   Load Seq:{3}   Server:{4}",  Settings.Sequence.InboundSequence , Settings.Sequence.OutboundSequence ,Settings.Sequence.LotSequence,Settings.Sequence.LoadSequence , Settings.ServerName);
            }

            Initializing = false;
        }


        private void Check_Setup()
        {

          
            if (Settings.SiteSetup.Is_Seed_Mill)
            {
                this.Variety.Visible = true;
                this.Carrier.Visible = false;
             
            }
            else
            {
                this.Variety.Visible = false;
                this.Carrier.Visible = true;
             
            }

        }


        private void frmMain_Activated(object sender, EventArgs e)
        {
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.IsMdiChild && frm.Name != this.Name)
                {
                    frm.Show();
                    frm.WindowState = FormWindowState.Maximized;
                    this.SendToBack();


                    break;
                }
            }


            if (m_bLayoutCalled == false)
            {
                m_bLayoutCalled = true;
                m_dt = DateTime.Now;
                this.Activate();
                SplashScreen.SplashScreen.CloseForm();
                //if (Settings.CheckUpdateAtStartup && (AppUpdater.UpdateInfo.CheckUpdates)) CheckForUpdate();
                Initialize_Location();
                UpdateInfo.UpdatePath = Properties.Settings.Default.UpdatePath;
                UpdateInfo.ProgramName = "NWGrain.exe";
             
                StartGridUpdate();
                Scales.Connect();
                Globals.MainPoint = this.Location;
            }

            
            UpdateData();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            
        }


        public void UpdateData()

        {
            try { 
            if ((DateTime.Now - LastUpdate).TotalSeconds > 1)
            {
                
            
                Form frmActiveChild = Program.frmMdiMain.ActiveMdiChild;
                bool Active = (frmActiveChild != null) ? (frmActiveChild.Name == "frmMain") ? true : false : false;
                // System.Diagnostics.Debug.Print( (frmActiveChild != null) ? frmActiveChild.Name :  "No Active Child");
                if (Active || !InitialUpdate )
                {
                    InitialUpdate = true;
                    System.Diagnostics.Debug.Print("Updating "+DateTime.Now.ToLongTimeString() );
                    tmrUpdate.Enabled = false;
                    using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                    {
                        int LoadCount = 0;
                        try
                        {
                            LoadCount = (int)Q.CountOfLoadsToday(Settings.Location_Id);
                            this.btnTodaysLoads.Text = "# Loads Today" + System.Environment.NewLine + string.Format("{0:N0}", LoadCount);
                        }
                        catch
                        {

                        }
                    }
                    int Position = this.weightSheetsForSelectionBindingSource.Position;
                    this.weightSheetsForSelectionTableAdapter.Fill(this.nWDataset.WeightSheetsForSelection, Settings.Location_Id);
                    try
                    {

                        this.weightSheetsForSelectionBindingSource.Position = Position;
                    }
                    catch
                    {

                    }
                    LastUpdate = DateTime.Now;
                    tmrUpdate.Enabled = true;
                    SplashScreen.SplashScreen.CloseForm();
                    BringFormToFront(this);
                }
            }
            }
            catch 
            {
                
            }
            
                
       
        }

        private  void BringFormToFront(Form form)
        {
            form.BringToFront();
       

            form.Activate();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            StopGridUpdate();
          

            using (frmSelect_Weight_Sheet_Type frmSelect_Weight_Sheet_Type = new frmSelect_Weight_Sheet_Type())
            {
                if (frmSelect_Weight_Sheet_Type.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Loading.Show("Creating Weight Sheet ", Form.ActiveForm);
                    Application.DoEvents();
                    if (frmSelect_Weight_Sheet_Type.LoadType == NWGrain.frmSelect_Weight_Sheet_Type.enumLoad_Type.Harvest)
                    {

                       
                        using (Harvest_Lot.frmSelect_Weight_Sheet_Lot frm = new Harvest_Lot.frmSelect_Weight_Sheet_Lot())
                        {
                           

                            
                            DialogResult DR = frm.ShowDialog();
                            Loading.Close();
                            if (DR == System.Windows.Forms.DialogResult.OK)
                            {
                                Loading.Show("Creating Inbound Load Ticket", Form.ActiveForm);

                                Guid Worksheet = frm.Selected_Weight_Sheet_UID;
                                this.vw_Open_Weight_SheetsTableAdapter.FillByWeight_Sheet_UID(nWDataset.vw_Open_Weight_Sheets,Settings.Location_Id, frm.Selected_Weight_Sheet_UID);
                                using (frmHarvest_Load frmHL = new frmHarvest_Load(this.nWDataset.vw_Open_Weight_Sheets[0]))
                                {

                                    frmHL.ShowDialog();
                                }
                            
                            }


                            

                        }
                    }
                    else
                    {
                        using (frmTransfer_Weight_Sheet_Details  frm = new frmTransfer_Weight_Sheet_Details())
                        {
                            DialogResult DR = frm.ShowDialog();
                            if (DR == System.Windows.Forms.DialogResult.OK)
                            {
                                Loading.Show("Creating Transfer Load Ticket", Form.ActiveForm);
                            Guid Worksheet =frm.Selected_Weight_Sheet_UID;
                                using (frmTransfer_Load frmTransfer = new frmTransfer_Load(Worksheet))
                                {
                                    frmTransfer.ShowDialog();
                                }
                            }

                          
                          
                        }
                    }
                  

                  
                    
                }
            }
            Loading.Close();
            tmrRefresh.Start();
            StartGridUpdate();


        }

       


  

    

        private void pnlMain_MouseUp(object sender, MouseEventArgs e)
        {
            this.tmrSetup.Stop();
        }

        private void frmMain_MouseDown(object sender, MouseEventArgs e)
        {
            this.tmrSetup.Start();
        }

        private void frmMain_MouseUp(object sender, MouseEventArgs e)
        {
            this.tmrSetup.Stop();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
          
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                StopGridUpdate();

                frmLots frm = new frmLots();



                Display.ShowForm(frm);

                
            }
            catch
            {

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
  
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            tmrUpdate.Stop();
            UpdateData();
            tmrUpdate.Start();
        }




        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }

     

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            this.dataGridView1.Enabled = false;
           
            try
            {
                if (e.RowIndex >= 0)
                {

                    if (e.ColumnIndex == btnEDIT.Index)
                    {
                        StopGridUpdate();

                        NWDataset.WeightSheetsForSelectionRow SelectedRow;
                        SelectedRow = (NWDataset.WeightSheetsForSelectionRow )(DataRow)((DataRowView)this.weightSheetsForSelectionBindingSource.Current).Row;


                        if (SelectedRow.Weight_Sheet_Type.ToUpper() == "INBOUND")
                        {
                            frmHarvest_WS frmHWS = new frmHarvest_WS(SelectedRow.Weight_Sheet_UID);

                            Display.ShowForm(frmHWS);
                        }
                        else
                        {
                            frmTransfer_WS frmTWS = new frmTransfer_WS(SelectedRow.Weight_Sheet_UID);
                             Display.ShowForm(frmTWS);
                            

                        }
                      


                    }
                    else if (e.ColumnIndex== btnNew.Index )
                    {
                        StopGridUpdate();
                        Loading.Show("Creating Ticket", Form.ActiveForm);

                        NWDataset.WeightSheetsForSelectionRow SelectedRow;
                        SelectedRow = (NWDataset.WeightSheetsForSelectionRow)(DataRow)((DataRowView)this.weightSheetsForSelectionBindingSource.Current).Row;
                        bool LoadOut_In_Use = false;
                        if (SelectedRow.Is_Loadout)
                        {

                            LoadOut_In_Use = Load_Out_Scale.IsRunning;
                        }


                        if (e.ColumnIndex != btnEDIT.Index)
                        {
                            if (SelectedRow.Can_Add==0)
                            {
                                Loading.Close();
                                Alert.Show("You Cannot Add Any More Loads To This Weight Sheet", "Max # Loads Reached", false);

                            }
                            else if (LoadOut_In_Use)
                            {
                                Loading.Close();
                                Alert.Show("The Load Out Is In Use", "Load Out In Use", false);

                            }
                            else
                            {
                                StopGridUpdate();
                                if (!SelectedRow.IsLot_NumberNull())
                                {
                                   this.vw_Open_Weight_SheetsTableAdapter.FillByWeight_Sheet_UID(this.nWDataset.vw_Open_Weight_Sheets,Settings.Location_Id , SelectedRow.Weight_Sheet_UID);
                                    Loading.Show("Loading Inbound Load", Form.ActiveForm);
                                    using (frmHarvest_Load frm = new frmHarvest_Load(this.nWDataset.vw_Open_Weight_Sheets[0]))
                                    {

                                        frm.ShowDialog();
                                    }
                                }
                                else
                                {
                                    using (frmTransfer_Load frmTransfer = new frmTransfer_Load(SelectedRow.Weight_Sheet_UID))
                                    {

                                        frmTransfer.ShowDialog();
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (!SelectedRow.IsLot_NumberNull())
                            {
                                using (frmHarvest_WS frmHWS = new frmHarvest_WS(SelectedRow.Weight_Sheet_UID))
                                {

                                    frmHWS.ShowDialog();
                                }
                            }

                        }
                    }




                }

            
            }
            catch (Exception ex)
            {
                Logging.Add_System_Log("frmMain.dataGridView1_CellContentClick", ex.Message);
            }
            Loading.Close();
            StartGridUpdate();
            this.dataGridView1.Enabled = true;
          

        }





     
       


        private void button7_Click(object sender, EventArgs e)
        {
            StopGridUpdate();
            using (PrintingTicket PrintingTicket = new NWGrain.PrintingTicket())
            {
                PrintingTicket.SetPrompt("Getting Locations With Weight Sheets");
                PrintingTicket.Show();
                Application.DoEvents();
                System.Threading.Thread.Sleep(500);
                int OriginalLocationId = Settings.Location_Id;
                NWDataset.WorkStation_SetupRow row = Settings.workStation_SetupRow;
               
                if (!row.Allow_Multi_Locations)
                {
                    Weight_Sheet.PrintEndOfDay(PrintingTicket); 
                    
                }
                else
                {
                    using (SettingsDataSet.vwSiteLocationsDataTable siteLocations = new SettingsDataSet.vwSiteLocationsDataTable())
                    {
                        this.vwSiteLocationsTableAdapter.Fill(siteLocations);
                        foreach (SettingsDataSet.vwSiteLocationsRow siteRow in siteLocations)
                        {
                            ChangeStation(siteRow.Location_Id, siteRow.ID_Description);
                            if (Alert.Show("Close "+ siteRow.ID_Description,"Close Location",true)== DialogResult.Yes)   Weight_Sheet.PrintEndOfDay(PrintingTicket);
                        }
                    }
                }

                if (Settings.Location_Id != OriginalLocationId) ChangeStation(OriginalLocationId, "Original Location");
            }
            
            StartGridUpdate();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmLoads frm = new frmLoads();
            Display.ShowForm(frm);

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void setupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //using (frmShadow frmShadow = new frmShadow())
            {

                StopGridUpdate();

                //frmShadow.Size = this.Size;
                //frmShadow.Top = this.Top;
                //frmShadow.Enabled = false;

                //frmShadow.Show();



                using (frmPassword frmPassword = new frmPassword())
                {

                    if (frmPassword.ShowDialog() == DialogResult.OK)
                    {


                        using (frmSetup frm = new frmSetup())
                        {
                            int CurrentID = Settings.Location_Id;
                             
                            frm.ShowDialog();
                           
                            if (CurrentID!=Settings.Location_Id )
                            {
                               
                                cboLocations.Visible = false;
                                lblLocation.Visible = false;
                                Loading.Show("Changing Location To" + System.Environment.NewLine + Settings.Location_Id, Form.ActiveForm);
                                System.Threading.Thread.Sleep(250);
                                
                            }
                            Initialize_Location();
                            StartGridUpdate();
                            Loading.Close();
                        }
                    }
                }
                Check_Setup();
            }
        }

        private void btnSeedLoads_Click(object sender, EventArgs e)
        {
            //
            //using (frmSeed_Ticket_List frm = new frmSeed_Ticket_List())
            //{
            //    frm.ShowDialog();
            //}
            //
        }

        private void tmrSetup_Tick(object sender, EventArgs e)
        {

        }

        private void cboLocations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Initializing)
            {try
                {
                    int Location_Id = ((NWGrain.SettingsDataSet.vwSiteLocationsRow)((System.Data.DataRowView)this.cboLocations.SelectedItem).Row).Location_Id;
                    string ID_Description = ((NWGrain.SettingsDataSet.vwSiteLocationsRow)((System.Data.DataRowView)this.cboLocations.SelectedItem).Row).ID_Description;
                    ChangeStation(Location_Id, ID_Description);
                }
                catch { }
            }
        }


        public void ChangeStation(int Location_Id, string ID_Description)
        {
            if (!Initializing)
            {

                if (this.cboLocations.SelectedIndex > -1)
                {
                    StopGridUpdate();
                    Initializing = true;
                     cboLocations.Visible = false;
                    Loading.Show("Changing Location To" + System.Environment.NewLine + ID_Description, Form.ActiveForm);
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(250);

                    Settings.Location_Id = Location_Id;
                    Initialize_Location();
                    StartGridUpdate();

                    Loading.Close();
                }
            }
        }

        private void menuStrip1_VisibleChanged(object sender, EventArgs e)
        {

        }

        private void frmMain_VisibleChanged(object sender, EventArgs e)
        {
            
            
        }

        private void button3_Click_1(object sender, EventArgs e)
        {

         
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateData();
            this.Refresh();
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            tmrRefresh.Stop();
            this.Refresh();

        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdate();
        }

        private void CheckForUpdate()
        {
          
        }

  
        private void bwCheckForUpdate_DoWork(object sender, DoWorkEventArgs e)
        {
          

             DoUpdate= false;
            using (NWDataset.vwOpen_Weight_Sheet_SelectionDataTable Test = new NWDataset.vwOpen_Weight_Sheet_SelectionDataTable())
            {
                using (NWDatasetTableAdapters.vwOpen_Weight_Sheet_SelectionTableAdapter vwOWTA = new NWDatasetTableAdapters.vwOpen_Weight_Sheet_SelectionTableAdapter())
                {
                    vwOWTA.Fill(Test, Settings.Location_Id);
                }
          //          this.vwOpen_Weight_Sheet_SelectionTableAdapter.Fill(Test, Settings.Location_Id);
                DoUpdate = Test.Count != this.nWDataset.vwOpen_Weight_Sheet_Selection.Count;
                if (!DoUpdate)
                    foreach (NWDataset.vwOpen_Weight_Sheet_SelectionRow row in Test)
                    {
                        NWDataset.vwOpen_Weight_Sheet_SelectionRow CurrentRow = this.nWDataset.vwOpen_Weight_Sheet_Selection.FindByWeight_Sheet_UID(row.Weight_Sheet_UID);
                        DoUpdate = (CurrentRow == null);
                        if (DoUpdate) break;
                        var array1 = row.ItemArray;
                        var array2 = CurrentRow.ItemArray;
                        DoUpdate = (!array1.SequenceEqual(array2));
                        if (DoUpdate) break;
                    }
            }
        }

        private void bwCheckForUpdate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            if (!CancelUpdate)
            {
                if (DoUpdate)
                {

                    UpdateData();

                }

                this.tmrUpdate.Start();
            }


        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        private void frmMain_Move(object sender, EventArgs e)
        {
            Globals.MainPoint = this.Location;
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
 
        }

        private void btnTodaysLoads_Click(object sender, EventArgs e)
        {
          
        }

        private void intakeTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {

            FixWeightSheet(false);



        }


        private void FixWeightSheet(bool Transfer)
        {
            NWGrain.Forms.Fix.frmSelectWeightSheet SelectWeightSheet = new Forms.Fix.frmSelectWeightSheet(Transfer);
            SelectWeightSheet.MdiParent = Program.frmMdiMain;
          //  FrmIntakeToTransfer.WindowState = FormWindowState.Maximized;
           // FrmIntakeToTransfer.FormBorderStyle = FormBorderStyle.SizableToolWindow;
            SelectWeightSheet.ControlBox = false;
            SelectWeightSheet.Show();
        }

        private void frmMain_Resize(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void transferIntakeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FixWeightSheet(true);



        }

     

        private void PrintDailyTotals()
        {
            Printing.PrintDailyIntakeReport(DateTime.Now, Settings.Location_Id);
            Printing.PrintDailyTransferReport(DateTime.Now, Settings.Location_Id);
            Printing.PrintDailyWeightSheetAsc(DateTime.Now, Settings.Location_Id);
            Printing.PrintDailyLoadsByCrop(DateTime.Now, Settings.Location_Id);
            Printing.PrintClosedLotReport(DateTime.Now ,DateTime.Now, Settings.Location_Id);
        }

        private void printINtakeReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Printing.PrintDailyIntakeReport(DateTime.Now, Settings.Location_Id);
           
        }

        private void printTransferReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Printing.PrintDailyTransferReport(DateTime.Now, Settings.Location_Id);
        }

        private void printWeightSheetsAscendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Printing.PrintDailyWeightSheetAsc(DateTime.Now, Settings.Location_Id);
        }

        private void printDailyLoadsByCommodityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Printing.PrintDailyLoadsByCrop(DateTime.Now, Settings.Location_Id);
        }

        private void printDailyBinReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Printing.PrintDailyBinReport(DateTime.Now, Settings.Location_Id);
        }

        private void lblTodaysLoads_Click(object sender, EventArgs e)
        {
          
        }

        private void btnTodaysLoads_Click_1(object sender, EventArgs e)
        {
            try
            {
                StopGridUpdate();

                TodaysLoads frm = new TodaysLoads();
                Display.ShowForm(frm);
            }
            catch
            {

            }
        }

        private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
        {
            if (tmrUpdate.Enabled)
            {
                tmrUpdate.Stop();
                tmrUpdate.Start();

            }
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
            if (tmrUpdate.Enabled)
            {
                tmrUpdate.Stop();
                tmrUpdate.Start();

            }

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (tmrUpdate.Enabled)
            {
                tmrUpdate.Stop();
                tmrUpdate.Start();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool tmrEnabled = tmrUpdate.Enabled;
            if (tmrEnabled) tmrUpdate.Stop();
            UpdateData();
            if (tmrEnabled) tmrUpdate.Start();
        }

        private void button3_Click_3(object sender, EventArgs e)
        {
           
        }

        private void certWeightOnlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (frmBasicTicket frm = new frmBasicTicket())
            {
                frm.ShowDialog();
            }
        }

        private void printClosedLotsReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ClosedLotsReport frm = new NWGrain.ClosedLotsReport())
            {
                frm.ShowDialog();
            }
        }
    }
}
