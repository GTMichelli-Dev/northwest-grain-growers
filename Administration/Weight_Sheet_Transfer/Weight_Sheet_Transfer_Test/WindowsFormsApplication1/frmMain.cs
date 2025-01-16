using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Weight_Sheet_Export
{
    public partial class frmMain : Form
    {


        public frmMain()
        {
            InitializeComponent();
            this.dtDate.Value = DateTime.Now;
            pnlStatus.Visible = false;

            this.Text += "  vers " + Application.ProductVersion.ToString();
            this.location_DistrictsTableAdapter1.Fill(this.nW_Data_MasterDataSet.Location_Districts);
            ckListDistricts.Items.Clear();
            foreach (NW_Data_MasterDataSet.Location_DistrictsRow row in nW_Data_MasterDataSet.Location_Districts)
            {
                ckListDistricts.Items.Add(row.District);
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
               string[] D = Properties.Settings.Default.SelectedDistricts.Split(',');
           // string[] D = txtSelectedItems.Text.Split(',');
            foreach (string Item in D)
            {
                for (int i = 0; i < ckListDistricts.Items.Count; i++)
                {
                    if ((string)ckListDistricts.Items[i] == Item) ckListDistricts.SetItemChecked(i, true);
                }

            }

            this.dtDate.Value = DateTime.Now.AddDays(-1);
            GetLocations();


        }


        public void GetLocations()
        {
            this.nW_Data_MasterDataSet.Locations.Clear();
            locationsTableAdapter.ClearBeforeFill = false;
            foreach (object itemChecked in ckListDistricts.CheckedItems)
            {
                this.locationsTableAdapter.FillByDistrict(this.nW_Data_MasterDataSet.Locations, itemChecked.ToString());
               

            }

    
            ckListLocations.Items.Clear();
            nW_Data_MasterDataSet.Locations.OrderBy(x => x.Description);
            foreach (NW_Data_MasterDataSet.LocationsRow row in nW_Data_MasterDataSet.Locations)
            {
                ckListLocations.Items.Add(row.text);
            }
            
        }


        private void dataGridView1_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.nW_Data_MasterDataSet.vwWeight_Sheet_Information.Clear();
            
            Application.DoEvents();
            pnlStatus.Visible = true;
            if (this.vwWeight_Sheet_InformationTableAdapter == null) this.vwWeight_Sheet_InformationTableAdapter = new NW_Data_MasterDataSetTableAdapters.vwWeight_Sheet_InformationTableAdapter();
            this.vwWeight_Sheet_InformationTableAdapter.ClearBeforeFill = false;
           
             //   this.vwWeight_Sheet_InformationTableAdapter.Fill(this.nW_Data_MasterDataSet.vwWeight_Sheet_Information, this.dtDate.Value, 3);
            foreach (object itemChecked in ckListLocations.CheckedItems)
            {
                foreach (NW_Data_MasterDataSet.LocationsRow row in nW_Data_MasterDataSet.Locations)
                {
                    if (itemChecked.ToString() == row.text)
                    {
                        lblStatus.Text = "Adding Records From "+System.Environment.NewLine  + row.text;
                        Application.DoEvents();
                        System.Threading.Thread.Sleep(100);
                        this.vwWeight_Sheet_InformationTableAdapter.Fill(this.nW_Data_MasterDataSet.vwWeight_Sheet_Information, this.dtDate.Value, row.Id);
                    }

                }

            }


        if (this.nW_Data_MasterDataSet.vwWeight_Sheet_Information.Count == 0)
            {
                lblStatus.Text = "No Records";
                Application.DoEvents();
                System.Threading.Thread.Sleep(2000);
             
            }

            
            pnlStatus.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog()== DialogResult.OK)
            {
                this.txtFolderPath.Text = folderBrowserDialog1.SelectedPath;
                 
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtFileName.Text ))
            {
                pnlStatus.Visible = true;
                lblStatus.Text = "Enter A File Name";
                Application.DoEvents();
                System.Threading.Thread.Sleep(2000);
                pnlStatus.Visible = false;
            }
            else if (string.IsNullOrEmpty(this.txtFolderPath.Text ))
            {
                pnlStatus.Visible = true;
                lblStatus.Text = "Enter A File Path";
                Application.DoEvents();
                System.Threading.Thread.Sleep(2000);
                pnlStatus.Visible = false;
            }
            else
            {
                string filepath = this.txtFolderPath.Text;
                if (!filepath.EndsWith(@"\")) filepath += @"\";
                filepath += txtFileName.Text ;
                Weight_Sheet_Transfer.TransferDataset Ds = new Weight_Sheet_Transfer.TransferDataset();
                foreach (NW_Data_MasterDataSet.vwWeight_Sheet_InformationRow SelectedRow in nW_Data_MasterDataSet.vwWeight_Sheet_Information)
                {
                    pnlStatus.Visible = true;
                    lblStatus.Text = string.Format("Adding Location {0} #{1} ",SelectedRow.Location_Id,SelectedRow.WS_Id) ;
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(100);
                    pnlStatus.Visible = false;
                    //  SelectedRow = (NW_Data_MasterDataSet.vwWeight_Sheet_InformationRow)(DataRow)((DataRowView)this.vwWeightSheetInformationBindingSource.Current).Row;

                    Weight_Sheet_Transfer.TransferDataset.TransferValsRow row = Ds.TransferVals.NewTransferValsRow();
                    row.Crop_ID = SelectedRow.Crop_Id;
                    if (!SelectedRow.IsProducer_IdNull()) row.Customer_ID = SelectedRow.Producer_Id;
                    row.Gross = SelectedRow.Net;
                    if (!SelectedRow.IsCarrier_IdNull()) row.Hauler_ID = SelectedRow.Carrier_Id;
                    if (!SelectedRow.IsRateNull()) row.Rate = SelectedRow.Rate.ToString();
                    row.Time_Date = SelectedRow.Creation_Date;
                    row.WeightSheet = SelectedRow.WS_Id;
                    if (!SelectedRow.IsLot_NumberNull()) row.Lot_ID =SelectedRow.Lot_Number.ToString();
                    if (!SelectedRow.IsOutbound_LocationNull()) row.Outbound_Location = SelectedRow.Outbound_Location;
                    row.Location_ID = SelectedRow.Location_Id;
                    row.Inbound = SelectedRow.IsOutbound_LocationNull();

                    Ds.TransferVals.AddTransferValsRow(row);

                }
                Weight_Sheet_Transfer.Transfer.TransferFileInfo Response = Weight_Sheet_Transfer.Transfer.Create_Transfer_File(filepath, Ds.TransferVals);
                if (Response.Success)
                {
                    pnlStatus.Visible = true;
                    lblStatus.Text ="Data Exported";
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(2000);
                    pnlStatus.Visible = false;
                }
                else
                {
                    pnlStatus.Visible = true;
                    lblStatus.Text = "Error Exporting Data.." + System.Environment.NewLine + Response.ErrorMessage;
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(3000);
                    pnlStatus.Visible = false;
                    
                }


            }

        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            string SelectedDistricts = string.Empty ;
            foreach (object itemChecked in ckListDistricts.CheckedItems)
            {
                if (!string.IsNullOrEmpty(SelectedDistricts)) SelectedDistricts += ",";
                SelectedDistricts += itemChecked.ToString();
            }
          //  txtSelectedItems.Text = SelectedDistricts;
            Properties.Settings.Default.SelectedDistricts = SelectedDistricts;
            Properties.Settings.Default.Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ckListLocations.Items.Count; i++)
                ckListLocations.SetItemChecked(i, false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ckListLocations.Items.Count; i++)
                ckListLocations.SetItemChecked(i, true);
        }

        private void txtFileName_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string filepath = this.txtFolderPath.Text;
                if (!filepath.EndsWith(@"\")) filepath += @"\";
                filepath += txtFileName.Text;
                System.Diagnostics.Process.Start("notepad.exe", filepath);
            }
            catch (Exception ex)
            {
                pnlStatus.Visible = true;
                lblStatus.Text = ex.Message ;
                Application.DoEvents();
                System.Threading.Thread.Sleep(2000);
                pnlStatus.Visible = false;
            }
        }

        private void cboDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetLocations();
        }

        private void ckListDistricts_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
        }

        private void ckListDistricts_Click(object sender, EventArgs e)
        {
       
        }

        private void ckListDistricts_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void ckListDistricts_MouseUp(object sender, MouseEventArgs e)
        {
            GetLocations();
        }

        private void ckListDistricts_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void frmMain_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {

        }

        private void ckListDistricts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            if (string.IsNullOrEmpty(this.txtFolderPath.Text))
            {
                pnlStatus.Visible = true;
                lblStatus.Text = "Enter A File Path";
                Application.DoEvents();
                System.Threading.Thread.Sleep(2000);
                pnlStatus.Visible = false;
            }
            else
            {

                pnlStatus.Visible = true;
                lblStatus.Text = "Exporting DPR Report To Excell";
                Application.DoEvents();
                var SelectedDate = this.dtDate.Value;
                string filepath = this.txtFolderPath.Text;
                if (!filepath.EndsWith(@"\")) filepath += @"\";

                var FileName = string.Format("DailyDPR{0}_{1}_{2}.xlsx", SelectedDate.Month, SelectedDate.Day, SelectedDate.Year);

                if (!filepath.EndsWith(@"\")) filepath += @"\";
                filepath += FileName;
                if ( DPR.ExportDPR(SelectedDate, filepath))
                {
                    pnlStatus.Visible = false;
                    MessageBox.Show("File Exported To " + filepath);
                    System.Diagnostics.Process.Start(filepath);
                }
                else
                {
                    pnlStatus.Visible = false;
                }

                //List<int> locations = new List<int>();

                //foreach(var item in ckListLocations.CheckedItems  )
                //{
                //    var Location = item.ToString();
                //    try
                //    {

                //        var Id = Convert.ToInt16( Location.Split('-')[0]);
                //        locations.Add(Id);
                //    }
                //    catch
                //    {
                //        MessageBox.Show("Could Not Get the Location Number For " + Location);
                //    }
                //}


            }
        }

        private void ckListLocations_Click(object sender, EventArgs e)
        {
            
            

        }

        private void ckListDistricts_MouseClick(object sender, MouseEventArgs e)
        {

        }
    }
}
