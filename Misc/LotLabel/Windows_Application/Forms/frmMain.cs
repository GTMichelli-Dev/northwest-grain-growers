using System;
using System.Configuration;
using System.Drawing.Printing;
using System.Windows.Forms;
using System.Collections.Generic;
namespace LotLabeler
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            PopulatePrinterDropdown(this.cboPrinter);
            SetupGrowerAutoComplete();
            SetupCropAutoComplete();
            ResetForm();
        }

        public static void PopulatePrinterDropdown(ComboBox dropdown)
        {
            // Clear the dropdown first
            dropdown.Items.Clear();

            // Get the list of installed printers
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                dropdown.Items.Add(printer);
            }

            string configuredPrinter = Properties.Settings.Default.SelectedPrinter;

            if (!string.IsNullOrEmpty(configuredPrinter) && dropdown.Items.Contains(configuredPrinter))
            {
                dropdown.SelectedItem = configuredPrinter;
            }
            else
            {
                // Optionally, select the first printer in the list if the configured printer is not found
                if (dropdown.Items.Count > 0)
                {
                    dropdown.SelectedIndex = 0;
                }
            }
        }

        private void cboPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPrinter.SelectedItem != null)
            {
                Properties.Settings.Default.SelectedPrinter = cboPrinter.SelectedItem.ToString();
                Properties.Settings.Default.Save();
            }
        }


        private void SetupGrowerAutoComplete()
        {

            using (NWDataset.Crop_VarietiesDataTable cropVariety = new NWDataset.Crop_VarietiesDataTable())
            {
                using (NWDatasetTableAdapters.Crop_VarietiesTableAdapter cropVarietyTableAdapter = new NWDatasetTableAdapters.Crop_VarietiesTableAdapter())
                {
                    cropVarietyTableAdapter.Fill(cropVariety);
                    var autoCompleteSource = new AutoCompleteStringCollection();
                    List<string> Varieties = new List<string>();
                    foreach (var item in cropVariety)
                    {
                        Varieties.Add(item.Description);
                    }
                    autoCompleteSource.AddRange(Varieties.ToArray());

                    txtVariety.AutoCompleteCustomSource = autoCompleteSource;
                    txtVariety.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    txtVariety.AutoCompleteSource = AutoCompleteSource.CustomSource;
                }
            }

        }


        private void SetupCropAutoComplete()
        {

            using (NWDataset.ProducersDataTable producers = new NWDataset.ProducersDataTable())
            {
                using (NWDatasetTableAdapters.ProducersTableAdapter producersTableAdapter = new NWDatasetTableAdapters.ProducersTableAdapter())
                {
                    producersTableAdapter.Fill(producers);
                    var autoCompleteSource = new AutoCompleteStringCollection();
                    List<string> Growers = new List<string>();
                    foreach (var item in producers)
                    {
                        Growers.Add(item.Description);
                    }
                    autoCompleteSource.AddRange(Growers.ToArray());

                    txtGrower.AutoCompleteCustomSource = autoCompleteSource;
                    txtGrower.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    txtGrower.AutoCompleteSource = AutoCompleteSource.CustomSource;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.lblPrinting.Visible = true;
            Application.DoEvents();
            using (NWDataset nwDataset = new NWDataset())
            {
                var row = nwDataset.SampleLabel.NewSampleLabelRow();
                row.Check_Protien = ((string)cboProtein.SelectedValue == "Yes") ? true : false;
                row.Lot_Number = txtLot.Text;
                row.Crop_Description = txtVariety.Text;
                row.Variety = "";
                row.Producer_LandLord = txtGrower.Text;
                row.Close_Date = dtDateClosed.Value;
                nwDataset.SampleLabel.AddSampleLabelRow(row);
                Printing.PrintSampleLabel(nwDataset, cboPrinter.SelectedItem.ToString(), (int)numericUpDown1.Value);
            }
            this.lblPrinting.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ResetForm();  
        }

        public void ResetForm()
        {
            txtLot.Text = "";
            txtVariety.Text = "";
            txtGrower.Text = "";
            dtDateClosed.Value = DateTime.Today;
            cboProtein.SelectedIndex = 1;
            numericUpDown1.Value = 1;
        }
    }
}
