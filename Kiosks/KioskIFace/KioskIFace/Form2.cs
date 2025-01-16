using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
namespace KioskIFace
{
    public partial class Form2 : Form
    {

        KioskWebService.LocalDataSet.Weigh_ScalesDataTable WeightScales = new KioskWebService.LocalDataSet.Weigh_ScalesDataTable();

        public Form2()
        {
            InitializeComponent();

            System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            this.cboSerialPort.Items.Clear();
                string[] SerialPorts = System.IO.Ports.SerialPort.GetPortNames();
                foreach (string port in SerialPorts)
                {
                    this.cboSerialPort.Items.Add(port);

                }
            this.cboSerialPort.Items.Add("");



            using ( KioskWebService.Kiosk Kiosk = new KioskWebService.Kiosk())
            {
                WeightScales = Kiosk.GetScales(DateTime.Now);
                {
                    foreach (KioskWebService.LocalDataSet.Weigh_ScalesRow  row in WeightScales)
                    {
                      
                        this.cboScale.Items.Add(row.Description);

                    }

                }


                using (KioskWebService.LocalDataSet.Site_PrintersDataTable   Printers = Kiosk.GetPrinterList())
                {
                    foreach (KioskWebService.LocalDataSet.Site_PrintersRow  row in Printers)
                    {
                        this.cboPrinters.Items.Add(row.Printer_Name);

                    }

                }



                using (KioskWebService.LocalDataSet.Site_SetupDataTable site_SetupDataTable = Kiosk.GetSites())
                {
                    foreach (KioskWebService.LocalDataSet.Site_SetupRow row in site_SetupDataTable )
                    {
                        cbo_Location.Items.Add(row.Location_Id.ToString());
                    }
                }
            }




            string Printer = config.AppSettings.Settings["Printer"].Value;


            foreach (string items in cboPrinters.Items)
            {

                if (items == Printer)
                {
                    this.cboPrinters.SelectedIndex = this.cboPrinters.FindString(Printer);
                }



            }

            string ScaleDescription = config.AppSettings.Settings["Scale"].Value;


            foreach (string item in cboScale.Items)
            {


                if (item == ScaleDescription)
                {
                    this.cboScale.SelectedIndex = this.cboScale.FindString(ScaleDescription);
                }



            }


            string Location_ID = config.AppSettings.Settings["Location_ID"].Value;

            foreach (string item in cbo_Location.Items)
            {


                if (item == Location_ID)
                {
                    this.cbo_Location.SelectedIndex = this.cbo_Location.FindString(Location_ID);
                }



            }

            this.cboSerialPort.SelectedIndex = this.cboSerialPort.FindStringExact(config.AppSettings.Settings["ComPort"].Value);


        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["ComPort"].Value = this.cboSerialPort.Text;
                if (WeightScales.Count>0)
                {
                    foreach (KioskWebService.LocalDataSet.Weigh_ScalesRow row in WeightScales)
                    {
                        if (cboScale.Text == row.Description)
                        {
                            config.AppSettings.Settings["Scale"].Value = row.Description;
                            config.AppSettings.Settings["Scale_UID"].Value = row.UID.ToString();
                            break; 
                        }
                    }
                }
                else
                {
                    config.AppSettings.Settings["Scale"].Value = "";
                }


                config.AppSettings.Settings["Printer"].Value = this.cboPrinters.Text ;
                config.AppSettings.Settings["Location_ID"].Value = this.cbo_Location.Text;
                


                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error Saving Configuration " + ex.Message, "Error");
            }

            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
