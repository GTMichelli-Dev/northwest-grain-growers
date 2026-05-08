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
    public partial class frmSetup : Form
    {

        public string SamplePrinter  { get; set; }
        public string ReportPrinter { get; set; }

        public frmSetup()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Owner = Program.frmMdiMain;
            LoadData();
   
            
        }


        private void LoadData()
        {

        

            this.ckRemoteOriginal.Checked = SiteOptions.GetRemotePrintOriginal();
            this.ckTruckType.Checked = SiteOptions.GetPromptForTruckType();

            this.ckAllowTareLookup.Checked = Settings.AllowTareLookup;
            this.cboManualScalePrinter.Items.Add("");
            foreach (string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {

                this.cboGrade_Printer.Items.Add(printer);
                this.cboReport_Printer.Items.Add(printer);


            }


            using (WS.WSSoapClient Proxy = new WS.WSSoapClient())
            {
                try
                {
                    WS.LocalDataSet.Site_PrintersDataTable Printers = Proxy.GetPrinterList();


                    foreach (WS.LocalDataSet.Site_PrintersRow PrinterRow in Printers)
                    {
                        this.cboManualScalePrinter.Items.Add(PrinterRow.Printer_Name);
                    }
                }
                catch (Exception ex)
                {
                    Alert.Show("Error Retrieving Printer List From Server. " + ex.Message, "Printer List Error", false);
                }

            }

            foreach (Scales.ScaleData Scale in Scales.CurScales)
            {
                cboDefaultScale.Items.Add(Scale.Description);
            }
            if (cboDefaultScale.Items.Count ==0) cboDefaultScale.Items.Add("Manual");

          


            lblSiteSetup.Text = string.Format("{0} - {1}", Settings.CurrentWorkStationLocationRow.Description, Settings.CurrentWorkStationLocationRow.Id);


            NWDataset.WorkStation_SetupRow row = Settings.workStation_SetupRow;
            this.ckAllowMultlipleLocations.Checked = row.Allow_Multi_Locations;
            this.cboDefaultScale.SelectedIndex = this.cboDefaultScale.FindString(row.Weigh_Scale);

            this.cboManualScalePrinter.SelectedIndex = this.cboManualScalePrinter.FindString(Settings.ManualWeightPrinter);

            this.cboGrade_Printer.SelectedIndex = cboGrade_Printer.FindString(row.Grade_Printer);
            this.cboReport_Printer.SelectedIndex = cboReport_Printer.FindString(row.Report_Printer);
            //using (WS.WSSoapClient proxy = new WS.WSSoapClient())
            //{
            //    WS.NWDataset.PrinterListDataTable PrinterList = new WS.NWDataset.PrinterListDataTable();
            //    PrinterList = proxy.GetPrinterList();
            //    foreach (WS.NWDataset.PrinterListRow row in PrinterList)
            //    {
            //        cboInboundPrinter.Items.Add(row.Printer_Name);
            //        cboOutboundPrinter.Items.Add(row.Printer_Name);
            //        this.cboReport_Printer.Items.Add(row.Printer_Name);
            //    }
            //}




            //    this.weigh_ScalesTableAdapter.Fill(this.nWDataset.Weigh_Scales, Settings.Location_Id);

          
            string Station = Environment.MachineName;

            NWDataset.WorkStation_SetupRow SetupRow = Settings.workStation_SetupRow;


            if (Settings.CurrentWorkStationLocationRow.Id==0)
            {
                SetLocation();
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();

        }

        private void btnNewLot_Click(object sender, EventArgs e)
        {
          
            
            DialogResult DR = DialogResult.OK;
         
            Settings.AllowTareLookup = this.ckAllowTareLookup.Checked;
            Settings.ManualWeightPrinter = this.cboManualScalePrinter.Text;
            if (this.cboDefaultScale.SelectedIndex == -1)
            {
                Alert.Show("You Must Select A Scale or Select No Default", "Select Scale", false);
                DR = System.Windows.Forms.DialogResult.Cancel;
                this.cboDefaultScale.Focus();
            }

            else if (this.cboGrade_Printer.SelectedIndex == -1)
            {
                Alert.Show("You Must Select A Grade Printer", "Select Printer", false);
                DR = System.Windows.Forms.DialogResult.Cancel;
                this.cboGrade_Printer.Focus();
            }
            else if (this.cboReport_Printer.SelectedIndex == -1)
            {
                Alert.Show("You Must Select A Report Printer", "Select Printer", false);
                DR = System.Windows.Forms.DialogResult.Cancel;
                this.cboReport_Printer.Focus();
            }

            //else if (this.cboInboundPrinter.SelectedIndex == -1)
            //{
            //    Alert.Show("You Must Select An Inbound Printer", "Select Printer", false);
            //    DR = System.Windows.Forms.DialogResult.Cancel;
            //    this.cboInboundPrinter.Focus();
            //}

            //else if (this.cboOutboundPrinter.SelectedIndex == -1)
            //{
            //    Alert.Show("You Must Select An Outbound Printer", "Select Printer", false);
            //    DR = System.Windows.Forms.DialogResult.Cancel;
            //    this.cboOutboundPrinter.Focus();
            //}
            
            if (DR == System.Windows.Forms.DialogResult.OK)
            {

                SiteOptions.SetRemotePrintOriginal(this.ckRemoteOriginal.Checked);
                SiteOptions.SetPromptForTruckType(this.ckTruckType.Checked);


                NWDataset.WorkStation_SetupRow row = Settings.workStation_SetupRow;
                row.Allow_Multi_Locations= this.ckAllowMultlipleLocations.Checked;
                row.Weigh_Scale = this.cboDefaultScale.Text;
                row.Grade_Printer = this.cboGrade_Printer.Text;
                row.Report_Printer  = this.cboReport_Printer.Text;
                Settings.SaveWorkStationSettings();
            

                //  this.Validate();

                //  this.workStationSetupBindingSource.EndEdit();

                ////  this.nWDataset.WorkStation_Setup[0].Weigh_Scale = this.cboDefaultScale.Text;

                //  this.workStation_SetupTableAdapter.Update(this.nWDataset);
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
        }

       

      

        private void Setup_Load(object sender, EventArgs e)
        {
       


        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void lot_Id_SeedLabel_Click(object sender, EventArgs e)
        {

        }

        private void current_Fuel_PriceTextBox_Validating(object sender, CancelEventArgs e)
        {
        
        }

        private void frmSetup_Activated(object sender, EventArgs e)
        {
            SplashScreen.SplashScreen.CloseForm();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void outbound_Kiosk_PrinterLabel_Click(object sender, EventArgs e)
        {

        }

        private void inbound_Kiosk_PrinterLabel_Click(object sender, EventArgs e)
        {

        }

        private void current_Fuel_PriceLabel_Click(object sender, EventArgs e)
        {

        }


        private void lblSiteSetup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SetLocation();

        }


        private void SetLocation()
        {
            using (frmGetLocation frm = new frmGetLocation())
            {
                DialogResult dr=  frm.ShowDialog();
                if ((Settings.CurrentWorkStationLocationRow.Id == 0) && (dr == DialogResult.Cancel))
                {
                    if (System.Windows.Forms.Application.MessageLoop)
                    {
                        // WinForms app
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        // Console app
                        System.Environment.Exit(1);
                    }
                }
                else
                {
                    LoadData();
                }

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //var xmlDoc = new XmlDocument();
            //xmlDoc.Load(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            //xmlDoc.SelectSingleNode("//geoSettings/summary/add[@key='Country']").Attributes["value"].Value = "Old Zeeland";
            //xmlDoc.Save(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);

            //ConfigurationManager.RefreshSection("geoSettings/summary");
        }

        private void label3_Click(object sender, EventArgs e)
        {
        
        }

        private void bwReportPrintTest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblTestPrint.Visible = false;
            button5.Enabled = true;
        }

        private void bwSamplePrintTest_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblTestPrint.Visible = false;
            button4.Enabled = true;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            lblTestPrint.Visible = true;
            button4.Enabled = false;
            SamplePrinter = cboGrade_Printer.Text; 
            lblTestPrint.Text = "Printing Test Sent To " + SamplePrinter;
            bwSamplePrintTest.RunWorkerAsync();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            lblTestPrint.Visible = true;
            button5.Enabled = false;
            ReportPrinter = cboReport_Printer.Text;
            lblTestPrint.Text = "Printing Test Sent To " + ReportPrinter;
            bwReportPrintTest.RunWorkerAsync();
        }

        private void bwSamplePrintTest_DoWork(object sender, DoWorkEventArgs e)
        {
            Printing.printSampleLabelTest(SamplePrinter);
        }

        private void bwReportPrintTest_DoWork(object sender, DoWorkEventArgs e)
        {
            Printing.PrintReportPrinterTest(ReportPrinter);
        }

      
    }
}
